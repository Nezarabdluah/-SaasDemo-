# Code Nodes

## Entities & Relationships
| Entity | Module | Relationships |
|---|---|---|
| `BlogPost` | BlogPosts | Has many `BlogPostCategory`, `BlogPostTag`, `SlugRedirect` |
| `BlogCategory` | BlogPosts | M2M with BlogPost via `BlogPostCategory` |
| `BlogTag` | BlogPosts | M2M with BlogPost via `BlogPostTag` |
| `BlogPostCategory` | BlogPosts | Join table (CompositeKey: BlogPostId + BlogCategoryId) |
| `BlogPostTag` | BlogPosts | Join table (CompositeKey: BlogPostId + BlogTagId) |
| `SlugRedirect` | BlogPosts | Belongs to BlogPost (old slugs for 301 redirect) |

## DbContext/DbSets
| DbSet | Context | Description |
|---|---|---|
| `BlogPosts` | SaasDemoDbContext | Unique index on `Slug` |
| `BlogTags` | SaasDemoDbContext | — |
| `BlogPostTags` | SaasDemoDbContext | Composite key |
| `BlogCategories` | SaasDemoDbContext | — |
| `BlogPostCategories` | SaasDemoDbContext | Composite key |
| `SlugRedirects` | SaasDemoDbContext | Unique index on `OldSlug`, FK to BlogPost |

## Key Service Methods
| Service | Method | Functionality |
|---|---|---|
| `SlugHelper` | `Normalize(title)` | Static. Converts title → URL-friendly slug (Latin + Arabic) |
| `SlugGenerator` | `GenerateUniqueSlugAsync(title, excludeId?)` | Generates unique slug; appends `-2`, `-3` for duplicates |
| `BlogPostAppService` | `GetBySlugAsync(slug)` | Finds post by slug, checks SlugRedirects for old URLs |
| `BlogPostAppService` | `IncrementViewCountAsync(id)` | Fire & Forget view counter |
| `BlogPostAppService` | `CreateAsync(dto)` | Auto-generates slug if empty, calculates ReadingTime |
| `BlogPostAppService` | `UpdateAsync(id, dto)` | Saves old slug to SlugRedirect on change |

## Frontend Component Patterns
| Component | UI Pattern | Notes |
|---|---|---|
