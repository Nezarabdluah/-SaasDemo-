using System;

namespace SaasDemo.BlogPosts.Dtos;

[Serializable]
public class CreateUpdateBlogPostDto
{
    public string Title { get; set; }

    public string Slug { get; set; }

    public string Content { get; set; }

    public string ShortDescription { get; set; }

    public bool IsPublished { get; set; }
}