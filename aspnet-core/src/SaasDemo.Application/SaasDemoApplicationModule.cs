using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using Volo.Blogging;
using Volo.Blogging.Admin;
using Volo.CmsKit;

namespace SaasDemo;

[DependsOn(
    typeof(SaasDemoDomainModule),
    typeof(AbpAccountApplicationModule),
    typeof(SaasDemoApplicationContractsModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
[DependsOn(typeof(BloggingApplicationModule))]
    [DependsOn(typeof(BloggingAdminApplicationModule))]
    [DependsOn(typeof(CmsKitApplicationModule))]
    public class SaasDemoApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<SaasDemoApplicationModule>(validate: false);
        });
    }
}
