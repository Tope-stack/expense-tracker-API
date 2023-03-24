using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Models.Domain
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
