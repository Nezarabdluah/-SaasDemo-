using System;

namespace SaasDemo.BlogPosts.Dtos;

[Serializable]
public class CreateUpdateBlogTagDto
{
    public string Name { get; set; }
}