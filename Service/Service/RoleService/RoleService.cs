using AutoMapper;
using Common.Enum.ErrorEnum;
using CoreValidation.Requests.Role;
using Helper.NLog;
using Infrastructure.Repositories;
using Object.Model;

namespace Service.Service.RoleService
{
    public interface IRoleService
    {
        bool AddRole(int userId, AddRoleRequest request);
    }

    public class RoleService : IRoleService
    {
        private readonly IBaseCommand<Role> _baseRoleCommand;
        private readonly IMapper _mapper;

        public RoleService(IBaseCommand<Role> baseRoleCommand, IMapper mapper)
        {
            _baseRoleCommand = baseRoleCommand;
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
    }
}
