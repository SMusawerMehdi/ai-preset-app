namespace AiPresetApp.Api.Application.Interfaces;

public interface IImageStorageService
{
    Task<string> UploadImageAsync(Stream fileStream, string fileName);
}
