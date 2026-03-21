using System;
using Volo.Abp.Domain.Repositories;

namespace SaasDemo.BlogPosts;

public interface IBlogPostRepository : IRepository<BlogPost, Guid>
{
}
