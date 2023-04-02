using ExpenseTracker.Models.DTO;
using ExpenseTracker.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    [ApiController]
    [Route("user-authentication")]
    public class AuthController : Controller
    {
        private readonly IUserAuthRepository _userAuthRepository;

        public AuthController(IUserAuthRepository userAuthRepository)
        {
            _userAuthRepository = userAuthRepository;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto userRegistration)
        {
            var userResult = await _userAuthRepository.RegisterUserAsync(userRegistration);
            return !userResult.Succeeded ? new BadRequestObjectResult(userResult) : StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] UserLoginDto userLogin)
        {
            return !await _userAuthRepository.ValidateUserAsync(userLogin)
                ? Unauthorized() : Ok(new { Token = await _userAuthRepository.CreateTokenAsync() });
        }
    }
}
