using Shouldly;
using Xunit;

namespace SaasDemo.BlogPosts;

/// <summary>
/// Tests for the static SlugHelper.Normalize method.
/// These are pure unit tests (no DI needed) since Normalize is a static method.
/// </summary>
public class SlugGeneratorTests
{
    [Fact]
    public void Should_Generate_Slug_From_English_Title()
    {
        var result = SlugHelper.Normalize("My First Blog Post");
        result.ShouldBe("my-first-blog-post");
    }

    [Fact]
    public void Should_Handle_Arabic_Title()
    {
        var result = SlugHelper.Normalize("مقالة تجريبية");
        result.ShouldNotBeNullOrWhiteSpace();
        result.ShouldContain("مقالة");
    }

    [Fact]
    public void Should_Remove_Special_Characters()
    {
        var result = SlugHelper.Normalize("Hello! World? @2026 #Best");
        result.ShouldBe("hello-world-2026-best");
    }

    [Fact]
    public void Should_Collapse_Multiple_Hyphens()
    {
        var result = SlugHelper.Normalize("Hello   ---   World");
        result.ShouldNotContain("--");
    }

    [Fact]
    public void Should_Trim_Leading_Trailing_Hyphens()
    {
        var result = SlugHelper.Normalize("  -Hello World-  ");
        result.ShouldNotStartWith("-");
        result.ShouldNotEndWith("-");
    }

    [Fact]
    public void Should_Handle_Accented_Characters()
    {
        var result = SlugHelper.Normalize("Café résumé naïve");
        result.ShouldBe("cafe-resume-naive");
    }

    [Fact]
    public void Should_Return_Fallback_For_Empty_After_Processing()
    {
        // Only special characters that get stripped
        var result = SlugHelper.Normalize("!@#$%^&*()");
        result.ShouldNotBeNullOrWhiteSpace();
        result.Length.ShouldBe(8); // GUID fallback is 8 chars
    }
}
