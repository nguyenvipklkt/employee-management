using AutoMapper;
using CoreValidation.Requests.Role;
using Helper.NLog;
using Infrastructure.Repositories;
using Object.Model;

namespace Service.Service.RoleService
{
    public interface IRoleService
    {
        bool AddRole(int userId, AddRoleRequest request);
        string GetRoleCode(int userId);
    }

    public class RoleService : IRoleService
    {
        private readonly IBaseCommand<Role> _baseRoleCommand;
        private readonly IBaseCommand<User> _baseUserCommand;
        private readonly IMapper _mapper;

        public RoleService(IBaseCommand<Role> baseRoleCommand, IBaseCommand<User> baseUserCommand, IMapper mapper)
        {
            _baseRoleCommand = baseRoleCommand;
            _baseUserCommand = baseUserCommand;
            _mapper = mapper;
        }

        public bool AddRole(int userId, AddRoleRequest request)
        {
            try
            {
                var newRole = new Role();
                _mapper.Map(request, newRole);
                newRole.CreateBy = userId;
                var createdRole = _baseRoleCommand.Create(newRole);
                return true;
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }
        }

        public string GetRoleCode(int userId)
        {
            try
            {
                var  user = _baseUserCommand.FindByCondition(x => x.UserId == userId).FirstOrDefault();
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                if (user.IsSuperAdmin == 1)
                {
                    return "SUPPERADMIN";
                }
                return user.RoleCode;
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }
        }
    }
}
