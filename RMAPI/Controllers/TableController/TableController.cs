using CoreValidation.Requests.Table;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Object.Setting;
using RMAPI.Middleware;
using Service.Service.TableService;

namespace RMAPI.Controllers.TableController
{
    [ApiController]
    [Route("api/table")]
    [Authorize]
    public class TableController : BaseApiController<TableController>
    {
        private readonly ITableService _tableService;
        private readonly IBaseQuery _baseQuery;

        public TableController(ITableService tableService, IBaseQuery baseQuery)
        {
            _tableService = tableService;
            _baseQuery = baseQuery;
        }

        [HttpPost]
        [Route("add-table")]
        [HasPermission("")]
        public APIResponse AddTable(AddTableRequest request)
        {
            try
            {
                var res = _tableService.AddTable(UserId, request);
                return new APIResponse { Data = res };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpPost]
        [Route("update-table-by-id")]
        [HasPermission("")]
        public APIResponse UpdateTableById(UpdateTableRequest request)
        {
            try
            {
                var res = _tableService.UpdateTable(UserId, request);
                return new APIResponse { Data = res };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpPost]
        [Route("delete-table-by-id")]
        [HasPermission("")]
        public APIResponse DeleteTableById(int tableId)
        {
            try
            {
                var res = _tableService.DeleteTable(UserId, tableId);
                return new APIResponse { Data = res };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }
    }
}
