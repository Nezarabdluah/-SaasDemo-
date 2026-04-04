using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SaasDemo.Settings.Dtos;
using Volo.Abp.Domain.Repositories;

namespace SaasDemo.Settings
{
    // TODO: Add strict permission requirement here when integrating permissions
    // [Authorize(SaasDemoPermissions.SiteSettings.Manage)]
    [Authorize] 
    public class SiteSettingsAppService : SaasDemoAppService, ISiteSettingsAppService
    {
        private readonly ISiteSettingsRepository _siteSettingsRepository;

        public SiteSettingsAppService(ISiteSettingsRepository siteSettingsRepository)
        {
            _siteSettingsRepository = siteSettingsRepository;
        }

        public async Task<SiteSettingsDto> GetAsync()
        {
            var settings = await _siteSettingsRepository.FirstOrDefaultAsync();

            if (settings == null)
            {
                // Return defaults if not configured
                return new SiteSettingsDto
                {
                    SiteName = "My Application",
                    PrimaryColor = "#007bff",
                    SecondaryColor = "#6c757d"
                };
            }
            
            var dto = ObjectMapper.Map<SiteSettings, SiteSettingsDto>(settings);
            // Flag to indicate if SMTP password is set without revealing the actual password
            dto.HasSmtpPassword = !string.IsNullOrEmpty(settings.SmtpPassword);
            return dto;
        }

        public async Task<SiteSettingsDto> UpdateAsync(UpdateSiteSettingsDto input)
        {
            var settings = await _siteSettingsRepository.FirstOrDefaultAsync();

            if (settings == null)
            {
                settings = SiteSettings.Create(
                    GuidGenerator.Create(),
                    input.SiteName,
                    input.PrimaryColor,
                    input.SecondaryColor,
                    input.LogoMediaId
                );
                
                settings.UpdateSocialLinks(input.FacebookUrl, input.TwitterUrl, input.InstagramUrl, input.LinkedInUrl);
                settings.UpdateEmailSettings(input.SmtpHost, input.SmtpPort, input.SmtpUserName, input.SmtpPassword, input.SenderEmail, input.SenderName);

                await _siteSettingsRepository.InsertAsync(settings);
            }
            else
            {
                settings.UpdateBasicInfo(input.SiteName, input.PrimaryColor, input.SecondaryColor, input.LogoMediaId);
                settings.UpdateSocialLinks(input.FacebookUrl, input.TwitterUrl, input.InstagramUrl, input.LinkedInUrl);
                
                // For password: if it's explicitly passed as empty or null, we might either clear it or ignore it.
                // Given the flow, usually we only update it if provided. But if user wants to clear it, they can't.
                // Let's assume if input.SmtpPassword is not null/empty, we update. Otherwise, we keep existing.
                var passToSave = string.IsNullOrEmpty(input.SmtpPassword) ? settings.SmtpPassword : input.SmtpPassword;
                
                settings.UpdateEmailSettings(input.SmtpHost, input.SmtpPort, input.SmtpUserName, passToSave, input.SenderEmail, input.SenderName);

                await _siteSettingsRepository.UpdateAsync(settings);
            }

            var resultDto = ObjectMapper.Map<SiteSettings, SiteSettingsDto>(settings);
            resultDto.HasSmtpPassword = !string.IsNullOrEmpty(settings.SmtpPassword);
            return resultDto;
        }
    }
}
