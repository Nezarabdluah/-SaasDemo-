using System;

namespace SaasDemo.BlogPosts.Dtos;

public class BlogPostVersionDto
{
    public Guid Id { get; set; }
    public Guid BlogPostId { get; set; }
    public int VersionNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? ChangeNote { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? OgImageUrl { get; set; }
    public DateTime CreationTime { get; set; }
}

/// <summary>
/// Lightweight DTO for the version history list (timeline).
/// </summary>
public class BlogPostVersionListDto
{
    public Guid Id { get; set; }
    public int VersionNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? ChangeNote { get; set; }
    public DateTime CreationTime { get; set; }
}
