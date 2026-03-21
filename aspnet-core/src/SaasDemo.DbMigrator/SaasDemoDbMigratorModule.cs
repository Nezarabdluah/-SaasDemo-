using SaasDemo.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace SaasDemo.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(SaasDemoEntityFrameworkCoreModule),
    typeof(SaasDemoApplicationContractsModule)
    )]
public class SaasDemoDbMigratorModule : AbpModule
{
}
