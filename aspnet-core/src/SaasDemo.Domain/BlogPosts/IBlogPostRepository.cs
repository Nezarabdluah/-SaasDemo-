using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SaasDemo.BlogPosts;

public interface IBlogPostRepository : IRepository<BlogPost, Guid>
{
    /// <summary>
    /// Checks if a slug already exists, optionally excluding a specific BlogPost (for updates).
    /// </summary>
    Task<bool> SlugExistsAsync(string slug, Guid? excludeId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds a BlogPost by its slug. Returns null if not found.
    /// </summary>
    Task<BlogPost?> FindBySlugAsync(string slug, CancellationToken cancellationToken = default);
}
