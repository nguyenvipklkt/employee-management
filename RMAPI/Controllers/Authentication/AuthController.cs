using Common.Enum;
using CoreValidation.Requests.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMAPI.Services.Authentication;

namespace RMAPI.Controllers.Authentication
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseApiController<AuthController>
    {
        private readonly UserAuthentication _userAuthentication;
        
        public AuthController(UserAuthentication userAuthentication)
        {
            _userAuthentication = userAuthentication;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public APIResponse Login(Login request)
        {
            try
            {
                var res = _userAuthentication.Login(request);
                return new APIResponse { Data = res, Message = "Đăng nhập thành công" };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }
    }
}
