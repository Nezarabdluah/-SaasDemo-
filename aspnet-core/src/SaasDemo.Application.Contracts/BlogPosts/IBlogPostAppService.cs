using System;
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

}