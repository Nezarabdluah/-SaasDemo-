using System;
using System.ComponentModel;
using Volo.Abp.Application.Dtos;

namespace SaasDemo.BlogPosts.Dtos;

[Serializable]
public class BlogPostGetListInput : PagedAndSortedResultRequestDto
{
    public string? Title { get; set; }

    public string? Slug { get; set; }

    public string? Content { get; set; }

    public string? ShortDescription { get; set; }

    public bool? IsPublished { get; set; }
}