using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SaasDemo.BlogPosts;

/// <summary>
/// Domain entity representing a category specifically for Blog Posts.Isolated from E-commerce or general categories to maintain Bounded Contexts.
/// </summary>
public static class BlogCategoryEfCoreQueryableExtensions
{
    public static IQueryable<BlogCategory> IncludeDetails(this IQueryable<BlogCategory> queryable, bool include = true)
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
