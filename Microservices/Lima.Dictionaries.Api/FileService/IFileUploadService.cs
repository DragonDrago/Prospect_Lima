using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Lima.Dictionaries.Api.FileService
{
    public interface IFileUploadService
    {
        public string FileUploader(IFormFile file, IWebHostEnvironment environment);
    }
}
