using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace SaasDemo.BlogPosts;

/// <summary>
/// Pure utility class for slug normalization. No dependencies — safe for Domain layer.
/// </summary>
public static class SlugHelper
{
    /// <summary>
    /// Normalizes a title into a URL-friendly slug.
    /// Handles Latin and Arabic characters.
    /// </summary>
    public static string Normalize(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return string.Empty;
        }

        // Lowercase
        var slug = title.ToLowerInvariant();

        // Remove diacritics (accents) for Latin characters
        slug = RemoveDiacritics(slug);

        // Replace spaces and common separators with hyphens
        slug = Regex.Replace(slug, @"[\s_]+", "-");

        // Keep only alphanumeric, hyphens, and Arabic/Unicode letters
        slug = Regex.Replace(slug, @"[^\w\u0600-\u06FF-]", "", RegexOptions.None);

        // Collapse multiple hyphens into one
        slug = Regex.Replace(slug, @"-{2,}", "-");

        // Trim leading/trailing hyphens
        slug = slug.Trim('-');

        // Fallback if slug is empty after processing
        if (string.IsNullOrWhiteSpace(slug))
        {
            slug = Guid.NewGuid().ToString("N")[..8];
        }

        return slug;
    }

    private static string RemoveDiacritics(string text)
    {
        var normalized = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (var c in normalized)
        {
            var category = CharUnicodeInfo.GetUnicodeCategory(c);
            if (category != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(c);
            }
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}
