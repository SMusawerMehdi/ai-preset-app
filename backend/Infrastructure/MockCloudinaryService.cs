using AiPresetApp.Api.Application.Interfaces;

namespace AiPresetApp.Api.Infrastructure;

public class MockCloudinaryService : IImageStorageService
{
    public Task<string> UploadImageAsync(Stream fileStream, string fileName)
    {
        var fakeUrl = $"https://res.cloudinary.com/demo/image/upload/v1/{Guid.NewGuid()}/{fileName}";
        return Task.FromResult(fakeUrl);
    }
}
