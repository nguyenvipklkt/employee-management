using AutoMapper;
using Common.Enum.RoleEnum;
using Helper.NLog;
using Infrastructure.Repositories;
using Object.Dto;
using Object.Model;

namespace Service.Service.UserService
{
    public interface IUserService
    {
        UserDto GetProfile(int userId);
        List<UserDto> SearchUserByName(int userId, string name);
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

        public List<UserDto> SearchUserByName(int UserId, string name)
        {
            List<User> users = new List<User>();
            try
            {
                var currentUser = _baseUserCommand.FindByCondition(x => x.UserId == UserId).FirstOrDefault();
                if (currentUser == null)
                    throw new Exception("User không tồn tại");

                if (currentUser.RoleCode == RoleEnum.ADMIN)
                {
                    users = _baseUserCommand.FindByCondition(x =>
                        x.Name.Contains(name) &&
                        x.IsActive == 1 &&
                        x.IsDeleted == false
                    ).ToList();
                }
                else
                {
                    throw new Exception("Bạn không có quyền tìm kiếm người dùng.");
                }
                List<UserDto> userDtos = _mapper.Map<List<UserDto>>(users);
                return userDtos;
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }
        }
    }
}
