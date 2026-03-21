using Volo.Abp.Modularity;

namespace SaasDemo;

[DependsOn(
    typeof(SaasDemoApplicationModule),
    typeof(SaasDemoDomainTestModule)
)]
public class SaasDemoApplicationTestModule : AbpModule
{

}
