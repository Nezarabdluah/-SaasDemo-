# Current Context

## ⏳ Current Phase: Phase 1 (CMS + SEO Core)
- **Session Date:** 2026-03-27
- **Status**: 🟢 Phase 1 Nearly Complete — All Batches 1–5.3 Done
- **Focus**: SSR investigated and blocked (ABP incompatible). SEO Meta Tags implemented as alternative. 
- **Next Phase**: Phase 2 (Site Settings + Email Templates Engine)
- **Remaining in Phase 1**: sitemap.xml / robots.txt (optional)

## 📊 Phase 1 Progress
| Batch | Feature | Status |
|-------|---------|--------|
| 1 | Slug System + SEO Fields + ReadingTime + ViewCount | ✅ Done |
| 2 | Comments UI (Nested Replies via CmsKit) | ✅ Done |
| 3 | Content Versioning (Auto-Snapshot, Restore, Diff) | ✅ Done |
| 4 | Media Library (Backend + Angular UI) | ✅ Done |
| 4.5 | Media Library Integration (Cover Picker, Quill, Copy URL, Drag & Drop) | ✅ Done |
| 5.1 | Article Statistics (Views, Comments, Reactions) | ✅ Done |
| 5.2 | Angular SSR | ⛔ Blocked (ABP Lepton-X غير متوافق) |
| 5.3 | SEO Meta Tags (Title + OG) | ✅ Done |

## 🐛 Known Issues / Lessons Learned
1. **ABP Permissions**: New permissions need `DbMigrator` to seed into DB, otherwise 403 Forbidden.
2. **Swashbuckle + IFormFile**: Cannot use individual `[FromForm]` params. Must use a model class (`MediaUploadForm`).
3. **BlobStoring Path**: Use `ContentRootPath` for local dev. `D:\` may lack write permissions.
4. **Angular Image URLs**: Must prefix with backend API URL from `EnvironmentService`.
5. **ABP Auto-API vs Custom Controller**: Use `[RemoteService(IsEnabled = false)]` on AppService methods that have custom controllers.
6. **Quill Image Handler**: Override via `quillModules.toolbar.handlers.image` function to open custom UI.
7. **CmsKit Integration**: `ICommentRepository.GetCountAsync()` does not have an `(entityType, entityId)` overload. To count comments/reactions, use `IReadOnlyRepository<Comment, Guid>` with `GetQueryableAsync()` and `AsyncExecuter.CountAsync`.
8. **ABP Lepton-X ≠ Angular SSR**: ABP Lepton-X v4.3 directly uses `document.createElement`, `document.body.dir`, `document.location.href` during Angular DI initialization → fundamentally breaks SSR. Workaround: CSR + Meta Tags.
9. **Webpack Import Hoisting**: `import` statements are hoisted above all inline code → polyfills in `server.ts` execute after vendor modules. Solution: external launcher script.
10. **Node.js v24 fetch ≠ NODE_TLS_REJECT_UNAUTHORIZED**: Node's built-in `fetch` (undici) ignores `process.env.NODE_TLS_REJECT_UNAUTHORIZED` set inside JS. Must set via OS env variable before process starts.
11. **Angular 20 CommonEngine**: Requires explicit `allowedHosts` in constructor. `ng build` only builds browser; `ng run ProjectName:server` is needed for server bundle.
