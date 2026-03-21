using Volo.Abp.Modularity;

namespace SaasDemo;

/* Inherit from this class for your domain layer tests. */
public abstract class SaasDemoDomainTestBase<TStartupModule> : SaasDemoTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
