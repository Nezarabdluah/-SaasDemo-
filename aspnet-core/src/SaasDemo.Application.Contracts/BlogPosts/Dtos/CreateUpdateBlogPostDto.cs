using System;
using System.Collections.Generic;

namespace SaasDemo.BlogPosts.Dtos;

[Serializable]
public class CreateUpdateBlogPostDto
{
    public string Title { get; set; }

    /// <summary>
    /// Optional — if empty or null, slug is auto-generated from Title.
    /// </summary>
    public string? Slug { get; set; }

    public string Content { get; set; }

    public string? ShortDescription { get; set; }

    public PublishStatus Status { get; set; }

    public DateTime? PublishedAt { get; set; }

    public string? FeaturedImageUrl { get; set; }

    // SEO Fields
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? OgImageUrl { get; set; }

    public List<Guid>? CategoryIds { get; set; }

    public List<Guid>? TagIds { get; set; }
}