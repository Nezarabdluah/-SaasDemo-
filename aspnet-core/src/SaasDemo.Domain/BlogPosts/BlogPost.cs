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

    // SEO Fields
    public string? MetaTitle { get; private set; }
    public string? MetaDescription { get; private set; }
    public string? OgImageUrl { get; private set; }

    // Content Stats
    public int ReadingTimeMinutes { get; private set; }
    public long ViewCount { get; private set; }

    /// <summary>
    /// Private constructor for ORM (Entity Framework Core) and DDD standard.
    /// </summary>
    private BlogPost()
    {
    }

    private BlogPost(
        Guid id,
        string title,
        string slug,
        string content,
        string? shortDescription,
        PublishStatus status,
        DateTime? publishedAt,
        string? featuredImageUrl,
        string? metaTitle,
        string? metaDescription,
        string? ogImageUrl)
        : base(id)
    {
        Title = Check.NotNullOrWhiteSpace(title, nameof(title));
        Slug = Check.NotNullOrWhiteSpace(slug, nameof(slug));
        Content = Check.NotNullOrWhiteSpace(content, nameof(content));
        ShortDescription = shortDescription;
        Status = status;
        PublishedAt = publishedAt;
        FeaturedImageUrl = featuredImageUrl;
        MetaTitle = metaTitle;
        MetaDescription = metaDescription;
        OgImageUrl = ogImageUrl;
        ViewCount = 0;
        CalculateReadingTime();
    }

    /// <summary>
    /// Static factory method to strictly control the creation of instances.
    /// </summary>
    public static BlogPost Create(
        Guid id,
        string title,
        string slug,
        string content,
        string? shortDescription = null,
        PublishStatus status = PublishStatus.Draft,
        string? featuredImageUrl = null,
        DateTime? publishedAt = null,
        string? metaTitle = null,
        string? metaDescription = null,
        string? ogImageUrl = null)
    {
        return new BlogPost(id, title, slug, content, shortDescription, status, publishedAt, featuredImageUrl, metaTitle, metaDescription, ogImageUrl);
    }

    /// <summary>
    /// DDD Update method to encapsulate property changes.
    /// </summary>
    public void Update(
        string title,
        string slug,
        string content,
        string? shortDescription,
        PublishStatus status,
        string? featuredImageUrl,
        DateTime? publishedAt = null,
        string? metaTitle = null,
        string? metaDescription = null,
        string? ogImageUrl = null)
    {
        Title = Check.NotNullOrWhiteSpace(title, nameof(title));
        Slug = Check.NotNullOrWhiteSpace(slug, nameof(slug));
        Content = Check.NotNullOrWhiteSpace(content, nameof(content));
        ShortDescription = shortDescription;
        Status = status;
        FeaturedImageUrl = featuredImageUrl;
        PublishedAt = publishedAt;
        MetaTitle = metaTitle;
        MetaDescription = metaDescription;
        OgImageUrl = ogImageUrl;
        CalculateReadingTime();
    }

    /// <summary>
    /// Calculates estimated reading time based on word count (average 200 words/min).
    /// Called automatically on Create and Update.
    /// </summary>
    public void CalculateReadingTime()
    {
        if (string.IsNullOrWhiteSpace(Content))
        {
            ReadingTimeMinutes = 0;
            return;
        }

        var wordCount = Content.Split(
            new[] { ' ', '\n', '\r', '\t' },
            StringSplitOptions.RemoveEmptyEntries).Length;

        ReadingTimeMinutes = Math.Max(1, (int)Math.Ceiling(wordCount / 200.0));
    }

    /// <summary>
    /// Increments the view counter by 1. Designed for Fire & Forget usage.
    /// </summary>
    public void IncrementViewCount()
    {
        ViewCount++;
    }
}
