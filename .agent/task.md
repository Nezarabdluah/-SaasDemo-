# GSoC 2026 — Large Project Plan (350h)
## ABP SaaS Boilerplate: Production-Ready Open-Source Foundation

> الهدف: أول Boilerplate مفتوح المصدر يجمع ABP Framework + Angular
> بمعايير إنتاج حقيقية — يوفر على أي مطور 3 أشهر من العمل

---

## ✅ ما تم إنجازه (Pre-GSoC Foundation)
- [x] هيكل Clean Architecture + DDD كامل
- [x] نظام BlogPost (Status, Cover, Tags, Categories)
- [x] Manual Mapping بديل AutoMapper
- [x] Unit Tests (7 Domain + 5 AppService)
- [x] Quill Editor في Angular

---

## 📦 المرحلة 1: نظام المحتوى الاحترافي (CMS + SEO)
### الأسبوع 1–3 | ~90 ساعة
### المقرر سابقاً ✅
- [x] Comments UI كاملة (Nested Replies)
- [x] SEO Layer: MetaTitle, MetaDesc, OgImage
- [ ] Angular SSR
- [ ] sitemap.xml و robots.txt ديناميكياً

### 🆕 الإضافات الاحترافية

**Slug System احترافي:**
- [x] توليد URL Slug تلقائي من العنوان (`my-post-title`)
- [x] منع التكرار مع إضافة رقم تلقائي (`my-post-title-2`)
- [x] Redirect تلقائي عند تغيير الـ Slug لحماية SEO

**Reading Time & Content Stats:**
- [x] حساب وقت القراءة تلقائياً من عدد الكلمات
- [ ] إحصائيات كل مقال: المشاهدات، التعليقات، التفاعلات
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
- [ ] تحسين واجهة المكتبة: Drag & Drop للرفع + تأثيرات بصرية عند التحميل

---

## 📦 المرحلة 2: محرك الإعدادات والهوية البصرية
### الأسبوع 4–5 | ~60 ساعة

### المقرر سابقاً ✅
- [ ] SiteSettings Entity (اسم، ألوان، شعار)
- [ ] SocialLinks + EmailSettings
- [ ] Dynamic Footer
- [ ] Theme Token System بـ CSS Variables

### 🆕 الإضافات الاحترافية

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
- [ ] اختبارات تغطي سيناريوهات كاملة من A إلى Z
- [ ] استهداف تغطية 70%+ من الـ Critical Paths

---

## 📊 ملخص المراحل

| المرحلة | الساعات |
|---------|---------|
| CMS + SEO + Media | ~90h |
| Site Settings + Email Templates | ~60h |
| Subscription Plans + Trial System | ~120h |
| SignalR + Webhooks | ~50h |
| Analytics + CI/CD + Tests + Docs | ~90h |
| **المجموع** | **~410h** |

> الـ 60 ساعة الإضافية = مرونة للتعقيدات غير المتوقعة (هامش 15–20%)
