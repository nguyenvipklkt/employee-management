using AutoMapper;
using Helper.NLog;
using Infrastructure.Repositories;
using Object.Dto;
using Object.Model;

namespace Service.Service.UserService
{
    public interface IUserService
    {
        UserDto GetProfile(int userId);
    }

    class UserService
    {
        private readonly IBaseCommand<User> _baseCommand;
        private readonly IMapper _mapper;

        public UserService(IBaseCommand<User> baseCommand, IMapper mapper)
        {
            _baseCommand = baseCommand;
            _mapper = mapper;
        }

        public UserDto GetProfile(int userId)
        {
            try
            {
                var user = _baseCommand.FindByCondition(x => x.UserId == userId).FirstOrDefault();
                UserDto userDto = _mapper.Map<UserDto>(user);
                return userDto;
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }
        }
    }
}
