using Microsoft.AspNetCore.Http;

namespace AiPresetApp.Api.Application.DTOs;

public class StyleMatchRequestDto
{
    public required IFormFile ReferenceImage { get; set; }
    public required IFormFile TargetImage { get; set; }
}
