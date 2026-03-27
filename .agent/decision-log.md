# Decision Log

| Date | Decision | Reason & Impact |
|---|---|---|
| 2026-03-21 | Use `nezar\SQLEXPRESS` for DB<br>*(استخدام SQLEXPRESS لقاعدة البيانات)* | Match DbMigrator configuration. Resolves HTTP 500.<br>*(لمطابقة إعدادات DbMigrator وحل مشكلة HTTP 500.)* |
| 2026-03-21 | Revert CmsKit Angular config<br>*(إزالة إعدادات CmsKit من Angular)* | `@abp/ng.cms-kit` does not exist in v9.3.6.<br>*(الحزمة غير موجودة في هذه النسخة، مما يمنع فشل البناء.)* |
| 2026-03-21 | Manual DDD AppService overriding<br>*(تجاوز دوال AppService يدوياً)* | `abphelper` code bypasses Entities state.<br>*(للحفاظ على سلامة الـ Domain Entity وعدم استخدام AutoMapper في الخصائص المحمية.)* |
| 2026-03-21 | Use `SaasDemo.DbMigrator` after creating UI entities<br>*(استخدام DbMigrator بعد إنشاء الكيانات)* | Admin requires dynamic permission allocation. Fixed 403.<br>*(لوحة التحكم تحتاج توزيع الصلاحيات ديناميكياً. يحل مشكلة 403.)* |
| 2026-03-26 | Install Serilog packages<br>*(تثبيت حزم Serilog للتسجيل الاحترافي)* | Professional logging system. Observability ready.<br>*(نظام تسجيل احترافي يسهل تتبع الأخطاء كبديل للافتراضي.)* |
| 2026-03-26 | Automate Localization file scanning<br>*(أتمتة فحص ملفات الترجمة)* | Script finds missing keys and AI translates them.<br>*(يوفر وقت الترجمة اليدوية بشكل كبير مع كل ميزة جديدة.)* |
| 2026-03-27 | Revert Angular SSR — ABP Lepton-X incompatible<br>*(التراجع عن SSR لعدم توافق Lepton-X)* | Lepton-X uses direct DOM APIs (`document.createElement`). Crashing Node.js SSR. CSR + Meta Tags used instead.<br>*(الثيم يستخدم أوامر المتصفح مباشرة مما يعطل Node.js. البديل CSR مع Meta Tags.)* |
| 2026-03-27 | Use Angular `Title` + `Meta` services<br>*(استخدام خدمات Angular للـ SEO)* | Googlebot 2024+ executes JS. Dynamic Meta Tags work.<br>*(عناكب بحث جوجل الحديثة تشغل JS كاملاً مما يجعل الـ Meta Tags الفعالة كافية.)* |
| 2026-03-27 | `npm run build` ≠ Server build<br>*(أمر البناء لا يشمل السيرفر)* | `ng build` compiles browser only. Server needs separate script.<br>*(أمر הבנاء يقتصر على المتصفح، السيرفر يحتاج أمر مخصص.)* |
