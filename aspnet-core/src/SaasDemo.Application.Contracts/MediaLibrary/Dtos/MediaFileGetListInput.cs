using Volo.Abp.Application.Dtos;

namespace SaasDemo.MediaLibrary.Dtos;

public class MediaFileGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public string? FolderPath { get; set; }
}
