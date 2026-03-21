using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace SaasDemo.BlogPosts;

public class BlogPost : FullAuditedAggregateRoot<Guid>
{
    public string Title { get; private set; }
    public string Slug { get; private set; }
    public string Content { get; private set; }
    public string? ShortDescription { get; private set; }
    public PublishStatus Status { get; private set; }
    public DateTime? PublishedAt { get; private set; }
    public string? FeaturedImageUrl { get; private set; }

    /// <summary>
    /// Private constructor for ORM (Entity Framework Core) and DDD standard.
    /// </summary>
    private BlogPost()
    {
    }

    private BlogPost(Guid id, string title, string slug, string content, string? shortDescription, PublishStatus status, DateTime? publishedAt, string? featuredImageUrl) 
        : base(id)
    {
        Title = Check.NotNullOrWhiteSpace(title, nameof(title));
        Slug = Check.NotNullOrWhiteSpace(slug, nameof(slug));
        Content = Check.NotNullOrWhiteSpace(content, nameof(content));
        ShortDescription = shortDescription;
        Status = status;
        PublishedAt = publishedAt;
        FeaturedImageUrl = featuredImageUrl;
    }

    /// <summary>
    /// Static factory method to strictly control the creation of instances.
    /// </summary>
    public static BlogPost Create(Guid id, string title, string slug, string content, string? shortDescription = null, PublishStatus status = PublishStatus.Draft, string? featuredImageUrl = null, DateTime? publishedAt = null)
    {
        return new BlogPost(id, title, slug, content, shortDescription, status, publishedAt, featuredImageUrl);
    }

    /// <summary>
    /// DDD Update method to encapsulate property changes.
    /// </summary>
    public void Update(string title, string slug, string content, string? shortDescription, PublishStatus status, string? featuredImageUrl, DateTime? publishedAt = null)
    {
        Title = Check.NotNullOrWhiteSpace(title, nameof(title));
        Slug = Check.NotNullOrWhiteSpace(slug, nameof(slug));
        Content = Check.NotNullOrWhiteSpace(content, nameof(content));
        ShortDescription = shortDescription;
        Status = status;
        FeaturedImageUrl = featuredImageUrl;
        PublishedAt = publishedAt;
    }
}
