using System;
using Shouldly;
using Xunit;

namespace SaasDemo.BlogPosts;

/// <summary>
/// Pure domain tests for BlogTag entity.
/// No ABP test base needed — these test Domain logic only (no DI, no DB).
/// </summary>
public class BlogTagDomainTests
{
    [Fact]
    public void Create_Should_Set_Name()
    {
        var id = Guid.NewGuid();
        var name = "Angular";

        var tag = BlogTag.Create(id, name);

        tag.Id.ShouldBe(id);
        tag.Name.ShouldBe("Angular");
    }

    [Fact]
    public void Create_Should_Throw_On_Empty_Name()
    {
        Should.Throw<ArgumentException>(() =>
        {
            BlogTag.Create(Guid.NewGuid(), "");
        });
    }

    [Fact]
    public void Create_Should_Throw_On_Null_Name()
    {
        Should.Throw<ArgumentException>(() =>
        {
            BlogTag.Create(Guid.NewGuid(), null!);
        });
    }

    [Fact]
    public void Update_Should_Change_Name()
    {
        var tag = BlogTag.Create(Guid.NewGuid(), "OldName");
        tag.Update("NewName");
        tag.Name.ShouldBe("NewName");
    }

    [Fact]
    public void Update_Should_Throw_On_Empty_Name()
    {
        var tag = BlogTag.Create(Guid.NewGuid(), "ValidName");

        Should.Throw<ArgumentException>(() =>
        {
            tag.Update("");
        });
    }

    [Fact]
    public void Update_Should_Throw_On_Null_Name()
    {
        var tag = BlogTag.Create(Guid.NewGuid(), "ValidName");

        Should.Throw<ArgumentException>(() =>
        {
            tag.Update(null!);
        });
    }

    [Fact]
    public void Constructor_Should_Set_Properties()
    {
        var id = Guid.NewGuid();
        var tag = new BlogTag(id, "CSharp");

        tag.Id.ShouldBe(id);
        tag.Name.ShouldBe("CSharp");
    }
}