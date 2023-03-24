using AutoMapper;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using ExpenseTracker.Models.Domain;
using ExpenseTracker.Models.DTO;
using ExpenseTracker.Utilities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Repositories
{
    public class ExpenseRepository : IExpense
    {
        private readonly ExpenseTrackerDbContext _context;
        private IMapper _mapper;

        public ExpenseRepository(ExpenseTrackerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ExpenseDto>> GetAllExpenses()
        {
            IEnumerable<Expense> expenses = await _context.Expenses.ToListAsync();
            return _mapper.Map<List<ExpenseDto>>(expenses);
            
        }

        public async Task<ExpenseDto> GetExpense(Guid id)
        {
            Expense expense = await _context.Expenses.FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<ExpenseDto>(expense);
        }

        public async Task<CreateExpenseDto> CreateExpense(CreateExpenseDto expense)
        {
            Expense newExpense = _mapper.Map<CreateExpenseDto, Expense>(expense);

            await _context.AddAsync(newExpense);  
            await _context.SaveChangesAsync();

            return expense;
        }

        public async Task<UpdateExpenseDto> UpdateExpense(Guid id, UpdateExpenseDto expense)
        {
            Expense existingExpense = _mapper.Map<UpdateExpenseDto, Expense>(expense);

            if (id != existingExpense.Id)
            {
                _context.Expenses.Update(existingExpense);
            }

            await _context.SaveChangesAsync();

            return _mapper.Map<Expense, UpdateExpenseDto>(existingExpense);
        }

        public async Task<bool> DeleteExpense(Guid id)
        {
            Expense existingExpense = await _context.Expenses.FirstOrDefaultAsync(x => x.Id == id);
            
            if (existingExpense == null) return false;

            _context.Expenses.Remove(existingExpense);
            await _context.SaveChangesAsync();

            return true;
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

        public IEnumerable<object> WeeklyExpenses()
        {
            var weeklyExpense = _context.Expenses.AsEnumerable()
                .OrderBy(x => x.ExpenseDate)
                .GroupBy(j => j.ExpenseDate.StartOfWeek(DayOfWeek.Monday),
                (key, group) => new
                {
                    startWeekDate = key.ToString("MM / dd / yyyy"),
                    endWeekDate = key.AddDays(7).ToString("MM / dd / yyyy"),
                    totalExpense = group.Sum(y => y.Amount)
                }
                );

            return weeklyExpense;
        }

        public IEnumerable<object> YearlyEpenses()
        {
            var yearlyEpenses = _context.Expenses.AsEnumerable()
                .GroupBy(x => new { Year = x.ExpenseDate.ToString("yyyy"), x.ExpenseDate }, (key, group) => new
                {
                    Year = key.Year,
                    totalExpense = group.Sum(y => y.Amount)
                });
            return yearlyEpenses;
        }

    }
}
