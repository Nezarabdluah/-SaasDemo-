using System;
using System.Threading.Tasks;
using SaasDemo.BlogPosts;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace SaasDemo.EntityFrameworkCore.BlogPosts;

public class BlogCategoryRepositoryTests : SaasDemoEntityFrameworkCoreTestBase
{
    private readonly IBlogCategoryRepository _blogCategoryRepository;

    public BlogCategoryRepositoryTests()
    {
        _blogCategoryRepository = GetRequiredService<IBlogCategoryRepository>();
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
