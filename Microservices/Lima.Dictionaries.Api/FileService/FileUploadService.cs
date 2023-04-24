using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Lima.Dictionaries.Api.FileService
{
    public class FileUploadService:IFileUploadService
    {
        public string FileUploader(IFormFile file, IWebHostEnvironment environment)
        {
            string imageFilePath = string.Empty;
            if (file.Length > 0 && file != null)
            {
                string uploadFolder = Path.Combine(environment.WebRootPath, "Upload");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                imageFilePath = Path.Combine(uploadFolder, uniqueFileName);
                file.CopyTo(new FileStream(imageFilePath, FileMode.Create));
                return imageFilePath;
            }
            return "";
        }
    }
}
