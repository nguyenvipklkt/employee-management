using CoreValidation.Requests.Department;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Object.Setting;
using RMAPI.Middleware;
using Service.Service.DepartmentService;

namespace RMAPI.Controllers.DepartmentController
{
    [ApiController]
    [Route("api/department")]
    [Authorize]
    public class DepartmentController : BaseApiController<DepartmentController>
    {
        private readonly IDepartmentService _departmentService;
        private readonly IBaseQuery _baseQuery;
        
        public DepartmentController(IDepartmentService departmentService, IBaseQuery baseQuery)
        {
            _departmentService = departmentService;
            _baseQuery = baseQuery;
        }

        [HttpGet]
        [Route("get-all-departments")]
        [HasPermission("VIEW_ALL_DEPARTMENTS")]
        public APIResponse GetAllDepartments()
        {
            try
            {
                var res = _departmentService.GetAllDepartments(UserId);
                return new APIResponse { Data = res };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpPost]
        [Route("add-department")]
        [HasPermission("ADD_DEPARTMENT")]
        public APIResponse AddDepartment([FromForm] AddDepartmentRequest request)
        {
            try
            {
                var res = _departmentService.AddDepartment(request,UserId);
                return new APIResponse { Data = res };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpPost]
        [Route("update-department-by-id")]
        [HasPermission("UPDATE_DEPARTMENT")]
        public APIResponse UpdateDepartmentById([FromForm] UpdateDepartmentRequest request)
        {
            try
            {
                var res = _departmentService.UpdateDepartmentById(request, UserId);
                return new APIResponse { Data = res };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpPost]
        [Route("delete-department-by-id")]
        [HasPermission("DELETE_DEPARTMENT")]
        public APIResponse DeleteDepartmentById(int departmentId)
        {
            try
            {
                var res = _departmentService.DeleteDepartmentById(UserId, departmentId);
                return new APIResponse { Data = res };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }
    }
}
