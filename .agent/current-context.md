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

## 🐛 Known Issues / Lessons Learned | المشاكل المعروفة والدروس المستفادة
1. **ABP Permissions**: New permissions need `DbMigrator` to seed into DB, otherwise 403 Forbidden.
   *(صلاحيات ABP: أي صلاحية جديدة تحتاج تشغيل `DbMigrator` لزرعها في قاعدة البيانات، وإلا ستظهر مشكلة 403 Forbidden.)*
2. **Swashbuckle + IFormFile**: Cannot use individual `[FromForm]` params. Must use a model class (`MediaUploadForm`).
   *(لا يمكن استخدام `[FromForm]` مفرد مع رفع الملفات في Swagger، يجب وضعها داخل كلاس `MediaUploadForm`.)*
3. **BlobStoring Path**: Use `ContentRootPath` for local dev. `D:\` may lack write permissions.
   *(مسار التخزين: استخدم `ContentRootPath` في التطوير المحلي لأن بعض الأقراص قد تفتقر لصلاحيات الكتابة.)*
4. **Angular Image URLs**: Must prefix with backend API URL from `EnvironmentService`.
   *(روابط الصور في Angular: يجب وضع رابط الـ API قبلها باستخدام `EnvironmentService`.)*
5. **ABP Auto-API vs Custom Controller**: Use `[RemoteService(IsEnabled = false)]` on AppService methods that have custom controllers.
   *(إلغاء الـ Auto-API: استخدم `[RemoteService(IsEnabled = false)]` على دوال الـ AppService التي لها Controller مخصص.)*
6. **Quill Image Handler**: Override via `quillModules.toolbar.handlers.image` function to open custom UI.
   *(محرر Quill: لتغيير رفع الصور، قم بعمل Override عبر دالة `handlers.image` لفتح واجهتك المخصصة.)*
7. **CmsKit Integration**: To count comments/reactions, use `IReadOnlyRepository<Comment, Guid>` with `GetQueryableAsync()`.
   *(التكامل مع CmsKit: لعد التعليقات، استخدم `IReadOnlyRepository<Comment, Guid>` بدلاً من `ICommentRepository` المعزول.)*
8. **ABP Lepton-X ≠ Angular SSR**: ABP Lepton-X v4.3 directly uses DOM APIs (`document.createElement`) during Angular DI. Workaround: CSR + Meta Tags.
   *(عدم توافق ABP SSR: ثيم Lepton-X يستخدم أوامر المتصفح مباشرة للوصول للـ DOM مما يعطل الـ SSR. الحل البديل هو CSR مع Meta Tags.)*
9. **Webpack Import Hoisting**: `import` statements are hoisted. Solution for polyfills: external launcher script.
   *(الـ Hoisting في Webpack: الـ imports تُنفذ دائماً أولاً. الحل للـ Polyfills هو سكريبت خارجي.)*
10. **Node.js v24 fetch**: Built-in `fetch` ignores `NODE_TLS_REJECT_UNAUTHORIZED` inside JS. Set via OS env variable.
    *(دالة fetch في Node 24: تتجاهل إعدادات SSL من داخل الكود. يجب تمريرها كمتغير بيئة من الـ Terminal.)*
11. **Angular 20 CommonEngine**: Requires `allowedHosts` in constructor. `ng build` only builds browser.
    *(نجاح הـ SSR يتطلب `allowedHosts`، وأمر البناء العادي يبني المتصفح فقط.)*

