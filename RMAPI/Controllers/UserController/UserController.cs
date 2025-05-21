using Common.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMAPI.Services.UserService;

namespace RMAPI.Controllers.UserController
{
    [ApiController]
    [Route("api/user")]
    [Authorize]
    public class UserController : BaseApiController<UserController>
    {
        private readonly UserService _userService;
        
        public UserController(UserService userAuthentication)
        {
            _userService = userAuthentication;
        }

        [HttpGet]
        [Route("getProfile")]
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
    }
}
