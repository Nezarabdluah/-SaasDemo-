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
- **Completed:** Media Library Integration (Copy URL + Cover Picker + Quill Image Insert) ✅
- **Completed:** Article Statistics (Views, Comments, Reactions via CmsKit) ✅
- **Pending:** Angular SSR, Sitemap, Robots.txt

## Key Commands
- Backend: `dotnet run --project src\SaasDemo.HttpApi.Host`
- Frontend: `npm start` (in `angular`)
- Build: `dotnet build src\SaasDemo.HttpApi.Host` / `npm run build` (angular)
- Migrations: `dotnet run --project src\SaasDemo.DbMigrator` -> **CRITICAL:** Use this to re-seed Permissions after creating new entities/permissions, to avoid 403 Forbidden issues.

## Storage Configuration
- **BlobStoring Provider:** FileSystem (local dev) — configured in `SaasDemoHttpApiHostModule.cs`
- **Current Path:** `{ContentRootPath}/MediaStorage` (relative to HttpApi.Host project root)
- **Future:** Can swap to Azure Blob Storage by changing provider config only.

## Critical Patterns & Gotchas
1. **Swashbuckle + IFormFile**: NEVER use individual `[FromForm] IFormFile` params. Always create a Model class (e.g. `MediaUploadForm`).
2. **ABP Auto-API Disabling**: Use `[RemoteService(IsEnabled = false)]` on AppService methods that have custom controllers.
3. **[AllowAnonymous] vs [Authorize]**: Never put both on conflicting levels — causes ASP0026 and Swagger crash.
4. **Permission Seeding**: Run `DbMigrator` after adding new permissions. Until then, policy-based `[Authorize]` returns 403.
5. **Angular Image URLs**: Prefix with ABP `EnvironmentService` API base URL for `<img src>`, since dev server port ≠ backend port.
6. **Quill Image Handler Override**: Use `quillModules.toolbar.handlers.image` function to open custom UI instead of default prompt.
7. **CmsKit Cross-Module Integration**: Use `IReadOnlyRepository<Comment, Guid>` instead of `ICommentRepository` for aggregate root counting to access LINQ querying extensions securely without adding properties to the Domain Entity.
