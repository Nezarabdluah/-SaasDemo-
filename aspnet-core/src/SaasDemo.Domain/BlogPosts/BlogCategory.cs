using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace SaasDemo.BlogPosts;

/// <summary>
/// Domain entity representing a category specifically for Blog Posts.
/// Isolated from E-commerce or general categories to maintain Bounded Contexts.
/// </summary>
public class BlogCategory : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; private set; }
    public string Slug { get; private set; }
    public string? Description { get; private set; }

    private BlogCategory()
    {
    }

    private BlogCategory(Guid id, string name, string slug, string? description) : base(id)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        Slug = Check.NotNullOrWhiteSpace(slug, nameof(slug));
        Description = description;
    }

    public static BlogCategory Create(Guid id, string name, string slug, string? description = null)
    {
        return new BlogCategory(id, name, slug, description);
    }

    public void Update(string name, string slug, string? description)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        Slug = Check.NotNullOrWhiteSpace(slug, nameof(slug));
        Description = description;
    }

}
