# Current Context

## ⏳ Current Phase: Phase 1 (CMS + SEO Core)
- **Session Date:** 2026-03-26
- **Status**: 🟢 Batches 1–4.5 Completed (Slug, SEO, Comments, Versioning, Media Library + Integration)
- **Focus**: Phase 1 nearly complete. Remaining: Angular SSR, sitemap.xml/robots.txt
- **Next Phase**: Phase 2 (Site Settings + Email Templates Engine)

## 📊 Phase 1 Progress
| Batch | Feature | Status |
|-------|---------|--------|
| 1 | Slug System + SEO Fields + ReadingTime + ViewCount | ✅ Done |
| 2 | Comments UI (Nested Replies via CmsKit) | ✅ Done |
| 3 | Content Versioning (Auto-Snapshot, Restore, Diff) | ✅ Done |
| 4 | Media Library (Backend + Angular UI) | ✅ Done |
| 4.5 | Media Library Integration (Cover Picker, Quill, Copy URL) | ✅ Done |
| 5 | Angular SSR + sitemap.xml + robots.txt | 🔲 Pending |

## 🐛 Known Issues / Lessons Learned
1. **ABP Permissions**: New permissions need `DbMigrator` to seed into DB, otherwise 403 Forbidden.
2. **Swashbuckle + IFormFile**: Cannot use individual `[FromForm]` params. Must use a model class (`MediaUploadForm`).
3. **BlobStoring Path**: Use `ContentRootPath` for local dev. `D:\` may lack write permissions.
4. **Angular Image URLs**: Must prefix with backend API URL from `EnvironmentService`.
5. **ABP Auto-API vs Custom Controller**: Use `[RemoteService(IsEnabled = false)]` on AppService methods that have custom controllers.
6. **Quill Image Handler**: Override via `quillModules.toolbar.handlers.image` function to open custom UI.
