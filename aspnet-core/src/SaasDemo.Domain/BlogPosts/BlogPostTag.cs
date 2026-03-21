using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace SaasDemo.BlogPosts;

/// <summary>
/// Explicit mapping entity to manage the Many-to-Many relationship between BlogPost and BlogTag.
/// </summary>
public class BlogPostTag : CreationAuditedEntity<Guid>
{
    public Guid BlogPostId { get; private set; }
    public Guid BlogTagId { get; private set; }

    protected BlogPostTag()
    {
    }

    public BlogPostTag(Guid id, Guid blogPostId, Guid blogTagId) : base(id)
    {
        BlogPostId = blogPostId;
        BlogTagId = blogTagId;
    }
}
