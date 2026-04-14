using AiPresetApp.Api.Application.DTOs;
using AiPresetApp.Api.Application.Interfaces;

namespace AiPresetApp.Api.Application.Services;

public class StyleService : IStyleService
{
    private readonly IImageStorageService _imageStorage;
    private readonly IProcessingService _processingService;

    public StyleService(IImageStorageService imageStorage, IProcessingService processingService)
    {
        _imageStorage = imageStorage;
        _processingService = processingService;
    }

    public async Task<StyleMatchResponseDto> MatchStyleAsync(IFormFile referenceImage, IFormFile targetImage)
    {
        // Upload both images
        using var refStream = referenceImage.OpenReadStream();
        var referenceUrl = await _imageStorage.UploadImageAsync(refStream, referenceImage.FileName);

        using var targetStream = targetImage.OpenReadStream();
        var targetUrl = await _imageStorage.UploadImageAsync(targetStream, targetImage.FileName);

        // Process style match
        var result = await _processingService.ProcessStyleMatchAsync(referenceUrl, targetUrl);

        return new StyleMatchResponseDto
        {
            ProcessedImageUrl = result.PreviewUrl,
            PresetJson = new
            {
                parameters = result.Parameters,
                referenceImageUrl = referenceUrl
            }
        };
    }
}
