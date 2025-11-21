using AutoMapper;
using CoreValidation.Requests.Customer;
using Object.Dto;
using Object.Model;

namespace Service.ServiceRegistration
{
    public class MapperRegistraion : Profile
    {
        public MapperRegistraion()
        {
            CreateMap<User, UserDto>();
            CreateMap<Customer, AddCustomerRequest>().ReverseMap();
            CreateMap<Customer, UpdateCustomerRequest>().ReverseMap();
        }
    }
}
