using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaasDemo.MediaLibrary;
using Volo.Abp.AspNetCore.Mvc;

namespace SaasDemo.Controllers;

[Route("api/app/media")]
public class MediaController : AbpControllerBase
{
    private readonly IMediaFileAppService _mediaFileAppService;

    public MediaController(IMediaFileAppService mediaFileAppService)
    {
        _mediaFileAppService = mediaFileAppService;
    }

    /// <summary>
    /// Returns the raw binary content of a media file (e.g., for an img src).
    /// </summary>
    [HttpGet("{id}/content")]
    [AllowAnonymous]
    [ResponseCache(Duration = 31536000, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> GetContentAsync(Guid id)
    {
        var fileDto = await _mediaFileAppService.GetAsync(id);
        var bytes = await _mediaFileAppService.GetContentAsync(id);

        return File(bytes, fileDto.ContentType, fileDto.FileName);
    }

    /// <summary>
    /// Handles multipart/form-data uploads from Angular.
    /// Uses a model class (MediaUploadForm) for Swashbuckle compatibility.
    /// </summary>
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    [Authorize]
    public async Task<SaasDemo.MediaLibrary.Dtos.MediaFileDto> UploadAsync([FromForm] MediaUploadForm form)
    {
        if (form.Content == null || form.Content.Length == 0)
        {
            throw new Volo.Abp.UserFriendlyException("File is empty.");
        }

        using var memoryStream = new System.IO.MemoryStream();
        await form.Content.CopyToAsync(memoryStream);

        var dto = new SaasDemo.MediaLibrary.Dtos.MediaFileUploadDto
        {
            FileName = form.Content.FileName,
            ContentType = form.Content.ContentType,
            FolderPath = form.FolderPath,
            AltText = form.AltText,
            Content = memoryStream.ToArray()
        };

        return await _mediaFileAppService.UploadAsync(dto);
    }
}
