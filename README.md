# SaasDemo: Production-Grade ABP & Angular Boilerplate 🚀

[![Build](https://img.shields.io/badge/build-passing-brightgreen)](https://github.com/)
[![Tests](https://img.shields.io/badge/tests-19%20passing-green)](https://github.com/)
[![QA](https://img.shields.io/badge/QA-7%2F7%20passed-blue)](./evidence/testing/)
[![ABP Framework](https://img.shields.io/badge/ABP_Framework-9.3.6-blue.svg)](https://abp.io/)
[![Angular](https://img.shields.io/badge/Angular-17.0%2B-red.svg)](https://angular.dev/)
[![.NET Core](https://img.shields.io/badge/.NET_Core-8.0-purple.svg)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)

> **A production-grade open-source boilerplate built on ABP Framework + Angular, demonstrating enterprise patterns in a real working codebase.** 
> *(مشروع احترافي مفتوح المصدر مبني على بيئة عمل ABP Framework + Angular، يستعرض تصميمات وتطبيقات هيكلية معمارية قوية للأنظمة المؤسسية في بيئة حقيقية.)*

---

## 📖 About The Project

**SaasDemo** is a custom enterprise-grade boilerplate project built on top of the [ABP Framework](https://abp.io/) and Angular. It goes beyond standard scaffolding by manually implementing advanced enterprise patterns, proving that theoretical concepts can be beautifully translated into working code.

This project demonstrates how to properly integrate complex features like SEO, Media Libraries, Content Versioning, and dynamic Site Settings into a strict DDD environment — with **every feature tested and documented** like a real enterprise product.

---

## 📸 Feature Highlights

### ⚙️ Dynamic Site Settings — Full Admin Panel
<img src="./evidence/testing/site-settings/03-save-result.png" width="800" alt="Site Settings — Save Success" />

> **Singleton Entity pattern** with site branding, colors, logo from Media Library, social links, and SMTP configuration — all managed from one professional admin interface with real-time validation and instant save confirmation.

---

## ✨ Key Technical Achievements

- 🏛️ **Strict Clean Architecture & DDD**: Hand-crafted Entities and AppServices replacing generic scaffolding tools (`abphelper`) to maintain absolute domain integrity.
- 📝 **Professional CMS & Blogging Engine**:
  - Fully integrated **CmsKit** for nested comments and reactions.
  - Custom Content Versioning (Audit history, snapshots, and rollbacks).
  - Advanced SEO integration (Dynamic Title, OpenGraph Meta Tags for Googlebot).
  - Smart Slug generation (auto-incrementing, uniqueness handling).
- 🖼️ **Centralized Media Library**:
  - Drag & Drop uploading with visual feedback and hover animations.
  - Headless `BlobStoring` integration (locally stored, ready to swap to Azure/AWS).
  - Cross-module integration (Cover Image Picker Modal, custom Quill Editor image handlers, "Copy URL" features).
- ⚙️ **Dynamic Site Settings Engine**:
  - Singleton Entity pattern (one row in DB, auto-created on first save).
  - Site branding: Name, Primary/Secondary Colors, Logo from Media Library.
  - Social Links management (Facebook, Twitter, Instagram, LinkedIn).
  - Email/SMTP configuration with secure password handling.
- 🔧 **Debugging & Stability Techniques**:
  - Documented workarounds for ABP Lepton-X SSR incompatibilities directly in the project logs.
  - Manual permission seeding via `DbMigrator` eliminating caching and 403 authorization bugs.

---

## 🎭 Quality Assurance System

This project follows an **enterprise-grade QA process** using a custom `/playwright-qa` testing skill. Every feature goes through a structured testing workflow before it's considered complete.

### How It Works

Every feature is tested through a **6-step process**:

| Step | Action | Output |
|------|--------|--------|
| 1 | Execute structured test scenarios (P0 → P1 → P2) | Test results |
| 2 | Capture screenshots at every major step | `evidence/testing/[feature]/*.png` |
| 3 | Record full session video | `evidence/testing/[feature]/session-video.webp` |
| 4 | Generate official QA report | `evidence/testing/qa-report-[feature]-[date].md` |
| 5 | Write reusable Playwright E2E spec | `tests/e2e/[feature].spec.ts` |
| 6 | Archive all evidence in organized folders | Full audit trail |

### Testing Evidence Structure

```
📁 evidence/testing/
│
├── 📋 qa-report-site-settings-2026-04-04.md    ← Official QA Report
│   Contains: test scenarios, pass/fail status,
│   bug reports, recommendations
│
└── 📂 site-settings/                           ← Feature Evidence Folder
    ├── 01-page-load.png          → Page structure verification
    ├── 02-form-filled.png        → Data entry test
    ├── 03-save-result.png        → Save confirmation (Toast ✅)
    ├── 04-empty-validation.png   → Required field validation
    ├── 05-social-links.png       → Social links section
    ├── 06-email-settings.png     → SMTP configuration
    ├── 07-final-save.png         → Full save test
    ├── 08-reload-verify.png      → Data persistence check
    └── session-video.webp        → Full session recording 🎬

📁 tests/e2e/
└── site-settings.spec.ts                       ← Playwright E2E Spec
    Contains: 6 automated test scenarios
    (Page Load, Happy Path, Validation,
     Social Links, Email, Persistence)
```

> 💡 **All 8 screenshots + session video + QA report are available** in the [`evidence/testing/`](./evidence/testing/) folder for full transparency.

---

## 🛠️ Tech Stack

| Layer | Technology |
|-------|-----------|
| **Backend** | C#, ASP.NET Core 8, Entity Framework Core, SQL Server |
| **Frontend** | Angular 17, TypeScript, ABP Lepton-X Lite Theme |
| **Testing** | Playwright, Custom QA Skill, Structured Evidence Collection |
| **Libraries** | QuillJS, Serilog, Swashbuckle, Node.js |

---

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- Node.js (v20+) & NPM
- SQL Server LocalDB or SQLExpress (`nezar\SQLEXPRESS` by default)

### 1. Database & Migrations (Crucial)
```bash
cd src/SaasDemo.DbMigrator
dotnet run
```

### 2. Run the Backend (API)
```bash
cd src/SaasDemo.HttpApi.Host
dotnet run
```

### 3. Run the Frontend (Angular)
```bash
cd angular
npm install
npm start
```
The app will be available at `http://localhost:4200`.

---

## 📁 Project Structure

```
SaasDemo/
├── aspnet-core/                → Backend (ABP + .NET 8 + EF Core)
│   ├── src/
│   │   ├── SaasDemo.Domain/              → Entities, Repositories
│   │   ├── SaasDemo.Application/         → AppServices, AutoMapper
│   │   ├── SaasDemo.EntityFrameworkCore/  → DbContext, Migrations
│   │   └── SaasDemo.HttpApi.Host/        → API Host, Swagger
│   └── test/                             → Unit Tests
│
├── angular/                    → Frontend (Angular 17 + Lepton-X)
│   └── src/app/
│       ├── blogs/                → Blog CRUD + Detail + Editor
│       ├── media-library/        → Upload, Search, Picker Modal
│       ├── settings/             → Site Settings Admin Panel
│       └── shared/               → Reusable Components & Services
│
├── evidence/testing/           → 📊 QA Reports + Screenshots + Videos
├── tests/e2e/                  → 🧪 Playwright E2E Specs
│
├── .agent/                     → 🤖 AI Context & Documentation
│   ├── project-memory.md         → Module registry & architecture
│   ├── decision-log.md           → Why we chose X over Y
│   ├── code-nodes.md             → Entity/Service reference map
│   ├── standards.md              → Clean Architecture rules
│   ├── task.md                   → Project roadmap (350+ hours)
│   └── workflows/                → 12 custom skills (29 total)
│
└── docs/                       → 📚 Project Documentation
```

---

## 🤖 AI-Augmented Architecture

This project leverages advanced AI pair-programming methodologies to strictly enforce **Clean Architecture** and **DDD** boundaries. All architectural decisions, custom workflows (29 skills), and prompts are documented transparently in the `.agent/` directory to share our AI-engineering approach with the community.

## 📄 License
Distributed under the MIT License. Feel free to clone, explore, fork, and learn from it!
