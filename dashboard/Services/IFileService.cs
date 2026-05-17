namespace HorofDashboard.Services
{
    public interface IFileService
    {
        Task<UploadResult> UploadAudioAsync(IFormFile file);
    }
}

public interface IFtpService
{
    Task<string> UploadAudioAsync(IFormFile file);
}

public class UploadResult
{
    public string FileName { get; set; }
    public string FileUrl { get; set; }
    public long Size { get; set; }
}