using AutoMapper;
using ExpenseTracker.Models.Domain;
using ExpenseTracker.Models.DTO;

namespace ExpenseTracker.Mappings
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<UserRegistrationDto, User>();
        }
    }
}
