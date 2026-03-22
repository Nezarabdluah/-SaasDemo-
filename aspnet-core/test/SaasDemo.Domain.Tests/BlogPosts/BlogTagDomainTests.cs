using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace SaasDemo.BlogPosts;

public class BlogTagDomainTests : SaasDemoDomainTestBase<SaasDemoDomainTestModule>
{
    /// <summary>
    /// Tests that Create factory method produces a valid BlogTag with correct Name.
    /// </summary>
    [Fact]
    public void Create_Should_Set_Name()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Angular";

        // Act
        var tag = BlogTag.Create(id, name);

        // Assert
        tag.Id.ShouldBe(id);
        tag.Name.ShouldBe("Angular");
    }

    /// <summary>
    /// Tests that Create factory method throws on null or empty name.
    /// </summary>
    [Fact]
    public void Create_Should_Throw_On_Empty_Name()
    {
        // Arrange & Act & Assert
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

    /// <summary>
    /// Tests that Update method changes Name correctly.
    /// </summary>
    [Fact]
    public void Update_Should_Change_Name()
    {
        // Arrange
        var tag = BlogTag.Create(Guid.NewGuid(), "OldName");

        // Act
        tag.Update("NewName");

        // Assert
        tag.Name.ShouldBe("NewName");
    }

    /// <summary>
    /// Tests that Update method throws on null or empty name.
    /// </summary>
    [Fact]
    public void Update_Should_Throw_On_Empty_Name()
    {
        // Arrange
        var tag = BlogTag.Create(Guid.NewGuid(), "ValidName");

        // Act & Assert
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

    /// <summary>
    /// Tests that constructor with parameters produces a valid BlogTag.
    /// </summary>
    [Fact]
    public void Constructor_Should_Set_Properties()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var tag = new BlogTag(id, "CSharp");

        // Assert
        tag.Id.ShouldBe(id);
        tag.Name.ShouldBe("CSharp");
    }
}