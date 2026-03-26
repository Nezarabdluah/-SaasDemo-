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
| `MediaFile` | MediaLibrary | Standalone entity. Tracks file metadata + BlobName in IBlobContainer |

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
| `MediaFiles` | SaasDemoDbContext | `FileName`, `BlobName`, `ContentType`, `FileSize`, `FolderPath`, `AltText` |

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
| `BlogPostAppService` | `RestoreVersionAsync(postId, versionId)` | Restores a post to a previous version |
| `MediaFileAppService` | `UploadAsync(dto)` | Saves blob + metadata in DB. Called by `MediaController` |
| `MediaFileAppService` | `GetListAsync(input)` | Paginated list with filter by name/folder |
| `MediaFileAppService` | `GetContentAsync(id)` | Returns raw byte[] from blob storage |
| `MediaFileAppService` | `DeleteAsync(id)` | Deletes blob + DB record |

## Controllers (Custom, non-ABP auto-generated)
| Controller | Route | Purpose |
|---|---|---|
| `MediaController` | `api/app/media/{id}/content` (GET) | Returns raw binary image with 1-year cache |
| `MediaController` | `api/app/media/upload` (POST) | Accepts `multipart/form-data` via `MediaUploadForm` model |

## Angular Components
| Component | Location | Purpose |
|---|---|---|
| `CommentListComponent` | shared/components/comments/ | Fetches & renders comment tree |
| `CommentItemComponent` | shared/components/comments/ | Recursive single comment with reply/edit/delete |
| `CommentFormComponent` | shared/components/comments/ | Add/edit comment form with idempotencyToken |
| `CommentService` | shared/services/ | CmsKit public API integration (CRUD) |
| `MediaLibraryComponent` | media-library/ | Standalone. Upload form + Grid view + Filter + Delete |
| `MediaService` | shared/services/ | Upload (FormData), GetList, Delete, UpdateMetadata |

## Angular Routing
| Path | Component | Menu Label |
|---|---|---|
| `/blogs` | BlogPostComponent | Blogs |
| `/media-library` | MediaLibraryComponent | مكتبة الوسائط |

## Permissions
| Permission Key | Description |
|---|---|
| `SaasDemo.BlogPost` | Blog Posts CRUD |
| `SaasDemo.BlogCategory` | Blog Categories CRUD |
| `SaasDemo.BlogTag` | Blog Tags CRUD |
| `SaasDemo.MediaLibrary.Default` | Media Library Read (NOT seeded yet) |
| `SaasDemo.MediaLibrary.Create` | Media Library Upload (NOT seeded yet) |
| `SaasDemo.MediaLibrary.Delete` | Media Library Delete (NOT seeded yet) |
