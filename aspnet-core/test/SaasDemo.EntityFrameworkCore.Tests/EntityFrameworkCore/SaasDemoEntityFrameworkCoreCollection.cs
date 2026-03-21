using Xunit;

namespace SaasDemo.EntityFrameworkCore;

[CollectionDefinition(SaasDemoTestConsts.CollectionDefinitionName)]
public class SaasDemoEntityFrameworkCoreCollection : ICollectionFixture<SaasDemoEntityFrameworkCoreFixture>
{

}
