using System;
using System.Linq;
using Shouldly;
using Xunit;

namespace SaasDemo.BlogPosts;

/// <summary>
/// Pure domain tests for BlogPost entity.
/// No ABP test base needed — these test Domain logic only (no DI, no DB).
/// </summary>
public class BlogPostDomainTests
{
    /// <summary>
    /// Tests that Create factory method sets all properties including new SEO fields.
    /// </summary>
    [Fact]
    public void Create_Should_Set_All_Properties()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var post = BlogPost.Create(
            id, "Test Title", "test-title", "This is the content body.",
            shortDescription: "Short desc",
            status: PublishStatus.Published,
            featuredImageUrl: "https://img.test/cover.jpg",
            publishedAt: new DateTime(2026, 3, 26),
            metaTitle: "SEO Title",
            metaDescription: "SEO Description for search engines",
            ogImageUrl: "https://img.test/og.jpg");

        // Assert
        post.Id.ShouldBe(id);
        post.Title.ShouldBe("Test Title");
        post.Slug.ShouldBe("test-title");
        post.Content.ShouldBe("This is the content body.");
        post.ShortDescription.ShouldBe("Short desc");
        post.Status.ShouldBe(PublishStatus.Published);
        post.FeaturedImageUrl.ShouldBe("https://img.test/cover.jpg");
        post.MetaTitle.ShouldBe("SEO Title");
        post.MetaDescription.ShouldBe("SEO Description for search engines");
        post.OgImageUrl.ShouldBe("https://img.test/og.jpg");
    }

    [Fact]
    public void Create_Should_Throw_On_Empty_Title()
    {
        Should.Throw<ArgumentException>(() =>
        {
            BlogPost.Create(Guid.NewGuid(), "", "slug", "content");
        });
    }

    [Fact]
    public void Create_Should_Throw_On_Empty_Slug()
    {
        Should.Throw<ArgumentException>(() =>
        {
            BlogPost.Create(Guid.NewGuid(), "Title", "", "content");
        });
    }

    [Fact]
    public void Create_Should_Throw_On_Empty_Content()
    {
        Should.Throw<ArgumentException>(() =>
        {
            BlogPost.Create(Guid.NewGuid(), "Title", "slug", "");
        });
    }

    [Fact]
    public void Update_Should_Change_All_Properties()
    {
        var post = BlogPost.Create(Guid.NewGuid(), "Old Title", "old-slug", "Old content.");

        post.Update(
            "New Title", "new-slug", "New content body.",
            "New short desc", PublishStatus.Published, "https://new-cover.jpg",
            new DateTime(2026, 6, 1),
            "New Meta Title", "New Meta Description", "https://new-og.jpg");

        post.Title.ShouldBe("New Title");
        post.Slug.ShouldBe("new-slug");
        post.Content.ShouldBe("New content body.");
        post.ShortDescription.ShouldBe("New short desc");
        post.Status.ShouldBe(PublishStatus.Published);
        post.MetaTitle.ShouldBe("New Meta Title");
        post.MetaDescription.ShouldBe("New Meta Description");
        post.OgImageUrl.ShouldBe("https://new-og.jpg");
    }

    [Fact]
    public void Update_Should_Throw_On_Empty_Title()
    {
        var post = BlogPost.Create(Guid.NewGuid(), "Title", "slug", "content");

        Should.Throw<ArgumentException>(() =>
        {
            post.Update("", "slug", "content", null, PublishStatus.Draft, null);
        });
    }

    /// <summary>
    /// 400 words / 200 wpm = 2 minutes
    /// </summary>
    [Fact]
    public void CalculateReadingTime_Should_Return_Correct_Minutes()
    {
        var words = string.Join(" ", new string[400].Select(_ => "word"));
        var post = BlogPost.Create(Guid.NewGuid(), "Title", "slug", words);

        post.ReadingTimeMinutes.ShouldBe(2);
    }

    [Fact]
    public void CalculateReadingTime_Should_Return_Minimum_One_Minute()
    {
        var post = BlogPost.Create(Guid.NewGuid(), "Title", "slug", "Short content.");
        post.ReadingTimeMinutes.ShouldBe(1);
    }

    [Fact]
    public void IncrementViewCount_Should_Increase_By_One()
    {
        var post = BlogPost.Create(Guid.NewGuid(), "Title", "slug", "content");
        post.ViewCount.ShouldBe(0);

        post.IncrementViewCount();
        post.ViewCount.ShouldBe(1);

        post.IncrementViewCount();
        post.ViewCount.ShouldBe(2);
    }

    [Fact]
    public void Constructor_Should_Set_Default_ViewCount_Zero()
    {
        var post = BlogPost.Create(Guid.NewGuid(), "Title", "slug", "content");
        post.ViewCount.ShouldBe(0);
    }
}