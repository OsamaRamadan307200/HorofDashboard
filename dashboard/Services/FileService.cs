using System.Text.RegularExpressions;
using FluentFTP;

using HorofDashboard.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace HorofDashboard.Services
{
    public class FileService : IFileService
    {
        private readonly IConfiguration _config;

        public FileService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<UploadResult> UploadAudioAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new Exception("No file uploaded");

            var allowedExtensions = _config.GetSection("FileUpload:AllowedAudioExtensions").Get<string[]>();
            var maxSizeMB = _config.GetValue<int>("FileUpload:MaxFileSizeMB");

            var extension = Path.GetExtension(file.FileName).ToLower();

            // 🔹 Validate extension
            if (!allowedExtensions.Contains(extension))
                throw new Exception("Invalid file type");

            // 🔹 Validate size
            if (file.Length > maxSizeMB * 1024 * 1024)
                throw new Exception("File too large");

            // 🔹 Clean file name
            var cleanName = Regex.Replace(
                Path.GetFileNameWithoutExtension(file.FileName),
                @"[^a-zA-Z0-9_-]",
                ""
            );

            var uniqueName = $"{cleanName}_{Guid.NewGuid()}{extension}";

            // 🔹 Build full path
            var basePath = _config["FileUpload:BasePath"];
            var audioFolder = _config["FileUpload:AudioFolder"];

            var fullFolderPath = Path.Combine(basePath, audioFolder);

            if (!Directory.Exists(fullFolderPath))
                Directory.CreateDirectory(fullFolderPath);

            var fullPath = Path.Combine(fullFolderPath, uniqueName);

            // 🔹 Save file
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return new UploadResult
            {
                FileName = uniqueName,
                FileUrl = $"/uploads/{audioFolder}/{uniqueName}",
                Size = file.Length
            };
        }
    }
}

public class FtpService : IFtpService
{
    private readonly IConfiguration _config;

    public FtpService(IConfiguration config)
    {
        _config = config;
    }

    public async Task<string> UploadAudioAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new Exception("No file uploaded");

        var allowedExtensions = new[] { ".mp3", ".wav", ".ogg" };
        var ext = Path.GetExtension(file.FileName).ToLower();

        if (!allowedExtensions.Contains(ext))
            throw new Exception("Invalid file type");

        if (file.Length > 10 * 1024 * 1024)
            throw new Exception("File too large");

        // 🔹 Clean name
        var cleanName = Regex.Replace(
            Path.GetFileNameWithoutExtension(file.FileName),
            @"[^a-zA-Z0-9_-]",
            ""
        );

        var fileName = $"{cleanName}_{Guid.NewGuid()}{ext}";

        var host = _config["FtpSettings:Host"];
        var user = _config["FtpSettings:Username"];
        var pass = _config["FtpSettings:Password"];
        var basePath = _config["FtpSettings:BasePath"];
        var audioFolder = _config["FtpSettings:AudioFolder"];

        var remotePath = $"{basePath}/{audioFolder}/{fileName}";

        using var client = new FtpClient(host, user, pass);
         client.Connect();

        // 🔹 Ensure directory exists
        client.CreateDirectory($"{basePath}/{audioFolder}", true);

        using var stream = file.OpenReadStream();

        var status =  client.UploadStream(
            stream,
            remotePath,
            FtpRemoteExists.Overwrite,
            true
        );

        if (status != FtpStatus.Success)
            throw new Exception("FTP upload failed");

         client.Disconnect();

        // 🔹 Return URL for website usage
        return $"/uploads/{audioFolder}/{fileName}";
    }
}



