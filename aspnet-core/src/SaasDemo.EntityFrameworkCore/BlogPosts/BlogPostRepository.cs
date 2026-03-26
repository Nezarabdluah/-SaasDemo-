using System;
using System.Collections.Generic;
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

    public async Task<List<BlogPostVersion>> GetVersionsAsync(Guid blogPostId, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        return await dbContext.BlogPostVersions
            .AsNoTracking()
            .Where(x => x.BlogPostId == blogPostId)
            .OrderByDescending(x => x.VersionNumber)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async Task<BlogPostVersion?> GetVersionAsync(Guid versionId, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        return await dbContext.BlogPostVersions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == versionId, GetCancellationToken(cancellationToken));
    }

    public async Task<int> GetLatestVersionNumberAsync(Guid blogPostId, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var max = await dbContext.BlogPostVersions
            .Where(x => x.BlogPostId == blogPostId)
            .MaxAsync(x => (int?)x.VersionNumber, GetCancellationToken(cancellationToken));

        return max ?? 0;
    }
}