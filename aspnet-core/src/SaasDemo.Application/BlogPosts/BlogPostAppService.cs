using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
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
    private readonly IRepository<BlogPostCategory, Guid> _blogPostCategoryRepository;
    private readonly IRepository<BlogPostTag, Guid> _blogPostTagRepository;

    public BlogPostAppService(
        IBlogPostRepository repository,
        IRepository<BlogPostCategory, Guid> blogPostCategoryRepository,
        IRepository<BlogPostTag, Guid> blogPostTagRepository) : base(repository)
    {
        _repository = repository;
        _blogPostCategoryRepository = blogPostCategoryRepository;
        _blogPostTagRepository = blogPostTagRepository;

        LocalizationResource = typeof(SaasDemoResource);
    }

    protected override async Task<IQueryable<BlogPost>> CreateFilteredQueryAsync(BlogPostGetListInput input)
    {
        return (await base.CreateFilteredQueryAsync(input))
            .WhereIf(!string.IsNullOrWhiteSpace(input.Title), x => x.Title.Contains(input.Title!))
            .WhereIf(!string.IsNullOrWhiteSpace(input.Slug), x => x.Slug.Contains(input.Slug!))
            .WhereIf(!string.IsNullOrWhiteSpace(input.Content), x => x.Content.Contains(input.Content!))
            .WhereIf(!string.IsNullOrWhiteSpace(input.ShortDescription), x => x.ShortDescription != null && x.ShortDescription.Contains(input.ShortDescription!))
            .WhereIf(input.Status != null, x => x.Status == input.Status)
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
            input.Status,
            input.FeaturedImageUrl,
            input.PublishedAt
        );

        await _repository.InsertAsync(entity);

        if (input.CategoryIds != null && input.CategoryIds.Any())
        {
            foreach (var categoryId in input.CategoryIds)
            {
                await _blogPostCategoryRepository.InsertAsync(
                    new BlogPostCategory(GuidGenerator.Create(), entity.Id, categoryId));
            }
        }

        if (input.TagIds != null && input.TagIds.Any())
        {
            foreach (var tagId in input.TagIds)
            {
                await _blogPostTagRepository.InsertAsync(
                    new BlogPostTag(GuidGenerator.Create(), entity.Id, tagId));
            }
        }

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
            input.Status,
            input.FeaturedImageUrl,
            input.PublishedAt
        );

        await _repository.UpdateAsync(entity);

        if (input.CategoryIds != null)
        {
            var oldCategories = await _blogPostCategoryRepository.GetListAsync(x => x.BlogPostId == entity.Id);
            
            foreach (var oldCategory in oldCategories)
            {
                if (!input.CategoryIds.Contains(oldCategory.BlogCategoryId))
                {
                    await _blogPostCategoryRepository.DeleteAsync(oldCategory);
                }
            }

            foreach (var newCategoryId in input.CategoryIds)
            {
                if (!oldCategories.Any(x => x.BlogCategoryId == newCategoryId))
                {
                    await _blogPostCategoryRepository.InsertAsync(
                        new BlogPostCategory(GuidGenerator.Create(), entity.Id, newCategoryId));
                }
            }
        }

        if (input.TagIds != null)
        {
            var oldTags = await _blogPostTagRepository.GetListAsync(x => x.BlogPostId == entity.Id);
            
            foreach (var oldTag in oldTags)
            {
                if (!input.TagIds.Contains(oldTag.BlogTagId))
                {
                    await _blogPostTagRepository.DeleteAsync(oldTag);
                }
            }

            foreach (var newTagId in input.TagIds)
            {
                if (!oldTags.Any(x => x.BlogTagId == newTagId))
                {
                    await _blogPostTagRepository.InsertAsync(
                        new BlogPostTag(GuidGenerator.Create(), entity.Id, newTagId));
                }
            }
        }

        return await MapToGetOutputDtoAsync(entity);
    }

    protected override async Task<BlogPostDto> MapToGetOutputDtoAsync(BlogPost entity)
    {
        var dto = MapToDto(entity);

        var categories = await _blogPostCategoryRepository.GetListAsync(x => x.BlogPostId == entity.Id);
        dto.CategoryIds = categories.Select(x => x.BlogCategoryId).ToList();
        
        var tags = await _blogPostTagRepository.GetListAsync(x => x.BlogPostId == entity.Id);
        dto.TagIds = tags.Select(x => x.BlogTagId).ToList();

        return dto;
    }

    protected override Task<BlogPostDto> MapToGetListOutputDtoAsync(BlogPost entity)
    {
        return Task.FromResult(MapToDto(entity));
    }

    /// <summary>
    /// Manual mapping — bypasses broken ObjectMapper DI resolution.
    /// </summary>
    private static BlogPostDto MapToDto(BlogPost entity)
    {
        return new BlogPostDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Slug = entity.Slug,
            Content = entity.Content,
            ShortDescription = entity.ShortDescription,
            Status = entity.Status,
            PublishedAt = entity.PublishedAt,
            FeaturedImageUrl = entity.FeaturedImageUrl,
            CreationTime = entity.CreationTime,
            CreatorId = entity.CreatorId,
            LastModificationTime = entity.LastModificationTime,
            LastModifierId = entity.LastModifierId,
        };
    }
}
