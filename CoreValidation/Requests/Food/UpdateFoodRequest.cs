using Microsoft.AspNetCore.Http;

namespace CoreValidation.Requests.Food
{
    public class UpdateFoodRequest
    {
        public int FoodId { get; set; } // ID món ăn
        public string Name { get; set; } = string.Empty; // Tên món ăn
        public string? Description { get; set; } // Mô tả món ăn
        public long Price { get; set; } // Giá món ăn
        public string Unit { get; set; } = string.Empty; // Đơn vị tính
        public IFormFile? Image { get; set; } // Ảnh món ăn
    }
}
