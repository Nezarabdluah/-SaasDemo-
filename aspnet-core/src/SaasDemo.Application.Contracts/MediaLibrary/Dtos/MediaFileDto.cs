using System;

namespace SaasDemo.MediaLibrary.Dtos;

public class MediaFileDto
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string BlobName { get; set; } = string.Empty;
    public string? FolderPath { get; set; }
    public string? AltText { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public DateTime CreationTime { get; set; }
}
