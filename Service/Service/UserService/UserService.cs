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

    public class UserService : IUserService
    {
        private readonly IBaseCommand<User> _baseUserCommand;
        private readonly IMapper _mapper;

        public UserService(IBaseCommand<User> baseUserCommand, IMapper mapper)
        {
            _baseUserCommand = baseUserCommand;
            _mapper = mapper;
        }

        public UserDto GetProfile(int userId)
        {
            try
            {
                var user = _baseUserCommand.FindByCondition(x => x.UserId == userId).FirstOrDefault();
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
