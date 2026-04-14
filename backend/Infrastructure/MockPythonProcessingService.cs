using AiPresetApp.Api.Application.DTOs;
using AiPresetApp.Api.Application.Interfaces;

namespace AiPresetApp.Api.Infrastructure;

public class MockPythonProcessingService : IProcessingService
{
    public Task<ProcessingResultDto> ProcessStyleMatchAsync(string referenceUrl, string targetUrl)
    {
        var result = new ProcessingResultDto
        {
            PreviewUrl = $"https://res.cloudinary.com/demo/image/upload/v1/preview_{Guid.NewGuid()}.jpg",
            Parameters = new
            {
                brightness = 12,
                contrast = 8,
                saturation = 15,
                temperature = 18,
                tint = 4,
                highlights = -10,
                shadows = 20,
                whites = 5,
                blacks = -8,
                vibrance = 10
            }
        };

        return Task.FromResult(result);
    }
}
