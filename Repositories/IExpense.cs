using ExpenseTracker.Models;
using ExpenseTracker.Models.Domain;
using ExpenseTracker.Models.DTO;

namespace ExpenseTracker.Repositories
{
    public interface IExpense
    {
        Task<IEnumerable<ExpenseDto>> GetAllExpenses();
        Task<ExpenseDto> GetExpense(Guid id);
        Task<CreateExpenseDto> CreateExpense(CreateExpenseDto expense);
        Task<UpdateExpenseDto> UpdateExpense(Guid id, UpdateExpenseDto expense);
        Task<bool> DeleteExpense(Guid id);
        Task<string> TotalExpense();
        Task<string> TotalExpenseByCategory(Category category);
        IEnumerable<object> MonthlyExpenses();
        IEnumerable<object> DailyExpenses();
        IEnumerable<object> WeeklyExpenses();
        IEnumerable<object> YearlyEpenses();

    }
}
