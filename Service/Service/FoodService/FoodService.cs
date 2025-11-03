using AutoMapper;
using CoreValidation.Requests.Food;
using Helper.FileHelper;
using Helper.NLog;
using Infrastructure.Repositories;
using Object.Model;

namespace Service.Service.FoodService
{
    public interface IFoodService
    {
        Task<bool> AddFood(int userId, AddFoodRequest request);
        Task<bool> UpdateFood(int userId, UpdateFoodRequest request);
        bool DeleteFood(int userId, int foodId);
    }
    public class FoodService: IFoodService
    {
        private readonly IBaseCommand<Food> _baseFoodCommand;
        private readonly IMapper _mapper;
        private readonly IFileHelper _fileHelper;
        public FoodService(IBaseCommand<Food> baseFoodCommand, IMapper mapper, IFileHelper fileHelper)
        {
            _baseFoodCommand = baseFoodCommand;
            _mapper = mapper;
            _fileHelper = fileHelper;
        }
        public async Task<bool> AddFood(int userId, AddFoodRequest request)
        {
            try
            {
                var newFood = new Food();
                _mapper.Map(request, newFood);
                newFood.CreateBy = userId;
                if (request.Image != null)
                {
                    var relativePath = await _fileHelper.SaveAsync(request.Image, "foods");

                    // Ghi đường dẫn vào DB
                    newFood.ImageUrl = relativePath;
                }

                var createdRole = _baseFoodCommand.Create(newFood);
                return true;
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }
        }
        public async Task<bool> UpdateFood (int userId, UpdateFoodRequest request)
        {
            try
            {
                var existingFood = _baseFoodCommand.FindOrFail(request.FoodId);
                _mapper.Map(request, existingFood);
                existingFood.UpdateBy = userId;
                existingFood.UpdateAt = DateTime.Now;
                if (request.Image != null)
                {
                    var relativePath = await _fileHelper.SaveAsync(request.Image, "foods");

                    // Ghi đường dẫn vào DB
                    existingFood.ImageUrl = relativePath;
                }
                var updatedFood = _baseFoodCommand.UpdateByEntity(existingFood);
                return true;
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }
        }

        public bool DeleteFood(int userId, int foodId)
        {
            try
            {
                var existingFood = _baseFoodCommand.FindOrFail(foodId);
                existingFood.UpdateAt = DateTime.Now;
                existingFood.UpdateBy = userId;
                existingFood.IsDeleted = true;
                var deletedFood = _baseFoodCommand.UpdateByEntity(existingFood);
                return true;
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }
        }
    }
}
