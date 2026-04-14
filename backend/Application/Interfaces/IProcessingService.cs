using AiPresetApp.Api.Application.DTOs;

namespace AiPresetApp.Api.Application.Interfaces;

public interface IProcessingService
{
    Task<ProcessingResultDto> ProcessStyleMatchAsync(string referenceUrl, string targetUrl);
}
