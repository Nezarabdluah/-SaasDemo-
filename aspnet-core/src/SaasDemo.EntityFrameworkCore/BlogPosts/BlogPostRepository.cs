using System;
using System.Linq;
using System.Threading.Tasks;
using SaasDemo.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SaasDemo.BlogPosts;

public class BlogPostRepository : EfCoreRepository<SaasDemoDbContext, BlogPost, Guid>, IBlogPostRepository
{
    public BlogPostRepository(IDbContextProvider<SaasDemoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public override async Task<IQueryable<BlogPost>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }
}