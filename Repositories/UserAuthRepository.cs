using AutoMapper;
using ExpenseTracker.Models.Domain;
using ExpenseTracker.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ExpenseTracker.Repositories
{
    internal sealed class UserAuthRepository : IUserAuthRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private User _user;

        public UserAuthRepository(UserManager<User> userManager, IMapper mapper, IConfiguration configuration)
        {
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<IdentityResult> RegisterUserAsync(UserRegistrationDto userRegistration)
        {
            var user = _mapper.Map<User>(userRegistration);
            var result = await _userManager.CreateAsync(user, userRegistration.Password);
            return result;
        }

        public async Task<bool> ValidateUserAsync(UserLoginDto userLogin)
        {
            _user = await _userManager.FindByNameAsync(userLogin.UserName);
            var result = _user != null && await _userManager.CheckPasswordAsync(_user, userLogin.Password);
            return result;
        }



        public async Task<string> CreateTokenAsync()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private SigningCredentials GetSigningCredentials()
        {
            var jwtConfig = _configuration.GetSection("jwtConfig");
            var key = Encoding.UTF8.GetBytes(jwtConfig["Secret"]);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, _user.UserName)
        };
            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtConfig");
            var tokenOptions = new JwtSecurityToken
            (
            issuer: jwtSettings["validIssuer"],
            audience: jwtSettings["validAudience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expiresIn"])),
            signingCredentials: signingCredentials
            );
            return tokenOptions;
        }

        public async Task<string> ChangeUserPassword(ChangePasswordDTO changePassword)
        {
            var currentUser = await _userManager.FindByEmailAsync(changePassword.Email);

            //if (currentUser == null)
            //{
            //    throw new ArgumentException($" {currentUser} does not exist. ");
            //}

            var changedPassword = await _userManager.ChangePasswordAsync(currentUser, changePassword.CurrentPassword, changePassword.NewPassword);

            if (changedPassword.Succeeded)
            {
                return string.Empty;
            }

            else
            {
                string error = changedPassword.Errors.First().Description;

                if (error == "PasswordMismatch")
                {
                    return "Current password is incorrect";
                }
                else
                {
                    return "We are not able to change your password right now. Please contact admin";
                }
            }
        }
    }
}
