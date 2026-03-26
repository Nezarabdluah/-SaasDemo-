using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SaasDemo.MediaLibrary.Dtos;
using SaasDemo.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;

namespace SaasDemo.MediaLibrary;

[Authorize]
public class MediaFileAppService : ApplicationService, IMediaFileAppService
{
    private readonly IRepository<MediaFile, Guid> _repository;
    private readonly IBlobContainer _blobContainer;

    public MediaFileAppService(
        IRepository<MediaFile, Guid> repository,
        IBlobContainer blobContainer)
    {
        _repository = repository;
        _blobContainer = blobContainer;
    }

    public async Task<MediaFileDto> UploadAsync(MediaFileUploadDto input)
    {
        // Generate a unique blob name to avoid overwrites
        var blobName = $"{Guid.NewGuid():N}{Path.GetExtension(input.FileName)}";

        // Save to Blob Storage (FileSystem)
        await _blobContainer.SaveAsync(blobName, input.Content, overrideExisting: true);

        // Optional: Simple width/height detection could go here using ImageSharp,
        // but for now we just save the file and its metadata.
        
        var mediaFile = new MediaFile(
            GuidGenerator.Create(),
            input.FileName,
            input.ContentType,
            input.Content.Length,
            blobName,
            input.FolderPath,
            input.AltText
        );

        await _repository.InsertAsync(mediaFile);

        return MapToDto(mediaFile);
    }

    [AllowAnonymous]
    public async Task<PagedResultDto<MediaFileDto>> GetListAsync(MediaFileGetListInput input)
    {
        var query = await _repository.GetQueryableAsync();

        if (!string.IsNullOrWhiteSpace(input.Filter))
        {
            query = query.Where(x => x.FileName.Contains(input.Filter) || (x.AltText != null && x.AltText.Contains(input.Filter)));
        }

        if (!string.IsNullOrWhiteSpace(input.FolderPath))
        {
            query = query.Where(x => x.FolderPath == input.FolderPath);
        }

        var totalCount = await AsyncExecuter.CountAsync(query);
        
        var entities = await AsyncExecuter.ToListAsync(
            query.OrderByDescending(x => x.CreationTime)
                 .Skip(input.SkipCount)
                 .Take(input.MaxResultCount)
        );

        return new PagedResultDto<MediaFileDto>(
            totalCount,
            entities.Select(MapToDto).ToList()
        );
    }

    [AllowAnonymous]
    public async Task<MediaFileDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        return MapToDto(entity);
    }

    public async Task<MediaFileDto> UpdateMetadataAsync(Guid id, string? altText, string? folderPath)
    {
        var entity = await _repository.GetAsync(id);
        
        entity.UpdateMetadata(altText, folderPath);
        
        await _repository.UpdateAsync(entity);
        return MapToDto(entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);

        // Delete from Blob Storage
        await _blobContainer.DeleteAsync(entity.BlobName);

        // Delete from Database
        await _repository.DeleteAsync(entity);
    }

    /// <summary>
    /// For downloading the file via a Controller.
    /// This returns the API path or a direct download path.
    /// In local dev, it might be a custom controller route like /api/app/media/{id}/content
    /// </summary>
    [AllowAnonymous]
    public Task<string> GetDownloadUrlAsync(Guid id)
    {
        // Local relative URL handled by a custom MediaController we'll build next
        return Task.FromResult($"/api/app/media/{id}/content");
    }

    /// <summary>
    /// Returns raw bytes of the blob. Best used by a Controller to answer HTTP GET requests.
    /// </summary>
    [AllowAnonymous]
    public async Task<byte[]> GetContentAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var bytes = await _blobContainer.GetAllBytesOrNullAsync(entity.BlobName);
        
        if (bytes == null)
        {
            throw new Volo.Abp.UserFriendlyException("File not found in storage.");
        }
        
        return bytes;
    }

    private static MediaFileDto MapToDto(MediaFile entity)
    {
        return new MediaFileDto
        {
            Id = entity.Id,
            FileName = entity.FileName,
            ContentType = entity.ContentType,
            FileSize = entity.FileSize,
            BlobName = entity.BlobName,
            FolderPath = entity.FolderPath,
            AltText = entity.AltText,
            Width = entity.Width,
            Height = entity.Height,
            CreationTime = entity.CreationTime
        };
    }
}
