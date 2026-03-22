using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SaasDemo.BlogPosts;

/// <summary>
/// Domain entity representing a Tag specifically for Blog Posts.
/// </summary>
public static class BlogTagEfCoreQueryableExtensions
{
    public static IQueryable<BlogTag> IncludeDetails(this IQueryable<BlogTag> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable
            // .Include(x => x.xxx) // TODO: AbpHelper generated
            ;
    }
}
