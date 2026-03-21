using Volo.Abp.Modularity;

namespace SaasDemo;

public abstract class SaasDemoApplicationTestBase<TStartupModule> : SaasDemoTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
