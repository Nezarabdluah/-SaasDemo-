using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace SaasDemo.BlogPosts;

public class BlogPostAppServiceTests : SaasDemoApplicationTestBase<SaasDemoApplicationTestModule>
{
    private readonly IBlogPostAppService _blogPostAppService;

    public BlogPostAppServiceTests()
    {
        _blogPostAppService = GetRequiredService<IBlogPostAppService>();
    }

    /*
    [Fact]
    public async Task Test1()
    {
        // Arrange

        // Act

        // Assert
    }
    */
}

