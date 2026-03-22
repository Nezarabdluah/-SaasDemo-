using System;
using System.Linq;
using System.Threading.Tasks;
using SaasDemo.Permissions;
using SaasDemo.BlogPosts.Dtos;
using Volo.Abp.Application.Services;
using SaasDemo.Localization;

namespace SaasDemo.BlogPosts;


/// <summary>
/// Domain entity representing a Tag specifically for Blog Posts.
/// </summary>
public class BlogTagAppService : CrudAppService<BlogTag, BlogTagDto, Guid, BlogTagGetListInput, CreateUpdateBlogTagDto, CreateUpdateBlogTagDto>,
    IBlogTagAppService
{
    protected override string? GetPolicyName { get; set; } = SaasDemoPermissions.BlogTag.Default;
    protected override string? GetListPolicyName { get; set; } = SaasDemoPermissions.BlogTag.Default;
    protected override string? CreatePolicyName { get; set; } = SaasDemoPermissions.BlogTag.Create;
    protected override string? UpdatePolicyName { get; set; } = SaasDemoPermissions.BlogTag.Update;
    protected override string? DeletePolicyName { get; set; } = SaasDemoPermissions.BlogTag.Delete;

    private readonly IBlogTagRepository _repository;

    public BlogTagAppService(IBlogTagRepository repository) : base(repository)
    {
        _repository = repository;

        LocalizationResource = typeof(SaasDemoResource);
    }

    protected override async Task<IQueryable<BlogTag>> CreateFilteredQueryAsync(BlogTagGetListInput input)
    {
        // TODO: AbpHelper generated
        return (await base.CreateFilteredQueryAsync(input))
            .WhereIf(!string.IsNullOrWhiteSpace(input.Name), x => x.Name.Contains(input.Name!))
            ;
    }

    public override async Task<BlogTagDto> CreateAsync(CreateUpdateBlogTagDto input)
    {
        await CheckCreatePolicyAsync();

        var id = GuidGenerator.Create();
        var entity = BlogTag.Create(id, input.Name);

        await _repository.InsertAsync(entity, autoSave: true);

        return MapToDto(entity);
    }

    public override async Task<BlogTagDto> UpdateAsync(Guid id, CreateUpdateBlogTagDto input)
    {
        await CheckUpdatePolicyAsync();

        var entity = await _repository.GetAsync(id);

        entity.Update(input.Name);

        await _repository.UpdateAsync(entity, autoSave: true);

        return MapToDto(entity);
    }

    /// <summary>
    /// Manual mapping from BlogTag entity to BlogTagDto.
    /// Bypasses ObjectMapper to avoid DI resolution issues with module-scoped mappers.
    /// </summary>
    private static BlogTagDto MapToDto(BlogTag entity)
    {
        return new BlogTagDto
        {
            Id = entity.Id,
            Name = entity.Name,
            CreationTime = entity.CreationTime,
            CreatorId = entity.CreatorId,
            LastModificationTime = entity.LastModificationTime,
            LastModifierId = entity.LastModifierId,
        };
    }

    protected override Task<BlogTagDto> MapToGetOutputDtoAsync(BlogTag entity)
    {
        return Task.FromResult(MapToDto(entity));
    }

    protected override Task<BlogTagDto> MapToGetListOutputDtoAsync(BlogTag entity)
    {
        return Task.FromResult(MapToDto(entity));
    }
}
