using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SaasDemo.BlogPosts;

public static class BlogPostEfCoreQueryableExtensions
{
    public static IQueryable<BlogPost> IncludeDetails(this IQueryable<BlogPost> queryable, bool include = true)
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
