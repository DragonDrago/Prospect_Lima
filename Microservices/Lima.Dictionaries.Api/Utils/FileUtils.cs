using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Lima.Dictionaries.Api.Utils
{
    public static class FileUtils
    {
        public static string SaveImageFile(string base64File, string savePath, ILogger logger)
        {
            if (base64File == null)
                return null;

            try
            {
                string fileExtension = GetFileExtension(base64File);

                if (fileExtension == null)
                    return null;

                string fileName = Path.GetRandomFileName().Replace(".", "").Substring(0, 8) + "." + fileExtension;
                string fullPath = Path.Combine(savePath, fileName);

                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                File.WriteAllBytes(fullPath, Convert.FromBase64String(base64File));

                return fileName;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, nameof(FileUtils));
                return null;
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
