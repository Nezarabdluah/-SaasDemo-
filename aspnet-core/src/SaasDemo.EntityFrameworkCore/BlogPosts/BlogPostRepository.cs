using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

    public async Task<bool> SlugExistsAsync(string slug, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        var dbSet = await GetDbSetAsync();

        return await dbSet
            .AsNoTracking()
            .WhereIf(excludeId.HasValue, x => x.Id != excludeId!.Value)
            .AnyAsync(x => x.Slug == slug, GetCancellationToken(cancellationToken));
    }

    public async Task<BlogPost?> FindBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var dbSet = await GetDbSetAsync();

        return await dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Slug == slug, GetCancellationToken(cancellationToken));
    }
}