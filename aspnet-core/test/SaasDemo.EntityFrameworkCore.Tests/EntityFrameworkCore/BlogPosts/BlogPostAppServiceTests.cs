using System;
using System.Linq;
using System.Threading.Tasks;
using SaasDemo.BlogPosts;
using SaasDemo.BlogPosts.Dtos;
using Shouldly;
using Xunit;

namespace SaasDemo.EntityFrameworkCore.BlogPosts;

/// <summary>
/// Integration tests for BlogPostAppService.
/// Runs through EF Core test project where full DI (repos + DB) is available.
/// </summary>
public class BlogPostAppServiceTests : SaasDemoEntityFrameworkCoreTestBase
{
    private readonly IBlogPostAppService _blogPostAppService;

    public BlogPostAppServiceTests()
    {
        _blogPostAppService = GetRequiredService<IBlogPostAppService>();
    }

    [Fact]
    public async Task Should_Auto_Generate_Slug_When_Empty()
    {
        var result = await WithUnitOfWorkAsync(async () =>
        {
            return await _blogPostAppService.CreateAsync(new CreateUpdateBlogPostDto
            {
                Title = "My First Blog Post",
                Slug = null,
                Content = "Some meaningful content here.",
                Status = PublishStatus.Draft
            });
        });

        result.ShouldNotBeNull();
        result.Slug.ShouldNotBeNullOrWhiteSpace();
        result.Slug.ShouldBe("my-first-blog-post");
    }

    [Fact]
    public async Task Should_Return_SEO_Fields_In_DTO()
    {
        var result = await WithUnitOfWorkAsync(async () =>
        {
            return await _blogPostAppService.CreateAsync(new CreateUpdateBlogPostDto
            {
                Title = "SEO Test Post",
                Slug = "seo-test",
                Content = "Content for SEO testing.",
                Status = PublishStatus.Published,
                MetaTitle = "Custom SEO Title",
                MetaDescription = "A carefully crafted meta description.",
                OgImageUrl = "https://example.com/og-image.jpg"
            });
        });

        result.MetaTitle.ShouldBe("Custom SEO Title");
        result.MetaDescription.ShouldBe("A carefully crafted meta description.");
        result.OgImageUrl.ShouldBe("https://example.com/og-image.jpg");
    }

    [Fact]
    public async Task Should_Calculate_Reading_Time()
    {
        var longContent = string.Join(" ", new string[400].Select(_ => "word"));

        var result = await WithUnitOfWorkAsync(async () =>
        {
            return await _blogPostAppService.CreateAsync(new CreateUpdateBlogPostDto
            {
                Title = "Long Article",
                Slug = "long-article",
                Content = longContent,
                Status = PublishStatus.Draft
            });
        });

        result.ReadingTimeMinutes.ShouldBe(2);
    }

    [Fact]
    public async Task Should_Increment_View_Count()
    {
        var created = await WithUnitOfWorkAsync(async () =>
        {
            return await _blogPostAppService.CreateAsync(new CreateUpdateBlogPostDto
            {
                Title = "View Count Test",
                Slug = "view-count-test",
                Content = "Content for view count testing.",
                Status = PublishStatus.Draft
            });
        });

        created.ViewCount.ShouldBe(0);

        await WithUnitOfWorkAsync(async () =>
        {
            await _blogPostAppService.IncrementViewCountAsync(created.Id);
        });

        var updated = await WithUnitOfWorkAsync(async () =>
        {
            return await _blogPostAppService.GetAsync(created.Id);
        });

        updated.ViewCount.ShouldBe(1);
    }

    [Fact]
    public async Task Should_Generate_Unique_Slug_For_Duplicate_Title()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            await _blogPostAppService.CreateAsync(new CreateUpdateBlogPostDto
            {
                Title = "Duplicate Title",
                Slug = "duplicate-title",
                Content = "First post content.",
                Status = PublishStatus.Draft
            });
        });

        var result = await WithUnitOfWorkAsync(async () =>
        {
            return await _blogPostAppService.CreateAsync(new CreateUpdateBlogPostDto
            {
                Title = "Duplicate Title Again",
                Slug = "duplicate-title",
                Content = "Second post content.",
                Status = PublishStatus.Draft
            });
        });

        result.Slug.ShouldBe("duplicate-title-2");
    }

    [Fact]
    public async Task Should_Return_Stats_With_Zero_Comments_For_New_Post()
    {
        var created = await WithUnitOfWorkAsync(async () =>
        {
            return await _blogPostAppService.CreateAsync(new CreateUpdateBlogPostDto
            {
                Title = "Stats Test Post",
                Slug = "stats-test-post",
                Content = "Content for stats testing.",
                Status = PublishStatus.Draft
            });
        });

        var stats = await WithUnitOfWorkAsync(async () =>
        {
            return await _blogPostAppService.GetStatsAsync(created.Id);
        });

        stats.ShouldNotBeNull();
        stats.Id.ShouldBe(created.Id);
        stats.CommentCount.ShouldBe(0);
        stats.ReactionCount.ShouldBe(0);
        stats.ViewCount.ShouldBe(0);
        stats.ReadingTimeMinutes.ShouldBeGreaterThan(0);
    }
}
