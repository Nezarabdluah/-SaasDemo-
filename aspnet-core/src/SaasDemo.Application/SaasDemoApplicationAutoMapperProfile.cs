using SaasDemo.BlogPosts;
using SaasDemo.BlogPosts.Dtos;
using AutoMapper;

namespace SaasDemo;

public class SaasDemoApplicationAutoMapperProfile : Profile
{
    public SaasDemoApplicationAutoMapperProfile()
    {
        // ========== Entity -> DTO (Read mappings — actually used) ==========
        CreateMap<BlogPost, BlogPostDto>()
            .ForMember(x => x.CategoryIds, opt => opt.Ignore())
            .ForMember(x => x.TagIds, opt => opt.Ignore());

        CreateMap<BlogCategory, BlogCategoryDto>();

        CreateMap<BlogTag, BlogTagDto>();

        // ========== DTO -> Entity (Required by CrudAppService DI validation) ==========
        // ABP's CrudAppService requires these mappings to exist in its DI container,
        // but we NEVER use them — our AppServices override Create/Update
        // with DDD factory methods (entity.Create / entity.Update).
        //
        // We use ConvertUsing to completely bypass AutoMapper's constructor
        // and member mapping — entities have private/protected constructors
        // that AutoMapper cannot invoke.

        CreateMap<CreateUpdateBlogPostDto, BlogPost>()
            .ConvertUsing((src, dest, ctx) => dest!);

        CreateMap<CreateUpdateBlogCategoryDto, BlogCategory>()
            .ConvertUsing((src, dest, ctx) => dest!);

        CreateMap<CreateUpdateBlogTagDto, BlogTag>()
            .ConvertUsing((src, dest, ctx) => dest!);
    }
}
