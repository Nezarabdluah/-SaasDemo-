# Decision Log

| Date | Decision | Reason | Impact |
|---|---|---|---|
| 2026-03-21 | Use `nezar\SQLEXPRESS` for DB | Match DbMigrator configuration. | Resolves HTTP 500 error in HttpApi.Host. |
| 2026-03-21 | Revert CmsKit Angular config | `@abp/ng.cms-kit` does not exist in v9.3.6. | Prevents TS build failures in Angular frontend. |
| 2026-03-21 | Manual DDD AppService overriding (BlogCategory) | `abphelper` generated code bypasses Entities state. | Maintains Domain Integrity and avoids AutoMapper mapping on restricted properties. |
| 2026-03-21 | Use `SaasDemo.DbMigrator` after creating UI entities | Backend caches Policies. Admin requires dynamic permission allocation. | Fixed recurring `403 Forbidden` issues instantly. |
| 2026-03-26 | Install Serilog packages for professional Logging | خلاصة الكتاب 16: نظام تسجيل أحداث احترافي بدل الافتراضي. الحزم: `Serilog.AspNetCore`, `Serilog.Enrichers.Environment`, `Serilog.Enrichers.CorrelationId`. | Observability و Distributed Tracing جاهزين للتطبيق. |
| 2026-03-26 | Automate Localization file scanning | خلاصة الكتاب 16: كتابة Script يفحص ملفات اللغة ويضع `TODO` للمفاتيح الناقصة، ثم AI يترجمها تلقائياً. | يوفّر وقت الترجمة اليدوية بشكل كبير مع كل ميزة جديدة. |
