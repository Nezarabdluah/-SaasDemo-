using Volo.Abp.Account;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using Volo.Blogging;
using Volo.Blogging.Admin;
using Volo.CmsKit;

namespace SaasDemo;

[DependsOn(
    typeof(SaasDemoDomainSharedModule),
    typeof(AbpAccountApplicationContractsModule),
    typeof(AbpFeatureManagementApplicationContractsModule),
    typeof(AbpIdentityApplicationContractsModule),
    typeof(AbpPermissionManagementApplicationContractsModule),
    typeof(AbpSettingManagementApplicationContractsModule),
    typeof(AbpTenantManagementApplicationContractsModule),
    typeof(AbpObjectExtendingModule)
)]
[DependsOn(typeof(BloggingApplicationContractsModule))]
    [DependsOn(typeof(BloggingAdminApplicationContractsModule))]
    [DependsOn(typeof(CmsKitApplicationContractsModule))]
    public class SaasDemoApplicationContractsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        SaasDemoDtoExtensions.Configure();
    }
}
