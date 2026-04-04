using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace SaasDemo.Settings
{
    public class SiteSettings : FullAuditedAggregateRoot<Guid>
    {
        // Basic Info
        public string SiteName { get; private set; }
        public string PrimaryColor { get; private set; }
        public string SecondaryColor { get; private set; }
        public Guid? LogoMediaId { get; private set; }

        // Social Links
        public string? FacebookUrl { get; private set; }
        public string? TwitterUrl { get; private set; }
        public string? InstagramUrl { get; private set; }
        public string? LinkedInUrl { get; private set; }

        // Email Settings
        public string? SmtpHost { get; private set; }
        public int? SmtpPort { get; private set; }
        public string? SmtpUserName { get; private set; }
        public string? SmtpPassword { get; private set; }
        public string? SenderEmail { get; private set; }
        public string? SenderName { get; private set; }

        protected SiteSettings()
        {
            // For ORM
        }

        private SiteSettings(
            Guid id, 
            string siteName, 
            string primaryColor, 
            string secondaryColor, 
            Guid? logoMediaId = null) : base(id)
        {
            SetSiteName(siteName);
            SetPrimaryColor(primaryColor);
            SetSecondaryColor(secondaryColor);
            LogoMediaId = logoMediaId;
        }

        public static SiteSettings Create(
            Guid id, 
            string siteName, 
            string primaryColor, 
            string secondaryColor, 
            Guid? logoMediaId = null)
        {
            return new SiteSettings(id, siteName, primaryColor, secondaryColor, logoMediaId);
        }

        public void UpdateBasicInfo(
            string siteName, 
            string primaryColor, 
            string secondaryColor, 
            Guid? logoMediaId)
        {
            SetSiteName(siteName);
            SetPrimaryColor(primaryColor);
            SetSecondaryColor(secondaryColor);
            LogoMediaId = logoMediaId;
        }

        public void UpdateSocialLinks(
            string? facebookUrl, 
            string? twitterUrl, 
            string? instagramUrl, 
            string? linkedInUrl)
        {
            FacebookUrl = facebookUrl;
            TwitterUrl = twitterUrl;
            InstagramUrl = instagramUrl;
            LinkedInUrl = linkedInUrl;
        }

        public void UpdateEmailSettings(
            string? smtpHost, 
            int? smtpPort, 
            string? smtpUserName, 
            string? smtpPassword, 
            string? senderEmail, 
            string? senderName)
        {
            SmtpHost = smtpHost;
            SmtpPort = smtpPort;
            SmtpUserName = smtpUserName;
            // Only update password if a new one is provided. Overwriting with null means keeping the old one or clearing? 
            // Usually, we handle this explicitly. Let's assume providing it updates it, unless we want a specific clear behavior.
            SmtpPassword = smtpPassword; 
            SenderEmail = senderEmail;
            SenderName = senderName;
        }

        private void SetSiteName(string siteName)
        {
            Check.NotNullOrWhiteSpace(siteName, nameof(siteName), maxLength: 128);
            SiteName = siteName;
        }

        private void SetPrimaryColor(string color)
        {
            Check.NotNullOrWhiteSpace(color, nameof(color));
            PrimaryColor = color;
        }

        private void SetSecondaryColor(string color)
        {
            Check.NotNullOrWhiteSpace(color, nameof(color));
            SecondaryColor = color;
        }
    }
}
