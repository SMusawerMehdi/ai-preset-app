using AiPresetApp.Api.Application.Interfaces;
using AiPresetApp.Api.Application.Services;
using AiPresetApp.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddScoped<IStyleService, StyleService>();
builder.Services.AddScoped<IImageStorageService, MockCloudinaryService>();
builder.Services.AddScoped<IProcessingService, MockPythonProcessingService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins(builder.Configuration.GetValue<string>("FrontendUrl") ?? "http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("Frontend");
app.MapControllers();

// Health check
app.MapGet("/api/health", () => Results.Ok(new { status = "healthy" }));

app.Run();
