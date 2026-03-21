using System;
using System.Linq;
using System.Threading.Tasks;
using SaasDemo.Permissions;
using SaasDemo.BlogPosts.Dtos;
using Volo.Abp.Application.Services;
using SaasDemo.Localization;

namespace SaasDemo.BlogPosts;


public class BlogPostAppService : CrudAppService<BlogPost, BlogPostDto, Guid, BlogPostGetListInput, CreateUpdateBlogPostDto, CreateUpdateBlogPostDto>,
    IBlogPostAppService
{
    protected override string? GetPolicyName { get; set; } = SaasDemoPermissions.BlogPost.Default;
    protected override string? GetListPolicyName { get; set; } = SaasDemoPermissions.BlogPost.Default;
    protected override string? CreatePolicyName { get; set; } = SaasDemoPermissions.BlogPost.Create;
    protected override string? UpdatePolicyName { get; set; } = SaasDemoPermissions.BlogPost.Update;
    protected override string? DeletePolicyName { get; set; } = SaasDemoPermissions.BlogPost.Delete;

    private readonly IBlogPostRepository _repository;

    public BlogPostAppService(IBlogPostRepository repository) : base(repository)
    {
        _repository = repository;

        LocalizationResource = typeof(SaasDemoResource);
        ObjectMapperContext = typeof(SaasDemoApplicationModule);
    }

    protected override async Task<IQueryable<BlogPost>> CreateFilteredQueryAsync(BlogPostGetListInput input)
    {
        // TODO: AbpHelper generated
        return (await base.CreateFilteredQueryAsync(input))
            .WhereIf(!input.Title.IsNullOrWhiteSpace(), x => x.Title.Contains(input.Title))
            .WhereIf(!input.Slug.IsNullOrWhiteSpace(), x => x.Slug.Contains(input.Slug))
            .WhereIf(!input.Content.IsNullOrWhiteSpace(), x => x.Content.Contains(input.Content))
            .WhereIf(!input.ShortDescription.IsNullOrWhiteSpace(), x => x.ShortDescription.Contains(input.ShortDescription))
            .WhereIf(input.IsPublished != null, x => x.IsPublished == input.IsPublished)
            ;
    }

    public override async Task<BlogPostDto> CreateAsync(CreateUpdateBlogPostDto input)
    {
        await CheckCreatePolicyAsync();

        var entity = BlogPost.Create(
            GuidGenerator.Create(),
            input.Title,
            input.Slug,
            input.Content,
            input.ShortDescription,
            input.IsPublished
        );

        await _repository.InsertAsync(entity);

        return await MapToGetOutputDtoAsync(entity);
    }

    public override async Task<BlogPostDto> UpdateAsync(Guid id, CreateUpdateBlogPostDto input)
    {
        await CheckUpdatePolicyAsync();

        var entity = await _repository.GetAsync(id);

        entity.Update(
            input.Title,
            input.Slug,
            input.Content,
            input.ShortDescription,
            input.IsPublished
        );

        await _repository.UpdateAsync(entity);

        return await MapToGetOutputDtoAsync(entity);
    }
}
