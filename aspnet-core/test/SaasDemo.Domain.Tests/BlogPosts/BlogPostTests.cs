using System;
using System.Linq;
using Shouldly;
using Xunit;

namespace SaasDemo.BlogPosts;

/// <summary>
/// Layer 0: Business Logic Tests for BlogPost Entity (Invariant Tests)
/// </summary>
public class BlogPostTests
{
    [Fact]
    public void Create_Should_Throw_Exception_When_Content_Is_Empty()
    {
        // Act
        var act = () => BlogPost.Create(Guid.NewGuid(), "Test Title", "test-slug", string.Empty);

        // Assert
        act.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void CalculateReadingTime_Should_Be_Minimum_One_Minute_For_Short_Content()
    {
        // Arrange
        var post = BlogPost.Create(Guid.NewGuid(), "Test Title", "test-slug", "Short content");

        // Act & Assert
        post.ReadingTimeMinutes.ShouldBe(1);
    }

    [Fact]
    public void CalculateReadingTime_Should_Calculate_Correctly_Based_On_Word_Count()
    {
        // Arrange (generate 450 words)
        var content = string.Join(" ", new string[450].Select(_ => "word"));
        var post = BlogPost.Create(Guid.NewGuid(), "Test Title", "test-slug", content);

        // Act
        post.CalculateReadingTime();

        // Assert (450 / 200 = 2.25 -> ceiling -> 3 minutes)
        post.ReadingTimeMinutes.ShouldBe(3);
    }

    [Fact]
    public void IncrementViewCount_Should_Increase_Count_By_One()
    {
        // Arrange
        var post = BlogPost.Create(Guid.NewGuid(), "Test Title", "test-slug", "Content");
        var initialViews = post.ViewCount;

        // Act
        post.IncrementViewCount();

        // Assert
        post.ViewCount.ShouldBe(initialViews + 1);
    }

    [Fact]
    public void Create_Should_Throw_Exception_When_Title_Is_Empty()
    {
        // Act
        var act = () => BlogPost.Create(Guid.NewGuid(), string.Empty, "slug", "Content");

        // Assert
        act.ShouldThrow<ArgumentException>();
    }
}
