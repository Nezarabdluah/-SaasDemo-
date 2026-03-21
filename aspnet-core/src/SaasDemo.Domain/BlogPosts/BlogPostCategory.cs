using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace SaasDemo.BlogPosts;

/// <summary>
/// Explicit mapping entity to manage the Many-to-Many relationship between BlogPost and BlogCategory.
/// </summary>
public class BlogPostCategory : CreationAuditedEntity<Guid>
{
    public Guid BlogPostId { get; private set; }
    public Guid BlogCategoryId { get; private set; }

    private BlogPostCategory()
    {
    }

    public BlogPostCategory(Guid id, Guid blogPostId, Guid blogCategoryId) : base(id)
    {
        BlogPostId = blogPostId;
        BlogCategoryId = blogCategoryId;
    }
}
