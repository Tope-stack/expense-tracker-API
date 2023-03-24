using ExpenseTracker.Models.DTO;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Repositories
{
    public interface IUserAuthRepository
    {
        Task<IdentityResult> RegisterUserAsync(UserRegistrationDto userRegistration);
        Task<bool> ValidateUserAsync(UserLoginDto userLogin);
        Task<string> CreateTokenAsync();
    }
}
