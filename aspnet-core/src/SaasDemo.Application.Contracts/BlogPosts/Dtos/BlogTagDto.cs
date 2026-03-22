using System;
using Volo.Abp.Application.Dtos;

namespace SaasDemo.BlogPosts.Dtos;

/// <summary>
/// Domain entity representing a Tag specifically for Blog Posts.
/// </summary>
[Serializable]
public class BlogTagDto : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; }
}