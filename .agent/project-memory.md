# Project Memory

## Project Overview
**Project Name:** SaasDemo
**Tech Stack:** ABP Framework 9.3.6 (ASP.NET Core Backend, Angular Frontend)
**Database Type:** SQL Server
**Architecture Style:** Clean Architecture (DDD, CQRS)

## Module Registry
- **Completed:** Identity, Account, Tenant Management, Feature Management, Setting Management.
- **Completed:** Volo.CmsKit ✅ مثبت
- **Completed:** BlogPost Entity + Backend CRUD + Angular UI ✅
- **Completed:** BlogCategory Entity + Backend (Custom DDD AppService) + Linked to BlogPost UI ✅
- **Pending:** BlogTag Generation and UI Integration.

## Key Commands
- Backend: `dotnet run` (in `SaasDemo.HttpApi.Host` or run via Visual Studio)
- Frontend: `npm start` (in `angular`)
- Migrations: `dotnet run` (in `SaasDemo.DbMigrator`) -> **CRITICAL:** Use this command to re-seed Permissions after creating new entities, to avoid 403 Forbidden issues.
