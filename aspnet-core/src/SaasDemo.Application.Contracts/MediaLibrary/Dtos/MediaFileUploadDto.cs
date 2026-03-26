using System;
using System.ComponentModel.DataAnnotations;

namespace SaasDemo.MediaLibrary.Dtos;

public class MediaFileUploadDto
{
    [Required]
    public string FileName { get; set; } = string.Empty;

    [Required]
    public string ContentType { get; set; } = string.Empty;

    public string? FolderPath { get; set; }
    public string? AltText { get; set; }
    
    [Required]
    public byte[] Content { get; set; } = Array.Empty<byte>();
}
