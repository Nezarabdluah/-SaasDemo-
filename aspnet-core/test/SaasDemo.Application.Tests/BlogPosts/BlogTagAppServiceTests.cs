using System;
using System.Threading.Tasks;
using SaasDemo.BlogPosts.Dtos;
using Shouldly;
using Xunit;

namespace SaasDemo.BlogPosts;

public class BlogTagAppServiceTests : SaasDemoApplicationTestBase<SaasDemoApplicationTestModule>
{
    private readonly IBlogTagAppService _blogTagAppService;

    public BlogTagAppServiceTests()
    {
        _blogTagAppService = GetRequiredService<IBlogTagAppService>();
    }

    /// <summary>
    /// Tests that CreateAsync creates a new BlogTag and returns the correct DTO.
    /// </summary>
    [Fact]
    public async Task Should_Create_A_BlogTag()
    {
        // Arrange
        var input = new CreateUpdateBlogTagDto { Name = "DotNet" };

        // Act
        var result = await _blogTagAppService.CreateAsync(input);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(Guid.Empty);
        result.Name.ShouldBe("DotNet");
    }

    /// <summary>
    /// Tests that GetAsync returns the created BlogTag by Id.
    /// </summary>
    [Fact]
    public async Task Should_Get_BlogTag_By_Id()
    {
        // Arrange
        var created = await _blogTagAppService.CreateAsync(
            new CreateUpdateBlogTagDto { Name = "Angular" });

        // Act
        var result = await _blogTagAppService.GetAsync(created.Id);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(created.Id);
        result.Name.ShouldBe("Angular");
    }

    /// <summary>
    /// Tests that UpdateAsync changes the BlogTag name.
    /// </summary>
    [Fact]
    public async Task Should_Update_BlogTag_Name()
    {
        // Arrange
        var created = await _blogTagAppService.CreateAsync(
            new CreateUpdateBlogTagDto { Name = "OldTag" });

        // Act
        var updated = await _blogTagAppService.UpdateAsync(
            created.Id,
            new CreateUpdateBlogTagDto { Name = "NewTag" });

        // Assert
        updated.Name.ShouldBe("NewTag");
    }

    /// <summary>
    /// Tests that DeleteAsync removes a BlogTag.
    /// </summary>
    [Fact]
    public async Task Should_Delete_BlogTag()
    {
        // Arrange
        var created = await _blogTagAppService.CreateAsync(
            new CreateUpdateBlogTagDto { Name = "ToDelete" });

        // Act
        await _blogTagAppService.DeleteAsync(created.Id);

        // Assert
        await Should.ThrowAsync<Exception>(async () =>
        {
            await _blogTagAppService.GetAsync(created.Id);
        });
    }

    /// <summary>
    /// Tests that GetListAsync returns paginated results.
    /// </summary>
    [Fact]
    public async Task Should_Get_List_Of_BlogTags()
    {
        // Arrange
        await _blogTagAppService.CreateAsync(new CreateUpdateBlogTagDto { Name = "Tag1" });
        await _blogTagAppService.CreateAsync(new CreateUpdateBlogTagDto { Name = "Tag2" });
        await _blogTagAppService.CreateAsync(new CreateUpdateBlogTagDto { Name = "Tag3" });

        // Act
        var result = await _blogTagAppService.GetListAsync(
            new BlogTagGetListInput { MaxResultCount = 10, SkipCount = 0 });

        // Assert
        result.TotalCount.ShouldBeGreaterThanOrEqualTo(3);
        result.Items.ShouldContain(x => x.Name == "Tag1");
        result.Items.ShouldContain(x => x.Name == "Tag2");
        result.Items.ShouldContain(x => x.Name == "Tag3");
    }
}
