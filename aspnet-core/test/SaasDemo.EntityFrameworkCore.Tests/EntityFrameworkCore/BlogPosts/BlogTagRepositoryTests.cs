using System;
using System.Threading.Tasks;
using SaasDemo.BlogPosts;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace SaasDemo.EntityFrameworkCore.BlogPosts;

public class BlogTagRepositoryTests : SaasDemoEntityFrameworkCoreTestBase
{
    private readonly IBlogTagRepository _blogTagRepository;

    public BlogTagRepositoryTests()
    {
        _blogTagRepository = GetRequiredService<IBlogTagRepository>();
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
