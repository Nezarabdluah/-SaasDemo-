using System;
using System.Linq;
using System.Threading.Tasks;
using SaasDemo.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SaasDemo.BlogPosts;

public class BlogTagRepository : EfCoreRepository<SaasDemoDbContext, BlogTag, Guid>, IBlogTagRepository
{
    public BlogTagRepository(IDbContextProvider<SaasDemoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public override async Task<IQueryable<BlogTag>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }
}