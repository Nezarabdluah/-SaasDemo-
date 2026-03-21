using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace SaasDemo.BlogPosts;

public class BlogCategoryAppServiceTests : SaasDemoApplicationTestBase<SaasDemoApplicationTestModule>
{
    private readonly IBlogCategoryAppService _blogCategoryAppService;

    public BlogCategoryAppServiceTests()
    {
        _blogCategoryAppService = GetRequiredService<IBlogCategoryAppService>();
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

