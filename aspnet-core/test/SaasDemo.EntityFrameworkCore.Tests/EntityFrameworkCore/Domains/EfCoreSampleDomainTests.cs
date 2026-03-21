using SaasDemo.Samples;
using Xunit;

namespace SaasDemo.EntityFrameworkCore.Domains;

[Collection(SaasDemoTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<SaasDemoEntityFrameworkCoreTestModule>
{

}
