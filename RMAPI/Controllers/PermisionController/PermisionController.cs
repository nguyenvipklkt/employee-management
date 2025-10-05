using CoreValidation.Requests.Authentication;
using CoreValidation.Requests.Permision;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Object.Setting;
using RMAPI.Controllers;
using RMAPI.Middleware;
using Service.Service.PermissionService;

[ApiController]
[Route("api/permission")]
[Authorize]
public class PermissionController : BaseApiController<PermissionController>
{
    private readonly IPermissionService _permissionService;

    public PermissionController(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    [HttpPost("grant")]
    [HasPermission("GRANT_PERMISSION")]
    public APIResponse GrantPermission([FromBody] GrantPermissionRequest request)
    {
        try
        {
            var result = _permissionService.GrantPermission(UserId, request);
            return new APIResponse { Data = result };
        }
        catch (Exception ex)
        {
            return NG(ex);
        }
    }

    [HttpPost("revoke")]
    [HasPermission("GRANT_PERMISSION")]
    public APIResponse RevokePermission([FromBody] RevokePermissionRequest request)
    {
        try
        {
            var result = _permissionService.RevokePermission(UserId, request);
            return new APIResponse { Data = result };
        }
        catch (Exception ex)
        {
            return NG(ex);
        }
    }

    [HttpGet("functions/{userId}")]
    [HasPermission("VIEW_USER_PERMISSIONS")]
    public APIResponse GetUserPermissions(int userId)
    {
        try
        {
            var result = _permissionService.GetPermissionsOfUser(userId);
            return new APIResponse { Data = result };
        }
        catch (Exception ex)
        {
            return NG(ex);
        }
    }

    [HttpGet("can-grant")]
    [HasPermission("GRANT_PERMISSION")]
    public APIResponse GetFunctionsICanGrant()
    {
        try
        {
            var result = _permissionService.GetFunctionsUserCanGrant(UserId);
            return new APIResponse { Data = result };
        }
        catch (Exception ex)
        {
            return NG(ex);
        }
    }
}
