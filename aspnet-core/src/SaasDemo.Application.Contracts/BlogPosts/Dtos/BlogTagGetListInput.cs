using System;
using System.ComponentModel;
using Volo.Abp.Application.Dtos;

namespace SaasDemo.BlogPosts.Dtos;

[Serializable]
public class BlogTagGetListInput : PagedAndSortedResultRequestDto
{
    public string? Name { get; set; }
}