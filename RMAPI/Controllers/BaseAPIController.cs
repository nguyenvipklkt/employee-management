using Common.Enum;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace RMAPI.Controllers
{
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BaseApiController<T> : ControllerBase where T : BaseApiController<T>
    {
        public int UserId
        {
            get
            {
                return int.Parse(User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value);
            }
        }

        public string Username
        {
            get
            {
                return this.User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName)?.Value;
            }
        }

        protected APIResponse NG(Exception ex)
        {
            var response = new APIResponse { Code = "Error", Message = ex.Message, Data = null };
            if (ex.GetType().Name == "ValidateError")
            {
                response.Code = "ValidateError";
                return response;
            }
            else if (ex.GetType().Name == "SystemError")
            {
                response.Code = "SystemError";
                return response;
            }
            return response;
        }
    }
}
