using System;
using Volo.Abp.Application.Dtos;

namespace SaasDemo.Settings.Dtos
{
    public class SiteSettingsDto : AuditedEntityDto<Guid>
    {
        // Basic Info
        public string SiteName { get; set; } = string.Empty;
        public string PrimaryColor { get; set; } = string.Empty;
        public string SecondaryColor { get; set; } = string.Empty;
        public Guid? LogoMediaId { get; set; }

        // Social Links
        public string? FacebookUrl { get; set; }
        public string? TwitterUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string? LinkedInUrl { get; set; }

        // Email Settings
        public string? SmtpHost { get; set; }
        public int? SmtpPort { get; set; }
        public string? SmtpUserName { get; set; }
        // We usually don't send the password to the frontend for security reasons,
        // but we might need a flag to indicate if it's set. 
        // For simplicity, we just won't expose it here.
        public bool HasSmtpPassword { get; set; } 
        public string? SenderEmail { get; set; }
        public string? SenderName { get; set; }
    }
}
