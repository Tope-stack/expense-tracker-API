using AutoMapper;
using ExpenseTracker.Models.Domain;
using ExpenseTracker.Models.DTO;

namespace ExpenseTracker.Mappings
{
    public class ExpenseMapping : Profile
    {
        public ExpenseMapping()
        {
            CreateMap<Expense, ExpenseDto>();

            CreateMap<CreateExpenseDto, Expense>();

            CreateMap<UpdateExpenseDto, Expense>().ReverseMap();
        }

    }
}

