using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace SaasDemo.BlogPosts;

/// <summary>
/// Stores old slugs that have been replaced, enabling 301 redirects to protect SEO.
/// When a BlogPost's slug changes, the old slug is saved here so incoming links
/// to the old URL are automatically redirected to the new one.
/// </summary>
public class SlugRedirect : CreationAuditedEntity<Guid>
{
    public string OldSlug { get; private set; }
    public Guid BlogPostId { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    protected SlugRedirect()
    {
    }

    public SlugRedirect(Guid id, string oldSlug, Guid blogPostId) : base(id)
    {
        OldSlug = Check.NotNullOrWhiteSpace(oldSlug, nameof(oldSlug));
        BlogPostId = blogPostId;
    }
}
