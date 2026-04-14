using AiPresetApp.Api.Application.DTOs;
using AiPresetApp.Api.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AiPresetApp.Api.Controllers;

[ApiController]
[Route("api/style")]
public class StyleController : ControllerBase
{
    private readonly IStyleService _styleService;
    private static readonly string[] AllowedTypes = ["image/jpeg", "image/png", "image/webp"];
    private const long MaxFileSize = 10 * 1024 * 1024; // 10 MB

    public StyleController(IStyleService styleService)
    {
        _styleService = styleService;
    }

    [HttpPost("match")]
    public async Task<IActionResult> Match([FromForm] StyleMatchRequestDto request)
    {
        // Validate reference image
        var refError = ValidateFile(request.ReferenceImage, "referenceImage");
        if (refError is not null) return refError;

        // Validate target image
        var targetError = ValidateFile(request.TargetImage, "targetImage");
        if (targetError is not null) return targetError;

        try
        {
            var result = await _styleService.MatchStyleAsync(request.ReferenceImage, request.TargetImage);
            return Ok(result);
        }
        catch (HttpRequestException)
        {
            return StatusCode(503, new { error = "Processing service is unavailable. Please try again later." });
        }
        catch (Exception ex)
        {
            return StatusCode(503, new { error = $"An unexpected error occurred: {ex.Message}" });
        }
    }

    private BadRequestObjectResult? ValidateFile(IFormFile file, string fieldName)
    {
        if (file.Length == 0)
            return BadRequest(new { error = $"{fieldName} is empty." });

        if (file.Length > MaxFileSize)
            return BadRequest(new { error = $"{fieldName} exceeds the 10 MB size limit." });

        if (!AllowedTypes.Contains(file.ContentType.ToLower()))
            return BadRequest(new { error = $"{fieldName} must be JPEG, PNG, or WebP." });

        return null;
    }
}
