export enum PublishStatus {
  Draft = 0,
  Published = 1,
  Scheduled = 2
}

export interface BlogPostDto {
  id?: string;
  title?: string;
  slug?: string;
  content?: string;
  shortDescription?: string;
  status: PublishStatus;
  publishedAt?: string;
  featuredImageUrl?: string;
  categoryIds?: string[];
  tagIds?: string[];
  viewCount?: number;
  commentCount?: number;
  reactionCount?: number;
  readingTimeMinutes?: number;
}
