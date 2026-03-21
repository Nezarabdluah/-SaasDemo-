using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace SaasDemo.BlogPosts;

public class BlogPost : FullAuditedAggregateRoot<Guid>
{
    public string Title { get; private set; }
    public string Slug { get; private set; }
    public string Content { get; private set; }
    public string ShortDescription { get; private set; }
    public bool IsPublished { get; private set; }

    /// <summary>
    /// Private constructor for ORM (Entity Framework Core) and DDD standard.
    /// </summary>
    private BlogPost()
    {
    }

    private BlogPost(Guid id, string title, string slug, string content, string shortDescription, bool isPublished) 
        : base(id)
    {
        Title = Check.NotNullOrWhiteSpace(title, nameof(title));
        Slug = Check.NotNullOrWhiteSpace(slug, nameof(slug));
        Content = Check.NotNullOrWhiteSpace(content, nameof(content));
        ShortDescription = shortDescription;
        IsPublished = isPublished;
    }

    /// <summary>
    /// Static factory method to strictly control the creation of instances.
    /// </summary>
    public static BlogPost Create(Guid id, string title, string slug, string content, string shortDescription, bool isPublished = false)
    {
        return new BlogPost(id, title, slug, content, shortDescription, isPublished);
    }
}
