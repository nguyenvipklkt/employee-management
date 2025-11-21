using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Object.Setting;
using Service.Service.UserService;

namespace NVAPI.Controllers.UserController
{
    [ApiController]
    [Route("api/user")]
    [Authorize]
    public class UserController : BaseApiController<UserController>
    {
        private readonly IUserService _userService;
        
        public UserController(IUserService userAuthentication)
        {
            _userService = userAuthentication;
        }

        [HttpGet]
        [Route("get-profile")]
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
