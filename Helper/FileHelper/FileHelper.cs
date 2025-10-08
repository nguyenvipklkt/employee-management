using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Helper.FileHelper.FileHelper;

namespace Helper.FileHelper
{
    public interface IFileHelper
    {
        Task<string> SaveAsync(IFormFile file, string subFolder);
    }

    public class FileHelper : IFileHelper
    {
        public async Task<string> SaveAsync(IFormFile file, string subFolder)
        {
            var fileName = file.FileName;

            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", subFolder);
            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);

            var fullPath = Path.Combine(rootPath, fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream); // IFormFile → dùng CopyToAsync
            }

            return $"/uploads/{subFolder}/{fileName}";
        }
    }
}
