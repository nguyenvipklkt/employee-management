using CoreValidation.Requests.Warehouse;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Object.Setting;
using RMAPI.Middleware;
using Service.Service.TemplateService;
using Service.Service.WarehouseService;

namespace RMAPI.Controllers.WarehouseController
{
    [ApiController]
    [Route("api/warehouse")]
    [Authorize]
    public class WarehouseController : BaseApiController<WarehouseController>
    {
        private readonly ITemplateService _templateService;
        private readonly IWarehouseService _warehouseService;
        private readonly IBaseQuery _baseQuery;

        public WarehouseController(ITemplateService templateService, IWarehouseService warehouseService, IBaseQuery baseQuery)
        {
            _templateService = templateService;
            _warehouseService = warehouseService;
            _baseQuery = baseQuery;
        }

        [HttpGet]
        [Route("get-import-template")]
        [HasPermission("")]
        public IActionResult GenerateImportTemplate([FromQuery] int warehouseId)
        {
            try
            {
                var outputFileName = _templateService.GenerateImportTemplate(UserId, warehouseId);
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "generated", outputFileName);
                var fileBytes = System.IO.File.ReadAllBytes(fullPath);
                return File(fileBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    outputFileName);
            }
            catch (Exception)
            {

                throw;
            }
        }

        //[HttpPost]
        //[Route("import-template-to-warehouse")]
        //[HasPermission("")]
        //public APIResponse ImportTemplateToWarehouse()
        //{
        //    try
        //    {
        //        var res = _templateService
        //    }
        //    catch (Exception ex)
        //    {
        //        return NG(ex);
        //    }
        //}

        [HttpPost]
        [Route("add-warehouse")]
        [HasPermission("")]
        public APIResponse AddWarehouse(AddWarehouseRequest request)
        {
            try
            {
                var res = _warehouseService.AddWarehouse(UserId, request.WarehouseName, request.DepartmentId);
                return new APIResponse { Data = res };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }
    }
}
