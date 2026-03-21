using System;
using System.ComponentModel;
using Volo.Abp.Application.Dtos;

namespace SaasDemo.BlogPosts.Dtos;

[Serializable]
public class BlogCategoryGetListInput : PagedAndSortedResultRequestDto
{
    public string? Name { get; set; }

    public string? Slug { get; set; }

    public string? Description { get; set; }
}