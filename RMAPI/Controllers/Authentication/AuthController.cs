using CoreValidation.Requests.Authentication;
using CoreValidation.ValidatorFunc;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Object.Setting;
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
        [Route("login")]
        public APIResponse Login(Login request, IValidator<Login> validator)
        {
            try
            {
                ValidatorFunc.ValidateRequest(validator, request);
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
        [Route("register")]
        public APIResponse Register(Register request, IValidator<Register> validator)
        {
            try
            {
                ValidatorFunc.ValidateRequest(validator, request);
                var res = _userAuthentication.Register(request);
                return new APIResponse { Data = res, Message = "Đăng ký thành công. Vui lòng xác minh tài khoản." };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("sendOTP")]
        public async Task<APIResponse> SendEmail(string email)
        {
            try
            {
                var res = await _userAuthentication.SendEmail(email);

                return new APIResponse { Data = res, Message = "Đã gửi thành công mã xác thực về email!" };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("verifyCode")]
        public APIResponse VerifyCode(VerifyCode request, IValidator<VerifyCode> validator)
        {
            try
            {
                ValidatorFunc.ValidateRequest(validator, request);
                var res = _userAuthentication.VerifyCode(request);
                return new APIResponse { Data = res, Message = "Tài khoản xác thực thành công!" };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpPost]
        [Route("refreshToken")]
        public APIResponse RefreshToken([FromBody] RefreshRequestToken request)
        {
            try
            {
                var res = _userAuthentication.RefreshToken(request, UserId);
                return new APIResponse { Data = res, Message = "Tài khoản xác thực thành công!" };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }


    }
}
