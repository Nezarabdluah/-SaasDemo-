using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace SaasDemo.BlogPosts;

/// <summary>
/// Application-layer service for generating unique slugs.
/// Depends on IBlogPostRepository for uniqueness checks — must live in Application layer.
/// </summary>
public class SlugGenerator : ITransientDependency
{
    private readonly IBlogPostRepository _blogPostRepository;

    public SlugGenerator(IBlogPostRepository blogPostRepository)
    {
        _blogPostRepository = blogPostRepository;
    }

    /// <summary>
    /// Generates a unique slug from a title. If the slug already exists,
    /// appends -2, -3, etc. until a unique one is found.
    /// </summary>
    /// <param name="title">The title to generate slug from.</param>
    /// <param name="excludeId">Optional ID to exclude (for updates — don't conflict with self).</param>
    public async Task<string> GenerateUniqueSlugAsync(string title, Guid? excludeId = null)
    {
        Check.NotNullOrWhiteSpace(title, nameof(title));

        var baseSlug = SlugHelper.Normalize(title);
        var slug = baseSlug;
        var counter = 2;

        while (await _blogPostRepository.SlugExistsAsync(slug, excludeId))
        {
            slug = $"{baseSlug}-{counter}";
            counter++;
        }

        return slug;
    }
}
