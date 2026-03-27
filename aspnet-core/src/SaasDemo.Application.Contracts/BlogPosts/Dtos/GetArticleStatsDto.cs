using System;

namespace SaasDemo.BlogPosts.Dtos;

/// <summary>
/// Lightweight DTO that returns only the statistics for a single blog post.
/// Useful for dashboard widgets and stat counters without loading full content.
/// </summary>
[Serializable]
public class GetArticleStatsDto
{
    public Guid Id { get; set; }
    public long ViewCount { get; set; }
    public int CommentCount { get; set; }
    public int ReactionCount { get; set; }
    public int ReadingTimeMinutes { get; set; }
}
