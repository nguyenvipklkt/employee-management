using AutoMapper;
using CoreValidation.Requests.Department;
using CoreValidation.Requests.Food;
using CoreValidation.Requests.Role;
using Object.Dto;
using Object.Model;

namespace Service.ServiceRegistration
{
    public class MapperRegistraion : Profile
    {
        public MapperRegistraion()
        {
            CreateMap<User, UserDto>();
            CreateMap<Department, DepartmentDto>();
            CreateMap<Department, AddDepartmentRequest>().ReverseMap();
            CreateMap<Role, AddRoleRequest>().ReverseMap();
            CreateMap<Food, AddFoodRequest>().ReverseMap();
            CreateMap<Food, UpdateFoodRequest>().ReverseMap();
        }
    }
}
