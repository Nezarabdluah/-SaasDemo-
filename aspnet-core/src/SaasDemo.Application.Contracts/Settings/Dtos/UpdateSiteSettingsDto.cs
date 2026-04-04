using System;
using System.ComponentModel.DataAnnotations;

namespace SaasDemo.Settings.Dtos
{
    public class UpdateSiteSettingsDto
    {
        // Basic Info
        [Required]
        [MaxLength(128)]
        public string SiteName { get; set; } = string.Empty;

        [Required]
        [MaxLength(32)]
        public string PrimaryColor { get; set; } = string.Empty;

        [Required]
        [MaxLength(32)]
        public string SecondaryColor { get; set; } = string.Empty;

        public Guid? LogoMediaId { get; set; }

        // Social Links
        [MaxLength(256)]
        public string? FacebookUrl { get; set; }
        
        [MaxLength(256)]
        public string? TwitterUrl { get; set; }
        
        [MaxLength(256)]
        public string? InstagramUrl { get; set; }
        
        [MaxLength(256)]
        public string? LinkedInUrl { get; set; }

        // Email Settings
        [MaxLength(256)]
        public string? SmtpHost { get; set; }

        public int? SmtpPort { get; set; }

        [MaxLength(256)]
        public string? SmtpUserName { get; set; }

        [MaxLength(256)]
        public string? SmtpPassword { get; set; }

        [MaxLength(256)]
        public string? SenderEmail { get; set; }

        [MaxLength(128)]
        public string? SenderName { get; set; }
    }
}
