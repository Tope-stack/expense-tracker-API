using ExpenseTracker.Models;
using ExpenseTracker.Models.Domain;

namespace ExpenseTracker.Repositories
{
    public interface IExpense
    {
        Task<IEnumerable<Expense>> GetAllExpenses();
        Task<Expense> GetExpense(Guid id);
        Task<Expense> CreateExpense(Expense expense);
        Task<Expense> UpdateExpense(Guid id, Expense expense);
        Task<Expense> DeleteExpense(Guid id);
        Task<string> TotalExpense();
        Task<string> TotalExpenseByCategory(Category category);
        IEnumerable<object> MonthlyExpenses();
        IEnumerable<object> DailyExpenses();
        //IEnumerable<object> WeeklyExpenses();

    }
}
