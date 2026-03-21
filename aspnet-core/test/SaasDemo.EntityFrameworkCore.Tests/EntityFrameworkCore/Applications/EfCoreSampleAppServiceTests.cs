using SaasDemo.Samples;
using Xunit;

namespace SaasDemo.EntityFrameworkCore.Applications;

[Collection(SaasDemoTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<SaasDemoEntityFrameworkCoreTestModule>
{

}
