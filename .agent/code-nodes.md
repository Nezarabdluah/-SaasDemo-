# Code Nodes

## Entities & Relationships
| Entity | Module | Relationships |
|---|---|---|
| `BlogPost` | BlogPosts | Has many `BlogPostCategory`, `BlogPostTag`, `SlugRedirect`, `BlogPostVersion` |
| `BlogCategory` | BlogPosts | M2M with BlogPost via `BlogPostCategory` |
| `BlogTag` | BlogPosts | M2M with BlogPost via `BlogPostTag` |
| `BlogPostCategory` | BlogPosts | Join table (CompositeKey: BlogPostId + BlogCategoryId) |
| `BlogPostTag` | BlogPosts | Join table (CompositeKey: BlogPostId + BlogTagId) |
| `SlugRedirect` | BlogPosts | Belongs to BlogPost (old slugs for 301 redirect) |
| `BlogPostVersion` | BlogPosts | Belongs to BlogPost (snapshot of post at each edit) |

## DbContext/DbSets
| DbSet | Context | Description |
|---|---|---|
| `BlogPosts` | SaasDemoDbContext | Unique index on `Slug` |
| `BlogTags` | SaasDemoDbContext | — |
| `BlogPostTags` | SaasDemoDbContext | Composite key |
| `BlogCategories` | SaasDemoDbContext | — |
| `BlogPostCategories` | SaasDemoDbContext | Composite key |
| `SlugRedirects` | SaasDemoDbContext | Unique index on `OldSlug`, FK to BlogPost |
| `BlogPostVersions` | SaasDemoDbContext | Unique index on `(BlogPostId, VersionNumber)`, FK to BlogPost |

## Key Service Methods
| Service | Method | Functionality |
|---|---|---|
| `SlugHelper` | `Normalize(title)` | Static. Converts title → URL-friendly slug (Latin + Arabic) |
| `SlugGenerator` | `GenerateUniqueSlugAsync(title, excludeId?)` | Generates unique slug; appends `-2`, `-3` for duplicates |
| `BlogPostAppService` | `GetBySlugAsync(slug)` | Finds post by slug, checks SlugRedirects for old URLs |
| `BlogPostAppService` | `IncrementViewCountAsync(id)` | Fire & Forget view counter |
| `BlogPostAppService` | `CreateAsync(dto)` | Auto-generates slug if empty, calculates ReadingTime |
| `BlogPostAppService` | `UpdateAsync(id, dto)` | Auto-snapshots before update, saves redirects on slug change |
| `BlogPostAppService` | `GetVersionsAsync(postId)` | Returns timeline of all version snapshots for a post |
| `BlogPostAppService` | `GetVersionAsync(versionId)` | Returns full snapshot content of a specific version |
| `BlogPostAppService` | `RestoreVersionAsync(postId, versionId)` | Restores a post to a previous version (auto-saves before restore) |
| `BlogPostVersion` | `CreateFromPost(post, versionNum)` | Static factory: creates a snapshot from current post state |

## Angular Components
| Component | Location | Purpose |
|---|---|---|
| `CommentListComponent` | shared/components/comments/ | Fetches & renders comment tree |
| `CommentItemComponent` | shared/components/comments/ | Recursive single comment with reply/edit/delete |
| `CommentFormComponent` | shared/components/comments/ | Add/edit comment form with idempotencyToken |
| `CommentService` | shared/services/ | CmsKit public API integration (CRUD) |
