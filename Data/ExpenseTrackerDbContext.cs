using ExpenseTracker.Models.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace ExpenseTracker.Data
{
    public class ExpenseTrackerDbContext : IdentityDbContext<User>
    {
        public ExpenseTrackerDbContext(DbContextOptions options) : base(options)
        {

        }

        

        public DbSet<Expense> Expenses { get; set; }
    }
}
