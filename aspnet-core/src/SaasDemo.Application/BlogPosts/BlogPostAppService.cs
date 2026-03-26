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
    private readonly IRepository<SlugRedirect, Guid> _slugRedirectRepository;
    private readonly IRepository<BlogPostVersion, Guid> _versionRepository;
    private readonly SlugGenerator _slugGenerator;

    public BlogPostAppService(
        IBlogPostRepository repository,
        IRepository<BlogPostCategory, Guid> blogPostCategoryRepository,
        IRepository<BlogPostTag, Guid> blogPostTagRepository,
        IRepository<SlugRedirect, Guid> slugRedirectRepository,
        IRepository<BlogPostVersion, Guid> versionRepository,
        SlugGenerator slugGenerator) : base(repository)
    {
        _repository = repository;
        _blogPostCategoryRepository = blogPostCategoryRepository;
        _blogPostTagRepository = blogPostTagRepository;
        _slugRedirectRepository = slugRedirectRepository;
        _versionRepository = versionRepository;
        _slugGenerator = slugGenerator;

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

        // Auto-generate slug from title if not provided
        var slug = string.IsNullOrWhiteSpace(input.Slug)
            ? await _slugGenerator.GenerateUniqueSlugAsync(input.Title)
            : await _slugGenerator.GenerateUniqueSlugAsync(input.Slug);

        var entity = BlogPost.Create(
            GuidGenerator.Create(),
            input.Title,
            slug,
            input.Content,
            input.ShortDescription,
            input.Status,
            input.FeaturedImageUrl,
            input.PublishedAt,
            input.MetaTitle,
            input.MetaDescription,
            input.OgImageUrl
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
        var oldSlug = entity.Slug;

        // *** Auto-snapshot: save current state before applying changes ***
        var latestVersionNumber = await _repository.GetLatestVersionNumberAsync(id);
        var snapshot = BlogPostVersion.CreateFromPost(entity, latestVersionNumber + 1);
        await _versionRepository.InsertAsync(snapshot);

        // Generate new slug if changed or auto-generate from title
        string newSlug;
        if (string.IsNullOrWhiteSpace(input.Slug))
        {
            newSlug = await _slugGenerator.GenerateUniqueSlugAsync(input.Title, id);
        }
        else
        {
            newSlug = await _slugGenerator.GenerateUniqueSlugAsync(input.Slug, id);
        }

        // Save old slug as redirect if it changed
        if (!string.Equals(oldSlug, newSlug, StringComparison.OrdinalIgnoreCase))
        {
            await _slugRedirectRepository.InsertAsync(
                new SlugRedirect(GuidGenerator.Create(), oldSlug, entity.Id));
        }

        entity.Update(
            input.Title,
            newSlug,
            input.Content,
            input.ShortDescription,
            input.Status,
            input.FeaturedImageUrl,
            input.PublishedAt,
            input.MetaTitle,
            input.MetaDescription,
            input.OgImageUrl
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

    /// <summary>
    /// Gets a BlogPost by its slug. Also checks SlugRedirects for old slugs.
    /// Returns null if neither found.
    /// </summary>
    public async Task<BlogPostDto?> GetBySlugAsync(string slug)
    {
        var entity = await _repository.FindBySlugAsync(slug);

        if (entity != null)
        {
            return await MapToGetOutputDtoAsync(entity);
        }

        // Check if it's an old slug that was redirected
        var redirect = await _slugRedirectRepository.FirstOrDefaultAsync(x => x.OldSlug == slug);
        if (redirect != null)
        {
            entity = await _repository.GetAsync(redirect.BlogPostId);
            var dto = await MapToGetOutputDtoAsync(entity);
            dto.Slug = entity.Slug; // Return the current slug so frontend can redirect
            return dto;
        }

        return null;
    }

    /// <summary>
    /// Increments the view count for a blog post. Designed for Fire & Forget usage.
    /// </summary>
    public async Task IncrementViewCountAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        entity.IncrementViewCount();
        await _repository.UpdateAsync(entity);
    }

    public async Task<List<BlogPostVersionListDto>> GetVersionsAsync(Guid postId)
    {
        var versions = await _repository.GetVersionsAsync(postId);

        return versions.Select(v => new BlogPostVersionListDto
        {
            Id = v.Id,
            VersionNumber = v.VersionNumber,
            Title = v.Title,
            ChangeNote = v.ChangeNote,
            CreationTime = v.CreationTime
        }).ToList();
    }

    public async Task<BlogPostVersionDto> GetVersionAsync(Guid versionId)
    {
        var v = await _repository.GetVersionAsync(versionId)
            ?? throw new Volo.Abp.BusinessException("Version not found.");

        return new BlogPostVersionDto
        {
            Id = v.Id,
            BlogPostId = v.BlogPostId,
            VersionNumber = v.VersionNumber,
            Title = v.Title,
            Content = v.Content,
            Slug = v.Slug,
            ShortDescription = v.ShortDescription,
            ChangeNote = v.ChangeNote,
            MetaTitle = v.MetaTitle,
            MetaDescription = v.MetaDescription,
            OgImageUrl = v.OgImageUrl,
            CreationTime = v.CreationTime
        };
    }

    public async Task RestoreVersionAsync(Guid postId, Guid versionId)
    {
        var entity = await _repository.GetAsync(postId);
        var version = await _repository.GetVersionAsync(versionId)
            ?? throw new Volo.Abp.BusinessException("Version not found.");

        // Save current state as a new version before restoring
        var latestVersionNumber = await _repository.GetLatestVersionNumberAsync(postId);
        var snapshotBeforeRestore = BlogPostVersion.CreateFromPost(entity, latestVersionNumber + 1, "Auto-saved before restore");
        await _versionRepository.InsertAsync(snapshotBeforeRestore);

        // Restore entity from the chosen version
        entity.Update(
            version.Title,
            version.Slug,
            version.Content,
            version.ShortDescription,
            entity.Status,
            entity.FeaturedImageUrl,
            entity.PublishedAt,
            version.MetaTitle,
            version.MetaDescription,
            version.OgImageUrl
        );

        await _repository.UpdateAsync(entity);
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
            MetaTitle = entity.MetaTitle,
            MetaDescription = entity.MetaDescription,
            OgImageUrl = entity.OgImageUrl,
            ReadingTimeMinutes = entity.ReadingTimeMinutes,
            ViewCount = entity.ViewCount,
            CreationTime = entity.CreationTime,
            CreatorId = entity.CreatorId,
            LastModificationTime = entity.LastModificationTime,
            LastModifierId = entity.LastModifierId,
        };
    }
}
