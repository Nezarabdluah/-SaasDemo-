using Microsoft.AspNetCore.Http;

namespace SaasDemo.Controllers;

/// <summary>
/// Form model for file uploads via MediaController.
/// Swashbuckle requires a single model class for [FromForm] with IFormFile.
/// </summary>
public class MediaUploadForm
{
    public IFormFile Content { get; set; } = default!;
    public string? FolderPath { get; set; }
    public string? AltText { get; set; }
}
