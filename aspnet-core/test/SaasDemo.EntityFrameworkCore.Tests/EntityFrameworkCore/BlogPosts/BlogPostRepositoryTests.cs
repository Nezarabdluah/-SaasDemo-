using System;
using System.Threading.Tasks;
using SaasDemo.BlogPosts;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace SaasDemo.EntityFrameworkCore.BlogPosts;

public class BlogPostRepositoryTests : SaasDemoEntityFrameworkCoreTestBase
{
    private readonly IBlogPostRepository _blogPostRepository;

    public BlogPostRepositoryTests()
    {
        _blogPostRepository = GetRequiredService<IBlogPostRepository>();
    }

    /*
    [Fact]
    public async Task Test1()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            // Arrange

            // Act

            //Assert
        });
    }
    */
}
