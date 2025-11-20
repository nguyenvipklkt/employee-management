using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Object.Setting;
using NVAPI.Middleware;
using Service.Service.UserService;

namespace NVAPI.Controllers.UserController
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
        [Route("search-user-by-name")]
        [HasPermission("SEARCH_USER_BY_NAME")]
        public APIResponse SearchUserByName([FromQuery] string name)
        {
            try
            {
                var res = _userService.SearchUserByName(UserId, name);
                return new APIResponse { Data = res };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }
    }
}
