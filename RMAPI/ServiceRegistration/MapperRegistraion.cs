using AutoMapper;
using Common.Dto;
using Shared;

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
