using ExpenseTracker.Data;
using ExpenseTracker.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Repositories
{
    public class ExpenseRepository : IExpense
    {
        private readonly ExpenseTrackerDbContext _context;

        public ExpenseRepository(ExpenseTrackerDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Expense>> GetAllExpenses()
        {
            return await _context.Expenses.ToListAsync();
        }

        public async Task<Expense> GetExpense(Guid id)
        {
            var existingExpense = await _context.Expenses.FirstOrDefaultAsync(x => x.Id == id);

            return existingExpense == null ? null : existingExpense;
        }

        public async Task<Expense> CreateExpense(Expense expense)
        {
            expense.Id = new Guid();
            await _context.Expenses.AddAsync(expense);  
            await _context.SaveChangesAsync();

            return expense;
        }

        public async Task<Expense> UpdateExpense(Guid id, Expense expense)
        {
            var existingExpense =await _context.Expenses.FirstOrDefaultAsync(x => x.Id == id);

            existingExpense.Category = expense.Category;
            existingExpense.MerchantName = expense.MerchantName;
            existingExpense.Amount = expense.Amount;

            await _context.SaveChangesAsync();

            return existingExpense;
        }

        public async Task<Expense> DeleteExpense(Guid id)
        {
            var existingExpense = await _context.Expenses.FirstOrDefaultAsync(x => x.Id == id);
            
            if (existingExpense == null) return null;

            _context.Expenses.Remove(existingExpense);
            await _context.SaveChangesAsync();

            return existingExpense;
        }
    }
}
