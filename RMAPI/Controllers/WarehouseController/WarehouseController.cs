using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Object.Setting;
using OfficeOpenXml;
using RMAPI.Middleware;
using Service.Service.TemplateService;
using Service.Service.UserService;
using static Service.Service.TemplateService.TemplateService;

namespace RMAPI.Controllers.WarehouseController
{
    [ApiController]
    [Route("api/warehouse")]
    [Authorize]
    public class WarehouseController : BaseApiController<WarehouseController>
    {
        private readonly ITemplateService _templateService;
        private readonly IBaseQuery _baseQuery;
        
        public WarehouseController(ITemplateService templateService, IBaseQuery baseQuery)
        {
            _templateService = templateService;
            _baseQuery = baseQuery;
        }

        [HttpGet("get-import-template")]
        [HttpGet("generate-import-template")]
        [Authorize]
        public IActionResult GenerateImportTemplate([FromQuery] int warehouseId)
        {
            try
            {
                var outputFileName = _templateService.GenerateImportTemplate(UserId, warehouseId);
                return Ok(new
                {
                    fileUrl = $"/generated/{outputFileName}"
                });
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
