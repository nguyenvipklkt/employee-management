using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Object.Model;
using Object.Setting;
using RMAPI.Helpers;
using RMAPI.Services.UserService;

namespace RMAPI.Controllers.UserController
{
    [ApiController]
    [Route("api/user")]
    [Authorize]
    public class UserController : BaseApiController<UserController>
    {
        private readonly UserService _userService;
        private readonly BaseQuery _baseQuery;
        
        public UserController(UserService userAuthentication, BaseQuery baseQuery)
        {
            _userService = userAuthentication;
            _baseQuery = baseQuery;
        }

        [HttpGet]
        [Route("getProfile")]
        public async Task<APIResponse> GetProfile()
        {
            try
            {
                var hasPermission = await AuthorizationUser.HasPermissionAsync(UserId, "VIEW_PROFILE", _baseQuery);
                if (!hasPermission)
                {
                    return new APIResponse
                    {
                        Code = "-1",
                        Message = "Không có quyền truy cập"
                    };
                }
                var res = _userService.GetProfile(UserId);
                return new APIResponse { Data = res };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }
    }
}
