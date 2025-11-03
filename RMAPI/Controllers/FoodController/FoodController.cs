using CoreValidation.Requests.Food;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Object.Model;
using Object.Setting;
using RMAPI.Middleware;
using Service.Service.FoodService;

namespace RMAPI.Controllers.FoodController
{
    [ApiController]
    [Route("api/food")]
    [Authorize]
    public class FoodController : BaseApiController<FoodController>
    {
        private readonly IFoodService _foodService;
        private readonly IBaseQuery _baseQuery;
        
        public FoodController(IFoodService foodService, IBaseQuery baseQuery)
        {
            _foodService = foodService;
            _baseQuery = baseQuery;
        }

        [HttpPost]
        [Route("add-food")]
        [HasPermission("")]
        public APIResponse AddFood([FromForm] AddFoodRequest request)
        {
            try
            {
                var res = _foodService.AddFood(UserId, request);
                return new APIResponse { Data = res };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpPost]
        [Route("update-food-by-id")]
        [HasPermission("")]
        public APIResponse UpdateFoodById([FromForm] UpdateFoodRequest request)
        {
            try
            {
                var res = _foodService.UpdateFood(UserId, request);
                return new APIResponse { Data = res };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpPost]
        [Route("delete-food-by-id")]
        [HasPermission("")]
        public APIResponse DeleteFoodById(int foodId)
        {
            try
            {
                var res = _foodService.DeleteFood(UserId, foodId);
                return new APIResponse { Data = res };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }
    }
}
