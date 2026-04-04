using System.Threading.Tasks;
using SaasDemo.Settings.Dtos;
using Volo.Abp.Application.Services;

namespace SaasDemo.Settings
{
    public interface ISiteSettingsAppService : IApplicationService
    {
        Task<SiteSettingsDto> GetAsync();
        Task<SiteSettingsDto> UpdateAsync(UpdateSiteSettingsDto input);
    }
}
