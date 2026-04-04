using System;
using Volo.Abp.Domain.Repositories;

namespace SaasDemo.Settings
{
    public interface ISiteSettingsRepository : IRepository<SiteSettings, Guid>
    {
    }
}
