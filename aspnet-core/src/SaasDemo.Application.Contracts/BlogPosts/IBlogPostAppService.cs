using System;
using System.Threading.Tasks;
using SaasDemo.BlogPosts.Dtos;
using Volo.Abp.Application.Services;

namespace SaasDemo.BlogPosts;

public interface IBlogPostAppService :
    ICrudAppService< 
        BlogPostDto, 
        Guid, 
        BlogPostGetListInput,
        CreateUpdateBlogPostDto,
        CreateUpdateBlogPostDto>
{
    Task<BlogPostDto?> GetBySlugAsync(string slug);
    Task IncrementViewCountAsync(Guid id);
}