using System;
using System.Linq;
using System.Threading.Tasks;
using SaasDemo.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SaasDemo.BlogPosts;

public class BlogCategoryRepository : EfCoreRepository<SaasDemoDbContext, BlogCategory, Guid>, IBlogCategoryRepository
{
    public BlogCategoryRepository(IDbContextProvider<SaasDemoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public override async Task<IQueryable<BlogCategory>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }
}