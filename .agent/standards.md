---
description: Engineering principles, enterprise standards, and quality rules — the single source of truth for HOW and WHY we build software.
---

# 🛡️ Engineering Standards & Principles

> [!IMPORTANT]
> This is the **master reference** for all coding standards. All generated code MUST comply with these rules.
> For the advanced "Agent Constitution" (Clean Architecture, DDD, CQRS, Observability), see `workflows/software-engineering-mastery-generic.md`.

---

## 1. 🏛️ المبادئ المعمارية الصارمة (Clean Architecture & SOLID)

### Clean Architecture Strict Rules
*   **اتجاه التبعية (Dependency Rule):** التبعيات تشير دائماً للداخل (Presentation → Application → Domain) و (Infrastructure → Domain).
*   **عزل الكيانات (Entity Isolation):** يُمنع ذكر أي نوع معرّف في `Infrastructure` أو `Presentation` داخل طبقة النطاق `Domain`. ❌ لا تستدعِ `DbContext` داخل الكيان أبداً.
*   **الاعتماد على التجريد (Dependency Inversion):** طبقة التطبيق لا تعتمد على `SqlConnection`، بل على `IRepository`. استخدم Interfaces في الـ Domain وتفاصيلها في الـ Infrastructure.
*   **Command vs Query (CQRS الأساسي):** الـ Command يغير الحالة ولا يعيد بيانات (Void/Task)، والـ Query يقرأ البيانات ولا يغير الحالة قط.

### SOLID in .NET (Review Checklist)
1.  **S - Single Responsibility:** الفئة يجب أن يكون لها "سبب واحد للتغيير". ❌ فئة `OrderProcessor` تحفظ البيانات وترسل إيميل. ✅ افصلها لـ `OrderRepository` و `EmailService`.
2.  **O - Open/Closed:** الكود مفتوح للإضافة مغلق للتعديل. ❌ كثرة الـ `switch(type)`. ✅ استخدم `Interfaces` وأنماط كالـ Strategy.
3.  **L - Liskov Substitution:** لا تكسر العقد. ❌ صنف يرث ويرمي `NotImplementedException`. ✅ افصل الواجهات.
4.  **I - Interface Segregation:** واجهات صغيرة لغرض واحد. ❌ `IMachine` فيه طباعة، فاكس، وتصوير. ✅ قطّعها إلى `IPrinter`, `IFaxer`.
5.  **D - Dependency Inversion:** لا تعتمد على Concrete classes. ✅ احقن الواجهات عبر `Constructor Injection`.

---

## 2. 🚀 Backend Performance & Scalability (Non-Negotiable)

### Database Interactions (The "No-Crash" Rules)
1. **Strict Server-Side Pagination:**
   * ❌ NEVER load an entire table without filtering.
   * ✅ ALWAYS use pagination (Skip/Take or equivalent).
   * *Why?* Loading 100k rows into memory will crash the server (OOM).

2. **Read-Only Optimization:**
   * ✅ ALWAYS use read-only queries (e.g., `.AsNoTracking()`, read replicas) for list/detail operations.
   * *Why?* Reduces memory overhead by disabling change tracking.

3. **Prevent N+1 Queries:**
   * ❌ DO NOT loop through entities and query DB inside the loop.
   * ✅ Use eager loading for required relationships or explicit loading where needed.

4. **Async Everywhere:**
   * ✅ ALL database operations MUST be asynchronous.
   * ❌ NEVER block async methods synchronously.

---

## 3. 🛢️ Advanced Database Performance & Indexing (SQL Server / Azure SQL)

> **المراجع المعتمدة:** 
> - *SQL Server Execution Plans (Grant Fritchey)*
> - *Expert Performance Indexing in Azure SQL (Edward Pollack & Jason Strate)*

### 1. تصميم الفهارس وتجنب الـ Lookups (Indexing Mastery)
* ✅ **Covering Indexes:** صمم الفهارس (Non-Clustered Indexes) لتشمل (Include) الأعمدة المطلوبة في الـ `SELECT` لتجنب مشكلة الـ **Key Lookups** / **RID Lookups** المكلفة جداً.
* ✅ **SARGability:** استعلاماتك يجب أن تكون SARGable (Search Argument Able).
  * ❌ تجنب استخدام الدوال على الأعمدة: `WHERE YEAR(CreatedDate) = 2023` (هذا يعطل الفهرس تماماً ويسبب Scan).
  * ✅ البديل الصحيح: `WHERE CreatedDate >= '2023-01-01' AND CreatedDate < '2024-01-01'`
* ✅ **التوازن (Write vs Read):** الفهارس تسرّع القراءة لكنها تبطئ عمليات الكتابة (INSERT/UPDATE/DELETE). راقب `Index Usage Stats` وقم بإزالة الفهارس غير المستخدمة لتخفيف الحمل.

### 2. خطط التنفيذ والأداء (Execution Plans & Query Tuning)
* ❌ **التحويل الضمني (Implicit Conversions):** لا تمرر المتغيرات بنوع بيانات مختلف عن الحقل في قاعدة البيانات (مثل تمرير `NVARCHAR` لعمود `VARCHAR` في Entity Framework). التحويل الضمني يُظهر تحذيراً في خطة التنفيذ ويمنع الـ Index Seek!
* ❌ **Spill to TempDB:** راقب تحذيرات الـ Sort و Hash Match في خطط التنفيذ. إذا ظهر تحذير Spill، فهذا يعني أن الإحصائيات (Statistics) قديمة ويجب تحديثها ليعرف الـ Optimizer حجم الذاكرة المطلوبة.
* ✅ **Scans vs Seeks:** الـ **Index Seek** هو الهدف الذي يجب أن تراه في خطة التنفيذ. وجود **Index Scan** على جدول ضخم (ملايين السجلات) يعتبر كارثة أداء تعني أن الفهرس مفقود أو الاستعلام غير SARGable.

---

## 4. 🔐 Application Security (OWASP & WAHH Strict Standards)

### 1. JWT & Session Management (Zero Trust)
*   **Storage Ban:** ❌ NEVER store JWTs in `localStorage` or `sessionStorage` (immediate XSS vulnerability). ✅ ALWAYS store them in `HttpOnly`, `Secure`, `SameSite=Strict` cookies.
*   **Refresh Token Rotation:** ✅ Issue a new Refresh Token on every request. If an old token is reused, REVOKE the entire token family immediately (Compromised Alert).
*   **Algorithm Confusion Setup:** ✅ Explicitly define `ValidAlgorithms = new[] { SecurityAlgorithms.RsaSha256 }` in `TokenValidationParameters` to prevent "alg: none" bypasses.

### 2. Broken Access Control & IDOR Prevention
*   **Horizontal Escalation (IDOR):** ❌ Do not assume a logged-in user owns the requested record. ✅ ALWAYS enforce Ownership/Tenant Checks at the DB level: `_db.Records.FirstOrDefault(r => r.Id == id && r.OwnerUserId == currentUserId)`.
*   **Resource-Based Authorization:** ✅ For complex ownership business rules, use .NET `IAuthorizationService` and `AuthorizationHandler` rather than cluttering Controllers with `if` statements.

### 3. Injection & Advanced XSS (Frontend & Backend)
*   **Second-Order SQLi:** ❌ Do not trust data just because it was retrieved from your own DB. ✅ Always use Parameterized Queries/ORM, even when manipulating internal system data.
*   **SPA DOM XSS Blacklist:** ❌ NEVER use React's `dangerouslySetInnerHTML`, Angular's `bypassSecurityTrustHtml`, or Vanilla `innerHTML` / `eval()`. ✅ Use strict `.textContent` or standard template bindings.

### 4. Bilingual Compliance
*   ✅ Every user-facing text field SHOULD have a corresponding localized field. APIs should return localized messages based on `Accept-Language`.

---

## 4. 🎨 Frontend Optimization

1. **Rendering Performance:**
   * ✅ Use optimized change detection strategies for list components.
   * ✅ Use `trackBy` in list rendering to prevent unnecessary DOM re-renders.
   * ✅ Use server-side filtering; never filter large arrays in the browser.

2. **Modal Dialogs:**
   * ✅ Use change detection triggers for state updates in modals to prevent timing errors.

---

## 5. 🗃️ Caching Strategy

1. **Lookup Data:**
   * ✅ Cache frequently accessed lookups (Countries, Cities, Currencies).
   * ✅ Cache duration: 5-15 min for static data.

2. **API Responses:**
   * ✅ Use response caching for public GET endpoints that rarely change.

---

## 6. 📊 Logging & Monitoring

1. **Performance Logging:**
   * ✅ Log slow queries (> 1 second).
   * ✅ Add performance counters for critical APIs.

2. **Error Handling:**
   * ✅ Use structured logging.
   * ✅ Never expose stack traces to users in production.

---

## 7. التفكير الأدائي (Performance Mindset)

### السؤال الذهبي قبل كل Query
```
🤔 "إذا كان في الجدول مليون سجل، هل سيعمل هذا؟"
```

### Lazy vs Eager Loading
| النوع | متى تستخدم |
|----------|-----------|
| **Eager** | عندما تحتاج العلاقة دائماً |
| **Lazy** | ❌ تجنب — يسبب N+1 |
| **Explicit** | عندما تحتاج العلاقة أحياناً |

---

## 8. 🧊 Domain-Driven Design (DDD) & Separation of Concerns

### DDD Core Rules in .NET
*   **Aggregate Roots:** للجذر هوية عالمية (Global Identity)، وهو المسؤول الأوحد عن التعديلات وضمان القواعد (Invariants) للكائنات التي بداخله. ❌ كائن خارجي يعدّل كائناً صغيراً داخل المجمع مباشرة.
*   **Value Objects:** للكائنات المعرفة بخصائصها لا بهويتها (كالمبلغ، الإحداثيات). ✅ استخدم التكوين الثابت في دوت نت: `public readonly record struct` لضمان الـ Immutability.
*   **Domain Events vs Integration:** 
    *   **Domain Event:** يطلق داخل الـ Bounded Context للإشعار بتغيير.
    *   **Integration Event:** يطلق عبر Message Bus للأنظمة الخارجية.
*   **Bounded Contexts:** حدود النطاق تُحدد باختلاف المعنى (Ubiquitous Language). إذا اختلف المعنى لكلمة، يجب فصل السياق.

### Layers Flow
```text
Presentation Layer (API / Frontend)
    ↓
Application Layer (Use Cases, Commands, Queries)
    ↓
Domain Layer (Entities, Value Objects, Domain Events, Repository Interfaces)
    ↑
Infrastructure Layer (DbContext, External APIs, Repository Implementations)
```

---

## 9. 🧪 Quality Assurance

1. **The "Stress Test" Mental Check:**
   * *"If this table has 1 million rows, will this query time out?"*
   * *"If 1000 users hit this button at once, will the DB lock?"*

2. **Code Review Checklist:**
   - [ ] هل يتبع SOLID؟
   - [ ] هل يمر "اختبار المليون سجل"؟
   - [ ] هل يوجد Localization للنصوص؟
   - [ ] هل يوجد Unit Tests؟

---

## 10. ملخص المهندس المحترف

| الجانب | القاعدة |
|--------|------------|
| **تصميم** | SOLID, Separation of Concerns |
| **أداء** | Server-side pagination, Read-only queries |
| **أمان** | Authorization, Input Validation |
| **جودة** | Unit tests, Code review |
| **نمو** | Extensible, Loosely coupled |

---

## 11. 🏛️ أسرار هندسة النظم (System Architecture Secrets)

### 1. أسرار قواعد البيانات (WAL & CDC)
*   **Sequential I/O vs Random I/O:** الداتا بيز لا تعدّل الجداول (B-Tree) مباشرة (Random I/O البطيء). بل تكتب التعديل أولاً في "دفتر يوميات" (Write-Ahead Log - WAL) كإضافة سطر جديد (Append-only) وهو (Sequential I/O السريع جداً). هذا يضمن السرعة الخارقة والأمان ضد انقطاع الكهرباء، ثم تُنقل التعديلات للجداول الحقيقية بهدوء في الخلفية.
*   **Change Data Capture (CDC):** لنقل البيانات (مثلاً إلى Kafka أو للبحث)، ❌ لا تستخدم استعلامات `SELECT` دورية ترهق الداتا بيز. ✅ استخدم أدوات CDC (مثل Debezium) التي تقرأ التغييرات مباشرة من ملف الـ Log (مثل `.ldf` في SQL Server) كـ "الشبح" دون تشكيل أي عبء على محرك قاعدة البيانات.

### 2. أسرار الـ API Gateway (YARP / ABP)
*   **Rate Limiting (خوارزمية Token Bucket):** لحماية السيرفرات من هجمات الـ DDoS والضغط العشوائي، الـ Gateway يستخدم "دلو الرموز" (Token Bucket). كل مستخدم/IP لديه عدد محدد من الطلبات (Tokens) تتجدد بريت معين. إذا استهلكها، يرفض الـ Gateway الطلبات الزائدة فوراً بـ (`429 Too Many Requests`)، مما يحمي الـ Backend من الانهيار.
*   **Load Balancing (توزيع الأحمال):** عند وجود ضغط هائل (مثل آلاف المستخدمين في نفس الوقت)، يتم استنساخ الـ Backend. الـ Gateway يوزع الطلبات بذكاء باستخدام خوارزميات مثل:
    *   **Round Robin:** التوزيع بالتناوب وبشكل عادل بين السيرفرات.
    *   **Least Connections:** توجيه الطلب الجديد للسيرفر الأقل انشغالاً (الأقل اتصالات نشطة) لضمان أعلى سرعة استجابة.

---

*آخر تحديث: 2026-03-21*
