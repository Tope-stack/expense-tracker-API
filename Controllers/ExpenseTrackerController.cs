using ExpenseTracker.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers
{
    [ApiController]
    [Route("api/expenses")]
    public class ExpenseTrackerController : Controller
    {
        private readonly ExpenseTrackerDbContext _context;

        public ExpenseTrackerController(ExpenseTrackerDbContext _context)
        {
            this._context = _context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Expenses.ToListAsync());
        }
    }
}
