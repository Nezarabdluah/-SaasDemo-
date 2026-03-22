using System;
using System.Linq;
using SaasDemo.Localization;
using System.Threading.Tasks;
using SaasDemo.Permissions;
using SaasDemo.BlogPosts.Dtos;
using Volo.Abp.Application.Services;

namespace SaasDemo.BlogPosts;


/// <summary>
/// Domain entity representing a category specifically for Blog Posts.
/// Isolated from E-commerce or general categories to maintain Bounded Contexts.
/// </summary>
public class BlogCategoryAppService : CrudAppService<BlogCategory, BlogCategoryDto, Guid, BlogCategoryGetListInput, CreateUpdateBlogCategoryDto, CreateUpdateBlogCategoryDto>,
    IBlogCategoryAppService
{
    protected override string? GetPolicyName { get; set; } = SaasDemoPermissions.BlogCategory.Default;
    protected override string? GetListPolicyName { get; set; } = SaasDemoPermissions.BlogCategory.Default;
    protected override string? CreatePolicyName { get; set; } = SaasDemoPermissions.BlogCategory.Create;
    protected override string? UpdatePolicyName { get; set; } = SaasDemoPermissions.BlogCategory.Update;
    protected override string? DeletePolicyName { get; set; } = SaasDemoPermissions.BlogCategory.Delete;

    private readonly IBlogCategoryRepository _repository;

    public BlogCategoryAppService(IBlogCategoryRepository repository) : base(repository)
    {
        _repository = repository;

        LocalizationResource = typeof(SaasDemoResource);
    }

    protected override async Task<IQueryable<BlogCategory>> CreateFilteredQueryAsync(BlogCategoryGetListInput input)
    {
        return (await base.CreateFilteredQueryAsync(input))
            .WhereIf(!input.Name.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Name!))
            .WhereIf(!input.Slug.IsNullOrWhiteSpace(), x => x.Slug.Contains(input.Slug!))
            .WhereIf(input.Description != null, x => x.Description == input.Description)
            ;
    }

    public override async Task<BlogCategoryDto> CreateAsync(CreateUpdateBlogCategoryDto input)
    {
        await CheckCreatePolicyAsync();

        var entity = BlogCategory.Create(
            GuidGenerator.Create(),
            input.Name,
            input.Slug,
            input.Description
        );

        await _repository.InsertAsync(entity, autoSave: true);
        return MapToDto(entity);
    }

    public override async Task<BlogCategoryDto> UpdateAsync(Guid id, CreateUpdateBlogCategoryDto input)
    {
        await CheckUpdatePolicyAsync();

        var entity = await _repository.GetAsync(id);

        entity.Update(input.Name, input.Slug, input.Description);

        await _repository.UpdateAsync(entity, autoSave: true);
        return MapToDto(entity);
    }

    protected override Task<BlogCategoryDto> MapToGetOutputDtoAsync(BlogCategory entity)
    {
        return Task.FromResult(MapToDto(entity));
    }

    protected override Task<BlogCategoryDto> MapToGetListOutputDtoAsync(BlogCategory entity)
    {
        return Task.FromResult(MapToDto(entity));
    }

    /// <summary>
    /// Manual mapping — bypasses broken ObjectMapper DI resolution.
    /// </summary>
    private static BlogCategoryDto MapToDto(BlogCategory entity)
    {
        return new BlogCategoryDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Slug = entity.Slug,
            Description = entity.Description,
            CreationTime = entity.CreationTime,
            CreatorId = entity.CreatorId,
            LastModificationTime = entity.LastModificationTime,
            LastModifierId = entity.LastModifierId,
        };
    }
}
