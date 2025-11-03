using Microsoft.AspNetCore.Http;

namespace CoreValidation.Requests.Warehouse
{
    public class ImportExcel
    {
        public IFormFile File { get; set; } = null!;
    }
}
