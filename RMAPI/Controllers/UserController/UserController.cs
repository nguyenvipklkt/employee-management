using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Object.Setting;
using RMAPI.Middleware;
using Service.Service.UserService;

namespace RMAPI.Controllers.UserController
{
    [ApiController]
    [Route("api/user")]
    [Authorize]
    public class UserController : BaseApiController<UserController>
    {
        private readonly IUserService _userService;
        private readonly IBaseQuery _baseQuery;
        
        public UserController(IUserService userAuthentication, IBaseQuery baseQuery)
        {
            _userService = userAuthentication;
            _baseQuery = baseQuery;
        }

        [HttpGet]
        [Route("get-profile")]
        [HasPermission("")]
        public APIResponse GetProfile()
        {
            try
            {
                var res = _userService.GetProfile(UserId);
                return new APIResponse { Data = res };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpGet]
        [Route("search-managers-by-name")]
        [HasPermission("SEARCH_MANAGER_BY_NAME")]
    }
}
