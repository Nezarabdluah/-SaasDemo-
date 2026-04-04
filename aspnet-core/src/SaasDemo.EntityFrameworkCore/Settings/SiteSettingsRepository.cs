using System;
using SaasDemo.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SaasDemo.Settings
{
    public class SiteSettingsRepository : EfCoreRepository<SaasDemoDbContext, SiteSettings, Guid>, ISiteSettingsRepository
    {
        public SiteSettingsRepository(IDbContextProvider<SaasDemoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
