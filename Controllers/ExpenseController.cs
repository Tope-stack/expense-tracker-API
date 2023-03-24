using ExpenseTracker.Models;
using ExpenseTracker.Models.Domain;
using ExpenseTracker.Models.DTO;
using ExpenseTracker.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExpenseController : Controller
    {
        protected ResponseDto _response;
        private readonly IExpense _expenseRepository;

        public ExpenseController(IExpense expenseRepository)
        {
            _expenseRepository = expenseRepository;
            this._response = new ResponseDto();
        }

        [Authorize]
        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                IEnumerable<ExpenseDto> expenseDtos = await _expenseRepository.GetAllExpenses();
                _response.Result = expenseDtos;
            } 
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [Authorize]
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<object> GetById(Guid id)
        {
            try
            {
                ExpenseDto expenseDto = await _expenseRepository.GetExpense(id);
                _response.Result = expenseDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string>() { ex.ToString() };
            }
            return _response;
            
        }

        [Authorize]
        [HttpPost]
        [Route("new-expense")]
        public async Task<object> Create([FromBody] CreateExpenseDto expense)
        {
            try
            {
                CreateExpenseDto model = await _expenseRepository.CreateExpense(expense);
                _response.Result = model;
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string>() { ex.ToString() };
            }
            

            return _response;
        }

        [Authorize]
        [HttpPut]
        public async Task<object> Update(Guid id, UpdateExpenseDto expense)
        {
            try
            {
                UpdateExpenseDto model = await _expenseRepository.UpdateExpense(id, expense);
                _response.Result = model;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string>() { ex.ToString() };
            }


            return _response;
           
        }

        [Authorize]
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<object> Delete(Guid id)
        {
            try
            {
               bool isSuccess = await _expenseRepository.DeleteExpense(id);
                _response.Result = isSuccess;
            } 
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string>() { ex.ToString() };
            }
            return _response;
           
        }

        [Authorize]
        [HttpGet]
        [Route("total-expense")]
        public async Task<object> TotalExpense()
        {
            try
            {
                var totalExpense = await _expenseRepository.TotalExpense();
                _response.Result = totalExpense;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string>() { ex.ToString() };
            }
            return _response;
            
        }

        [Authorize]
        [HttpGet]
        [Route("total-expense/category")]
        public async Task<object> TotalExpenseByCategory(Category category)
        {
            try
            {
                var expense = await _expenseRepository.TotalExpenseByCategory(category);
                _response.Result = expense;
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [Authorize]
        [HttpGet]
        [Route("monthly-expense")]
        public IEnumerable<object> MonthlyExpenses()
        {
             
            return _expenseRepository.MonthlyExpenses();
        }

        [Authorize]
        [HttpGet]
        [Route("daily-expense")]
        public IEnumerable<object> DailyExpenses()
        {
           
            return _expenseRepository.DailyExpenses();
            
        }

        [Authorize]
        [HttpGet]
        [Route("weekly-expense")]
        public IEnumerable<object> WeeklyExpenses()
        {
            return _expenseRepository.WeeklyExpenses();
        }

        [Authorize]
        [HttpGet]
        [Route("total-expense/year")]
        public IEnumerable<object> YearlyExpenses()
        {
            return _expenseRepository.YearlyEpenses();
        }
    }
}
