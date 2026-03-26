using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace SaasDemo.BlogPosts;

/// <summary>
/// Stores a snapshot of a BlogPost at a specific point in time.
/// Created automatically before each update to preserve edit history.
/// </summary>
public class BlogPostVersion : CreationAuditedEntity<Guid>
{
    public Guid BlogPostId { get; private set; }
    public int VersionNumber { get; private set; }
    public string Title { get; private set; }
    public string Content { get; private set; }
    public string Slug { get; private set; }
    public string? ShortDescription { get; private set; }
    public string? ChangeNote { get; private set; }

    // SEO snapshot
    public string? MetaTitle { get; private set; }
    public string? MetaDescription { get; private set; }
    public string? OgImageUrl { get; private set; }

#pragma warning disable CS8618 // EF Core constructor
    protected BlogPostVersion() { }
#pragma warning restore CS8618

    public BlogPostVersion(
        Guid id,
        Guid blogPostId,
        int versionNumber,
        string title,
        string content,
        string slug,
        string? shortDescription = null,
        string? changeNote = null,
        string? metaTitle = null,
        string? metaDescription = null,
        string? ogImageUrl = null
    ) : base(id)
    {
        BlogPostId = blogPostId;
        VersionNumber = versionNumber;
        Title = Check.NotNullOrWhiteSpace(title, nameof(title));
        Content = Check.NotNullOrWhiteSpace(content, nameof(content));
        Slug = Check.NotNullOrWhiteSpace(slug, nameof(slug));
        ShortDescription = shortDescription;
        ChangeNote = changeNote;
        MetaTitle = metaTitle;
        MetaDescription = metaDescription;
        OgImageUrl = ogImageUrl;
    }

    /// <summary>
    /// Creates a version snapshot from a BlogPost's current state.
    /// </summary>
    public static BlogPostVersion CreateFromPost(
        BlogPost post,
        int versionNumber,
        string? changeNote = null)
    {
        return new BlogPostVersion(
            Guid.NewGuid(),
            post.Id,
            versionNumber,
            post.Title,
            post.Content,
            post.Slug,
            post.ShortDescription,
            changeNote,
            post.MetaTitle,
            post.MetaDescription,
            post.OgImageUrl
        );
    }
}
