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
        Task<string> SaveAsync(Stream fileStream, string fileName, string subFolder);
    }

    public class FileHelper : IFileHelper
    {
        public async Task<string> SaveAsync(Stream fileStream, string fileName, string subFolder)
        {
            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", subFolder);
            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);

            var fullPath = Path.Combine(rootPath, fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await fileStream.CopyToAsync(stream);
            }

            return $"/uploads/{subFolder}/{fileName}";
        }
    }
}
