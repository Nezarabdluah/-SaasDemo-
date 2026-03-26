using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace SaasDemo.MediaLibrary;

/// <summary>
/// Represents a file (image/document) stored in the media library.
/// Metadata is in DB, actual binary is in BlobStore.
/// </summary>
public class MediaFile : FullAuditedAggregateRoot<Guid>
{
    public string FileName { get; private set; }
    public string ContentType { get; private set; }
    public long FileSize { get; private set; }
    public string BlobName { get; private set; }
    public string? FolderPath { get; private set; }
    public string? AltText { get; private set; }
    public int? Width { get; private set; }
    public int? Height { get; private set; }

#pragma warning disable CS8618 // EF Core constructor
    protected MediaFile() { }
#pragma warning restore CS8618

    public MediaFile(
        Guid id,
        string fileName,
        string contentType,
        long fileSize,
        string blobName,
        string? folderPath = null,
        string? altText = null,
        int? width = null,
        int? height = null
    ) : base(id)
    {
        FileName = Check.NotNullOrWhiteSpace(fileName, nameof(fileName));
        ContentType = Check.NotNullOrWhiteSpace(contentType, nameof(contentType));
        FileSize = fileSize;
        BlobName = Check.NotNullOrWhiteSpace(blobName, nameof(blobName));
        FolderPath = folderPath;
        AltText = altText;
        Width = width;
        Height = height;
    }

    public void UpdateMetadata(string? altText, string? folderPath)
    {
        AltText = altText;
        FolderPath = folderPath;
    }
}
