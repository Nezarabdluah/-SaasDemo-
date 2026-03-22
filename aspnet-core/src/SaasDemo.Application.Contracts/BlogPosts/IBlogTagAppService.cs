using System;
using SaasDemo.BlogPosts.Dtos;
using Volo.Abp.Application.Services;

namespace SaasDemo.BlogPosts;


/// <summary>
/// Domain entity representing a Tag specifically for Blog Posts.
/// </summary>
public interface IBlogTagAppService :
    ICrudAppService< 
        BlogTagDto, 
        Guid, 
        BlogTagGetListInput,
        CreateUpdateBlogTagDto,
        CreateUpdateBlogTagDto>
{

}