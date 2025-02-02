using Common.Enum;
using CoreValidation.Requests.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMAPI.Services.Authentication;

namespace RMAPI.Controllers.Authentication
{
    [ApiController]
    [Route("api/authentication")]
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

        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public APIResponse Register(Register request)
        {
            try
            {
                var res = _userAuthentication.Register(request);
                return new APIResponse { Data = res, Message = "Đăng ký thành công. Vui lòng xác minh tài khoản." };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }
    }
}
