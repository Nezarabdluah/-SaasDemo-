using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace SaasDemo.BlogPosts;

/// <summary>
/// Domain entity representing a Tag specifically for Blog Posts.
/// </summary>
public class BlogTag : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; private set; }

#pragma warning disable CS8618 // EF Core constructor
    protected BlogTag()
    {
    }
#pragma warning restore CS8618

    public BlogTag(
        Guid id,
        string name
    ) : base(id)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name));
    }

    public static BlogTag Create(Guid id, string name)
    {
        return new BlogTag(id, name);
    }

    public void Update(string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name));
    }
}
