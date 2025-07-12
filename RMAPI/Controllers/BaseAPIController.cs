using System.Security.Claims;
using Common.Enum;
using Helper.NLog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Object.Setting;

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
                var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? null;
                if (string.IsNullOrEmpty(userId))
                {
                    BaseNLog.logger.Error("Không thể lấy thông tin id người dùng");
                    throw new Exception("Lỗi máy chủ");
                }
                return int.Parse(userId);
            }
        }

        public string UserName
        {
            get
            {
                var userName = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? null;
                if (string.IsNullOrEmpty(userName))
                {
                    BaseNLog.logger.Error("Không thể lấy thông tin tên người dùng");
                    throw new Exception("Lỗi máy chủ");
                }    
                return userName;
            }
        }

        protected APIResponse NG(Exception ex)
        {
            var response = new APIResponse { Code = "Error", Message = ex.Message, Data = null };
            if (ex.GetType().Name == "ValidateError")
            {
                response.Code = "ValidateError";
                response.Message = "Failed";
                return response;
            }
            else if (ex.GetType().Name == "SystemError")
            {
                response.Code = "SystemError";
                response.Message = "Failed";
                return response;
            }
            return response;
        }
    }
}
