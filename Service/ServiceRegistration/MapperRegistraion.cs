using AutoMapper;
using CoreValidation.Requests.Department;
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
            CreateMap<Department, AddDepartmentRequest>();
        }
    }
}
