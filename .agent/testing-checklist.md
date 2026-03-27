---
description: Universal Pre-deployment Testing Checklist | قائمة التحقق العامة قبل النشر لأي مشروع
---

# ✅ Universal Testing Checklist | قائمة التحقق

> [!NOTE]
> This file is framework-agnostic. These standards must be met before merging any code or deploying to production.
> *(هذا الملف عام ولا يعتمد على لغة برمجة أو إطار عمل محدد. يجب الالتزام بهذه المعايير قبل دمج أي كود أو نشره في بيئة الإنتاج.)*

---

## 🔧 Before Every Commit (Repo Rules) | قبل كل Commit (قواعد المستودع)

### Code & Build | الكود والبناء
- [ ] Build succeeds without any errors.
  *(عملية البناء تنجح تماماً بدون أي أخطاء.)*
- [ ] No new warnings introduced in this PR.
  *(لا يوجد أي تحذيرات جديدة تم إضافتها في هذا البول ريكويست.)*
- [ ] All automated tests (Unit/Integration) pass (100%).
  *(جميع الاختبارات الآلية تعمل بنجاح.)*
- [ ] No orphaned commented-out code or `// TODO: FIX` left behind.
  *(لا يوجد تعليقات برمجية مهجورة أو عبارات `// TODO: FIX` متروكة في الكود المكتوب.)*
- [ ] No debug print statements (e.g., `console.log`, `print`) left in the final code.
  *(لا يوجد جمل الطباعة المخصصة للاختبار متروكة في الكود النهائي.)*

---

## 🆕 Before Adding a New Feature/Entity | قبل إضافة ميزة أو كائن جديد

### Architecture Basics | الأساسيات المعمارية
- [ ] Adheres to the approved project architecture (Clean Architecture, MVC, etc.).
  *(التقيد بهيكلية المشروع المعتمدة.)*
- [ ] DTOs are present for proper abstraction between APIs and databases.
  *(وجود ملفات DTOs للفصل الاستيعابي بين واجهات الـ API وقواعد البيانات.)*
- [ ] Complex functions/methods are properly documented.
  *(توثيق الدوال المعقدة بالشكل السليم.)*

### Database & Data | قواعد البيانات والبيانات
- [ ] Relevant DB migration is created if needed.
  *(تم إنشاء ملف التهجير الخاص بالميزة إن لزم الأمر.)*
- [ ] Audit trail fields (CreatedDate, Creator, ModifiedDate) are present for core data.
  *(وجود حقول تتبع المراجعة للبيانات الهامة.)*
- [ ] Core paths are covered by Unit Tests.
  *(تم تغطية جميع الحالات الأساسية باختبارات الوحدة.)*

---

## 🎨 UI/UX & Frontend | واجهات المستخدم وتجربة المستخدم

### Quality & Reliability | الجودة والاعتمادية
- [ ] UI is fully responsive across supported screen sizes.
  *(الواجهة تستجيب لجميع أحجام الشاشات المعتمدة.)*
- [ ] Proper loading states (spinners/skeletons) during data fetching or form submission.
  *(التعامل السليم مع حالات الـ Loading.)*
- [ ] Proper error handling with clear, user-friendly messages instead of technical traces or blank screens.
  *(التعامل السليم مع الأخطاء وإظهار رسائل واضحة ومفهومة للمستخدم.)*
- [ ] "Empty states" are properly handled when no data exists.
  *(التعامل مع حالات "البيانات الفارغة".)*
- [ ] Console is free of TypeScript/browser runtime errors.
  *(العمل المعماري خالي من أخطاء الـ TypeScript أو المتصفح.)*

---

## 🔒 Security Checklist | قائمة التحقق الأمنية

- [ ] Exposed APIs are protected by proper Authentication/Authorization.
  *(واجهات الـ API المفتوحة محمية بصلاحيات مرور مناسبة.)*
- [ ] Server-side input validation is implemented for all user inputs.
  *(التحقق من جميع المُدخلات القادمة من المستخدم من جهة السيرفر.)*
- [ ] Secure components (passwords, tokens) are appropriately encrypted/hashed.
  *(التشفير الآمن للمعلومات الحساسة.)*
- [ ] No sensitive data or server stack traces are leaked to the client.
  *(عدم وجود تسريب لبيانات أو أخطاء السيرفر للعميل العادي.)*
- [ ] UI is protected against common vulnerabilities (e.g., XSS).
  *(حماية الواجهات من الثغرات المعتادة المكتوبة مسبقا مثل الـ XSS.)*

---

## 🌐 Localization & i18n | دعم اللغات والمحلية

- [ ] No hardcoded strings in the UI; use translation/localization files.
  *(لا تستخدم نصوص ثابتة في الواجهات؛ استخدم ملفات الترجمة.)*
- [ ] New feature works correctly when switching languages and supports RTL/LTR layouts if required.
  *(الميزة الجديدة تعمل بشكل صحيح عند تغيير لغة العرض وتدعم الاتجاهات.)*
- [ ] API error messages are backed by proper translations.
  *(رسائل الأخطاء القادمة من الـ API مدعومة بترجمة صحيحة.)*

---

## 🚀 Pre-Production Deployment | ما قبل النشر لبيئة الإنتاج

- [ ] Production environment variables are correctly set and contain no test data.
  *(الإعدادات الخاصة ببيئة الإنتاج مضبوطة ولا تحتوي على بيانات تجريبية.)*
- [ ] API endpoints point to correct production servers/ports.
  *(الـ API Endpoints تشير للخوادم والمنافذ الصحيحة للإنتاج.)*
- [ ] Core authentication flows (Login, Logout, Account Recovery) function correctly.
  *(اختبارات الدخول، تسجيل الخروج، واستعادة الحساب تعمل بشكل سليم.)*
- [ ] Verified that a minified/uglified production build is generated.
  *(التاكد من بناء الإنتاج محسّن الأداء.)*
- [ ] Error Logging System / monitoring tools are active.
  *(التأكد من تشغيل أداة المراقبة وإدارة سجلات الأخطاء.)*

---

*Note: Always review this checklist before submitting a Pull Request.*
*(ملحوظة: يُفضل دائماً مراجعة هذا الملف قبل رفع طلب الاعتماد PR الخاص بك.)*
