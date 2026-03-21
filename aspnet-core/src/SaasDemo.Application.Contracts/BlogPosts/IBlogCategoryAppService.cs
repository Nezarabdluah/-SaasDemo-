using System;
using SaasDemo.BlogPosts.Dtos;
using Volo.Abp.Application.Services;

namespace SaasDemo.BlogPosts;


/// <summary>
/// Domain entity representing a category specifically for Blog Posts.Isolated from E-commerce or general categories to maintain Bounded Contexts.
/// </summary>
public interface IBlogCategoryAppService :
    ICrudAppService< 
        BlogCategoryDto, 
        Guid, 
        BlogCategoryGetListInput,
        CreateUpdateBlogCategoryDto,
        CreateUpdateBlogCategoryDto>
{

}