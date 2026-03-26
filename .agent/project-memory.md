# Project Memory

## Project Overview
**Project Name:** SaasDemo
**Tech Stack:** ABP Framework 9.3.6 (ASP.NET Core Backend, Angular 17+ Frontend)
**Database Type:** SQL Server
**Architecture Style:** Clean Architecture (DDD, CQRS)

## Module Registry
- **Completed:** Identity, Account, Tenant Management, Feature Management, Setting Management.
- **Completed:** Volo.CmsKit ✅ مثبت (Comments system)
- **Completed:** BlogPost Entity + Backend CRUD + Angular UI ✅
- **Completed:** BlogCategory Entity + Backend (Custom DDD AppService) + Linked to BlogPost UI ✅
- **Completed:** BlogTag Entity + Backend CRUD ✅
- **Completed:** Content Versioning (BlogPostVersion) ✅
- **Completed:** Media Library (MediaFile + BlobStoring + Upload/Grid/Delete) ✅
- **In Progress:** Media Library Integration (Cover Picker, Quill Insert, Copy URL)
- **Pending:** Angular SSR, Sitemap, Robots.txt

## Key Commands
- Backend: `dotnet run --project src\SaasDemo.HttpApi.Host` (or run via Visual Studio / IIS Express)
- Frontend: `npm start` (in `angular`)
- Build: `dotnet build src\SaasDemo.HttpApi.Host`
- Migrations: `dotnet run --project src\SaasDemo.DbMigrator` -> **CRITICAL:** Use this to re-seed Permissions after creating new entities/permissions, to avoid 403 Forbidden issues.

## Storage Configuration
- **BlobStoring Provider:** FileSystem (local dev) — configured in `SaasDemoHttpApiHostModule.cs`
- **Current Path:** `{ContentRootPath}/MediaStorage` (relative to HttpApi.Host project root)
- **Future:** Can swap to Azure Blob Storage by changing provider config only.

## Critical Patterns & Gotchas
1. **Swashbuckle + IFormFile**: NEVER use individual `[FromForm] IFormFile` params in controllers. Always create a Model class (e.g. `MediaUploadForm`) and bind with `[FromForm] ModelClass form`.
2. **ABP Auto-API Disabling**: When creating custom controllers for AppService methods, add `[RemoteService(IsEnabled = false)]` to the AppService interface method to prevent ABP from generating a conflicting API route.
3. **[AllowAnonymous] vs [Authorize]**: Never put `[AllowAnonymous]` on a class level and `[Authorize]` on a method level — causes ASP0026 warning and Swagger crash.
4. **Permission Seeding**: After adding new permissions in `SaasDemoPermissions.cs` and `SaasDemoPermissionDefinitionProvider.cs`, MUST run `DbMigrator` to seed them into the database. Until then, all policy-based `[Authorize]` will return 403.
5. **Angular Image URLs**: For `<img src>` tags, always prefix with the ABP Environment API base URL via `EnvironmentService`, since Angular dev server (port 4200) != Backend (port 44368).
