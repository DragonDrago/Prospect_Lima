using Lima.Static.Api.Domain;
using Lima.Static.Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace Lima.Static.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class StaticContentController : ControllerBase
    {
#if DEBUG
        private const string SAVE_FILE_PATH = @"C:/static/images";
#endif
#if !DEBUG
        private const string SAVE_FILE_PATH = @"D:/web/storage/images";
#endif

        private StaticFileOptions _staticFileOptions;
        private ILogger _logger;

        public StaticContentController(ILogger logger, IOptions<StaticFileOptions> fileOptions)
        {
            _logger = logger;
            _staticFileOptions = fileOptions.Value;
        }

        [HttpPost("save-drug-photo")]
        public IActionResult SaveDrugPhoto([FromBody] StaticContentRequest request)
        {
            string endpoint = "drugs";

            if (request == null || string.IsNullOrEmpty(request.FileBase64))
                return Ok(new { FileName = "" });

            try
            {
                Image image = PictureUtils.Base64ToImage(request.FileBase64);

                if (image.Size.Width > 640)
                {
                    image = PictureUtils.ResizeImage(image, 640, 480);
                }

                string drugPhotoStoragePath = Path.Combine(SAVE_FILE_PATH, endpoint),
                       fileName = Path.GetRandomFileName().Replace(".", "").Substring(0, 8) + ".jpg",
                       fullPath = Path.Combine(drugPhotoStoragePath, fileName);
                
                if (!Directory.Exists(drugPhotoStoragePath))
                    Directory.CreateDirectory(drugPhotoStoragePath);

                image.Save(fullPath);

                return Ok(new { FileName = fileName });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(StaticContentController));
                return Ok(new { FileName = "" });
            }
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] StaticContentRequest staticContentRequest)
        {
            if (staticContentRequest == null || string.IsNullOrEmpty(staticContentRequest.FileBase64))
                return Ok(new { FileName = "" });

            try
            {
                string fileExtension = GetFileExtension(staticContentRequest.FileBase64);

                if (fileExtension == null)
                    return null;

                string fileName = Path.GetRandomFileName().Replace(".", "").Substring(0, 8) + "." + fileExtension;
                string fullPath = Path.Combine(SAVE_FILE_PATH, fileName);

                if (!Directory.Exists(SAVE_FILE_PATH))
                {
                    Directory.CreateDirectory(SAVE_FILE_PATH);
                }

                System.IO.File.WriteAllBytes(fullPath, Convert.FromBase64String(staticContentRequest.FileBase64));

                return Ok(new { FileName = fileName });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(StaticContentController));
                return Ok(new { FileName = "" });
            }
        }

        public static string GetFileExtension(string base64String)
        {
            var data = base64String.Substring(0, 5);

            switch (data.ToUpper())
            {
                case "IVBOR":
                    return "png";
                case "/9J/4":
                    return "jpg";
                case "AAAAF":
                    return "mp4";
                case "JVBER":
                    return "pdf";
                case "AAABA":
                    return "ico";
                case "UMFYI":
                    return "rar";
                case "E1XYD":
                    return "rtf";
                case "U1PKC":
                    return "txt";
                case "MQOWM":
                case "77U/M":
                    return "srt";
                default:
                    return string.Empty;
            }
        }
    }
}
