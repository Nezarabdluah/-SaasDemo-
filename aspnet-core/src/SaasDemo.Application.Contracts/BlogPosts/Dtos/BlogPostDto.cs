using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SaasDemo.BlogPosts.Dtos;

[Serializable]
public class BlogPostDto : FullAuditedEntityDto<Guid>
{
    public string Title { get; set; }

    public string Slug { get; set; }

    public string Content { get; set; }

    public string? ShortDescription { get; set; }

    public PublishStatus Status { get; set; }

    public DateTime? PublishedAt { get; set; }

    public string? FeaturedImageUrl { get; set; }

    public List<Guid> CategoryIds { get; set; } = new();

    public List<Guid> TagIds { get; set; } = new();
}