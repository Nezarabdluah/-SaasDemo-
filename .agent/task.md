# GSoC 2026 — Large Project Plan (350h)
## ABP SaaS Boilerplate: Production-Ready Open-Source Foundation

> الهدف: أول Boilerplate مفتوح المصدر يجمع ABP Framework + Angular
> بمعايير إنتاج حقيقية — يوفر على أي مطور 3 أشهر من العمل

---

## ✅ Pre-GSoC Foundation | ما تم إنجازه
- [x] هيكل Clean Architecture + DDD كامل
- [x] نظام BlogPost (Status, Cover, Tags, Categories)
- [x] Manual Mapping بديل AutoMapper
- [x] Unit Tests (7 Domain + 5 AppService)
- [x] Quill Editor في Angular

---

## 📦 Phase 1: Professional CMS & SEO | المرحلة 1: نظام المحتوى الاحترافي
### الأسبوع 1–3 | ~90 ساعة
### Planned Features | المقرر سابقاً ✅
- [x] Comments UI كاملة (Nested Replies)
- [x] SEO Layer: MetaTitle, MetaDesc, OgImage
- [x] SEO Meta Tags (Title + OG:image + OG:description) في BlogDetailComponent
- ~~[ ] Angular SSR~~ ⛔ محظور — ABP Lepton-X غير متوافق مع SSR
- [ ] sitemap.xml و robots.txt ديناميكياً

### Pro Additions | الإضافات الاحترافية 🆕

**Slug System احترافي:**
- [x] توليد URL Slug تلقائي من العنوان (`my-post-title`)
- [x] منع التكرار مع إضافة رقم تلقائي (`my-post-title-2`)
- [x] Redirect تلقائي عند تغيير الـ Slug لحماية SEO

**Reading Time & Content Stats:**
- [x] حساب وقت القراءة تلقائياً من عدد الكلمات
- [x] إحصائيات كل مقال: المشاهدات، التعليقات، التفاعلات
- [x] View Counter بدون تأثير على الأداء (Fire & Forget pattern)

**Content Versioning:**
- [x] حفظ كل نسخة عند التعديل (Audit History للمحتوى)
- [x] استعادة نسخة قديمة بضغطة زر
- [x] مقارنة نسختين جنباً إلى جنب (Diff View)

**Media Library (الأساس):**
- [x] مكتبة صور مركزية مربوطة بـ ABP BlobStoring (FileSystem محلي، قابل للتحويل لـ Azure)
- [x] رفع الصور عبر واجهة Angular مع FormData + IFormFile
- [x] بحث داخل المكتبة + الفلترة بالاسم والمجلد (Folders)
- [x] عرض الصور في Grid مع معلومات الحجم والنوع
- [x] حذف الصور من المكتبة والـ Blob Storage
- [x] MediaController مخصص لعرض الصور مباشرة عبر URL

**Media Library Integration (الربط مع النظام):**
- [x] زر "نسخ الرابط" في بطاقة كل صورة (Copy to Clipboard)
- [x] نافذة Modal لاختيار صورة غلاف (Cover Image Picker) من المكتبة عند إنشاء/تعديل مقال
- [x] ربط المكتبة بمحرر Quill: زر إدراج صورة من المكتبة في المحتوى
- [x] تحسين واجهة المكتبة: Drag & Drop للرفع + تأثيرات بصرية عند التحميل

---

## 📦 Phase 2: Settings Engine & Theme | المرحلة 2: محرك الإعدادات والهوية البصرية
### الأسبوع 4–5 | ~60 ساعة

### Planned Features | المقرر سابقاً ✅
- [x] SiteSettings Entity (اسم، ألوان، شعار)
- [x] SocialLinks + EmailSettings
- [ ] Dynamic Footer
- [ ] Theme Token System بـ CSS Variables

### Pro Additions | الإضافات الاحترافية 🆕

**Email Template Engine:**
- [ ] نظام قوالب بريد إلكتروني قابل للتخصيص
- [ ] كل مستأجر يعدّل قالب الترحيب والفاتورة وإعادة كلمة المرور
- [ ] Preview مباشر قبل الحفظ
- [ ] متغيرات ديناميكية: `{{user.name}}`, `{{site.name}}`

**Announcement System:**
- [ ] شريط إعلانات علوي يتحكم فيه الأدمن
- [ ] إعلان بتوقيت محدد (يبدأ وينتهي تلقائياً)
- [ ] ألوان مختلفة: معلومة / تحذير / عرض خاص

**Maintenance Mode:**
- [ ] تفعيل وضع الصيانة بزر واحد
- [ ] رسالة مخصصة تظهر للزوار
- [ ] قائمة IPs مستثناة (الأدمن يدخل عادي)

---

## 📦 المرحلة 3: نظام الاشتراكات وحراسة الميزات
### الأسبوع 6–9 | ~120 ساعة

### المقرر سابقاً ✅
- [ ] Plan Entity + FeatureLimits
- [ ] TenantSubscription
- [ ] Angular Feature Guard Directive
- [ ] HTTP Interceptor لمنع تجاوز الحدود
- [ ] Stripe Webhook (Sandbox)

### 🆕 الإضافات الاحترافية

**Trial System:**
- [ ] فترة تجريبية مجانية 14 يوم لكل مستأجر جديد
- [ ] عداد تنازلي يظهر في الواجهة "تبقى 5 أيام في تجربتك"
- [ ] إشعار تلقائي قبل انتهاء التجربة بـ 3 أيام و1 يوم
- [ ] تحويل تلقائي للخطة المجانية بعد انتهاء التجربة

**Grace Period:**
- [ ] عند انتهاء الاشتراك، المستأجر يحصل على 7 أيام مهلة
- [ ] خلال المهلة: القراءة فقط (بدون إنشاء محتوى جديد)
- [ ] إشعارات تذكيرية يومية

**Usage Alerts:**
- [ ] تنبيه عند الوصول لـ 80% من الحد الأقصى
- [ ] الإشعار يظهر في الواجهة وعبر البريد الإلكتروني

**Plan Comparison Page:**
- [ ] صفحة عامة تعرض مقارنة الخطط ديناميكياً
- [ ] البيانات تأتي من API (ليست Hardcoded)

---

## 📦 المرحلة 4: الإشعارات الفورية + نظام الـ Webhooks
### الأسبوع 9–10 | ~50 ساعة 🆕

**Real-Time Notifications بـ SignalR:**
- [ ] إشعارات فورية داخل التطبيق بدون Refresh
- [ ] Bell Icon مع Counter في Navbar
- [ ] قراءة الإشعارات + تحديد الكل كمقروء
- [ ] تخزين الإشعارات مع تاريخها

**Tenant Webhook System:**
- [ ] كل مستأجر يُعرّف Webhook URL الخاص فيه
- [ ] عند حدث معين — يُرسل POST للـ URL
- [ ] Retry تلقائي عند فشل الإرسال (3 محاولات)
- [ ] Webhook Log لعرض كل الإرسالات وحالتها

---

## 📦 المرحلة 5: لوحة التحليلات + الجودة + التوثيق
### الأسبوع 11–12 | ~90 ساعة

### المقرر سابقاً ✅
- [ ] Analytics APIs (مبيعات، مقالات، مستأجرين)
- [ ] Dashboard بـ Chart.js
- [ ] Super Admin Resource Monitor
- [ ] Getting Started Guide
- [ ] Architecture Decision Records

### 🆕 الإضافات الاحترافية

**Health Check System:**
- [ ] `GET /health` يُرجع حالة كل خدمة (DB, Blob, Stripe)
- [ ] Dashboard يعرض الحالة بالألوان (🟢🟡🔴)

**Audit Log Viewer:**
- [ ] سجل كامل بكل عملية في النظام (من فعل ماذا ومتى)
- [ ] فلترة بالتاريخ والمستخدم والنوع
- [ ] تصدير بصيغة CSV

**CI/CD Pipeline جاهز:**
- [ ] ملف `github-actions.yml` جاهز مع المشروع
- [ ] عند كل Push: تشغيل Tests تلقائياً
- [ ] عند Merge للـ main: Deploy تلقائي لـ Azure

**Integration Tests:**
- [x] اختبارات الأداء كجزء من QA Pyramid (k6)
- [x] فحص الأمان OWASP (Newman)
- [ ] استهداف تغطية 70%+ من الـ Critical Paths

---

## 📦 المرحلة 6: التدقيق المؤسسي Enterprise DevOps & QA
### الأسبوع 12–13 | تم التنفيذ ✔️

### ما تم إنجازه (Audit Phase) ✅
- [x] Step 1: تصنيف المشروع (T2-High)
- [x] Step 2: تفعيل 4 Golden Signals (Grafana, InfluxDB, Prometheus)
- [x] Step 3: تنفيذ 13 اختبار من QA Pyramid (Smoke, Load, Spike, etc.)
- [x] Step 4: تصنيف الأخطاء (7 Bugs: 1 P0, 3 P1, 3 P2)
- [x] Step 5: إطار SLO/SLI/SLA
- [x] Step 6: اختبارات الأمان (OWASP + Scan)
- [x] Step 7: أداء قاعدة البيانات (Pool Health)
- [x] Step 8: بطاقة جاهزية الإنتاج (21/60 - غير جاهز)
- [x] Step 9: مقاييس DORA (Low)
- [x] Step 10: إصدار التقرير النهائي للـ QA

### خطة الإصلاح (Remediation Phase) ⏳
- [ ] 🚨 P0 (BUG-001): إصلاح أداء نقطة نهاية BlogPost List (AsNoTracking + Truncate Content)
- [ ] ⚠️ P1 (BUG-002): إضافة Security Headers
- [ ] ⚠️ P1 (BUG-003): تحديث مكتبة Scriban لإغلاق ثغرة Critical
- [ ] ⚠️ P1 (BUG-004): تفعيل Rate Limiting لحماية الـ DB Pool
- [ ] 🟡 P2 (BUG-005): حماية `/api/app/blog-post` بـ Auth
- [ ] 🟡 P2 (BUG-006): إنشاء CI/CD Pipeline
- [ ] 🟡 P2 (BUG-007): حل مشاكل `npm audit`

---

## 📊 ملخص المراحل

| المرحلة | الساعات |
|---------|---------|
| CMS + SEO + Media | ~90h |
| Site Settings + Email Templates | ~60h |
| Subscription Plans + Trial System | ~120h |
| SignalR + Webhooks | ~50h |
| Analytics + CI/CD + Tests + Docs | ~90h |
| Enterprise DevOps & QA Audit | ~20h |
| **المجموع** | **~430h** |

> الـ 60 ساعة الإضافية = مرونة للتعقيدات غير المتوقعة (هامش 15–20%)
