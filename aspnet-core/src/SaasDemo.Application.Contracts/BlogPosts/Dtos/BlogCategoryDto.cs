using System;
using Volo.Abp.Application.Dtos;

namespace SaasDemo.BlogPosts.Dtos;

/// <summary>
/// Domain entity representing a category specifically for Blog Posts.Isolated from E-commerce or general categories to maintain Bounded Contexts.
/// </summary>
[Serializable]
public class BlogCategoryDto : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; }

    public string Slug { get; set; }

    public string? Description { get; set; }
}