using SaasDemo.BlogPosts;
using SaasDemo.BlogPosts.Dtos;
using AutoMapper;

namespace SaasDemo;

public class SaasDemoApplicationAutoMapperProfile : Profile
{
    public SaasDemoApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<BlogPost, BlogPostDto>();
        CreateMap<CreateUpdateBlogPostDto, BlogPost>(MemberList.Source);
        CreateMap<BlogCategory, BlogCategoryDto>();
        CreateMap<CreateUpdateBlogCategoryDto, BlogCategory>(MemberList.Source);
    }
}
