using AutoMapper;
using Object.Dto;
using Object.Model;

namespace RMAPI.ServiceRegistration
{
    public class MapperRegistraion : Profile
    {
        public MapperRegistraion()
        {
            CreateMap<User, UserDto>();
        }
    }
}
