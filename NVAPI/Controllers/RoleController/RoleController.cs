using CoreValidation.Requests.Role;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Object.Setting;
using NVAPI.Middleware;
using Service.Service.RoleService;
using Service.Service.UserService;

namespace NVAPI.Controllers.RoleController
{
    [ApiController]
    [Route("api/role")]
    [Authorize]
    public class RoleController : BaseApiController<RoleController>
    {
        private readonly IRoleService _roleService;
        private readonly IBaseQuery _baseQuery;

        public RoleController(IRoleService roleService, IBaseQuery baseQuery)
        {
            _roleService = roleService;
            _baseQuery = baseQuery;
        }

        [HttpPost]
        [Route("add-role")]
        [HasPermission("")]
        public APIResponse AddRole(AddRoleRequest request)
        {
            try
            {
                var res = _roleService.AddRole(UserId, request);
                return new APIResponse { Data = res };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpGet]
        [Route("get-role")]
        [HasPermission("")]
        public APIResponse GetRoleCode()
        {
            try
            {
                var res = _roleService.GetRoleCode(UserId);
                return new APIResponse { Data = res };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }
    }
}
