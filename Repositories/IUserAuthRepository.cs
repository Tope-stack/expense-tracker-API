using ExpenseTracker.Models.DTO;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Repositories
{
    public interface IUserAuthRepository
    {
        Task<IdentityResult> RegisterUserAsync(UserRegistrationDto userRegistration);
        Task<bool> ValidateUserAsync(UserLoginDto userLogin);
        //Task<IdentityResult> ChangePassword(string userName, ChangePasswordDTO changePasswordDTO);
        //Task<IdentityResult> ChangePassword(string userName, string currentPassword, string newPassword);
        //Task<IdentityUser> GetCurrentUserAsync();
        Task<string> ChangeUserPassword(ChangePasswordDTO changePassword);

        Task<string> CreateTokenAsync();
    }
}
