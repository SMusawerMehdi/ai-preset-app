namespace AiPresetApp.Api.Application.DTOs;

public class StyleMatchResponseDto
{
    public required string ProcessedImageUrl { get; set; }
    public required object PresetJson { get; set; }
}
