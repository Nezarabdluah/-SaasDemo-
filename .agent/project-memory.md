# Project Memory

## Project Overview
**Project Name:** SaasDemo
**Tech Stack:** ABP Framework 9.3.6 (ASP.NET Core Backend, Angular 17+ Frontend)
**Database Type:** SQL Server
**Architecture Style:** Clean Architecture (DDD, CQRS)

## Module Registry
- **Completed:** Identity, Account, Tenant Management, Feature Management, Setting Management ✅
- **Completed:** Volo.CmsKit (Comments system) ✅
- **Completed:** BlogPost Entity + Backend CRUD + Angular UI ✅
- **Completed:** BlogCategory + BlogTag Entities + Backend + UI ✅
- **Completed:** Content Versioning (BlogPostVersion) ✅
- **Completed:** Media Library (MediaFile + BlobStoring + Upload/Grid/Delete) ✅
- **Completed:** Media Library Integration (Copy URL + Cover Picker + Quill Image Insert + Drag & Drop) ✅
- **Completed:** Article Statistics (Views, Comments, Reactions via CmsKit) ✅
- **Completed:** SEO Meta Tags (Title + OG:title + OG:description + OG:image) in BlogDetailComponent ✅
- **Blocked:** Angular SSR ⛔ (ABP Lepton-X غير متوافق — DOM manipulation in DI)
- **Pending:** Sitemap.xml, Robots.txt

## Key Commands
- Backend: `dotnet run --project src\SaasDemo.HttpApi.Host`
- Frontend: `npm start` (in `angular`)
- Build: `dotnet build src\SaasDemo.HttpApi.Host` / `npm run build` (angular)
- Migrations: `dotnet run --project src\SaasDemo.DbMigrator` -> **CRITICAL:** Use this to re-seed Permissions after creating new entities/permissions, to avoid 403 Forbidden issues.

## Storage Configuration
- **BlobStoring Provider:** FileSystem (local dev) — configured in `SaasDemoHttpApiHostModule.cs`
- **Current Path:** `{ContentRootPath}/MediaStorage` (relative to HttpApi.Host project root)
- **Future:** Can swap to Azure Blob Storage by changing provider config only.

## Critical Patterns & Gotchas | الأنماط الحرجة والمشاكل الشائعة
1. **Swashbuckle + IFormFile**: NEVER use individual `[FromForm] IFormFile` params. Always create a Model class.
   *(لا تستخدم `[FromForm] IFormFile` بشكل مفرد في Swagger أبداً. قم بإنشاء كلاس Model مخصص.)*
2. **ABP Auto-API Disabling**: Use `[RemoteService(IsEnabled = false)]` on AppService methods that have custom controllers.
   *(لإلغاء دوال الـ API التلقائية من ABP، استخدم `[RemoteService(IsEnabled = false)]` لتلك الدوال.)*
3. **[AllowAnonymous] vs [Authorize]**: Never put both on conflicting levels — causes ASP0026 and Swagger crash.
   *(لا تضع `[AllowAnonymous]` و `[Authorize]` معاً في مستويات متعارضة حتى لا يتوقف Swagger عن العمل.)*
4. **Permission Seeding**: Run `DbMigrator` after adding new permissions. Until then, you get 403.
   *(شغّل `DbMigrator` بعد إضافة أي صلاحيات جديدة في الكود، وإلا ستحصل على خطأ 403.)*
5. **Angular Image URLs**: Prefix with ABP `EnvironmentService` API base URL for `<img src>`.
   *(ضع رابط الـ API الخاص بـ `EnvironmentService` قبل أي رابط صورة `<img src>` في Angular.)*
6. **Quill Image Handler Override**: Use `quillModules.toolbar.handlers.image` to open custom UI.
   *(لتجاوز رفع الصور في Quill، استخدم الخاصية `handlers.image` لربطها بواجهتك.)*
7. **CmsKit Cross-Module Integration**: Use `IReadOnlyRepository<Comment, Guid>` for aggregations.
   *(استخدم `IReadOnlyRepository<Comment, Guid>` للحصول على استعلامات أفضل من الـ `ICommentRepository`.)*
8. **Native Angular Drag & Drop**: Build a custom `@Directive('[appDragDrop]')` instead of heavy libraries.
   *(اصنع `@Directive('[appDragDrop]')` مخصص بدلاً من استخدام مكتبات ثقيلة لأجل رفع الملفات.)*
9. **ABP Lepton-X ≠ Angular SSR**: Uses DOM directly. Workaround: CSR + Meta Tags.
   *(ثيم ABP Lepton-X لا يدعم الـ SSR بسبب تعامله المباشر مع הـ DOM. استخدم CSR مع صفحات Meta Tags بدلاً منه.)*
