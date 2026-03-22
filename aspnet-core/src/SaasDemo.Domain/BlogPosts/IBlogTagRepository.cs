using System;
using Volo.Abp.Domain.Repositories;

namespace SaasDemo.BlogPosts;

/// <summary>
/// Domain entity representing a Tag specifically for Blog Posts.
/// </summary>
public interface IBlogTagRepository : IRepository<BlogTag, Guid>
{
}
