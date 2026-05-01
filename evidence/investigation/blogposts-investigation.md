# Investigation Report — SaasDemo (BlogPosts)
Date: 2026-05-01 | Investigator: AI Agent | Status: ✅ Complete

## 1. ما اكتشفته من الكود

### Roles & Permissions
| Role | الصلاحيات | القيود |
|------|-----------|--------|
| Any User (Default) | `SaasDemoPermissions.BlogPost.Default` | القراءة واستعراض القائمة |
| Editor/Admin | `SaasDemoPermissions.BlogPost.Create` | إنشاء مقالة جديدة |
| Editor/Admin | `SaasDemoPermissions.BlogPost.Update` | تعديل المقالة، إنشاء Versions جديدة |
| Admin | `SaasDemoPermissions.BlogPost.Delete` | حذف المقالة |

### Features المكتشفة
| Feature | الوصف | Operations | States | Rules |
|---------|-------|-----------|--------|-------|
| Slug Management | توليد slugs فريدة وتوجيه القديمة | Generate, Redirect | - | إذا تغير الـ slug أثناء التعديل، يُحفظ القديم في `SlugRedirect` للحفاظ على الـ SEO. |
| Versioning | أخذ لقطات تلقائية عند التعديل | Snapshot, Restore, List | - | كل عملية تحديث تحفظ نسخة من الحالة السابقة في `BlogPostVersion`. |
| Stats Tracking | إحصاء المشاهدات والوقت | Increment, Calculate | - | `CalculateReadingTime` مبنية على 200 كلمة/دقيقة. `IncrementViewCount` تعمل Fire & Forget. تعتمد على CmsKit لجلب التعليقات والتفاعلات. |

### Business Rules المستخرجة من Validators
| Rule | الشرط | النتيجة عند الخرق |
|------|-------|-----------------|
| Required Fields | Title, Slug, Content لا يمكن أن تكون فارغة | Exception من Domain (`Check.NotNullOrWhiteSpace`) |
| Auto-Slug | إذا كان `Slug` فارغاً في الإدخال | يتم توليده تلقائياً من الـ `Title`. |
| Unique Slug | الـ Slug يجب أن يكون فريداً (عبر `SlugGenerator`) | Exception من الـ DB/Domain |

### State Machines المكتشفة
| Entity | الحالات | الانتقالات المسموحة |
|--------|---------|-------------------|
| BlogPost | `PublishStatus` (Draft, Published... الخ) | يتم تعيينه عبر الـ Update/Create. لا توجد قواعد State Machine صارمة تمنع الانتقال (حرية كاملة للـ Editor). |

## 2. ما اكتشفته من قاعدة البيانات

### الجداول الرئيسية
| Table | الغرض | العلاقات | Constraints مهمة |
|-------|-------|---------|-----------------|
| BlogPost | تخزين المقالات الأساسية | Many-to-Many مع Categories و Tags. | Title و Slug إلزامية |
| BlogPostVersion | سجل تاريخ التعديلات | يتبع BlogPost (BlogPostId) | - |
| SlugRedirect | توجيه الروابط القديمة للجديدة | يتبع BlogPost (BlogPostId) | `OldSlug` فريد |

## 3. ما اكتشفته من النظام الشغّال (Frontend)

### الصفحات والـ Routes
| Route | ما تعرضه | Operations | Export? | Filter? |
|-------|---------|-----------|---------|---------|
| `/blogs` | قائمة المقالات مع Pagination | عرض، حذف | ❌ | Pagination (skipCount) |
| `/blogs/:id` | عرض تفاصيل المقالة | تحديث SEO Meta Tags ديناميكياً | ❌ | ❌ |
| `/blogs/create` | نموذج الإضافة | استخدام Quill Editor، ربط مع `MediaPicker` | ❌ | ❌ |

## 4. الثغرات في الفهم — أسئلة مطلوبة

| # | السؤال | لماذا مهم | الأولوية | الإجابة |
|---|--------|----------|---------|---------|
| 1 | هل يمكن حذف `BlogPostVersion` القديمة جداً لتوفير المساحة؟ | إذا كان الجدول سيكبر بسرعة، هل هناك Job للتنظيف؟ | تحسين | - |
| 2 | في حالة `PublishStatus.Published`، هل يتم إرسال Domain Event (مثلاً لإشعار المشتركين)؟ | حالياً لا يظهر أي Domain Event في `BlogPostAppService`. | مهم | - |

## 5. Feature DNA Analysis

| Feature | DATA | TIME | USERS | EXTERNAL | STATE | MEDIA | COMPUTATION | INTERFACE |
|---------|------|------|-------|----------|-------|-------|-------------|-----------|
| BlogPost | ✅ | ✅ | ✅ | ❌ | ✅ | ✅ | ✅ | ✅ |

### تفاصيل الأبعاد الفعّالة (✅ فقط)
| البعد | التفاصيل | Tests المولّدة |
|-------|---------|--------------|
| DATA | قراءة/كتابة، توجيه روابط، لقطات النسخ (Versions). | تعارض التحديث، التحقق من إنشاء SlugRedirect. التحقق من النسخة بعد التعديل. |
| TIME | `PublishedAt` و `CreationTime` للنسخ. | التحقق من صحة تاريخ النشر وحفظ وقت الـ Version. |
| USERS | الصلاحيات (Create, Update, Delete). | اختبارات الوصول (Authorization 403). |
| STATE | `PublishStatus`. | تغيير الحالة والحفظ بنجاح. |
| MEDIA | صور `FeaturedImageUrl` و `OgImageUrl` و Quill. | حفظ روابط الصور وعرضها بشكل صحيح وتحديث الـ Meta tags في الواجهة. |
| COMPUTATION | `CalculateReadingTime`. | اختبار مقال بـ 400 كلمة = دقيقتين. |
| INTERFACE | Quill Editor و Media Picker. | إضافة صور للـ Quill لا يكسر النموذج. Pagination يعمل. |

## 6. Discovered Test Plan

### Features × Test Types
| Feature | Logic Tests | API Tests | E2E Smoke | a11y | Security |
|---------|------------|-----------|-----------|------|---------|
| BlogPosts | `ReadingTime`, `SlugGen` | CRUD, `VersionRestore`, `SlugRedirect` | List, Detail (SEO Tags), Create (Quill) | ✅ | ✅ (XSS in Quill) |

## 7. قرار البدء
[x] كل الأسئلة الحرجة أُجيبت (بانتظار التأكيد على الأسئلة الجانبية)
[x] Investigation Report مكتمل
[x] Test Plan موافق عليه
[x] يمكن البدء بالاختبار ✅
