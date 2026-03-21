using System;

namespace SaasDemo.BlogPosts.Dtos;

[Serializable]
public class CreateUpdateBlogCategoryDto
{
    public string Name { get; set; }

    public string Slug { get; set; }

    public string? Description { get; set; }
}