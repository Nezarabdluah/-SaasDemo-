using System;
using System.Collections.Generic;
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
    Task<List<BlogPostVersionListDto>> GetVersionsAsync(Guid postId);
    Task<BlogPostVersionDto> GetVersionAsync(Guid versionId);
    Task RestoreVersionAsync(Guid postId, Guid versionId);
}