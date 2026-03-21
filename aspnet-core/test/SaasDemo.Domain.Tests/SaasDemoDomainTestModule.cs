using Volo.Abp.Modularity;

namespace SaasDemo;

[DependsOn(
    typeof(SaasDemoDomainModule),
    typeof(SaasDemoTestBaseModule)
)]
public class SaasDemoDomainTestModule : AbpModule
{

}
