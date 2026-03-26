using System;
using System.Threading.Tasks;
using SaasDemo.MediaLibrary.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp;

namespace SaasDemo.MediaLibrary;

public interface IMediaFileAppService : IApplicationService
{
    [RemoteService(IsEnabled = false)]
    Task<MediaFileDto> UploadAsync(MediaFileUploadDto input);
    Task<PagedResultDto<MediaFileDto>> GetListAsync(MediaFileGetListInput input);
    Task<MediaFileDto> GetAsync(Guid id);
    Task<MediaFileDto> UpdateMetadataAsync(Guid id, string? altText, string? folderPath);
    Task DeleteAsync(Guid id);
    Task<string> GetDownloadUrlAsync(Guid id);
    Task<byte[]> GetContentAsync(Guid id);
}
