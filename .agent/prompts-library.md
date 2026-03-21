---
description: Ready-to-use prompts library for AI coding assistant | مكتبة برومبتات جاهزة
---

# 📚 Prompts Library | مكتبة البرومبتات

## 🚀 بداية الجلسة | Session Start

### البرومبت الأساسي
```
/start-project
ثم أخبرني: ما المهمة التالية من current-context.md؟
```

### بدء مهمة محددة
```
/start-project
أريد العمل على [اسم الموديول/الميزة].
ما الملفات التي سأحتاج تعديلها؟
```

---

## 🔧 Backend Development (Erik Herman Methodology)

### 1. كتابة استعلامات قاعدة البيانات (EF Core)
```text
بدرجة حرارة 0.2، اكتب استعلام LINQ باستخدام EF Core لجلب [الهدف].
يجب استخدام `AsNoTracking()` لزيادة الأداء، وتأكد من أن الاستعلام يتم تنفيذه في جهة السيرفر (Server-side evaluation).
قم بتضمين (Include) جداول [الجداول المطلوبة] فقط.
تجنب جلب كافة الأعمدة عبر استخدام `Select` مخصص (Projection) لتحسين استهلاك الذاكرة.
```

### 2. تصميم دوال واجهات برمجة التطبيقات (API Design)
```text
قم بتصميم RESTful Endpoint باستخدام `[Controllers / Minimal APIs]` في .NET 8 لـ [الهدف].
يجب أن تقبل الدالة كائن `[DtoName]` فقط لضمان أمن البيانات.
قم بتضمين التحقق من الصحة (Validation) باستخدام `FluentValidation`.
يجب أن تعيد الـ API نتيجة `204 No Content` أو `200 OK` عند النجاح، 
و `400 Bad Request` مع شرح مفصل للخطأ عند فشل التحقق، مع الالتزام بمعايير `Problem Details`.
```

### 3. معالجة الأخطاء وتنظيف الموارد (Error Handling & Clean up)
```text
قم بتعديل دالة `[MethodName]` لإضافة معالجة استثناءات قوية.
يجب الإمساك بالاستثناءات المحددة بشكل منفصل (مثل `IOException` وغيرها).
اكتب في الـ Catch block كوداً يسجل الخطأ باستخدام `ILogger` مع تضمين Correlation ID.
يُحظر إعادة رسالة الخطأ التقنية (Stack Trace) للمستخدم؛ بدلاً من ذلك، أعد كود `500`.
تأكد من تنظيف الموارد (Cleanup Logic) قبل انهيار العملية.
```

### 4. إنشاء Entity / Domain Model
```text
/create-backend-module
صمم Class يمثل `[EntityName]` متبعاً قواعد الـ Clean Architecture.
يجب أن يكون الكيان `POCO` وُيمنع تماماً الاعتماد على `DbContext`.
الحقول: [قائمة الحقول]
العلاقات: [قائمة العلاقات]
استخدم `record struct` للـ Value Objects إن وجدت.
```

---

## 🎨 Frontend Development

### إنشاء List Component
```
/create-angular-module

أنشئ صفحة قائمة لـ [EntityName]:
- Load data on init
- Filters: [Filter1, Filter2]
- Pagination: server-side
- Actions: [Create, Edit, Delete]
- Follow existing list pattern
```

### إنشاء Form Component
```
أنشئ نموذج (form) لـ [EntityName]:
- Modal-based / Separate page
- Fields: [Field1, Field2, ...]
- Validation: [required, maxLength, ...]
- Bilingual inputs where needed
```

### إصلاح UI Bug
```
راجع /common-errors أولاً.

المشكلة: [وصف المشكلة]
الصفحة: [Component/Page name]
السلوك المتوقع: [...]
السلوك الحالي: [...]
```

---

## 🐛 Debugging

### خطأ 500
```
خطأ 500 في [Endpoint]:
- Full error from console/network: [...]
- Request body: [...]
- Check: AutoMapper? TenantId? Validation?

راجع /common-errors قسم 500 errors.
```

### خطأ Compilation
```
خطأ compilation:
[نص الخطأ الكامل]
الملف: [path/to/file]
السطر: [line number]
```

---

## 📊 Analysis & Planning

### تخطيط Feature جديدة
```
أريد تنفيذ [FeatureName]:

## المتطلبات
- [Requirement 1]
- [Requirement 2]

## القيود
- لا تعدّل [...]
- حافظ على backward compatibility

أعطني Implementation Plan قبل الكود.
```

---

## ✅ Review & Quality

### Code Review
```
راجع هذا الكود:
[paste code or file path]

تحقق من:
- [ ] Clean code principles
- [ ] Error handling
- [ ] Security issues
- [ ] Performance
- [ ] Bilingual support
```

### Pre-Commit Check
```
راجع التغييرات وتأكد:
1. لا يوجد console.log أو debugger
2. كل الحقول لها Localization
3. لا TODO متروكة
4. Tests pass
5. No new warnings
```

---

## 🔚 نهاية الجلسة | Session End

```
/end-session
أنهي الجلسة وحدّث كل الملفات.
```

---

## 🧠 هندسة الأوامر المتقدمة (AI Systems Architect Standards)

> مستوحى من كتاب *Optimizing Prompt Engineering for Generative AI* لضمان تحويل النموذج إلى مهندس معماري منضبط للأنظمة الموزعة (Multi-turn Agents).

### 1. بنية الـ System Prompt المثالي (The 5 Pillars)
كل أمر معقد يرسل للوكيل **يجب** أن يحتوي على:
1.  **الهوية (Identity):** تعيين دور دقيق (مثال: "بصفتك Principal .NET Architect...").
2.  **السياق (Context):** توفير تفاصيل البيئة التشغيلية وملف `current-context.md`.
3.  **التعليمات (Instructions):** ماذا تريد بالضبط بلغة موجهة نحو الفعل المباشر.
4.  **القيود الصارمة (Negative Prompting):** استخدم قوائم المنع الصريحة لمنع اختراع أدوات جديدة ("يُحظر تماماً استخدام أية مكتبات خارج القائمة البيضاء").
5.  **تنسيق المخرج (Output Format):** تحديد شكل المخرج المطالب به بوضوح تام.

### 2. تقنيات منع الهلوسة المعمارية (Zero-Hallucination)
*   **التأريض التقني (Grounding & Documentation):** لا تطلب كوداً من الفراغ. قم دائماً بإرفاق جزء من "التوثيق الرسمي" المحدث داخل البرومبت لربط المخرجات بحقائق تقنية ثابتة بعيداً عن التخمين.
*   **CoT مقابل المهام المتكررة:** استخدم *Chain-of-Thought* للمسائل المعقدة ليضع الخطط، وتجنبه في المهام المتكررة المباشرة (مثل كتابة DTOs) لحفظ الـ Tokens ومنع التوهان.
*   **Memory Injection للاتساق:** لضمان استقرار المشاريع الضخمة (Multi-turn)، قم بتمرير القرار المعماري السابق כجزء من السياق في المحادثات الجديدة لضمان عدم تناقض الكود.

### 3. القيادة والتحسين (Temperature & Few-Shot)
*   **البرودة الهندسية (Temp 0.2):** المهام البرمجية تتطلب إجابات حتمية لا تتغير (Deterministic). الإبداع مطلوب في الفن، أما الكود فيتطلب الدقة الرياضية.
*   **التشكيل بالأمثلة (Few-Shot Pattern Recognition):** بدلاً من شرح كيفية كتابة الكود، قم بتزويد الوكيل بقطعة كود "مكوّن ناجح ومثالي بنيته في مشروعك" وأمره باشتقاق النمط ذاته.

---

## 📎 مراجع سريعة

- **المعايير الهندسية** → `.agent/standards.md`
- **بنك الأخطاء** → `/common-errors`
- **قائمة المهارات** → `.agent/skills-summary.md`
- **سجل القرارات** → `.agent/decision-log.md`

---

*آخر تحديث: 2026-03-15*
