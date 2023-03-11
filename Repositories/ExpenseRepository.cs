using ExpenseTracker.Data;
using ExpenseTracker.Models;
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

        public async Task<string> TotalExpense()
        {
            var expenses = await _context.Expenses.ToListAsync();

            if (expenses is null)
                return "No expense found";

            var totalExpense = expenses.Sum(x => x.Amount);

            return $"Your total expense is {totalExpense}";
        }

        public async Task<string> TotalExpenseByCategory(Category category)
        {
            string categoryExpenses;
            var expenses = await _context.Expenses.ToListAsync();

            switch((int)category)
            {
                case 0:
                    var personalExpenses = expenses.FindAll(x => x.Category == 0).Sum(x => x.Amount);
                    categoryExpenses = $"Your total {Category.Personal} expense is {personalExpenses}";
                    break;
                case 1:
                    var businessExpenses = expenses.FindAll(x => (int)x.Category == 1).Sum(x => x.Amount);
                    categoryExpenses = $"Your total {Category.Business} expenses is {businessExpenses}";
                    break;
                case 2:
                    var socialExpenses = expenses.FindAll(x => (int)x.Category == 2).Sum(x => x.Amount);
                    categoryExpenses = $"Your total {Category.Social} expenses is {socialExpenses}";
                    break;
                default:
                    var totalExpenses = expenses.Sum(x => x.Amount) + 1;
                    categoryExpenses = $"Your total expenses is {totalExpenses}";
                    break;
            }

            return categoryExpenses;
        }

        public IEnumerable<object> MonthlyExpenses()
        {
            var monthlyExpense = _context.Expenses.AsEnumerable()
                    .GroupBy(x => new { Month = x.ExpenseDate.ToString("MMM"), x.ExpenseDate.Year },
                    (key, group) => new
                    {
                        year = key.Year,
                        month = key.Month,
                        totalExpense = group.Sum(y => y.Amount)
                    });

            return monthlyExpense;
        }


        public IEnumerable<object> DailyExpenses()
        {
            var dailyExpense = _context.Expenses.AsEnumerable()
                .OrderBy(x => x.ExpenseDate)
                .GroupBy(i => new { day = i.ExpenseDate.DayOfWeek, date = i.ExpenseDate.Date },
                (key, group) => new
                {
                    day = key.day.ToString(),
                    key.date,
                    totalExpense = group.Sum(y => y.Amount)
                }
                );

            return dailyExpense;
        }

        //public IEnumerable<object> WeeklyExpenses()
        //{
        //    var weeklyExpense = _context.Expenses.AsEnumerable()
        //        .OrderBy(x => x.ExpenseDate)
        //        .GroupBy(j => j.ExpenseDate.StartOfWeek(DayOfWeek.Monday),
        //        (key, group) => new
        //        {
        //            startWeekDate = key.ToString("MM / dd / yyyy"),
        //            endWeekDate = key.AddDays(7).ToString("MM / dd / yyyy"),
        //            totalExpense = group.Sum(y => y.Amount)
        //        }
        //        );

        //    return weeklyExpense;
        //}
    }
}
