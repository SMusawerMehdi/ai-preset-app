using AiPresetApp.Api.Application.DTOs;

namespace AiPresetApp.Api.Application.Interfaces;

public interface IStyleService
{
    Task<StyleMatchResponseDto> MatchStyleAsync(IFormFile referenceImage, IFormFile targetImage);
}
