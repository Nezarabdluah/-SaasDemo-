using System;
using Volo.Abp.Domain.Repositories;

namespace SaasDemo.BlogPosts;

/// <summary>
/// Domain entity representing a category specifically for Blog Posts.Isolated from E-commerce or general categories to maintain Bounded Contexts.
/// </summary>
public interface IBlogCategoryRepository : IRepository<BlogCategory, Guid>
{
}
