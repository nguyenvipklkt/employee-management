using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Object.Setting;
using System.Security.Claims;

namespace NVAPI.Middleware
{
    public class HasPermissionAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string _permission;

        public HasPermissionAttribute(string permission)
        {
            _permission = permission;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var httpContext = context.HttpContext;
            var user = httpContext.User;

            if (user == null || user.Identity == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var baseQuery = httpContext.RequestServices.GetService(typeof(IBaseQuery)) as IBaseQuery;
            if (baseQuery == null)
            {
                context.Result = new StatusCodeResult(500);
                return;
            }

            var hasPermission = await AuthorizationUser.HasPermissionAsync(userId, _permission, baseQuery);
            if (!hasPermission)
            {
                context.Result = new ObjectResult(new APIResponse
                {
                    Code = "-1",
                    Message = "Không có quyền truy cập",
                    Data = null
                })
                {
                    StatusCode = 403
                };
            }
        }
    }
}
