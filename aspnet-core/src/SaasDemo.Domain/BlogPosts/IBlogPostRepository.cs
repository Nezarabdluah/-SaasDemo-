using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SaasDemo.BlogPosts;

public interface IBlogPostRepository : IRepository<BlogPost, Guid>
{
    Task<bool> SlugExistsAsync(string slug, Guid? excludeId = null, CancellationToken cancellationToken = default);
    Task<BlogPost?> FindBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<List<BlogPostVersion>> GetVersionsAsync(Guid blogPostId, CancellationToken cancellationToken = default);
    Task<BlogPostVersion?> GetVersionAsync(Guid versionId, CancellationToken cancellationToken = default);
    Task<int> GetLatestVersionNumberAsync(Guid blogPostId, CancellationToken cancellationToken = default);
}
