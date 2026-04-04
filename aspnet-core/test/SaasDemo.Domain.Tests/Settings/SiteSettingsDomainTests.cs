using System;
using SaasDemo.Settings;
using Shouldly;
using Xunit;

namespace SaasDemo.Domain.Tests.Settings
{
    public class SiteSettingsDomainTests
    {
        [Fact]
        public void Create_SiteSettings_Should_Set_Properties()
        {
            // Arrange
            var id = Guid.NewGuid();
            var siteName = "Test Site";
            var primaryColor = "#FFFFFF";
            var secondaryColor = "#000000";

            // Act
            var settings = SiteSettings.Create(id, siteName, primaryColor, secondaryColor);

            // Assert
            settings.Id.ShouldBe(id);
            settings.SiteName.ShouldBe(siteName);
            settings.PrimaryColor.ShouldBe(primaryColor);
            settings.SecondaryColor.ShouldBe(secondaryColor);
        }

        [Fact]
        public void Create_SiteSettings_With_Empty_SiteName_Should_Throw_Exception()
        {
            // Arrange
            var id = Guid.NewGuid();
            var primaryColor = "#FFFFFF";
            var secondaryColor = "#000000";

            // Act & Assert
            Should.Throw<ArgumentException>(() => 
            {
                SiteSettings.Create(id, "", primaryColor, secondaryColor);
            });
        }

        [Fact]
        public void UpdateEmailSettings_Should_Set_Fields_Properly()
        {
             // Arrange
            var settings = SiteSettings.Create(Guid.NewGuid(), "Site", "#111", "#222");

            // Act
            settings.UpdateEmailSettings("smtp.test", 587, "user", "pass", "no-reply@test", "Support");

            // Assert
            settings.SmtpHost.ShouldBe("smtp.test");
            settings.SmtpPort.ShouldBe(587);
            settings.SmtpUserName.ShouldBe("user");
            settings.SmtpPassword.ShouldBe("pass");
            settings.SenderEmail.ShouldBe("no-reply@test");
            settings.SenderName.ShouldBe("Support");
        }
    }
}
