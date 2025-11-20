using AutoMapper;
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
            CreateMap<Role, AddRoleRequest>().ReverseMap();
        }
    }
}
