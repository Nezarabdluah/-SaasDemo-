# 🗺️ نظام مهام المشروع الشامل (Master Tasks Board)

## المرحلة الأولى: تنظيف الصلاحيات وإعادة الهيكلة ✅
- [x] إيقاف `Blogging` وترك `Comments`, `Reactions`, `Tags`.
- [x] تنظيف طبقات الـ EntityFramework.

## المرحلة الثانية: الاحترافية في BlogPost ⏳
- [x] إضافة حقول النشر (Status, PublishedAt, Cover).
- [x] بناء كيان وضبط كيانات تصنيفات المدونة (BlogCategory & Join Table).
- [x] ربط بيانات التصنيف في الواجهات الأمامية Angular.
- [x] بناء نظام الـ Tags (BlogTag entity, AppService, Angular UI, Many-to-Many).
- [x] إصلاح خطأ ObjectMapper/AutoMapper DI (Manual Mapping لجميع AppServices).
- [x] كتابة اختبارات وحدة BlogTag (7 Domain + 5 AppService).
- [x] دمج محرر نصوص احترافي (Quill Editor) في واجهة Angular.
- [ ] استهلاك CmsKit Comments API لبناء واجهة التعليقات.

## المرحلة الثالثة: SEO الاحترافي الكامل
- [ ] إضافة حقول `MetaTitle`, `MetaDesc`, `OgImage` لكيانات النشر.
- [ ] إعداد Angular SSR (Server-Side Rendering) لتمكين زواحف جوجل.
- [ ] برمجة Endpoints لتوليد `sitemap.xml` و `robots.txt` ديناميكياً.

## المرحلة الرابعة: إعدادات الموقع المركزية (Site Settings)
- [ ] تصميم كيان `SiteSettings` (الاسم، الألوان، الشعار).
- [ ] إعدادات التواصل الاجتماعي `SocialLinks` والبريد الإلكتروني `EmailSettings`.
- [ ] واجهة تحكم ديناميكية (Dynamic UI) لإدارة معلومات الـ Footer.

## المرحلة الخامسة: بنية الـ SaaS وخطط الاشتراك (Subscription Plans)
- [ ] بناء كيان الـ `Plan` ومصفوفة الصالحيات وحدود الاستهلاك (Features Limits).
- [ ] تسجيل الاشتراكات `TenantSubscription`.
- [ ] تأسيس Stripe Payment Gateway للفوترة الدورية (Webhooks).
- [ ] برمجة حراس (Interceptors) لمنع تجاوز الاستهلاك لكل مستأجر.

## المرحلة السادسة: المتجر الإلكتروني (E-commerce Core)
- [ ] بناء كيانات `Product` و `ProductCategory`.
- [ ] تصميم `ProductVariant` وتتبع المخزون `Inventory`.
- [ ] برمجة سلة المشتريات `Cart` بالاعتماد على التخزين الموزع (Redis).
- [ ] المعالجة المالية المباشرة باستخدام `Stripe One-Time Payments`.
- [ ] إنشاء `Order` و `OrderItem` وتوليد الفواتير.

## المرحلة السابعة: الموقع العام للعملاء (Public UI)
- [ ] Landing Page ترويجية ديناميكية تعرض خطط الـ SaaS.
- [ ] استدعاء معلومات من (CmsKit Pages API) لعرض الصفحات الثابتة بمرونة.

## المرحلة الثامنة: لوحة القيادة التحليلية (Analytics Dashboard)
- [ ] ربط مكتبة Chart.js بـ المبيعات والمقالات عبر APIs تحليليّة مخصصة.
- [ ] تصميم شاشة Super Admin Resource Monitor.
