# SKILL: QA Master Engineer v3.2

**Version:** 3.2  
**Category:** Testing & Quality Assurance  
**Replaces:** v3.1  
**Works with:** Claude · Gemini · Copilot · Cursor · Any AI Tool  
**Distilled from:** Google Testing Blog · Microsoft SDL · ISTQB · OWASP · Playwright Docs · Newman Docs · axe-core · W3C WCAG 2.1 · Kent Beck TDD · Martin Fowler Testing Patterns

---

## CHANGELOG v3.2

| # | التغيير | السبب |
|---|---------|-------|
| 1 | RULE 00: Detective Mode قبل أي test | 90% من المشاريع ما فيها توثيق |
| 2 | Investigation Protocol كامل | المختبر يكتشف المشروع بنفسه |
| 3 | نظام الأسئلة الذكية | لا افتراضات — أسئلة محددة فقط |
| 4 | Playwright → Smoke فقط | كان بطيئًا جدًا بدون مبرر |
| 5 | Newman → الثقل الرئيسي | أسرع 20x + يغطي المنطق |
| 6 | Layer 0: Business Logic | الأهم وكان غائبًا تمامًا |
| 7 | توثيق النتائج فقط لا الخطوات | تقليل الحجم مع الاحتفاظ بالقيمة |
| 8 | Discovered Checklist بدل ثابت | كل مشروع يولّد checklists مخصصة |

---

## TABLE OF CONTENTS

1. [RULE 00 — Detective Mode](#rule00)
2. [Hard Rules](#hard-rules)
3. [Installation](#installation)
4. [Investigation Protocol](#investigation)
5. [Smart Questions System](#questions)
6. [Test Pyramid v3.2](#pyramid)
7. [Layer 0 — Business Logic](#layer0)
8. [Layer 1 — Playwright Smoke Only](#playwright)
9. [Layer 2 — Newman (الثقل الرئيسي)](#newman)
10. [Layer 3 — Unit Tests](#unit)
11. [Discovered Checklist System](#checklist)
12. [Documentation Protocol](#documentation)
13. [Post-Session Analytics](#analytics)
14. [Regression Registry](#registry)
15. [CI/CD](#cicd)
16. [Activation Prompts](#prompts)
17. [Mandatory Outputs](#outputs)
18. [Folder Structure](#structure)

---

## 1. RULE 00 — DETECTIVE MODE {#rule00}

> هذا أهم rule في الـ skill كلها — لا يُتجاوز أبدًا

```
"إذا لم تجد توثيقًا — لا تفترض.
 احفر في الكود، قاعدة البيانات، والنظام الشغّال.
 اجمع الحقائق. سجّل الثغرات. اسأل بدقة.
 
 الاختبار الأعمى أخطر من عدم الاختبار."
```

### متى تُطبّق؟
```
دائمًا — في كل مشروع — حتى لو وُجد توثيق كامل
لأن الكود هو الحقيقة الوحيدة التي لا تكذب
```

---

## 2. HARD RULES {#hard-rules}

```
RULE 00: Detective Mode أولًا — لا test قبل فهم المشروع
RULE 01: شغّل الـ installer عند أول تفعيل في أي مشروع
RULE 02: اقرأ qa-config.json قبل أي test run
RULE 03: اقرأ REGRESSION-REGISTRY.md — اعرف ما يوجد
RULE 04: feature جديدة = investigation + tests + regression كاملة
RULE 05: Playwright للـ smoke فقط — لا تثقّل بـ slowMo في الـ regression
RULE 06: صوّر النتيجة النهائية فقط — لا كل خطوة
RULE 07: فيديو عند الفشل فقط — لا تسجيل مستمر
RULE 08: P0 Auth يشتغل أولًا — دائمًا
RULE 09: فشل → screenshot فوري → retry واحد → توقف وتقرير
RULE 10: لا CSS selectors — role أو data-testid فقط
RULE 11: Newman: الـ 5 folders لكل endpoint — إلزامي
RULE 12: Layer 0 قبل Newman — المنطق قبل الـ API
RULE 13: حدّث REGRESSION-REGISTRY.md بعد كل جلسة
RULE 14: a11y على كل صفحة جديدة — ليس اختياريًا
RULE 15: Security baseline على كل form وكل auth endpoint
RULE 16: احسب Health Score بعد كل جلسة
RULE 17: نظّف Test Data بعد كل جلسة E2E
RULE 18: افحص Server Logs بعد كل test run
RULE 19: لا تبدأ الاختبار قبل إجابة الأسئلة الحرجة
RULE 20: Discovered Checklist — اكتشف ثم اختبر، لا العكس
RULE 21: Feature غير معروفة → طبّق Feature DNA الـ 8 أبعاد — لا ارتجال أبدًا
```

---

## 3. INSTALLATION {#installation}

```bash
#!/bin/bash
# QA Master v3.2 — Project Self-Installer

PROJECT_ROOT=$(pwd)
PROJECT_NAME=$(basename "$PROJECT_ROOT")
AGENT_DIR="$PROJECT_ROOT/.agent"
DATE=$(date +%Y-%m-%d)
TIME=$(date +%H:%M:%S)

echo "🔧 QA Master v3.2 — Installing in: $PROJECT_NAME"

mkdir -p "$AGENT_DIR/skills"
mkdir -p "$PROJECT_ROOT/evidence/testing/_reports/playwright"
mkdir -p "$PROJECT_ROOT/evidence/testing/_reports/api"
mkdir -p "$PROJECT_ROOT/evidence/testing/_reports/coverage"
mkdir -p "$PROJECT_ROOT/evidence/testing/screenshots/failures"
mkdir -p "$PROJECT_ROOT/evidence/testing/screenshots/system-proof"
mkdir -p "$PROJECT_ROOT/evidence/testing/flows"
mkdir -p "$PROJECT_ROOT/evidence/investigation"
mkdir -p "$PROJECT_ROOT/evidence/architecture-decisions"
mkdir -p "$PROJECT_ROOT/tests/e2e/helpers"
mkdir -p "$PROJECT_ROOT/tests/api/envs"
mkdir -p "$PROJECT_ROOT/tests/logic"

cat > "$AGENT_DIR/qa-config.json" << 'EOF'
{
  "project": "",
  "skillVersion": "3.2",
  "environment": {
    "local":   { "ui": "http://localhost:4200", "api": "http://localhost:5000" },
    "staging": { "ui": "", "api": "" }
  },
  "credentials": {
    "roles": []
  },
  "execution": {
    "dev": { "headless": false, "slowMo": 150, "workers": 1, "video": "on-failure" },
    "ci":  { "headless": true,  "slowMo": 0,   "workers": 4, "video": "on-failure" }
  },
  "performanceBudgets": {
    "pageLoad":    3000,
    "apiResponse": 1000,
    "search":      500,
    "formSubmit":  1500
  },
  "documentation": {
    "screenshots": {
      "mode": "outcomes-only",
      "captureOn": ["final-pass", "any-fail", "system-proof"]
    },
    "video": { "mode": "on-failure-only" },
    "criticalFlows": []
  },
  "a11yLevel": "wcag21aa",
  "browsers": ["chromium", "firefox"]
}
EOF

cat > "$PROJECT_ROOT/evidence/testing/REGRESSION-REGISTRY.md" << EOF
# REGRESSION REGISTRY — $PROJECT_NAME
Skill Version: 3.2 | Initialized: $DATE | Health: N/A

## FEATURES
| ID | Feature | Logic | API | E2E | a11y | Sec | Screenshots | Last Run | Health |
|----|---------|-------|-----|-----|------|-----|-------------|----------|--------|

## BUGS
| ID | Feature | Severity | Layer | Status | Found | Fixed | Root Cause |
|----|---------|----------|-------|--------|-------|-------|------------|

## TREND
| Date | Features | Pass% | Bugs | Health |
|------|----------|-------|------|--------|
| $DATE | 0 | N/A | 0 | N/A |
EOF

cat > "$PROJECT_ROOT/evidence/investigation/INVESTIGATION-TEMPLATE.md" << 'EOF'
# Investigation Report — [PROJECT NAME]
Date: | Investigator: | Status: 🔍 In Progress

## 1. ما اكتشفته من الكود

### Roles & Permissions
| Role | الصلاحيات | القيود |
|------|-----------|--------|

### Features المكتشفة
| Feature | الوصف | Operations | States | Rules |
|---------|-------|-----------|--------|-------|

### Business Rules المستخرجة من Validators
| Rule | الشرط | النتيجة عند الخرق |
|------|-------|-----------------|

### State Machines المكتشفة
| Entity | الحالات | الانتقالات المسموحة |
|--------|---------|-------------------|

## 2. ما اكتشفته من قاعدة البيانات

### الجداول الرئيسية
| Table | الغرض | العلاقات | Constraints مهمة |
|-------|-------|---------|-----------------|

### Enums / Lookup Tables
| Enum | القيم | المعنى |
|------|-------|-------|

## 3. ما اكتشفته من النظام الشغّال

### الصفحات والـ Routes
| Route | ما تعرضه | Operations | Export? | Filter? |
|-------|---------|-----------|---------|---------|

### API Calls المرصودة (Network Tab)
| Endpoint | Method | متى يُستدعى | Parameters |
|----------|--------|------------|-----------|

### أخطاء موجودة مسبقًا (Console / Network)
| الخطأ | في أي صفحة | الأثر |
|-------|-----------|-------|

## 4. الثغرات في الفهم — أسئلة مطلوبة

| # | السؤال | لماذا مهم | الأولوية | الإجابة |
|---|--------|----------|---------|---------|

## 5. Feature DNA Analysis
(يُملأ لكل feature مكتشفة — قبل بناء الـ Test Plan)

| Feature | DATA | TIME | USERS | EXTERNAL | STATE | MEDIA | COMPUTATION | INTERFACE |
|---------|------|------|-------|----------|-------|-------|-------------|-----------|
| [feature] | ✅/❌ | ✅/❌ | ✅/❌ | ✅/❌ | ✅/❌ | ✅/❌ | ✅/❌ | ✅/❌ |

### تفاصيل الأبعاد الفعّالة (✅ فقط)
| البعد | التفاصيل | Tests المولّدة |
|-------|---------|--------------|

## 6. Discovered Test Plan
(يُملأ بعد الإجابة على الأسئلة + Feature DNA)

### Features × Test Types
| Feature | Logic Tests | API Tests | E2E Smoke | a11y | Security |
|---------|------------|-----------|-----------|------|---------|

## 6. قرار البدء
[ ] كل الأسئلة الحرجة أُجيبت
[ ] Investigation Report مكتمل
[ ] Test Plan موافق عليه
[ ] يمكن البدء بالاختبار ✅
EOF

echo "✅ QA Master v3.2 installed in: $PROJECT_NAME"
echo "→ الخطوة التالية: نفّذ Investigation Protocol"
```

---

## 4. INVESTIGATION PROTOCOL {#investigation}

> نفّذ هذا بالكامل قبل كتابة أي test

### STEP 1 — قراءة الكود (Backend)

```
الترتيب الإلزامي:

□ Domain Entities
  - ما هي الكيانات الرئيسية؟
  - ما علاقاتها ببعض؟
  - ما الـ properties وأنواعها؟
  - ما الـ value objects والـ enums؟

□ Application Services / Use Cases
  - ما العمليات المتاحة؟ (Create/Update/Delete/Approve...)
  - ما الـ DTOs المطلوبة لكل عملية؟
  - ما الـ validations على كل input؟

□ Domain Events
  - ما الذي يحدث خلف الكواليس؟
  - ما الـ side effects؟ (emails / notifications / updates)

□ Permissions / Authorization
  - ما الـ permission constants المعرّفة؟
  - من يملك أي permission؟
  - هل في resource-based authorization؟

□ Database Migrations / Schema
  - ما الـ constraints الفعلية؟ (unique / not null / check)
  - ما الـ indexes؟ (يدل على ما يُبحث كثيرًا)
  - ما الـ default values؟
```

### STEP 2 — قراءة الكود (Frontend)

```
□ Routing Module
  - كل الـ routes الموجودة
  - الـ guards على كل route
  - الـ lazy loaded modules

□ Components
  - ما الذي يُعرض في كل component؟
  - ما الـ @Input و @Output؟
  - ما الـ services المُستخدمة؟

□ Services / HTTP Calls
  - كل الـ API endpoints المستدعاة
  - طريقة التعامل مع الأخطاء
  - الـ caching إن وجد

□ Models / Interfaces
  - شكل البيانات المتوقع
  - الـ optional fields
  - الـ enums المستخدمة في الـ UI
```

### STEP 3 — استكشاف النظام الشغّال

```
□ افتح كل صفحة — سجّل:
  - ماذا تعرض؟
  - ما العمليات المتاحة؟
  - هل فيها filter؟ sort؟ export؟ print؟
  - ما الـ edge cases المرئية؟

□ افتح Network Tab — لكل صفحة:
  - ما الـ API calls التي تحدث؟
  - ما الـ query parameters؟
  - ما شكل الـ responses؟

□ افتح Console — سجّل:
  - كل error موجود مسبقًا
  - كل warning
  - كل failed request

□ جرّب حالات الفشل:
  - اتصال بطيء (Network throttling)
  - API error simulation
  - بيانات فارغة
```

### STEP 4 — رسم خريطة الترابط

```markdown
## خريطة الترابط — [PROJECT]

### Data Flow
[Entity A] ──creates──> [Entity B] ──triggers──> [Notification]
[Feature X] ──depends on──> [Feature Y]

### Critical Paths (الأكثر خطورة)
1. [المسار الذي يؤثر على المال / الموافقات / الحذف]
2. [المسار الذي يمر بأكثر عدد entities]
3. [المسار الذي يُستخدم أكثر من غيره]

### Shared State (بيانات مشتركة بين features)
- [entity]: يُستخدم في [feature A, B, C]
- تغييره يؤثر على: [...]
```

---

## 5. SMART QUESTIONS SYSTEM {#questions}

> كل سؤال يجب أن يكون محددًا ومبنيًا على ما اكتشفته

### قالب السؤال الذكي

```markdown
## سؤال #[N]
**المكتشف في الكود:** [ما وجدته فعلًا]
**الثغرة في الفهم:** [ما لم أجد إجابته]
**السؤال:** [سؤال محدد جدًا]
**لماذا مهم للاختبار:** [الأثر على test plan]
**الأولوية:** حرج / مهم / تحسين
```

### أمثلة على الأسئلة الذكية

```markdown
## سؤال #1
المكتشف: Application.Status له 4 قيم: Draft/Submitted/Approved/Rejected
الثغرة: ما وجدت logic لـ Re-open بعد Rejected
السؤال: هل يمكن إعادة فتح Application مرفوضة؟ ومن يملك صلاحية ذلك؟
لماذا مهم: State machine tests ستفشل لو افترضنا خطأ
الأولوية: حرج

## سؤال #2
المكتشف: ExportService يحتوي ExportToExcel لكن لا PDF method
الثغرة: لا أعرف إذا كان PDF مخططًا أم مؤجلًا
السؤال: هل تصدير PDF مطلوب لهذه الـ feature؟ وإذا نعم — هل لها layout خاص؟
لماذا مهم: لا أضيع وقتًا في اختبار feature غير موجودة
الأولوية: مهم

## سؤال #3
المكتشف: Scholarship.MaxApplicants = nullable
الثغرة: لا أعرف السلوك المطلوب لو كان null
السؤال: إذا كان MaxApplicants فارغًا — هل معناه "بلا حد" أم "مغلق"؟
لماذا مهم: Boundary tests ستختلف كليًا بحسب الإجابة
الأولوية: حرج
```

### قاعدة الأسئلة الحرجة

```
لن يبدأ الاختبار حتى تُجاب هذه الأسئلة:
  ✓ كل ما يتعلق بـ State Transitions
  ✓ كل ما يتعلق بالأموال أو الموافقات
  ✓ كل ما يتعلق بـ Permissions وصلاحيات الحذف
  
يمكن تأجيل هذه:
  ~ أسئلة UX وتجربة المستخدم
  ~ أسئلة الـ edge cases النادرة
  ~ أسئلة التحسينات المستقبلية
```

---

## 6. TEST PYRAMID v3.2 {#pyramid}

```
                    /\
                   /E2E\          ← 5%   Playwright (Smoke + Auth فقط)
                  /──────\
                 / Newman  \      ← 45%  API + Business Scenarios
                /────────────\
               / Unit + Logic \   ← 50%  xUnit/Jest + Domain Logic
              /________________\

المنطق:
  Layer 0 (Logic)  → أسرع — milliseconds — يكتشف المنطق الخاطئ
  Layer 2 (Newman) → سريع — seconds — يكتشف الـ API والـ integration
  Layer 1 (E2E)    → بطيء — دقائق — يكتشف ما لا يراه غيره في الـ UI
```

---

## 7. LAYER 0 — BUSINESS LOGIC TESTING {#layer0}

> الطبقة الأهم — غائبة تمامًا في v3.1

### 7.1 — State Machine Tests

```csharp
// لكل entity لها states — اختبر كل انتقال ممكن وغير ممكن
// اكتشف الـ states من الكود أولًا — لا تفترض

[Theory]
// ✅ انتقالات مسموحة
[InlineData("Draft",      "Submitted",   true)]
[InlineData("Submitted",  "UnderReview", true)]
[InlineData("UnderReview","Approved",    true)]
[InlineData("UnderReview","Rejected",    true)]
// ❌ انتقالات ممنوعة — اكتشفتها من Business Rules
[InlineData("Submitted",  "Draft",       false)]
[InlineData("Approved",   "Rejected",    false)]
[InlineData("Rejected",   "Approved",    false)]
public void Entity_StateTransition_EnforcesRules(
    string from, string to, bool shouldSucceed)
{
    // arrange
    var entity = CreateEntityWithStatus(from);
    // act
    var act = () => entity.TransitionTo(ParseStatus(to));
    // assert
    if (shouldSucceed)
        act.Should().NotThrow();
    else
        act.Should().Throw<BusinessException>();
}
```

### 7.2 — Invariant Tests (القوانين التي لا تُكسر)

```csharp
// لكل قانون اكتشفته من الكود أو من الأسئلة — اختبره

// مثال: المبلغ المعتمد لا يتجاوز الميزانية
[Fact]
public void ApprovedAmount_CannotExceedTotalBudget()
{
    var entity = new Entity { TotalBudget = 5000 };
    var act = () => entity.Approve(amount: 7000);
    act.Should().Throw<BusinessException>()
       .WithMessage("*exceed*budget*");
}

// مثال: لا تقديم بعد انتهاء المهلة
[Fact]
public void Submission_IsRejected_AfterDeadline()
{
    var entity = new Entity { Deadline = DateTime.Now.AddDays(-1) };
    var act = () => entity.Submit();
    act.Should().Throw<BusinessException>()
       .WithMessage("*deadline*");
}

// مثال: لا تجاوز الطاقة الاستيعابية
[Fact]
public void Capacity_CannotBeExceeded()
{
    var entity = new Entity { MaxCapacity = 10 };
    for (int i = 0; i < 10; i++) entity.AddMember();
    var act = () => entity.AddMember(); // الـ 11
    act.Should().Throw<BusinessException>()
       .WithMessage("*capacity*full*");
}
```

### 7.3 — Decision Table Tests

```markdown
## قبل الكتابة — ارسم جدول القرار من البيانات المكتشفة:

| الشرط 1     | الشرط 2      | الشرط 3   | النتيجة     |
|------------|-------------|----------|------------|
| GPA ≥ 3.5  | Level = B   | Full-time | ✅ مؤهل    |
| GPA ≥ 3.5  | Level = B   | Part-time | ❌ غير مؤهل |
| GPA < 3.5  | Level = B   | Full-time | ❌ غير مؤهل |
| GPA ≥ 3.5  | Level = M   | Full-time | ❌ غير مؤهل |
```

```csharp
// كل صف في الجدول = test واحد
[Theory]
[InlineData(3.5, "Bachelor", true,  true)]
[InlineData(3.5, "Bachelor", false, false)]
[InlineData(2.9, "Bachelor", true,  false)]
[InlineData(3.5, "Master",   true,  false)]
public void EligibilityRule_MatchesDecisionTable(
    double gpa, string level, bool fullTime, bool expected)
{
    var result = _service.CheckEligibility(new Request
    {
        GPA = gpa, Level = level, IsFullTime = fullTime
    });
    result.IsEligible.Should().Be(expected);
}
```

### 7.4 — Data Contradiction Tests

```csharp
// اكتشف التناقضات الممكنة من خريطة الترابط

// مثال: طالب مسجّل في منحتين متعارضتين
[Fact]
public void Student_CannotHold_ConflictingScholarships()
{
    var student = new Student();
    student.AssignScholarship(fullTimeRequired);   // ✅
    var act = () => student.AssignScholarship(partTimeRequired); // ❌
    act.Should().Throw<BusinessException>()
       .WithMessage("*conflict*");
}

// مثال: تاريخ البداية بعد تاريخ النهاية
[Fact]
public void Event_StartDate_CannotBeAfterEndDate()
{
    var act = () => new Event
    {
        StartDate = DateTime.Today.AddDays(5),
        EndDate   = DateTime.Today.AddDays(1) // قبل البداية
    };
    act.Should().Throw<ArgumentException>();
}
```

### 7.5 — Full Story Tests (القصة الكاملة)

```csharp
// اختبر الـ journey كاملًا — ليس فقط خطوات منفصلة
// اكتشف القصص من الـ User Stories أو من خريطة الترابط

[Fact]
public async Task Story_CompleteJourney_FromSubmitToCompletion()
{
    // فصل 1: التقديم
    var application = await _appService.CreateAsync(validRequest);
    application.Status.Should().Be("Draft");

    // فصل 2: الإرسال
    await _appService.SubmitAsync(application.Id);
    application = await _appService.GetAsync(application.Id);
    application.Status.Should().Be("Submitted");

    // فصل 3: المراجعة — تحقق من الـ side effects
    await _reviewService.StartReviewAsync(application.Id);
    var notification = await _notificationRepo.GetLatestAsync(application.UserId);
    notification.Should().NotBeNull(); // إشعار أُرسل

    // فصل 4: الموافقة — تحقق من الـ constraints
    await _approvalService.ApproveAsync(application.Id, amount: 3000);
    var budget = await _budgetService.GetRemainingAsync();
    budget.Should().BeGreaterOrEqualTo(0); // قانون ثابت لا يُكسر

    // فصل 5: التحقق النهائي
    application = await _appService.GetAsync(application.Id);
    application.Status.Should().Be("Approved");
    application.ApprovedAmount.Should().Be(3000);
}
```

---

## 8. LAYER 1 — PLAYWRIGHT SMOKE ONLY {#playwright}

> Playwright للـ smoke فقط — لا تُثقّل بما يمكن لـ Newman أن يفعله

### متى تستخدم Playwright؟

```
✅ استخدمه فقط لـ:
  - Auth flows (login / logout / session)
  - Critical UI التي Newman أعمى عنها
  - الـ UX الـ visual (layout / responsive / RTL)
  - الـ interactions التي تعتمد على الـ DOM

❌ لا تستخدمه لـ:
  - Validation rules (Newman يكفي)
  - Business logic (Layer 0 يكفي)
  - API responses (Newman يكفي)
```

### Configuration

```typescript
// playwright.config.ts
import { defineConfig, devices } from '@playwright/test';

const isCI = process.env.CI === 'true';

export default defineConfig({
  testDir:  './tests/e2e',
  timeout:  30_000,
  retries:  1,
  workers:  isCI ? 4 : 1,
  reporter: [
    ['html', { outputFolder: 'evidence/testing/_reports/playwright' }],
    ['json', { outputFile:   'evidence/testing/_reports/results.json' }],
    ['list']
  ],
  use: {
    headless:   isCI,
    slowMo:     isCI ? 0 : 150,       // ← أسرع من v3.1 (كان 300)
    video:     'on-failure',           // ← فيديو عند الفشل فقط
    screenshot: 'off',                 // ← نتحكم نحن بالـ screenshots
    trace:     'retain-on-failure',
    baseURL:    process.env.BASE_URL ?? 'http://localhost:4200',
  },
  projects: [
    { name: 'chromium', use: { ...devices['Desktop Chrome'] } },
    { name: 'firefox',  use: { ...devices['Desktop Firefox'] } },
    { name: 'mobile',   use: { ...devices['iPhone 13'] } },
  ],
});
```

### Smoke Test Structure

```typescript
// tests/e2e/smoke.spec.ts
// هذا الملف يغطي فقط الـ critical paths التي لا يراها Newman

test.describe('P0 — Auth (Critical)', () => {
  test('Login valid → dashboard loads', async ({ page }) => { });
  test('Login invalid → specific error shown', async ({ page }) => { });
  test('Protected route without token → redirect', async ({ page }) => { });
  test('Role-based UI visibility', async ({ page }) => { });
  test('Logout → session cleared', async ({ page }) => { });
});

test.describe('P1 — Critical UI Flows', () => {
  // فقط الـ flows التي لها UI behavior خاص
  // لا تعيد اختبار ما اختبرته Newman
  test('Main happy path — user can complete primary task', async ({ page }) => { });
  test('Empty state shown when no data', async ({ page }) => { });
  test('Error state shown on API failure', async ({ page }) => { });
});

test.describe('P2 — Layout & Responsiveness', () => {
  test('Mobile layout — no broken elements', async ({ page }) => { });
  test('RTL layout correct for Arabic', async ({ page }) => { });
  test('Keyboard navigation works', async ({ page }) => { });
});
```

### Screenshot Protocol (مُعدَّل)

```typescript
// helpers/screenshot.helper.ts
// نصوّر النتيجة فقط — لا كل خطوة

export type ScreenshotType = 'system-proof' | 'failure' | 'final-result';

export async function captureOutcome(
  page: Page,
  type: ScreenshotType,
  context: { feature: string; scenario: string; note: string }
): Promise<string> {

  const timestamp = new Date().toISOString().replace(/[-:.TZ]/g, '').slice(0, 15);
  const filename  = `${context.feature}-${context.scenario}-${type}-${timestamp}.png`;

  const dir = type === 'failure'
    ? 'evidence/testing/screenshots/failures'
    : type === 'system-proof'
    ? 'evidence/testing/screenshots/system-proof'
    : `evidence/testing/screenshots/${context.feature}`;

  fs.mkdirSync(dir, { recursive: true });

  await page.screenshot({
    path:     `${dir}/${filename}`,
    fullPage: type === 'failure' // full page فقط عند الفشل
  });

  return `${dir}/${filename}`;
}

// في الـ test:
// ✅ صحيح
await captureOutcome(page, 'final-result', {
  feature: 'blog', scenario: 'create-post', note: 'Post appeared in list'
});

// ❌ خطأ — لا نصوّر كل خطوة
await page.fill('[name=title]', 'My Post');
await page.screenshot({ path: 'step-2.png' }); // ممنوع
```

---

## 9. LAYER 2 — NEWMAN (الثقل الرئيسي) {#newman}

> هذا هو قلب الاختبار في v3.2 — يجب أن يكون شاملًا

### Setup

```bash
npm install -g newman newman-reporter-htmlextra

# تشغيل feature واحدة
newman run tests/api/[feature].collection.json \
  --environment tests/api/envs/local.json \
  --reporters cli,htmlextra,json \
  --reporter-htmlextra-export evidence/testing/_reports/api/[feature]-report.html \
  --reporter-json-export evidence/testing/_reports/api/[feature]-results.json \
  --bail

# تشغيل الـ regression كاملة
newman run tests/api/regression.collection.json \
  --environment tests/api/envs/local.json \
  --reporters cli,htmlextra \
  --reporter-htmlextra-export evidence/testing/_reports/api/regression-report.html
```

### هيكل Collection الكامل — Template

```json
{
  "info": {
    "name": "[Feature] — Complete Test Suite",
    "schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
  },
  "item": [

    {
      "name": "📁 Folder 1 — Happy Path",
      "item": [
        {
          "name": "Create — Valid Data",
          "event": [
            {
              "listen": "test",
              "script": {
                "exec": [
                  "pm.test('Status 200/201', () => pm.response.to.have.status(201));",
                  "pm.test('Response time < 1000ms', () => pm.expect(pm.response.responseTime).to.be.below(1000));",
                  "pm.test('Returns id', () => pm.expect(pm.response.json()).to.have.property('id'));",
                  "pm.test('Schema matches contract', () => pm.response.to.have.jsonSchema(pm.globals.get('schema')));",
                  "pm.environment.set('createdId', pm.response.json().id);"
                ]
              }
            }
          ]
        },
        {
          "name": "Read — Get by ID",
          "event": [{ "listen": "test", "script": { "exec": [
            "pm.test('Status 200', () => pm.response.to.have.status(200));",
            "pm.test('Data matches what was created', () => {",
            "  const body = pm.response.json();",
            "  pm.expect(body.id).to.eql(pm.environment.get('createdId'));",
            "});"
          ]}}]
        },
        {
          "name": "List — Pagination",
          "event": [{ "listen": "test", "script": { "exec": [
            "pm.test('Status 200', () => pm.response.to.have.status(200));",
            "pm.test('Has items array', () => pm.expect(pm.response.json().items).to.be.an('array'));",
            "pm.test('Has totalCount', () => pm.expect(pm.response.json().totalCount).to.be.a('number'));",
            "pm.test('Respects pageSize', () => pm.expect(pm.response.json().items.length).to.be.at.most(10));"
          ]}}]
        }
      ]
    },

    {
      "name": "📁 Folder 2 — Validation & Errors",
      "item": [
        {
          "name": "Empty required fields → 400",
          "event": [{ "listen": "test", "script": { "exec": [
            "pm.test('Status 400', () => pm.response.to.have.status(400));",
            "pm.test('Error message is specific (not generic)', () => {",
            "  const body = pm.response.json();",
            "  pm.expect(body.message || body.error).to.not.include('Something went wrong');",
            "  pm.expect(body.message || body.error).to.have.length.above(10);",
            "});"
          ]}}]
        },
        {
          "name": "Beyond max length → 400",
          "event": [{ "listen": "test", "script": { "exec": [
            "pm.test('Status 400', () => pm.response.to.have.status(400));",
            "pm.test('Mentions field name in error', () => {",
            "  pm.expect(JSON.stringify(pm.response.json())).to.include('title');",
            "});"
          ]}}]
        },
        {
          "name": "Invalid data type → 400",
          "event": [{ "listen": "test", "script": { "exec": [
            "pm.test('Status 400', () => pm.response.to.have.status(400));"
          ]}}]
        }
      ]
    },

    {
      "name": "📁 Folder 3 — Auth & Authorization",
      "item": [
        {
          "name": "No token → 401",
          "event": [{ "listen": "test", "script": { "exec": [
            "pm.test('Status 401', () => pm.response.to.have.status(401));"
          ]}}]
        },
        {
          "name": "Invalid token → 401",
          "event": [{ "listen": "test", "script": { "exec": [
            "pm.test('Status 401', () => pm.response.to.have.status(401));"
          ]}}]
        },
        {
          "name": "Wrong role (user tries admin endpoint) → 403",
          "event": [{ "listen": "test", "script": { "exec": [
            "pm.test('Status 403', () => pm.response.to.have.status(403));"
          ]}}]
        },
        {
          "name": "User accesses another user's resource → 403/404",
          "event": [{ "listen": "test", "script": { "exec": [
            "pm.test('Cannot access other user resource', () => {",
            "  pm.expect(pm.response.code).to.be.oneOf([403, 404]);",
            "});"
          ]}}]
        }
      ]
    },

    {
      "name": "📁 Folder 4 — Boundary & Edge Values",
      "item": [
        {
          "name": "Minimum valid value",
          "event": [{ "listen": "test", "script": { "exec": [
            "pm.test('Status 201 — minimum accepted', () => pm.response.to.have.status(201));"
          ]}}]
        },
        {
          "name": "Maximum valid value",
          "event": [{ "listen": "test", "script": { "exec": [
            "pm.test('Status 201 — maximum accepted', () => pm.response.to.have.status(201));"
          ]}}]
        },
        {
          "name": "Maximum + 1 → rejected",
          "event": [{ "listen": "test", "script": { "exec": [
            "pm.test('Status 400 — beyond maximum rejected', () => pm.response.to.have.status(400));"
          ]}}]
        },
        {
          "name": "XSS payload in text fields",
          "event": [{ "listen": "test", "script": { "exec": [
            "pm.test('XSS not executed — escaped or rejected', () => {",
            "  pm.expect(pm.response.text()).to.not.include('<script>alert');",
            "});"
          ]}}]
        },
        {
          "name": "SQL injection in inputs",
          "event": [{ "listen": "test", "script": { "exec": [
            "pm.test('SQLi: no 500 error', () => pm.expect(pm.response.code).to.not.equal(500));",
            "pm.test('SQLi: no stack trace leaked', () => {",
            "  pm.expect(pm.response.text()).to.not.include('SqlException');",
            "  pm.expect(pm.response.text()).to.not.include('at System.');",
            "});"
          ]}}]
        },
        {
          "name": "Arabic text input",
          "event": [{ "listen": "test", "script": { "exec": [
            "pm.test('Arabic text accepted and returned correctly', () => {",
            "  const body = pm.response.json();",
            "  pm.expect(JSON.stringify(body)).to.include('\\u0627\\u0644');",
            "});"
          ]}}]
        },
        {
          "name": "Null optional fields",
          "event": [{ "listen": "test", "script": { "exec": [
            "pm.test('Null optional fields accepted', () => pm.response.to.have.status(201));"
          ]}}]
        },
        {
          "name": "Pagination — beyond last page",
          "event": [{ "listen": "test", "script": { "exec": [
            "pm.test('Beyond last page: empty items, not error', () => {",
            "  pm.expect(pm.response.code).to.equal(200);",
            "  pm.expect(pm.response.json().items).to.be.an('array').that.is.empty;",
            "});"
          ]}}]
        }
      ]
    },

    {
      "name": "📁 Folder 5 — Chained Flow (القصة الكاملة)",
      "item": [
        {
          "name": "Step 1: Create",
          "event": [{ "listen": "test", "script": { "exec": [
            "pm.test('Created successfully', () => pm.response.to.have.status(201));",
            "pm.environment.set('chainId', pm.response.json().id);"
          ]}}]
        },
        {
          "name": "Step 2: Read — Verify created data",
          "event": [{ "listen": "test", "script": { "exec": [
            "pm.test('Data persisted correctly', () => {",
            "  const body = pm.response.json();",
            "  pm.expect(body.id).to.eql(pm.environment.get('chainId'));",
            "});"
          ]}}]
        },
        {
          "name": "Step 3: Update",
          "event": [{ "listen": "test", "script": { "exec": [
            "pm.test('Updated successfully', () => pm.response.to.have.status(200));"
          ]}}]
        },
        {
          "name": "Step 4: Read — Verify update applied",
          "event": [{ "listen": "test", "script": { "exec": [
            "pm.test('Update reflected in GET', () => {",
            "  pm.expect(pm.response.json().title).to.include('Updated');",
            "});"
          ]}}]
        },
        {
          "name": "Step 5: Delete",
          "event": [{ "listen": "test", "script": { "exec": [
            "pm.test('Deleted successfully', () => pm.response.to.have.status(204));"
          ]}}]
        },
        {
          "name": "Step 6: Read after delete → 404",
          "event": [{ "listen": "test", "script": { "exec": [
            "pm.test('Deleted resource returns 404', () => pm.response.to.have.status(404));"
          ]}}]
        }
      ]
    },

    {
      "name": "📁 Folder 6 — Business Logic Scenarios",
      "description": "هذا الـ folder يُملأ بعد Investigation — مخصص لكل مشروع",
      "item": [
        {
          "name": "[DISCOVERED RULE 1] — [وصف القاعدة]",
          "event": [{ "listen": "test", "script": { "exec": [
            "// اكتب tests بناءً على ما اكتشفته من Investigation"
          ]}}]
        }
      ]
    }

  ]
}
```

### Schema Validation Helper

```javascript
// في pre-request script أو في globals
pm.globals.set('validateSchema', function(data, schema) {
  const errors = [];

  // Check required fields
  if (schema.required) {
    schema.required.forEach(field => {
      if (data[field] === undefined || data[field] === null) {
        errors.push(`Missing required field: ${field}`);
      }
    });
  }

  // Check types
  if (schema.properties) {
    Object.entries(schema.properties).forEach(([key, def]) => {
      if (data[key] !== undefined) {
        const actualType = typeof data[key];
        if (def.type === 'array' && !Array.isArray(data[key])) {
          errors.push(`${key}: expected array, got ${actualType}`);
        } else if (def.type !== 'array' && actualType !== def.type) {
          errors.push(`${key}: expected ${def.type}, got ${actualType}`);
        }
      }
    });
  }

  return errors;
});
```

---

## 10. LAYER 3 — UNIT TESTS {#unit}

### Coverage Thresholds

```json
{
  "coverageThreshold": {
    "global": {
      "branches":  65,
      "functions": 75,
      "lines":     75
    }
  }
}
```

### Checklist لكل AppService

```
  ✅ Happy path → expected output
  ✅ كل Business Rule لها test منفصل
  ✅ Guard clauses (null / empty / invalid) → exception صحيح
  ✅ Boundary values
  ✅ Permission checks (mock ICurrentUser)
  ✅ Domain events published بعد العمليات
  ✅ Idempotency — نفس العملية مرتين → نفس النتيجة
```

### Checklist لكل Angular Component

```
  ✅ يُعرض بدون error
  ✅ @Input bindings ظاهرة في الـ template
  ✅ @Output events تُرسَل
  ✅ Service calls مُحاكاة (mocked)
  ✅ Loading state يُعرض
  ✅ Error state يُعرض
  ✅ Empty state يُعرض
```

---

## 11. DISCOVERED CHECKLIST SYSTEM {#checklist}

> لا checklist ثابت — كل مشروع يولّد checklists مخصصة من Investigation

### كيف تعمل؟

```
1. نفّذ Investigation Protocol
2. لكل feature مكتشفة → طبّق السؤال التالي:

   "ما الذي يمكن أن يحدث لهذه الـ feature؟"
   
3. الإجابة تولّد الـ checklist تلقائيًا
```

### Feature DNA Analysis — يعمل مع أي feature حتى غير المعروفة

> هذا النظام يأتي **قبل** الـ Universal Triggers
> يضمن تغطية أي feature — حتى لو لم تُذكر في هذا الملف أبدًا

```markdown
## Feature DNA — 8 أبعاد لأي feature

بعد Investigation، لكل feature مكتشفة اسأل الـ 8 أبعاد:
كل ✅ يولّد checklist مخصصة — كل ❌ يُحذف من الخطة

| البعد | السؤال الجوهري | لو ✅ — اختبر |
|-------|--------------|-------------|
| 1. DATA | هل تقرأ أو تكتب بيانات؟ | 0 records / 1 / كثير / تالفة / حساسة |
| 2. TIME | هل فيها توقيت أو real-time؟ | قبل/أثناء/بعد المهلة — انقطاع — تأخير |
| 3. USERS | كم مستخدم يتفاعل معها؟ | واحد: isolation — متعددون: تعارض + race condition |
| 4. EXTERNAL | هل تتصل بـ service خارجي؟ | نجاح / فشل / بطء / انقطاع كامل |
| 5. STATE | هل فيها حالات (states)؟ | كل انتقال مسموح + كل انتقال ممنوع |
| 6. MEDIA | هل تتعامل مع ملفات أو وسائط؟ | حجم / امتداد / ملف تالف / ملف كبير |
| 7. COMPUTATION | هل فيها حسابات أو AI؟ | دقة / overflow / قيم سالبة / نتائج غير منطقية |
| 8. INTERFACE | هل فيها تفاعل UI خاص؟ | كل gesture / shortcut / animation / live update |
```

```markdown
## Feature DNA — نموذج الملء في Investigation

### [Feature Name] — DNA Analysis
| البعد | موجود؟ | التفاصيل المكتشفة | Tests المولّدة |
|-------|--------|-----------------|--------------|
| DATA | ✅ | يقرأ ويكتب — بيانات حساسة | حفظ / تعارض / تسرب |
| TIME | ✅ | deadline + real-time updates | قبل/بعد المهلة / انقطاع |
| USERS | ✅ | 3 roles مختلفة | كل role + تقاطع الصلاحيات |
| EXTERNAL | ❌ | لا يوجد | — |
| STATE | ✅ | Draft→Submitted→Approved | State Machine كامل |
| MEDIA | ❌ | لا يوجد | — |
| COMPUTATION | ✅ | حساب المبلغ المتبقي | دقة + overflow + سالب |
| INTERFACE | ✅ | drag & drop + live cursor | كل gesture + انقطاع |

→ الـ dimensions الفعّالة: DATA, TIME, USERS, STATE, COMPUTATION, INTERFACE
→ Tests المولّدة تلقائيًا من هذه الـ 6 أبعاد فقط
```

```markdown
## مثال عملي — feature لم تُذكر في الملف
## "Real-time Collaborative Editing"

DNA Analysis:
  DATA        ✅ → اختبر: مستخدمان يحفظان نفس السطر → من يفوز؟
  TIME        ✅ → اختبر: تأخير 5 ثوانٍ في الـ sync → هل البيانات تتعارض؟
  USERS       ✅ → اختبر: 50 مستخدم في نفس الوقت → هل الأداء مقبول؟
  EXTERNAL    ❌ → يُحذف
  STATE       ✅ → اختبر: connected / disconnected / reconnecting
  MEDIA       ❌ → يُحذف
  COMPUTATION ✅ → اختبر: merge conflict algorithm — النتيجة منطقية؟
  INTERFACE   ✅ → اختبر: cursor مستخدم 2 يظهر لمستخدم 1 فوراً؟

Tests المولّدة تلقائيًا — بدون ذكر "Collaborative Editing" في الملف أبدًا:
  □ مستخدمان يعدّلان نفس الكلمة → نتيجة واحدة لا تعارض
  □ انقطاع أثناء الكتابة → البيانات محفوظة عند إعادة الاتصال
  □ مستخدم جديد ينضم → يرى الحالة الحالية كاملة
  □ 50 مستخدم → response time < budget
  □ cursor الآخرين يظهر/يختفي بشكل صحيح
```

---

### الـ Universal Triggers — أسئلة لكل feature

> هذه تُطبَّق **بعد** Feature DNA — للـ features الشائعة توسّع في التفاصيل

```markdown
## لكل Feature مكتشفة — اسأل هذه الأسئلة:

### إذا وجدت LIST:
  □ هل تعمل مع 0 records؟ (empty state)
  □ هل تعمل مع 1 record؟
  □ هل تعمل مع 10,000 record؟ (performance)
  □ هل فيها sort؟ → اختبر تصاعدي + تنازلي + يبقى مع pagination
  □ هل فيها filter؟ → انظر قسم الفلتر
  □ هل فيها search؟ → انظر قسم البحث
  □ هل فيها pagination؟ → الصفحة 1 + الأخيرة + ما بعد الأخيرة
  □ هل فيها export؟ → انظر قسم التصدير

### إذا وجدت FORM:
  □ هل كل الحقول الإلزامية تُرفض إذا كانت فارغة؟
  □ هل الأخطاء تظهر معًا لا واحدًا واحدًا؟
  □ هل البيانات تبقى بعد خطأ في الـ API؟
  □ هل يمنع الإرسال المزدوج (double submit)؟
  □ هل يُحذّر عند مغادرة الصفحة بدون حفظ؟
  □ هل Tab order منطقي؟
  □ هل Enter يُرسل الـ form؟ (وهل هذا مرغوب؟)
  □ هل يعمل مع copy-paste؟
  □ هل date picker يرفض تواريخ غير منطقية؟
  □ هل file upload يرفض الحجم الكبير؟
  □ هل file upload يرفض الامتدادات الخاطئة؟

### إذا وجدت FILTER:
  □ هل يعمل فلتر واحد؟
  □ هل يعمل فلترين معًا؟
  □ هل يعمل كل الفلاتر معًا؟
  □ هل "clear all" يمسح كل شيء؟
  □ هل الـ URL يتغير؟ (shareable link)
  □ هل الفلتر يبقى بعد refresh؟
  □ هل الفلتر يُعيد الـ pagination للصفحة 1؟
  □ هل فلتر بدون نتائج يعطي empty state؟

### إذا وجدت SEARCH:
  □ هل يعمل بـ partial text؟
  □ هل case-insensitive؟
  □ هل يعمل بالعربي؟
  □ هل نص غير موجود → رسالة واضحة؟
  □ هل special chars لا تكسر النظام؟
  □ هل يعمل مع debounce؟ (لا طلب لكل حرف)

### إذا وجدت EXPORT (PDF / Excel / CSV):
  □ هل يعمل مع 0 records؟ (ملف فارغ أم رسالة؟)
  □ هل يعمل مع 10,000 records؟ (لا تعليق)
  □ هل يصدّر المفلتر فقط أم الكل؟
  □ هل اسم الملف منطقي؟
  □ إذا PDF:
      □ هل Arabic من اليمين لليسار؟
      □ هل الجداول لا تنكسر؟
      □ هل الـ charts تظهر؟
      □ هل الصفحات مرقّمة؟
  □ إذا Excel:
      □ هل كل الأعمدة موجودة؟
      □ هل الـ headers صحيحة؟
      □ هل الأرقام numbers لا text؟

### إذا وجدت WORKFLOW / APPROVAL:
  □ هل كل انتقال حالة صحيح؟ (State Machine)
  □ هل الانتقالات الممنوعة مرفوضة؟
  □ هل إشعار يُرسَل عند كل مرحلة؟
  □ هل المُعيَّن إليه يستلم الإشعار؟
  □ هل يمكن تخطي مرحلة؟ (يجب لا)
  □ هل يمكن التراجع؟ (حسب الـ business rule)
  □ هل فيها deadline؟ → ماذا يحدث لو انتهى؟

### إذا وجدت PERMISSIONS:
  □ هل كل role يرى فقط ما يُسمح له؟
  □ هل الـ Admin UI مخفي عن الـ User؟
  □ هل الـ Admin URL يُرفض بدون صلاحية؟
  □ هل يمكن الوصول لبيانات مستخدم آخر؟ (IDOR)
  □ هل تغيير الدور يُحدِّث الصلاحيات فورًا؟

### إذا وجدت NOTIFICATIONS:
  □ هل Toast يظهر في 500ms؟
  □ هل Toast يختفي تلقائيًا؟
  □ هل يمكن إغلاقه يدويًا؟
  □ هل 3 Toasts معًا لا تتداخل؟
  □ هل رسالة الخطأ مختلفة بصريًا عن النجاح؟
  □ هل Confirmation dialogs تشرح العواقب؟

### إذا وجدت CHARTS / DASHBOARD:
  □ هل البيانات صحيحة؟ (تحقق من الـ API)
  □ هل تعمل مع 0 data points؟
  □ هل تعمل مع 10,000 data points؟
  □ هل الـ legends واضحة؟
  □ هل تتحدث عند تغيير الـ filter؟

### لكل صفحة — UI/UX Universal:
  □ هل الـ page title يتغير؟ (browser tab)
  □ هل الـ breadcrumb صحيح؟
  □ هل الـ loading state احترافي؟ (skeleton / spinner)
  □ هل الـ console خالي من errors؟
  □ هل يعمل على mobile؟ (375px)
  □ هل يعمل بـ browser zoom 150%؟
  □ هل a11y: no violations؟
```

### Feature DNA Analysis — لكل feature غير موجودة في القائمة أعلاه

> إذا اكتشفت feature لا تنتمي لأي trigger معروف — طبّق هذا التحليل
> يعمل مع أي شيء — حتى features لم تُخترع بعد

```markdown
## الـ 8 أبعاد — اسأل كل بعد عن كل feature مكتشفة

البعد 1: DATA (البيانات)
  □ هل تقرأ بيانات؟
      → اختبر: 0 records / 1 / كثير / بيانات تالفة / null
  □ هل تكتب بيانات؟
      → اختبر: الحفظ الصحيح / التحديث / التعارض عند الحفظ المتزامن
  □ هل البيانات حساسة؟
      → اختبر: لا تظهر في URL / لا تُسرَّب في logs / مشفرة في DB

البعد 2: TIME (الزمن)
  □ هل فيها توقيت أو مهلة؟
      → اختبر: قبل الموعد / أثناءه / بعده / منتهي
  □ هل فيها real-time / WebSocket؟
      → اختبر: التزامن بين مستخدمين / انقطاع الاتصال / إعادة الاتصال
  □ هل فيها جدولة أو cron؟
      → اختبر: التنفيذ في الوقت / الفشل وإعادة المحاولة / التكرار الخاطئ

البعد 3: USERS (المستخدمون)
  □ هل مستخدم واحد فقط؟
      → اختبر: الـ isolation / لا يرى بيانات غيره
  □ هل مستخدمون متعددون في نفس الوقت؟
      → اختبر: race condition / من يفوز عند التعارض / الـ locking
  □ هل أدوار مختلفة تتفاعل؟
      → اختبر: كل role على حدة + نقاط التقاطع بينها

البعد 4: EXTERNAL (الخارجي)
  □ هل تتصل بـ API أو service خارجي؟
      → اختبر: النجاح / timeout / 500 / انقطاع كامل / رد بطيء
  □ هل تستقبل بيانات من خارج؟ (webhook / callback)
      → اختبر: البيانات الصحيحة / المشوهة / المشبوهة / المكررة
  □ هل ترسل للخارج؟ (email / SMS / push)
      → اختبر: الإرسال الصحيح / الفشل / عدم التكرار

البعد 5: STATE (الحالة)
  □ هل فيها states أو workflow؟
      → ارسم State Machine → اختبر كل انتقال مسموح وممنوع
  □ هل الحالة تُحفظ بين الجلسات؟
      → اختبر: refresh / logout-login / جهاز آخر
  □ هل الحالة مشتركة بين مستخدمين؟
      → اختبر: التغيير يظهر للجميع فورًا / أو بعد refresh

البعد 6: MEDIA (الوسائط)
  □ هل فيها رفع ملفات أو صور؟
      → اختبر: الحجم الأقصى / امتدادات مرفوضة / ملف تالف / اسم خاص
  □ هل فيها عرض صور أو فيديو؟
      → اختبر: صورة معطوبة → fallback / فيديو لا يتحمل / بطء التحميل
  □ هل فيها معالجة ملفات؟ (PDF generation / Excel / compress)
      → اختبر: 0 records / 10,000 records / بيانات عربية / layout

البعد 7: COMPUTATION (الحساب)
  □ هل فيها حسابات أو معادلات؟
      → اختبر: الدقة العشرية / القيم السالبة / الـ overflow / القسمة على صفر
  □ هل فيها AI أو ML؟
      → اختبر: نتيجة منطقية / input غريب / input فارغ / تحيز في النتائج
  □ هل فيها تحويل أو تجميع بيانات؟
      → اختبر: دقة النتيجة / فقدان بيانات / أداء مع كميات كبيرة

البعد 8: INTERFACE (الواجهة)
  □ هل فيها interaction غير تقليدي؟ (drag-drop / canvas / map / swipe)
      → اختبر: كل gesture / keyboard alternative / mobile vs desktop
  □ هل فيها live updates في الـ UI؟
      → اختبر: التحديث التلقائي / عدم الـ flicker / الأداء مع updates كثيرة
  □ هل فيها navigation أو routing خاص؟
      → اختبر: direct URL / browser back / deep link / query params
```

### كيف تستخدم الـ Feature DNA

```markdown
## خطوات التطبيق — 3 دقائق لكل feature

1. اكتب اسم الـ feature
2. اسأل كل بعد من الـ 8: "هل ينطبق هذا على هذه الـ feature؟"
3. لكل بعد بـ ✅ → أضف الـ tests المقابلة للـ Test Plan
4. لكل بعد بـ ❌ → تجاهله تمامًا

## مثال تطبيقي: "Real-time Chat"

| البعد | ينطبق؟ | Tests المولّدة |
|-------|--------|--------------|
| DATA | ✅ | رسائل تُحفظ / لا تضيع عند انقطاع |
| TIME | ✅ | real-time delivery / timestamp صحيح |
| USERS | ✅ | مستخدم يكتب → يظهر لـ الآخر / blocking |
| EXTERNAL | ❌ | — |
| STATE | ✅ | read/unread / online/offline |
| MEDIA | ✅ | إرسال صور / ملفات / حجم أقصى |
| COMPUTATION | ❌ | — |
| INTERFACE | ✅ | scroll لأسفل تلقائيًا / Enter يرسل |

## مثال تطبيقي: "AI Scholarship Recommender"

| البعد | ينطبق؟ | Tests المولّدة |
|-------|--------|--------------|
| DATA | ✅ | بيانات ناقصة → هل يعمل؟ |
| TIME | ❌ | — |
| USERS | ✅ | نفس الطالب → نفس التوصية دائمًا؟ |
| EXTERNAL | ✅ | AI service فشل → fallback؟ |
| STATE | ❌ | — |
| MEDIA | ❌ | — |
| COMPUTATION | ✅ | نتيجة منطقية / input غريب / تحيز |
| INTERFACE | ✅ | توضيح سبب التوصية / كيف يتحدث |
```

### القاعدة الجديدة

```
RULE 21 — Feature DNA قبل أي Trigger:
"إذا لم تجد الـ feature في Universal Triggers
 → طبّق الـ 8 أبعاد → ولّد tests مخصصة
 → لا ترتجل ولا تتجاهل"
```

---

## 12. DOCUMENTATION PROTOCOL {#documentation}

> "Document Outcomes, Not Steps"

### ما نوثّقه

```
✅ نوثّق:
  System Proof   → صورة كاملة للواجهة وهي تعمل صح
  Failure State  → صورة + فيديو للخطأ للتحليل
  Bug Fix        → قبل وبعد
  Critical Flow  → فيديو للـ journey الكاملة

❌ لا نوثّق:
  كل خطوة في الاختبار
  حالات وسطية
  نفس الشيء مرتين
```

### System Proof Screenshot

```typescript
// عند اكتمال feature → صورة واحدة تثبت أنها تعمل
async function captureSystemProof(page: Page, feature: string, note: string) {
  await page.waitForLoadState('networkidle');
  await captureOutcome(page, 'system-proof', {
    feature, scenario: 'working-state', note
  });
}

// مثال الاستخدام — مرة واحدة فقط للـ feature
test('@proof Blog.List.WorkingState', async ({ page }) => {
  await loginAs(page, 'admin');
  await page.goto('/blog/posts');
  await page.waitForLoadState('networkidle');
  await captureSystemProof(page, 'blog', 'Blog list with real data — production ready');
});
```

### Bug Fix Documentation

```markdown
<!-- evidence/testing/screenshots/failures/BUG-[ID]/diff-notes.md -->

# BUG-[ID] Fix Documentation
**Feature:** [اسم الـ feature]
**Severity:** Critical / High / Medium / Low
**Found:** [تاريخ الاكتشاف]
**Fixed:** [تاريخ الإصلاح]

## المشكلة
[وصف المشكلة بدقة]

## Root Cause
[السبب الجذري من الكود]

## قبل الإصلاح
![before](before-[timestamp].png)
[ما كان يظهر]

## بعد الإصلاح
![after](after-[timestamp].png)
[ما يظهر الآن]

## الحل المُطبَّق
[ما تغيّر في الكود]

## كيف نمنع تكراره
[الـ test الذي أُضيف لمنع الـ regression]
```

### Critical Flow Video

```typescript
// فيديو واحد للـ journey الكاملة — مرة واحدة عند الانتهاء
test('@flow Auth.CompleteJourney', async ({ page }) => {
  // هذا الـ test يُشغَّل بـ video: 'on' مرة واحدة للتوثيق
  // ثم يُحفظ في evidence/testing/flows/auth-journey.webm
});
```

---

## 13. POST-SESSION ANALYTICS {#analytics}

### Health Score Formula (مُعدَّل)

```
Dimension               Weight   Score (0-10)
──────────────────────────────────────────────
Business Logic Tests      25%     (Layer 0 pass rate × 10)
API Tests (Newman)        25%     (passed/total × 10)
E2E Smoke                 15%     (passed/total × 10)
Observability Coverage    15%     (error states covered × 10)
a11y Compliance           10%     (0 violations=10, -1 per violation)
Security Baseline         10%     (0 issues=10, -2 per issue)
──────────────────────────────────────────────
TOTAL                    100%     /100

90-100 🟢 Excellent  — production ready
70-89  🟡 Good       — ship with monitoring
50-69  🟠 Fair       — fix before next feature
0-49   🔴 Poor       — do not ship
```

### Session Report Template

```markdown
## [DATE] — [FEATURE] — Health: [XX]/100

### Scores
| Dimension | Weight | Score | Weighted |
|-----------|--------|-------|---------|
| Business Logic | 25% | X/10 | X |
| API (Newman) | 25% | X/10 | X |
| E2E Smoke | 15% | X/10 | X |
| Observability | 15% | X/10 | X |
| a11y | 10% | X/10 | X |
| Security | 10% | X/10 | X |
| **TOTAL** | | | **/100** |

### Investigation Summary
| ما اكتشفته | المصدر | الأثر على الاختبار |
|------------|--------|------------------|

### Questions Asked & Answered
| السؤال | الإجابة | الأثر |
|--------|---------|-------|

### Discovered Checklist Coverage
| Feature | الـ triggers المكتشفة | المُختبَر | الناقص |
|---------|---------------------|---------|--------|

### Bugs Found
| ID | Feature | Severity | Layer | Root Cause |
|----|---------|----------|-------|-----------|

### Performance
| Endpoint/Page | Time | Budget | Status |
|---------------|------|--------|--------|

### Documentation
| النوع | الملف | الغرض |
|-------|-------|-------|

### Server Log Errors During Run
[أي أخطاء في الـ backend logs]

### الأمور التي بقيت بدون إجابة
[أسئلة لم تُجَب — تحتاج جلسة قادمة]

### Verdict
[ ] ✅ SHIP
[ ] ⚠️ SHIP WITH MONITORING  
[ ] ❌ HOLD
```

---

## 14. REGRESSION REGISTRY {#registry}

```markdown
# REGRESSION REGISTRY — [PROJECT]
Skill: v3.2 | Updated: [date] | Health: [score]/100

## INVESTIGATION STATUS
| Feature | Investigation | Open Questions | Test Plan |
|---------|--------------|----------------|-----------|
| Auth | ✅ Complete | 0 | ✅ Ready |
| Blog | 🔍 In Progress | 3 | ⏳ Pending |

## FEATURES
| ID | Feature | Logic | API | E2E | a11y | Sec | Health | Last Run |
|----|---------|-------|-----|-----|------|-----|--------|---------|
| F001 | Auth | ✅ | ✅ | ✅ | ✅ | ✅ | 94/100 | [date] |

## BUGS
| ID | Feature | Severity | Layer | Status | Found | Fixed | Root Cause |
|----|---------|----------|-------|--------|-------|-------|-----------|

## DISCOVERED CHECKLISTS STATUS
| Feature | Triggers Found | Covered | Coverage% |
|---------|--------------|---------|----------|
```

---

## 15. CI/CD {#cicd}

```yaml
# .github/workflows/qa.yml
name: QA Pipeline v3.2

on:
  push:         { branches: [main, develop] }
  pull_request: { branches: [main] }

jobs:

  logic-tests:
    name: "Layer 0 — Business Logic"
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - name: Run domain logic tests
        run: dotnet test tests/logic/ --logger trx
      - uses: actions/upload-artifact@v4
        with: { name: logic-results, path: TestResults/ }

  api-tests:
    name: "Layer 2 — Newman API"
    runs-on: ubuntu-latest
    needs: logic-tests
    steps:
      - uses: actions/checkout@v4
      - run: npm install -g newman newman-reporter-htmlextra
      - run: |
          newman run tests/api/regression.collection.json \
            --environment tests/api/envs/ci.json \
            --reporters cli,htmlextra \
            --reporter-htmlextra-export evidence/testing/_reports/api/ci-report.html \
            --bail
      - uses: actions/upload-artifact@v4
        if: always()
        with: { name: api-report, path: evidence/testing/_reports/api/ }

  smoke-tests:
    name: "Layer 1 — Playwright Smoke"
    runs-on: ubuntu-latest
    needs: api-tests
    env:
      CI: 'true'
      BASE_URL: ${{ secrets.STAGING_URL }}
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with: { node-version: '20' }
      - run: npx playwright install --with-deps chromium firefox
      - run: npx playwright test tests/e2e/smoke.spec.ts --project=chromium
      - uses: actions/upload-artifact@v4
        if: always()
        with:
          name: smoke-report
          path: |
            playwright-report/
            evidence/testing/screenshots/failures/
            evidence/testing/flows/
```

---

## 16. ACTIVATION PROMPTS {#prompts}

```
/qa-install      → أول مرة في مشروع (تشغيل الـ installer)
/qa-investigate  → Investigation Protocol كامل للمشروع أو لـ feature
/qa-questions    → عرض الأسئلة المفتوحة + طلب إجابات
/qa-plan         → بناء Test Plan من نتائج الـ Investigation
/qa-smoke        → Playwright smoke فقط (P0 + critical UI)
/qa-api          → Newman كامل لـ feature معينة
/qa-logic        → Layer 0 Business Logic tests
/qa-regression   → كل الـ layers (Logic + Newman + Smoke)
/qa-a11y         → Accessibility audit
/qa-security     → Security baseline
/qa-proof        → System Proof screenshots للـ feature
/qa-bug          → توثيق bug + before/after screenshots
/qa-report       → توليد Session Report + تحديث Registry
```

---

## 17. MANDATORY OUTPUTS — كل جلسة {#outputs}

```
OUTPUT 1: Console Summary (مع Health Score)
OUTPUT 2: evidence/investigation/[feature]-investigation.md
OUTPUT 3: tests/logic/[feature].tests.cs أو .spec.ts (Layer 0)
OUTPUT 4: tests/api/[feature].collection.json (Newman — 6 folders)
OUTPUT 5: tests/e2e/smoke.spec.ts (محدَّث)
OUTPUT 6: evidence/testing/screenshots/ (system-proof + failures فقط)
OUTPUT 7: REGRESSION-REGISTRY.md + ANALYTICS.md (محدَّثان)
```

### Console Summary Format

```
╔══════════════════════════════════════════════════════╗
║         QA MASTER v3.2 — SESSION REPORT              ║
╠══════════════════════════════════════════════════════╣
║ Project    : [name]                                  ║
║ Feature    : [feature]                               ║
║ Date       : [date]    Time: [time]                  ║
╠══════════════════════════════════════════════════════╣
║ 🔍 Investigation : ✅ Complete  │ Questions: 0 open  ║
╠══════════════════════════════════════════════════════╣
║ 🧠 Logic Tests   : XX passed / XX failed             ║
║ 🌐 API (Newman)  : XX passed / XX failed             ║
║ 🎭 E2E Smoke     : XX passed / XX failed             ║
║ ♿ a11y Issues   : X violations                      ║
║ 🛡️  Security      : X issues found                   ║
╠══════════════════════════════════════════════════════╣
║ 📸 System Proof  : X screenshots                     ║
║ 🎬 Failure Video : X recordings                      ║
╠══════════════════════════════════════════════════════╣
║ 🏥 HEALTH SCORE  : [XX]/100  [🟢 Excellent]          ║
╠══════════════════════════════════════════════════════╣
║ 🐛 BUGS FOUND    : X                                  ║
║    🔴 Critical: X  🟠 High: X  🟡 Medium: X          ║
╠══════════════════════════════════════════════════════╣
║ ❓ OPEN QUESTIONS: X (يجب الإجابة قبل الجلسة القادمة)║
╠══════════════════════════════════════════════════════╣
║ VERDICT: ✅ SHIP / ⚠️ SHIP WITH MONITORING / ❌ HOLD ║
╚══════════════════════════════════════════════════════╝
```

---

## 18. FOLDER STRUCTURE {#structure}

```
[project-root]/
├── .agent/
│   ├── qa-config.json                     ← اعدل بعد التثبيت مباشرة
│   ├── qa-install-log.md
│   └── skills/
│       └── qa-master.md                   ← نسخة هذا الملف
│
├── tests/
│   ├── logic/                             ← Layer 0 (NEW)
│   │   ├── [feature].state-machine.cs
│   │   ├── [feature].invariants.cs
│   │   ├── [feature].decision-table.cs
│   │   └── [feature].stories.cs
│   ├── e2e/
│   │   ├── smoke.spec.ts                  ← الملف الوحيد (auth + critical UI)
│   │   └── helpers/
│   │       ├── screenshot.helper.ts
│   │       ├── auth.helper.ts
│   │       └── data.factory.ts
│   └── api/
│       ├── [feature].collection.json      ← 6 folders لكل feature
│       ├── regression.collection.json
│       └── envs/
│           ├── local.json
│           ├── staging.json
│           └── ci.json
│
├── evidence/
│   ├── investigation/                     ← (NEW)
│   │   ├── INVESTIGATION-TEMPLATE.md
│   │   └── [feature]-investigation.md
│   ├── testing/
│   │   ├── REGRESSION-REGISTRY.md        ← لا تحذفه أبدًا
│   │   ├── ANALYTICS.md
│   │   ├── _reports/
│   │   │   ├── api/
│   │   │   └── playwright/
│   │   └── screenshots/
│   │       ├── system-proof/              ← صورة واحدة لكل واجهة
│   │       │   └── [feature]-working-[ts].png
│   │       ├── failures/                  ← عند الفشل فقط
│   │       │   └── BUG-[ID]/
│   │       │       ├── before-[ts].png
│   │       │       ├── after-[ts].png
│   │       │       └── diff-notes.md
│   │       └── flows/                     ← فيديو الـ journey
│   │           └── [story]-journey.webm
│   └── architecture-decisions/
│
└── .github/
    └── workflows/
        └── qa.yml
```

---

## CHANGELOG

| Version | Date | Key Changes |
|---------|------|-------------|
| v1.0 | — | Initial Playwright skill |
| v2.0 | — | + Newman, Registry, Observability, Performance |
| v3.0 | — | + Self-Install, a11y, Security, RTL, Health Score, CI/CD |
| v3.1 | — | + Screenshot System, P8/P9, DB Verification, Memory Leak |
| v3.2 | 2025 | + Detective Mode, Investigation Protocol, Smart Questions, Layer 0 Business Logic, Newman كـ backbone, Playwright للـ Smoke فقط, Documentation Outcomes-Only, Discovered Checklists, **Feature DNA (8 أبعاد لأي feature غير معروفة)** |