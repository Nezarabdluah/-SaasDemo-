================================================================================
  مهارة DevOps & QA Enterprise — الدليل الشامل الموحد
  Enterprise DevOps & QA Master Skill — Unified Reference
================================================================================

فهرس المحتويات / Table of Contents
----------------------------------------
  01. SKILL.md — الدليل الرئيسي
  02. references/performance-testing.md — اختبارات الأداء
  03. references/security-testing.md — الأمان
  04. references/database-profiling.md — قواعد البيانات
  05. references/monitoring-observability.md — المراقبة
  06. references/production-readiness.md — جاهزية الإنتاج
  07. scripts/k6-templates/complete-test-suite.js — سكربتات k6
  08. scripts/sql/profiling-queries.sql — استعلامات SQL
  09. scripts/docker/docker-compose.monitoring.yml — Docker Stack


================================================================================

  الملف 01: SKILL.md — الدليل الرئيسي

================================================================================


---
name: devops-qa-enterprise
description: |
  Enterprise-grade DevOps & QA analysis, testing strategy design, and production readiness assessment for any software project. Use this skill whenever:
  - User asks to "analyze", "audit", "test", "benchmark", or "review" a system or API
  - User mentions k6, Grafana, InfluxDB, Newman, OWASP ZAP, Lighthouse, or similar tools
  - User wants to build a QA suite, load testing plan, or performance testing strategy
  - User asks about production readiness, deployment checklist, or go-live criteria
  - User reports system crashes under load, memory leaks, slow APIs, or DB connection issues
  - User wants to set up monitoring, observability, or alerting
  - User mentions DevOps pipeline, CI/CD quality gates, or shift-left testing
  - User asks about security scanning, penetration testing, or OWASP compliance
  Always use this skill for any project performance, quality, or reliability topic — even if the user doesn't use exact technical terms.
---

# 🏆 Enterprise DevOps & QA Master Skill

A world-class, reusable framework for designing, executing, and reporting on complete QA and performance engineering suites — applicable to any web, API, or microservices project.

Built on: **Google SRE**, **Netflix Chaos Engineering**, **DORA Metrics**, **OWASP**, **ISO/IEC 25010**, and battle-tested patterns from Amazon, Microsoft, Cloudflare, and Stripe.

---

## 📐 FRAMEWORK OVERVIEW

This skill operates across **5 pillars**:

| Pillar | Focus | Tools |
|--------|-------|-------|
| **1. Observability** | Four Golden Signals | Grafana, InfluxDB, Prometheus |
| **2. Performance** | Load/Stress/Chaos | k6, Gatling, Artillery |
| **3. Security** | Shift-Left SecOps | OWASP ZAP, Newman, Trivy |
| **4. Reliability** | SLO/SLI/SLA + DORA | Custom dashboards |
| **5. Readiness** | Production Checklist | Automated gate scoring |

---

## 🚀 STEP 1 — PROJECT INTAKE & CLASSIFICATION

Before designing any test suite, classify the project:

### 1.1 Architecture Type
- [ ] Monolith (single deploy unit)
- [ ] Modular Monolith (modules, shared DB)
- [ ] Microservices (independent services, own DBs)
- [ ] Serverless / Edge
- [ ] Hybrid

### 1.2 Criticality Tier (determines test depth)
| Tier | Description | Examples |
|------|-------------|---------|
| **T1 — Critical** | Financial, healthcare, auth | Payment APIs, medical records |
| **T2 — High** | Core business logic, user-facing | E-commerce, SaaS apps |
| **T3 — Standard** | Internal tools, admin panels | CMS, dashboards |
| **T4 — Low** | POC, dev environments | Prototypes |

### 1.3 Environment Matrix
Document all environments and their constraints:
```
DEV   → IIS Express / Docker / local Kestrel (NOT for load testing)
STG   → Production-mirror (MUST match prod specs within 80%)
PROD  → Real infra, real load, real money
```

> ⚠️ **Global Rule:** NEVER run load tests against DEV/IIS Express. Results are invalid and the server will crash. Always test on STG or dedicated perf environment.

---

## 📊 STEP 2 — THE FOUR GOLDEN SIGNALS (Google SRE)

Every system must be measured on exactly these four dimensions. Instrument them FIRST before writing any test.

```
┌─────────────────────────────────────────────────────────┐
│  LATENCY   │ How long do requests take?                  │
│            │ Track: p50, p90, p95, p99 (NOT averages)   │
│            │ Targets: p95 < 200ms, p99 < 500ms          │
├─────────────────────────────────────────────────────────┤
│  TRAFFIC   │ How much demand is on the system?           │
│            │ Track: RPS, concurrent users, throughput    │
│            │ Establish baseline before testing           │
├─────────────────────────────────────────────────────────┤
│  ERRORS    │ What fraction of requests fail?             │
│            │ Track: 4xx, 5xx, timeouts, exceptions       │
│            │ Target: error rate < 0.1% under normal load │
├─────────────────────────────────────────────────────────┤
│  SATURATION│ How full is the system?                     │
│            │ Track: CPU%, RAM%, DB connections, threads  │
│            │ Alert threshold: 70%; Critical: 85%         │
└─────────────────────────────────────────────────────────┘
```

For full monitoring setup → see `references/monitoring-observability.md`

---

## 🧪 STEP 3 — THE 20-TEST PYRAMID STRATEGY

### Phase 1: Foundation Tests (8 tests) — "Does it work?"

| # | Test | Tool | Purpose | Pass Criteria |
|---|------|------|---------|---------------|
| 1 | **Smoke** | k6 | Basic health: 1-5 VUs, 1 min | 0 errors, p95 < 500ms |
| 2 | **Functional API** | Newman/Postman | All endpoints, all status codes | 100% pass rate |
| 3 | **Load Baseline** | k6 | Expected concurrent users, 10 min | p95 < 200ms, errors < 1% |
| 4 | **Load Sustained** | k6 | 2× expected users, 20 min | p95 < 500ms, errors < 5% |
| 5 | **Endpoint Isolation** | k6 | Each API in isolation | Identify top-3 slowest endpoints |
| 6 | **Soak/Endurance** | k6 | Normal load, 1-4 hours | No memory growth > 10%, no error rate increase |
| 7 | **DB Health** | sqlcmd/psql | Connection pool, query plans | Pool usage < 70%, no full scans |
| 8 | **Frontend Perf** | Lighthouse | SEO, performance, accessibility | Score > 90 all categories |

### Phase 2: Stress & Security Tests (6 tests) — "Where does it break?"

| # | Test | Tool | Purpose | Pass Criteria |
|---|------|------|---------|---------------|
| 9 | **Stress Test** | k6 | Ramp to 3-5× expected users | Graceful degradation, no data loss |
| 10 | **Spike Test** | k6 | Sudden 10× traffic burst | Recovery within 60s, no cascades |
| 11 | **Breakpoint Finder** | k6 | +N VUs every 30s until failure | Document exact breaking point |
| 12 | **OWASP Top 10** | ZAP/Burp | Injection, XSS, CSRF, IDOR | 0 Critical, 0 High findings |
| 13 | **Auth & AuthZ** | Newman | Token expiry, privilege escalation | 100% correct access enforcement |
| 14 | **Rate Limiting** | k6 | Exceed limits intentionally | 429s returned, system stays up |

### Phase 3: Enterprise Resilience Tests (6 tests) — "Can it survive chaos?"

| # | Test | Tool | Purpose | Pass Criteria |
|---|------|------|---------|---------------|
| 15 | **Circuit Breaker** | k6 + code review | Downstream dependency failure | System degrades, not crashes |
| 16 | **Chaos: DB Kill** | Chaos toolkit | Kill DB mid-traffic | Graceful error, no data corruption |
| 17 | **Chaos: Memory Pressure** | stress-ng | Fill RAM to 90% | OOM killer triggers, service restarts |
| 18 | **Recovery Test** | k6 | After crash, reduce to 10 VUs | Recovery < 30s without restart |
| 19 | **Multilingual/i18n** | k6 | Same ops in all supported languages | < 5% latency difference |
| 20 | **Payload & Compression** | k6 + curl | Response sizes, Gzip/Brotli | Compression ratio > 60% for text |

For k6 script templates → see `scripts/k6-templates/`

---

## 🔴 STEP 4 — CRITICAL BUG CLASSIFICATION SYSTEM

Use this taxonomy for every bug discovered. Priority is non-negotiable.

### P0 — LAUNCH BLOCKER 🚨
System cannot go to production. Examples:
- Connection pool exhaustion under normal load
- No self-recovery after crash (zombie connections)
- Data loss or corruption under any condition
- Authentication bypass
- Cascading failures with no circuit breaker

**Required Action:** Fix before any other work. Create war room. ETA < 48h.

### P1 — HIGH SEVERITY ⚠️
System degraded significantly. Examples:
- Single endpoint causing 5× performance regression
- Memory leak growing > 1MB/min under load
- Missing Gzip compression (> 80% bandwidth waste)
- Security headers missing (HSTS, CSP, X-Frame-Options)
- Transaction log 2× size of actual data

**Required Action:** Fix in current sprint. Block release until resolved.

### P2 — MEDIUM SEVERITY 🟡
System functional but suboptimal. Examples:
- Cacheable endpoints with no cache (latency 10× slower than needed)
- N+1 query patterns
- No health check endpoint `/health` or `/ready`
- Logs without correlation IDs

**Required Action:** Fix in next sprint. Document workaround.

### P3 — IMPROVEMENT 🟢
Nice-to-have enhancements. Examples:
- Redis vs in-memory cache (horizontal scale prep)
- Database index tuning for < 100ms queries
- CDN integration for static assets

---

## 📈 STEP 5 — SLO/SLI/SLA FRAMEWORK (Google SRE Standard)

Define these for every production system BEFORE going live:

```yaml
# Example SLO Definition
service: product-api
slos:
  availability:
    target: 99.9%           # "three nines" = 8.7h downtime/year
    measurement: success_rate_over_30d
    
  latency:
    target: 95% of requests < 200ms
    measurement: p95_over_1h
    
  throughput:
    target: sustain 500 RPS without degradation
    measurement: sustained_load_test_30min

error_budget:
  monthly_allowance: 0.1%   # = 43.8 minutes/month
  alert_at: 50%_consumed    # alert when 21.9 min burned
  freeze_deploys_at: 90%_consumed
```

---

## 🔒 STEP 6 — SECURITY TESTING PROTOCOL (OWASP + SHIFT-LEFT)

### 6.1 OWASP Top 10 Verification Checklist
For each item, test with specific payloads from `references/security-testing.md`:

- [ ] **A01 — Broken Access Control:** Test IDOR, path traversal, horizontal/vertical privesc
- [ ] **A02 — Cryptographic Failures:** Check TLS version, certificate, sensitive data in logs
- [ ] **A03 — Injection:** SQL, NoSQL, LDAP, OS command injection
- [ ] **A04 — Insecure Design:** Business logic flaws, missing rate limiting
- [ ] **A05 — Security Misconfiguration:** Default creds, debug mode, error disclosure
- [ ] **A06 — Vulnerable Components:** Check `npm audit`, `pip check`, NuGet advisories
- [ ] **A07 — Auth Failures:** Session fixation, weak passwords, no MFA
- [ ] **A08 — Integrity Failures:** Unsigned updates, insecure deserialization
- [ ] **A09 — Logging Failures:** No audit trail, sensitive data in logs
- [ ] **A10 — SSRF:** Blind SSRF via URL parameters

### 6.2 Required Security Headers
```http
Strict-Transport-Security: max-age=31536000; includeSubDomains
Content-Security-Policy: default-src 'self'
X-Frame-Options: DENY
X-Content-Type-Options: nosniff
Referrer-Policy: strict-origin-when-cross-origin
Permissions-Policy: camera=(), microphone=(), geolocation=()
```

Never expose: `Server: IIS/10.0`, `X-Powered-By: ASP.NET`, `X-AspNet-Version`

---

## 🗄️ STEP 7 — DATABASE PERFORMANCE PROTOCOL

### 7.1 SQL Server DMV Diagnostic Queries
Run these before AND after load tests to compare:

```sql
-- Top 10 slowest queries
SELECT TOP 10 
    total_elapsed_time / execution_count AS avg_ms,
    execution_count,
    SUBSTRING(st.text, 1, 200) AS query_preview
FROM sys.dm_exec_query_stats qs
CROSS APPLY sys.dm_exec_sql_text(qs.sql_handle) st
ORDER BY avg_ms DESC;

-- Connection pool health
SELECT 
    COUNT(*) AS total_connections,
    SUM(CASE WHEN status = 'sleeping' THEN 1 ELSE 0 END) AS idle,
    SUM(CASE WHEN status = 'running' THEN 1 ELSE 0 END) AS active,
    SUM(CASE WHEN wait_time > 5000 THEN 1 ELSE 0 END) AS waiting_over_5s
FROM sys.dm_exec_sessions
WHERE is_user_process = 1;

-- THREADPOOL wait (the #1 sign of connection exhaustion)
SELECT wait_type, waiting_tasks_count, wait_time_ms
FROM sys.dm_os_wait_stats
WHERE wait_type = 'THREADPOOL'
ORDER BY wait_time_ms DESC;
```

### 7.2 The PageViewMiddleware Anti-Pattern (Critical Fix)

**Problem:** Middleware that writes to DB on every request without connection management.

**Fix Pattern — BackgroundService with ConcurrentQueue:**
```csharp
// WRONG: Fire-and-forget DB writes in middleware
public async Task InvokeAsync(HttpContext context)
{
    _ = _db.PageViews.AddAsync(new PageView { ... }); // LEAK!
    await _next(context);
}

// RIGHT: Queue + Background Batch Processor
public class PageViewMiddleware
{
    private static readonly ConcurrentQueue<PageView> _queue = new();
    
    public async Task InvokeAsync(HttpContext context, IPageViewQueue queue)
    {
        queue.Enqueue(new PageView { Url = context.Request.Path, Timestamp = DateTime.UtcNow });
        await _next(context); // returns immediately, zero DB calls
    }
}

// BackgroundService flushes queue every 10 seconds in batch
public class PageViewFlusher : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(10), ct);
            var batch = _queue.DequeueAll(); // drain queue atomically
            if (batch.Any())
                await _db.BulkInsertAsync(batch); // single DB roundtrip
        }
    }
}
```

For full DB profiling guide → see `references/database-profiling.md`

---

## 🎯 STEP 8 — PRODUCTION READINESS SCORECARD

Score each item 0-2. Total: maximum 60 points.
- **≥ 54 (90%):** Production Ready ✅
- **45–53 (75-89%):** Conditional — fix P0/P1 first ⚠️
- **< 45 (< 75%):** Not Ready — significant work required 🚫

### Infrastructure (12 pts)
- [ ] Running on production-grade server (Kestrel/IIS/Nginx) — NOT IIS Express (2)
- [ ] Horizontal scaling tested and verified (2)
- [ ] Auto-restart on crash configured (systemd, PM2, K8s) (2)
- [ ] Health check endpoint `/health` returns 200 with DB/cache status (2)
- [ ] CDN configured for static assets (1)
- [ ] Gzip/Brotli compression enabled (1)

### Reliability (12 pts)
- [ ] No P0 bugs open (2)
- [ ] SLOs defined and dashboards live (2)
- [ ] Circuit breakers for all external dependencies (2)
- [ ] Graceful shutdown handling (2)
- [ ] Retry logic with exponential backoff (2)
- [ ] Rate limiting on all public endpoints (2)

### Observability (12 pts)
- [ ] Structured logging (JSON) with correlation IDs (2)
- [ ] Metrics exported to Grafana/DataDog (2)
- [ ] Distributed tracing enabled (OpenTelemetry) (2)
- [ ] Alerting configured for SLO breaches (2)
- [ ] Error tracking (Sentry, Application Insights) (2)
- [ ] Dashboard covering all Four Golden Signals (2)

### Security (12 pts)
- [ ] All OWASP Top 10 verified (0 Critical, 0 High) (2)
- [ ] All required security headers present (2)
- [ ] Secrets in vault/environment — NOT in code (2)
- [ ] TLS 1.2+ enforced, HTTP → HTTPS redirect (2)
- [ ] Input validation on all endpoints (2)
- [ ] Dependency vulnerability scan clean (2)

### Performance (12 pts)
- [ ] p95 latency < 200ms under expected load (2)
- [ ] Caching for all expensive/repeated queries (2)
- [ ] DB connection pool sized correctly (Max Pool Size ≥ 200) (2)
- [ ] No N+1 queries (verified via DB profiler) (2)
- [ ] Transaction log < 1.5× data size (2)
- [ ] Load test passing at 2× expected users (2)

---

## 📋 STEP 9 — DORA METRICS BASELINE

Before deployment, establish these four elite DevOps benchmarks:

| Metric | Elite | High | Medium | Low |
|--------|-------|------|--------|-----|
| **Deployment Frequency** | On-demand (multiple/day) | Weekly | Monthly | < Monthly |
| **Lead Time for Changes** | < 1 hour | 1 day–1 week | 1 week–1 month | > 1 month |
| **Change Failure Rate** | < 5% | 5–10% | 10–15% | > 15% |
| **Time to Restore (MTTR)** | < 1 hour | < 1 day | < 1 week | > 1 week |

---

## 📦 STEP 10 — DELIVERABLE REPORT TEMPLATE

Every QA engagement must produce a report containing:

```markdown
# QA Report: [Project Name]
**Date:** [date] | **Environment:** [env] | **Version:** [version]

## Executive Summary
- Overall Score: [X/60] — [Status]
- Critical Issues: [count]
- Recommended Action: [launch / fix P0s first / major rework]

## Test Results Matrix
[Table with all 20 tests, status, and key metrics]

## Critical Issues (P0)
[Each bug with: description, evidence, root cause, fix recommendation]

## Performance Profile
[Latency percentiles, breaking point, capacity estimate]

## Security Posture
[OWASP findings, header compliance, auth verification]

## Production Readiness Scorecard
[Filled scorecard from Step 8]

## Action Plan
[Prioritized, time-estimated task list]
```

---

## 🔗 REFERENCE FILES

Load these when needed for deep-dive work:

| File | When to Read |
|------|-------------|
| `references/monitoring-observability.md` | Setting up Grafana + InfluxDB + Prometheus stack |
| `references/performance-testing.md` | Detailed k6 patterns, scenario design, result interpretation |
| `references/security-testing.md` | Full OWASP payloads, ZAP config, Newman security collections |
| `references/database-profiling.md` | SQL Server + PostgreSQL + MySQL profiling queries and fixes |
| `references/production-readiness.md` | Extended checklist with Kubernetes, Docker, CI/CD gates |
| `scripts/k6-templates/` | Ready-to-use k6 scripts for all 8 load test types |
| `scripts/docker/docker-compose.monitoring.yml` | Full monitoring stack in one file |
| `scripts/sql/profiling-queries.sql` | All DMV queries for SQL Server diagnosis |



================================================================================

  الملف 02: references/performance-testing.md — اختبارات الأداء

================================================================================


# Performance Testing Deep Reference

## k6 Test Design Patterns

### The Scenario Matrix
Every project needs these test scenarios designed before any script is written:

```
Expected Users (EU) = your peak concurrent users estimate
Normal Load        = EU × 1.0
Stress Load        = EU × 2.0  
Spike Load         = EU × 10.0 (sudden burst)
Soak Duration      = 1–4 hours at Normal Load
Breakpoint Step    = +5% of EU every 30–60 seconds
```

---

## k6 Script Patterns

### Pattern 1: Smoke Test
```javascript
import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
  vus: 3,
  duration: '1m',
  thresholds: {
    http_req_failed: ['rate<0.01'],
    http_req_duration: ['p(95)<500'],
  },
};

export default function () {
  const res = http.get('https://your-api.com/health');
  check(res, {
    'status is 200': (r) => r.status === 200,
    'response time OK': (r) => r.timings.duration < 500,
  });
  sleep(1);
}
```

### Pattern 2: Ramp Load Test (Realistic)
```javascript
export const options = {
  stages: [
    { duration: '2m', target: 20 },    // ramp up
    { duration: '5m', target: 20 },    // stay at 20 VUs
    { duration: '2m', target: 100 },   // scale to load
    { duration: '10m', target: 100 },  // sustain load
    { duration: '3m', target: 0 },     // ramp down
  ],
  thresholds: {
    http_req_failed: ['rate<0.05'],    // < 5% errors
    http_req_duration: ['p(95)<200'],  // p95 under 200ms
    'http_req_duration{name:search}': ['p(95)<1000'], // per-endpoint threshold
  },
};
```

### Pattern 3: Breakpoint Finder
```javascript
export const options = {
  executor: 'ramping-vus',
  stages: Array.from({ length: 40 }, (_, i) => ({
    duration: '30s',
    target: (i + 1) * 5,  // +5 VUs every 30s
  })),
  thresholds: {
    http_req_failed: [{ threshold: 'rate<0.1', abortOnFail: true }], // stop at 10% errors
  },
};
```

### Pattern 4: Spike Test (Netflix-style)
```javascript
export const options = {
  stages: [
    { duration: '2m', target: 20 },      // normal baseline
    { duration: '30s', target: 300 },    // SPIKE: 10× sudden
    { duration: '3m', target: 300 },     // hold the spike
    { duration: '30s', target: 20 },     // drop back
    { duration: '5m', target: 20 },      // observe recovery
    { duration: '1m', target: 0 },
  ],
};
```

### Pattern 5: Soak Test (Memory Leak Detection)
```javascript
export const options = {
  stages: [
    { duration: '5m', target: 50 },
    { duration: '3h', target: 50 },  // 3-hour sustained
    { duration: '5m', target: 0 },
  ],
  thresholds: {
    http_req_duration: ['p(95)<300'],
    // If p95 GROWS over time → memory leak
  },
};
```

### Pattern 6: Per-Endpoint Isolation
```javascript
const ENDPOINTS = [
  { name: 'home', url: '/api/home' },
  { name: 'search', url: '/api/search?q=test' },
  { name: 'universities', url: '/api/universities' },
  { name: 'settings', url: '/api/settings' },
];

export default function() {
  for (const ep of ENDPOINTS) {
    const res = http.get(`${BASE_URL}${ep.url}`, {
      tags: { name: ep.name }
    });
    check(res, { [`${ep.name} OK`]: r => r.status === 200 });
    
    // Track per-endpoint metrics in InfluxDB via tags
    trend_duration.add(res.timings.duration, { endpoint: ep.name });
  }
}
```

---

## Interpreting k6 Results

### Percentile Guide
```
p50 (median):  50% of users experience ≤ this latency
p90:           The typical "acceptable" benchmark
p95:           Industry standard SLO measurement (use this)
p99:           "Worst acceptable" — 1 in 100 users
p99.9:         "Tail latency" — only matters at massive scale
```

**NEVER use averages for latency.** A 100ms average can hide 10-second tail latency that destroys UX.

### Result Classification
```
p95 < 100ms   → Excellent (cache hit, trivial query)
p95 < 200ms   → Good (production target)
p95 < 500ms   → Acceptable (complex queries, no cache)
p95 < 1000ms  → Poor (needs optimization)
p95 < 3000ms  → Critical (user abandonment > 53%)
p95 > 3000ms  → Unacceptable (fix before launch)
```

### The "Why is it slow?" Decision Tree
```
Slow endpoint detected (p95 > 500ms)?
├── Run endpoint in isolation → still slow?
│   ├── YES: Server-side issue
│   │   ├── Check DB query time (> 80% of response? → DB problem)
│   │   │   ├── Missing index? → Add index
│   │   │   ├── N+1 queries? → Use Include/Join
│   │   │   └── Full table scan? → Add WHERE clause index
│   │   └── App-level computation (< 20% DB) → Code review needed
│   └── NO: Load-induced slowdown
│       ├── Connection pool exhaustion? → Increase pool size
│       ├── Thread starvation? → Check THREADPOOL waits
│       └── GC pressure? → Profile memory
└── Slow only under load → concurrency issue
    ├── Locking/deadlocks → check sys.dm_exec_requests
    └── Shared resource contention → profiling needed
```

---

## InfluxDB + Grafana Setup

### k6 → InfluxDB Configuration
```bash
k6 run --out influxdb=http://localhost:8086/k6 script.js
```

### Essential Grafana Panels for Every Dashboard
1. **RPS over time** (requests per second)
2. **p50/p95/p99 latency** (three lines on one graph)
3. **Error rate %** (alert threshold line at 1%)
4. **Active VUs** (correlate with latency spikes)
5. **HTTP status code breakdown** (pie or stacked bar)
6. **Endpoint comparison** (bar chart, sorted by p95 desc)

### InfluxDB Query for Slow Endpoint Detection
```sql
SELECT percentile("value", 95) AS "p95"
FROM "http_req_duration"
WHERE time > now() - 1h
GROUP BY "name", time(30s)
ORDER BY p95 DESC
```



================================================================================

  الملف 03: references/security-testing.md — الأمان

================================================================================


# Security Testing Deep Reference

## The Shift-Left Security Model

Traditional security: test at end → find issues late → expensive fixes.
Shift-Left: test at every stage → find issues early → cheap fixes.

```
Code Commit → SAST → Build → DAST → Deploy → Pen Test
   (mins)     (mins) (mins) (hours) (hours)  (days)
      ↑ Find here = $1     ↑ Find here = $100   ↑ = $10,000
```

---

## OWASP Top 10 — Test Payloads & Verification

### A01 — Broken Access Control
**Test:** Can user A access user B's data?
```bash
# Get your own resource
GET /api/users/123/profile  (logged in as user 123)

# Try another user's resource
GET /api/users/124/profile  → MUST return 403, not the data

# Try admin endpoint as regular user  
GET /api/admin/users        → MUST return 403

# Try path traversal
GET /api/files/../../etc/passwd  → MUST return 400 or 403
```

### A03 — Injection (SQL, NoSQL, Command)
```bash
# SQL Injection payloads to test in EVERY string input:
' OR '1'='1
'; DROP TABLE users; --
1' UNION SELECT username,password FROM users--
admin'--
' OR 1=1--

# Expected: 400 Bad Request or sanitized query — NEVER raw DB error
# FAIL: "Unclosed quotation mark after..." visible to user
```

### A05 — Security Misconfiguration
```bash
# Check if debug mode is exposed
GET /elmah.axd          → MUST return 404 or 401
GET /trace.axd          → MUST return 404 or 401
GET /.env               → MUST return 404
GET /appsettings.json   → MUST return 404
GET /swagger            → OK in dev, MUST be disabled in prod

# Check error disclosure
GET /api/nonexistent    → MUST return generic 404, NOT stack trace
```

### A07 — Authentication Failures
```bash
# Test token expiry
1. Get valid JWT token
2. Wait for expiry (or manually expire)
3. Try request → MUST return 401

# Test after logout
1. Login, get token
2. Logout
3. Reuse old token → MUST return 401 (server-side invalidation)

# Test brute force protection
for i in {1..20}; do
  curl -X POST /api/auth/login -d '{"password":"wrong"}'
done
# After N attempts → MUST return 429 or 423 (account locked)
```

---

## Required Security Headers — Verification Script
```bash
#!/bin/bash
URL="https://your-api.com"
RESPONSE=$(curl -sI "$URL")

check_header() {
  if echo "$RESPONSE" | grep -qi "$1"; then
    echo "✅ $1 present"
  else
    echo "❌ MISSING: $1"
  fi
}

check_header "Strict-Transport-Security"
check_header "Content-Security-Policy"
check_header "X-Frame-Options"
check_header "X-Content-Type-Options"
check_header "Referrer-Policy"

# These MUST NOT be present
if echo "$RESPONSE" | grep -qi "X-Powered-By"; then
  echo "❌ EXPOSED: X-Powered-By — reveals technology stack"
fi
if echo "$RESPONSE" | grep -qi "Server: IIS\|Server: Apache\|Server: nginx/"; then
  echo "❌ EXPOSED: Server version — should be generic or hidden"
fi
```

---

## OWASP ZAP Configuration

### Automated Scan Setup
```yaml
# zap-config.yaml
env:
  contexts:
    - name: "API Context"
      urls:
        - "https://your-api.com/api"
      authentication:
        method: json
        loginUrl: "https://your-api.com/api/auth/login"
        loginRequestData: '{"username":"{%username%}","password":"{%password%}"}'
        tokenResponse: "$.token"

jobs:
  - type: activeScan
    parameters:
      maxRuleDurationInMins: 5
      maxScanDurationInMins: 60
  - type: report
    parameters:
      reportFile: "zap-report.html"
      reportTitle: "Security Scan - [Project]"
```

### ZAP Finding Severity Interpretation
| Severity | Action Required |
|----------|----------------|
| **Critical** | Stop everything. Fix NOW. Launch blocker. |
| **High** | Fix before launch. Document risk if deferred. |
| **Medium** | Fix in current sprint. Risk acceptable short-term. |
| **Low** | Fix in next sprint. Low exploitation probability. |
| **Informational** | Review only. No direct exploit path. |

---

## Newman (Postman CLI) Security Collection Template

```json
{
  "name": "Security Regression Suite",
  "item": [
    {
      "name": "SQL Injection - Login",
      "request": {
        "url": "{{baseUrl}}/api/auth/login",
        "method": "POST",
        "body": { "username": "' OR '1'='1", "password": "anything" }
      },
      "event": [{
        "listen": "test",
        "script": {
          "exec": [
            "pm.test('SQL injection blocked', () => {",
            "  pm.expect(pm.response.code).to.be.oneOf([400, 401]);",
            "  pm.expect(pm.response.text()).to.not.include('SQL');",
            "  pm.expect(pm.response.text()).to.not.include('syntax');",
            "});"
          ]
        }
      }]
    }
  ]
}
```

Run with:
```bash
newman run security-suite.json \
  --env-var "baseUrl=https://staging.your-api.com" \
  --reporters cli,html \
  --reporter-html-export security-report.html
```

---

## Dependency Vulnerability Scanning

### .NET / NuGet
```bash
dotnet list package --vulnerable --include-transitive
```

### Node.js / npm
```bash
npm audit --audit-level=high
npx snyk test
```

### Docker Images
```bash
trivy image your-app:latest
docker scout cves your-app:latest
```

### CI/CD Gate (GitHub Actions example)
```yaml
- name: Security Scan
  run: |
    dotnet list package --vulnerable --include-transitive | grep -i "critical\|high"
    if [ $? -eq 0 ]; then
      echo "❌ Critical/High vulnerabilities found!"
      exit 1
    fi
```



================================================================================

  الملف 04: references/database-profiling.md — قواعد البيانات

================================================================================


# Database Performance Profiling Reference

## The Database Performance Pyramid

```
                    ┌──────────────┐
                    │  CACHING     │  ← Hit this first (free speed)
                   ╱│  L1/L2/L3   │╲
                  ╱ └──────────────┘ ╲
                 ╱  ┌──────────────┐  ╲
                ╱   │    INDEXES   │   ╲  ← Best ROI for queries
               ╱    └──────────────┘    ╲
              ╱     ┌──────────────┐     ╲
             ╱      │   QUERIES    │      ╲
            ╱       │  (N+1, etc) │       ╲
           ╱        └──────────────┘        ╲
          ╱         ┌──────────────┐         ╲
         ╱          │  SCHEMA &    │          ╲
        ╱           │  RELATIONS   │           ╲
       ╱            └──────────────┘            ╲
      ╱─────────────────────────────────────────╲
      │         HARDWARE / CLOUD TIER            │  ← Most expensive
      └──────────────────────────────────────────┘
```

Always optimize from the top of the pyramid down.

---

## SQL Server DMV Diagnostic Suite

### 1. Connection Pool Health Check
```sql
-- Current connection state
SELECT 
    DB_NAME(database_id) AS database_name,
    COUNT(*) AS total_connections,
    SUM(CASE WHEN status = 'sleeping' THEN 1 ELSE 0 END) AS idle_connections,
    SUM(CASE WHEN status = 'running'  THEN 1 ELSE 0 END) AS active_connections,
    SUM(CASE WHEN wait_time > 5000    THEN 1 ELSE 0 END) AS stalled_over_5s,
    MAX(wait_time) AS max_wait_ms
FROM sys.dm_exec_sessions
WHERE is_user_process = 1
GROUP BY database_id
ORDER BY total_connections DESC;
```

**Healthy:** idle connections ≈ pool size, active = current load, stalled = 0
**Danger:** stalled > 0, or total_connections approaching max_pool_size

### 2. THREADPOOL Wait — The Connection Exhaustion Signal
```sql
SELECT 
    wait_type,
    waiting_tasks_count,
    wait_time_ms,
    wait_time_ms / NULLIF(waiting_tasks_count, 0) AS avg_wait_ms
FROM sys.dm_os_wait_stats
WHERE wait_type IN ('THREADPOOL', 'RESOURCE_SEMAPHORE', 'ASYNC_NETWORK_IO')
ORDER BY wait_time_ms DESC;
```

**Interpretation:**
- `THREADPOOL` count > 1,000 → connection pool exhausted (like ANLASH bug: 27,515!)
- `RESOURCE_SEMAPHORE` → memory pressure / grant waits
- `ASYNC_NETWORK_IO` → client not reading results fast enough (app-side issue)

### 3. Top Expensive Queries
```sql
SELECT TOP 20
    qs.execution_count,
    qs.total_elapsed_time / qs.execution_count AS avg_elapsed_ms,
    qs.total_logical_reads / qs.execution_count AS avg_logical_reads,
    qs.total_worker_time / qs.execution_count AS avg_cpu_ms,
    SUBSTRING(st.text, (qs.statement_start_offset/2)+1,
        ((CASE qs.statement_end_offset WHEN -1 THEN DATALENGTH(st.text)
         ELSE qs.statement_end_offset END - qs.statement_start_offset)/2)+1
    ) AS query_text,
    qp.query_plan
FROM sys.dm_exec_query_stats qs
CROSS APPLY sys.dm_exec_sql_text(qs.sql_handle) st
CROSS APPLY sys.dm_exec_query_plan(qs.plan_handle) qp
ORDER BY avg_elapsed_ms DESC;
```

### 4. Missing Indexes (Automatic Suggestion)
```sql
SELECT TOP 10
    ROUND(avg_total_user_cost * avg_user_impact * (user_seeks + user_scans), 0) AS impact_score,
    mid.statement AS table_name,
    mid.equality_columns,
    mid.inequality_columns,
    mid.included_columns,
    'CREATE INDEX IX_' + REPLACE(REPLACE(mid.statement, '[', ''), ']', '') + 
    '_missing ON ' + mid.statement + 
    ' (' + ISNULL(mid.equality_columns, '') + ')' AS suggested_index
FROM sys.dm_db_missing_index_groups mig
JOIN sys.dm_db_missing_index_group_stats migs ON mig.index_group_handle = migs.group_handle
JOIN sys.dm_db_missing_index_details mid ON mig.index_handle = mid.index_handle
ORDER BY impact_score DESC;
```

### 5. Transaction Log Health
```sql
SELECT 
    name AS database_name,
    log_size_mb = size * 8.0 / 1024,
    log_used_mb = FILEPROPERTY(name, 'SpaceUsed') * 8.0 / 1024,
    log_used_pct = CAST(100.0 * FILEPROPERTY(name, 'SpaceUsed') / size AS DECIMAL(5,2))
FROM sys.databases
WHERE database_id > 4;  -- skip system DBs
```

**Target:** log_used_pct < 50%, log_size_mb < 2× data size

**Fix for bloated logs:**
```sql
-- 1. Set to SIMPLE recovery if full backup isn't needed
ALTER DATABASE [YourDB] SET RECOVERY SIMPLE;

-- 2. Shrink log file
DBCC SHRINKFILE ([YourDB_log], 1);  -- shrink to 1MB

-- 3. Verify
EXEC sp_helpfile;
```

---

## PostgreSQL Profiling Queries

### Slow Query Log
```sql
-- Enable in postgresql.conf:
-- log_min_duration_statement = 500  (log queries > 500ms)

-- Then check:
SELECT query, calls, total_time/calls AS avg_ms, rows
FROM pg_stat_statements
ORDER BY avg_ms DESC
LIMIT 20;
```

### Index Usage
```sql
SELECT 
    relname AS table_name,
    seq_scan,
    idx_scan,
    ROUND(100.0 * idx_scan / NULLIF(seq_scan + idx_scan, 0), 1) AS index_use_pct
FROM pg_stat_user_tables
WHERE seq_scan > 100
ORDER BY seq_scan DESC;
```

---

## Caching Strategy — When and What to Cache

### The Caching Decision Matrix
```
Query changes every:          Cache duration:
< 1 second                →   Don't cache (real-time data)
1–60 seconds              →   5s–30s (near-real-time)
1–15 minutes              →   1–5 minutes (dynamic content)
15 min – 1 hour           →   5–15 minutes (semi-static)
1–24 hours                →   30–60 minutes (reference data)
> 24 hours / never        →   Until invalidated (static data)
```

### Cache Keys Pattern
```csharp
// Include all query parameters in cache key:
var cacheKey = $"search:{language}:{query}:{page}:{pageSize}";
var cacheKey = $"university:{id}:details";
var cacheKey = $"settings:global:v{settingsVersion}";

// WRONG: Missing parameter = stale data for different users
var cacheKey = "search"; // ❌ All searches return same result!
```

### Redis vs Memory Cache
| | In-Memory Cache | Redis |
|--|-----------------|-------|
| **Speed** | ~0.01ms | ~1ms |
| **Shared** | No (per-instance) | Yes (all instances) |
| **Persistence** | Lost on restart | Optional |
| **Size limit** | RAM of server | Configurable |
| **Use when** | Single instance, < 500MB | Multiple instances, distributed |

---

## Connection String Optimization

### SQL Server Connection String Best Practices
```
Server=prod-db.example.com;
Database=MyAppDb;
User Id=app_user;
Password=...;
Min Pool Size=10;
Max Pool Size=200;        -- Up from default 100
Connection Timeout=30;
Command Timeout=60;
Application Name=MyApp;   -- Helps identify in DMVs
MultipleActiveResultSets=True;
TrustServerCertificate=False;
Encrypt=True;
```

### Signs You Need More Pool Size
- `THREADPOOL` waits spiking during load tests
- Timeout errors starting at a specific VU count
- `System.InvalidOperationException: Timeout expired. The timeout period elapsed prior to obtaining a connection from the pool`



================================================================================

  الملف 05: references/monitoring-observability.md — المراقبة

================================================================================


# Monitoring & Observability Reference

## The Observability Stack

```
                APPLICATION
                    │
           ┌────────┴────────┐
           │                 │
       METRICS            LOGS            TRACES
     (Prometheus)        (Loki /        (Jaeger /
       Grafana)          Seq)           Zipkin)
           │                 │               │
           └────────┬────────┘               │
                    │                        │
               GRAFANA ───────────────────────┘
              (Unified Dashboard)
```

---

## Docker Compose: Full Monitoring Stack

```yaml
# docker-compose.monitoring.yml
version: '3.8'

services:
  influxdb:
    image: influxdb:1.8
    ports: ["8086:8086"]
    environment:
      INFLUXDB_DB: k6
      INFLUXDB_ADMIN_USER: admin
      INFLUXDB_ADMIN_PASSWORD: ${INFLUXDB_PASSWORD}
    volumes:
      - influxdb_data:/var/lib/influxdb
    healthcheck:
      test: ["CMD", "curl", "-f", "http://localhost:8086/ping"]
      interval: 10s
      timeout: 5s
      retries: 5

  grafana:
    image: grafana/grafana:latest
    ports: ["3000:3000"]
    environment:
      GF_SECURITY_ADMIN_PASSWORD: ${GRAFANA_PASSWORD}
      GF_USERS_ALLOW_SIGN_UP: 'false'
    volumes:
      - grafana_data:/var/lib/grafana
      - ./grafana/dashboards:/etc/grafana/provisioning/dashboards
    depends_on:
      influxdb:
        condition: service_healthy

  prometheus:
    image: prom/prometheus:latest
    ports: ["9090:9090"]
    volumes:
      - ./prometheus/prometheus.yml:/etc/prometheus/prometheus.yml

  # For .NET apps
  seq:
    image: datalust/seq:latest
    ports: ["5341:80"]
    environment:
      ACCEPT_EULA: "Y"
    volumes:
      - seq_data:/data

volumes:
  influxdb_data:
  grafana_data:
  seq_data:
```

---

## Four Golden Signals — Alert Thresholds

### Grafana Alert Rules (PromQL / InfluxQL)

#### Latency Alert
```promql
# Alert when p95 latency exceeds 500ms for 5 consecutive minutes
histogram_quantile(0.95, 
  rate(http_request_duration_seconds_bucket[5m])
) > 0.5
```

#### Error Rate Alert
```promql
# Alert when error rate > 1%
sum(rate(http_requests_total{status=~"5.."}[5m])) 
/ 
sum(rate(http_requests_total[5m])) > 0.01
```

#### Saturation Alert
```promql
# Alert when DB connection pool > 80% utilized
(db_pool_connections_active / db_pool_connections_max) > 0.8
```

---

## Structured Logging Standard

Every log entry MUST contain:
```json
{
  "timestamp": "2024-01-15T10:30:00.000Z",
  "level": "ERROR",
  "correlationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "requestId": "req_abc123",
  "userId": "usr_456",
  "service": "product-api",
  "version": "2.3.1",
  "environment": "production",
  "message": "Database query timeout",
  "error": {
    "type": "SqlException",
    "message": "Timeout expired after 30s",
    "stackTrace": "..."
  },
  "httpContext": {
    "method": "GET",
    "path": "/api/search",
    "statusCode": 500,
    "durationMs": 30043
  },
  "metadata": {
    "query": "university search",
    "queryDurationMs": 30041
  }
}
```

**Never log:** passwords, tokens, credit card numbers, SSNs, full request bodies

---

## Health Check Endpoint Standard

```csharp
// /health endpoint implementation (.NET)
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        var result = new
        {
            status = report.Status.ToString(),
            timestamp = DateTime.UtcNow,
            version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                duration = e.Value.Duration.TotalMilliseconds,
                description = e.Value.Description
            })
        };
        await context.Response.WriteAsJsonAsync(result);
    }
});

// Register checks
builder.Services.AddHealthChecks()
    .AddSqlServer(connectionString, name: "database")
    .AddRedis(redisConnectionString, name: "cache")
    .AddUrlGroup(new Uri("https://external-api.com/health"), name: "external-api");
```

**Response when healthy:**
```json
{
  "status": "Healthy",
  "timestamp": "2024-01-15T10:30:00Z",
  "version": "2.3.1",
  "checks": [
    { "name": "database", "status": "Healthy", "duration": 5.2 },
    { "name": "cache",    "status": "Healthy", "duration": 0.8 }
  ]
}
```

---

## OpenTelemetry Setup (.NET)

```csharp
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddSqlClientInstrumentation()
        .AddOtlpExporter(o => o.Endpoint = new Uri("http://jaeger:4317")))
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddRuntimeInstrumentation()
        .AddPrometheusExporter());
```

---

## SLO Burn Rate Alerting (Google SRE Model)

```yaml
# Multi-window multi-burn-rate alerting
# Catches both fast burns and slow bleeds

alerts:
  - name: SLO_FastBurn
    condition: 
      # 14.4× burn rate for 1 hour (consumes 2% error budget in 1h)
      error_rate_1h > (1 - slo_target) * 14.4
    severity: page  # Wake someone up
    
  - name: SLO_SlowBurn  
    condition:
      # 6× burn rate for 6 hours (consumes 5% error budget in 1d)
      error_rate_6h > (1 - slo_target) * 6
    severity: ticket  # Create incident ticket
```



================================================================================

  الملف 06: references/production-readiness.md — جاهزية الإنتاج

================================================================================


# Production Readiness Extended Checklist

## The Google SRE Production Readiness Review (PRR) Model

Adapted from Google's internal process. Originally described in the SRE Book.
Every system entering production MUST pass this review.

---

## Domain 1: Architecture Review

### Scalability
- [ ] What is the maximum expected load? (RPS, concurrent users)
- [ ] What happens when load doubles? Triples? 10×?
- [ ] Is there a horizontal scaling plan? (more instances, not bigger servers)
- [ ] Are there stateful components that prevent horizontal scaling?
- [ ] Is session state stored server-side? (prevents multi-instance scaling)
- [ ] Are uploads stored locally? (use S3/Blob instead)

### Single Points of Failure (SPOF)
- [ ] Database: is there a replica/failover?
- [ ] Cache: is Redis in cluster mode or single node?
- [ ] External APIs: what happens when they're down?
- [ ] DNS: is there TTL < 60s for quick failover?

---

## Domain 2: Deployment & Rollback

### CI/CD Pipeline Quality Gates
Every deployment pipeline MUST automatically check:
```yaml
# Required Quality Gates (fail = block deployment)
quality_gates:
  unit_tests:
    coverage_minimum: 70%
    pass_rate: 100%
    
  integration_tests:
    pass_rate: 100%
    
  security_scan:
    critical_vulnerabilities: 0
    high_vulnerabilities: 0
    
  performance_regression:
    p95_increase_threshold: 20%  # Block if 20% slower than baseline
    
  load_test_smoke:
    error_rate: < 1%
    vus: 20
    duration: 2m
```

### Rollback Plan (mandatory, tested quarterly)
```
Rollback Trigger Criteria:
├── Error rate > 5% for 5 continuous minutes
├── p95 latency > 2× baseline for 10 minutes  
├── Any P0 bug reported in production
└── Data integrity issue of any kind

Rollback Steps (must complete in < 15 minutes):
1. Traffic: Shift to previous version (blue/green switch)
2. Database: Verify migration is reversible (or use expand-contract)
3. Cache: Flush if schema changed
4. Notify: Alert team + status page update
5. Post-mortem: Schedule within 48 hours
```

---

## Domain 3: Chaos Engineering (Netflix Model)

### Chaos Principles
1. **Hypothesis first:** "If X fails, the system will Y (gracefully degrade/show error page)"
2. **Run in production** (Netflix does this — use feature flags to limit blast radius)
3. **Minimize blast radius:** Start with 1% of traffic
4. **Automate:** Manual chaos is too slow and inconsistent

### Chaos Experiment Catalog
| Experiment | Tool | Expected Outcome |
|-----------|------|-----------------|
| Kill one instance mid-traffic | Kill -9 or K8s pod delete | Load balancer routes around it in < 30s |
| Database unavailable for 60s | iptables block | Circuit breaker activates, cached data served |
| Slow network (500ms added) | tc netem | Timeouts triggered correctly, no cascades |
| Memory full (90%) | stress-ng | OOM killer fires, service restarts cleanly |
| CPU 100% for 5 min | stress-ng | Requests queued, no corruption |
| External API returns 500s | Mock/WireMock | Fallback logic activates |

---

## Domain 4: Data Safety

### Database Migration Safety (Expand-Contract Pattern)
```
NEVER:   Deploy code change + DB schema change simultaneously

SAFE:    Step 1: Add new column (nullable, old code ignores it)
         Step 2: Deploy new code (writes to both old + new column)  
         Step 3: Migrate data (backfill new column)
         Step 4: Deploy code that uses only new column
         Step 5: Drop old column (weeks later, after verification)
```

### Backup Verification (not just backup — verify restores)
```bash
#!/bin/bash
# Monthly backup restore test
# 1. Restore backup to isolated environment
# 2. Verify row counts match
# 3. Verify critical data integrity
# 4. Document restore time (RTO)
echo "Restore test: $(date)" >> /var/log/backup-tests.log
```

---

## Domain 5: Incident Response Plan

### Severity Levels
| Sev | Impact | Response Time | Examples |
|-----|--------|--------------|---------|
| **SEV-1** | Complete outage | 15 minutes | Site down, payments failing |
| **SEV-2** | Major feature broken | 1 hour | Search down, login failing |
| **SEV-3** | Minor feature degraded | 4 hours | Slow search, minor UI bug |
| **SEV-4** | Cosmetic/non-critical | Next sprint | Copy error, minor layout |

### Incident Runbook Template
```
# Incident: [Name]
# Severity: SEV-[1-4]
# Owner: [On-call engineer]

## Detection
- How was this detected? (alert / user report / monitoring)
- Time to detection: [X minutes after start]

## Impact
- Users affected: [number / percentage]
- Features affected: [list]
- Revenue impact: [estimate]

## Timeline
[HH:MM] Incident detected
[HH:MM] Team paged
[HH:MM] Root cause identified  
[HH:MM] Fix deployed
[HH:MM] Incident resolved

## Root Cause
[Technical explanation — no blame]

## Fix
[What was deployed to resolve]

## Prevention
[What changes prevent recurrence]
```

---

## Domain 6: Performance Budget (Stripe / Google Model)

Define performance budgets BEFORE building features:

```yaml
performance_budget:
  # These are enforced in CI/CD
  
  page_load:
    time_to_interactive: < 3.8s    # Google "good" threshold
    first_contentful_paint: < 1.8s
    total_blocking_time: < 200ms
    cumulative_layout_shift: < 0.1

  api_response:
    search_endpoint: p95 < 500ms
    read_endpoints: p95 < 100ms
    write_endpoints: p95 < 200ms

  bundle_size:
    javascript: < 300kb gzipped
    css: < 50kb gzipped
    images: all optimized (WebP, lazy loaded)

  # Lighthouse scores (CI gate)
  lighthouse:
    performance: >= 90
    accessibility: >= 95
    best_practices: >= 90
    seo: >= 95
```

---

## Final Launch Gate Checklist

Before any production deployment, complete:

```
□ All 20 QA tests passing
□ Readiness scorecard ≥ 54/60
□ 0 P0 bugs open
□ 0 P1 security findings (OWASP)
□ Rollback plan tested (not just documented)
□ On-call engineer assigned for 72h post-launch
□ Status page updated
□ Customer support briefed
□ Monitoring dashboards live and alerting configured
□ Load test on staging with production-like data complete
```



================================================================================

  الملف 07: scripts/k6-templates/complete-test-suite.js — سكربتات k6

================================================================================


// ============================================================
// ENTERPRISE K6 TEST SUITE — All Patterns in One File
// Copy the relevant section for each test type
// Run: k6 run --out influxdb=http://localhost:8086/k6 <script.js>
// ============================================================

import http from 'k6/http';
import { check, sleep, group } from 'k6';
import { Rate, Trend, Counter } from 'k6/metrics';

// ─────────────────────────────────────────
// CONFIGURATION — Edit these for your project
// ─────────────────────────────────────────
const BASE_URL = __ENV.BASE_URL || 'https://your-api.com';
const API_KEY  = __ENV.API_KEY  || '';

// ─────────────────────────────────────────
// CUSTOM METRICS
// ─────────────────────────────────────────
const errorRate       = new Rate('custom_error_rate');
const slowRequests    = new Counter('slow_requests_over_1s');
const searchDuration  = new Trend('search_endpoint_duration', true);

// ─────────────────────────────────────────
// SHARED HEADERS
// ─────────────────────────────────────────
const HEADERS = {
  'Content-Type': 'application/json',
  'Accept': 'application/json',
  'Accept-Encoding': 'gzip, deflate', // Test compression
  ...(API_KEY ? { 'Authorization': `Bearer ${API_KEY}` } : {}),
};

// ─────────────────────────────────────────
// TEST SCENARIO CONFIGURATIONS
// Uncomment the one you want to run
// ─────────────────────────────────────────

// === SMOKE TEST (1 — quick sanity check) ===
export const options = {
  vus: 3,
  duration: '1m',
  thresholds: {
    http_req_failed: ['rate<0.01'],
    http_req_duration: ['p(95)<500'],
  },
};

// === LOAD TEST (2 — expected production load) ===
// export const options = {
//   stages: [
//     { duration: '2m',  target: 20 },   // ramp up
//     { duration: '5m',  target: 20 },   // warm up
//     { duration: '2m',  target: 100 },  // scale to load
//     { duration: '10m', target: 100 },  // sustain
//     { duration: '3m',  target: 0 },    // ramp down
//   ],
//   thresholds: {
//     http_req_failed:   ['rate<0.05'],
//     http_req_duration: ['p(95)<200', 'p(99)<500'],
//     custom_error_rate: ['rate<0.05'],
//   },
// };

// === STRESS TEST (3 — find the breaking point gradually) ===
// export const options = {
//   stages: [
//     { duration: '5m',  target: 50 },
//     { duration: '5m',  target: 100 },
//     { duration: '5m',  target: 200 },
//     { duration: '5m',  target: 300 },
//     { duration: '5m',  target: 0 },
//   ],
//   thresholds: {
//     http_req_failed: [{ threshold: 'rate<0.3', abortOnFail: false }],
//   },
// };

// === SPIKE TEST (4 — sudden burst simulation) ===
// export const options = {
//   stages: [
//     { duration: '2m',  target: 20 },   // baseline
//     { duration: '30s', target: 300 },  // SPIKE: instant 15×
//     { duration: '3m',  target: 300 },  // hold spike
//     { duration: '30s', target: 20 },   // drop back
//     { duration: '5m',  target: 20 },   // observe recovery
//     { duration: '1m',  target: 0 },
//   ],
// };

// === SOAK TEST (5 — memory leak detection) ===
// export const options = {
//   stages: [
//     { duration: '5m',  target: 50 },
//     { duration: '3h',  target: 50 },   // sustained load
//     { duration: '5m',  target: 0 },
//   ],
//   thresholds: {
//     http_req_duration: ['p(95)<300'],
//   },
// };

// === BREAKPOINT FINDER (6 — precise failure point) ===
// export const options = {
//   stages: Array.from({ length: 60 }, (_, i) => ({
//     duration: '30s',
//     target: (i + 1) * 5,  // +5 VUs every 30s (up to 300)
//   })),
//   thresholds: {
//     http_req_failed: [{ threshold: 'rate<0.1', abortOnFail: true }],
//   },
// };

// === RECOVERY TEST (7 — does it recover after overload?) ===
// export const options = {
//   stages: [
//     { duration: '2m',  target: 20 },   // normal
//     { duration: '3m',  target: 200 },  // overload
//     { duration: '1m',  target: 10 },   // drop to minimal
//     { duration: '5m',  target: 10 },   // observe: does error rate drop?
//   ],
// };

// ─────────────────────────────────────────
// MAIN TEST FUNCTION
// ─────────────────────────────────────────
export default function () {

  group('Health & Static', () => {
    const res = http.get(`${BASE_URL}/health`, { headers: HEADERS, tags: { name: 'health' } });
    
    const passed = check(res, {
      'health: status 200':     (r) => r.status === 200,
      'health: response fast':  (r) => r.timings.duration < 100,
    });

    errorRate.add(!passed);
    if (res.timings.duration > 1000) slowRequests.add(1);
  });

  sleep(0.5);

  group('Core API — Read', () => {
    const endpoints = [
      { name: 'list',     url: '/api/items' },
      { name: 'detail',   url: '/api/items/1' },
      { name: 'settings', url: '/api/settings' },
    ];

    for (const ep of endpoints) {
      const res = http.get(`${BASE_URL}${ep.url}`, {
        headers: HEADERS,
        tags: { name: ep.name },
      });

      check(res, {
        [`${ep.name}: success`]:    (r) => r.status === 200,
        [`${ep.name}: not empty`]:  (r) => r.body && r.body.length > 0,
        [`${ep.name}: valid json`]: (r) => {
          try { JSON.parse(r.body); return true; }
          catch (e) { return false; }
        },
        [`${ep.name}: has gzip`]:   (r) => 
          r.headers['Content-Encoding'] === 'gzip' || 
          r.headers['content-encoding'] === 'gzip',
      });

      if (res.timings.duration > 1000) slowRequests.add(1, { endpoint: ep.name });
    }
  });

  sleep(0.5);

  group('Search — Language Comparison', () => {
    const searchTerms = [
      { lang: 'en', q: 'computer science' },
      { lang: 'ar', q: 'علوم الحاسوب'    },
    ];

    for (const term of searchTerms) {
      const res = http.get(`${BASE_URL}/api/search?q=${encodeURIComponent(term.q)}&lang=${term.lang}`, {
        headers: HEADERS,
        tags: { name: `search_${term.lang}` },
      });

      check(res, {
        [`search ${term.lang}: OK`]: (r) => r.status === 200,
      });

      searchDuration.add(res.timings.duration, { language: term.lang });
    }
  });

  sleep(1);
}

// ─────────────────────────────────────────
// SETUP — Runs once before test
// ─────────────────────────────────────────
export function setup() {
  console.log(`Starting test against: ${BASE_URL}`);
  
  const res = http.get(`${BASE_URL}/health`);
  if (res.status !== 200) {
    throw new Error(`Health check failed! Status: ${res.status}. Aborting test.`);
  }
  
  console.log('✅ Health check passed. Starting test...');
  return { startTime: new Date().toISOString() };
}

// ─────────────────────────────────────────
// TEARDOWN — Runs once after test  
// ─────────────────────────────────────────
export function teardown(data) {
  console.log(`Test started at: ${data.startTime}`);
  console.log(`Test ended at:   ${new Date().toISOString()}`);
}

// ─────────────────────────────────────────
// RESULT INTERPRETATION GUIDE
// ─────────────────────────────────────────
// After test, check these metrics:
//
// http_req_duration{p(95)}: Target < 200ms
// http_req_failed{rate}:    Target < 1%  
// custom_error_rate{rate}:  Target < 5%
// slow_requests:            Target = 0
//
// If p95 grows DURING soak test → Memory Leak!
// If errors spike at specific VU count → Connection Pool or Thread Starvation
// If one endpoint is 10× slower → Caching needed or N+1 query



================================================================================

  الملف 08: scripts/sql/profiling-queries.sql — استعلامات SQL

================================================================================


-- ============================================================
-- Enterprise SQL Server Performance Diagnostic Suite
-- Applicable to any SQL Server project
-- Run BEFORE and AFTER load tests for comparison
-- ============================================================

-- ============================================================
-- SECTION 1: QUICK HEALTH CHECK
-- ============================================================
PRINT '=== QUICK HEALTH CHECK ==='

SELECT 
    @@SERVERNAME AS server_name,
    @@VERSION AS sql_version,
    GETDATE() AS check_time,
    (SELECT COUNT(*) FROM sys.dm_exec_sessions WHERE is_user_process = 1) AS active_user_connections,
    (SELECT physical_memory_in_use_kb / 1024 FROM sys.dm_os_process_memory) AS sql_memory_used_mb;

-- ============================================================
-- SECTION 2: CONNECTION POOL DIAGNOSTICS
-- ============================================================
PRINT '=== CONNECTION POOL HEALTH ==='

SELECT 
    DB_NAME(database_id) AS database_name,
    COUNT(*) AS total_connections,
    SUM(CASE WHEN status = 'sleeping' THEN 1 ELSE 0 END) AS idle,
    SUM(CASE WHEN status = 'running'  THEN 1 ELSE 0 END) AS active,
    SUM(CASE WHEN wait_time > 5000    THEN 1 ELSE 0 END) AS stalled_5s_plus,
    SUM(CASE WHEN wait_time > 30000   THEN 1 ELSE 0 END) AS stalled_30s_plus,
    MAX(wait_time) AS max_wait_ms,
    MIN(login_time) AS oldest_connection
FROM sys.dm_exec_sessions
WHERE is_user_process = 1
GROUP BY database_id
ORDER BY total_connections DESC;

-- ============================================================
-- SECTION 3: CRITICAL WAIT STATS (The #1 Performance Signal)
-- ============================================================
PRINT '=== TOP WAIT TYPES ==='

SELECT TOP 15
    wait_type,
    waiting_tasks_count,
    wait_time_ms,
    max_wait_time_ms,
    ROUND(100.0 * wait_time_ms / SUM(wait_time_ms) OVER(), 2) AS pct_of_all_waits,
    CASE wait_type
        WHEN 'THREADPOOL'          THEN '🔴 Connection pool exhausted! Increase Max Pool Size'
        WHEN 'RESOURCE_SEMAPHORE'  THEN '🟡 Memory grant waits — query needs more memory'
        WHEN 'ASYNC_NETWORK_IO'    THEN '🟡 Client reading slowly — check app-side'
        WHEN 'LCK_M_X'            THEN '🔴 Exclusive lock waits — check for deadlocks'
        WHEN 'PAGEIOLATCH_SH'      THEN '🟡 Disk I/O — consider SSD or more RAM'
        WHEN 'PAGELATCH_EX'        THEN '🟡 In-memory page contention — hot page issue'
        WHEN 'CXPACKET'            THEN '🟡 Parallel query skew — review MAXDOP setting'
        WHEN 'SOS_SCHEDULER_YIELD' THEN '🟡 CPU pressure — queries spinning'
        ELSE '⚪ Normal'
    END AS diagnosis
FROM sys.dm_os_wait_stats
WHERE wait_type NOT IN (
    'SLEEP_TASK', 'REQUEST_FOR_DEADLOCK_SEARCH', 'RESOURCE_QUEUE',
    'SERVER_IDLE_CHECK', 'CLR_AUTO_EVENT', 'WAIT_XTP_OFFLINE_CKPT_NEW_LOG',
    'WAITFOR', 'BROKER_TO_FLUSH', 'BROKER_TASK_STOP', 'CLR_MANUAL_EVENT',
    'DISPATCHER_QUEUE_SEMAPHORE', 'FT_IFTS_SCHEDULER_IDLE_WAIT',
    'XE_DISPATCHER_WAIT', 'XE_TIMER_EVENT', 'SQLTRACE_INCREMENTAL_FLUSH_SLEEP'
)
ORDER BY wait_time_ms DESC;

-- ============================================================
-- SECTION 4: TOP 10 SLOWEST QUERIES
-- ============================================================
PRINT '=== TOP 10 SLOWEST QUERIES ==='

SELECT TOP 10
    qs.execution_count,
    ROUND(qs.total_elapsed_time / qs.execution_count / 1000.0, 2) AS avg_duration_ms,
    ROUND(qs.total_logical_reads / qs.execution_count, 0) AS avg_logical_reads,
    ROUND(qs.total_worker_time / qs.execution_count / 1000.0, 2) AS avg_cpu_ms,
    qs.total_rows / qs.execution_count AS avg_rows_returned,
    SUBSTRING(
        st.text, 
        (qs.statement_start_offset / 2) + 1,
        ((CASE qs.statement_end_offset 
          WHEN -1 THEN DATALENGTH(st.text)
          ELSE qs.statement_end_offset END 
          - qs.statement_start_offset) / 2) + 1
    ) AS query_text
FROM sys.dm_exec_query_stats qs
CROSS APPLY sys.dm_exec_sql_text(qs.sql_handle) st
WHERE qs.execution_count > 5  -- only queries that run frequently
ORDER BY avg_duration_ms DESC;

-- ============================================================
-- SECTION 5: MISSING INDEXES (SQL Engine Suggestions)
-- ============================================================
PRINT '=== MISSING INDEX SUGGESTIONS ==='

SELECT TOP 15
    ROUND(
        migs.avg_total_user_cost * migs.avg_user_impact * (migs.user_seeks + migs.user_scans),
        0
    ) AS impact_score,
    migs.user_seeks,
    migs.user_scans,
    migs.avg_user_impact AS pct_improvement_estimate,
    mid.statement AS table_name,
    mid.equality_columns,
    mid.inequality_columns,
    mid.included_columns,
    CONCAT(
        'CREATE NONCLUSTERED INDEX [IX_missing_', 
        ROW_NUMBER() OVER (ORDER BY migs.avg_total_user_cost * migs.avg_user_impact DESC),
        '] ON ', mid.statement,
        ' (', ISNULL(mid.equality_columns, ''), 
        CASE WHEN mid.inequality_columns IS NOT NULL 
             THEN ', ' + mid.inequality_columns ELSE '' END, ')',
        CASE WHEN mid.included_columns IS NOT NULL 
             THEN ' INCLUDE (' + mid.included_columns + ')' ELSE '' END
    ) AS create_index_sql
FROM sys.dm_db_missing_index_groups mig
JOIN sys.dm_db_missing_index_group_stats migs ON mig.index_group_handle = migs.group_handle
JOIN sys.dm_db_missing_index_details mid ON mig.index_handle = mid.index_handle
ORDER BY impact_score DESC;

-- ============================================================
-- SECTION 6: TRANSACTION LOG HEALTH
-- ============================================================
PRINT '=== TRANSACTION LOG STATUS ==='

SELECT 
    d.name AS database_name,
    d.recovery_model_desc,
    mf.name AS log_file_name,
    mf.size * 8 / 1024 AS log_size_mb,
    ROUND(100.0 * FILEPROPERTY(d.name, 'SpaceUsed') / mf.size, 2) AS log_used_pct,
    log_reuse_wait_desc AS cannot_shrink_until,
    CASE 
        WHEN FILEPROPERTY(d.name, 'SpaceUsed') * 8.0 / 1024 > mf.size * 8.0 / 1024 * 0.8
        THEN '🔴 Log > 80% full — immediate action needed'
        WHEN mf.size * 8 / 1024 > 2000
        THEN '🟡 Log file large — consider shrinking'
        ELSE '✅ Log size OK'
    END AS status
FROM sys.databases d
JOIN sys.master_files mf ON d.database_id = mf.database_id AND mf.type = 1
WHERE d.database_id > 4
ORDER BY mf.size DESC;

-- ============================================================
-- SECTION 7: CURRENTLY RUNNING REQUESTS
-- ============================================================
PRINT '=== CURRENTLY RUNNING (> 1 second) ==='

SELECT 
    r.session_id,
    r.status,
    r.wait_type,
    r.wait_time / 1000 AS wait_time_sec,
    r.total_elapsed_time / 1000 AS running_sec,
    r.cpu_time / 1000 AS cpu_sec,
    r.logical_reads,
    DB_NAME(r.database_id) AS database_name,
    SUBSTRING(st.text, 1, 200) AS query_preview
FROM sys.dm_exec_requests r
CROSS APPLY sys.dm_exec_sql_text(r.sql_handle) st
WHERE r.session_id > 50
  AND r.total_elapsed_time > 1000  -- only show queries > 1 second
ORDER BY r.total_elapsed_time DESC;

-- ============================================================
-- SECTION 8: INDEX FRAGMENTATION (Run Weekly)
-- ============================================================
PRINT '=== INDEX FRAGMENTATION ==='

SELECT TOP 20
    OBJECT_NAME(ips.object_id) AS table_name,
    i.name AS index_name,
    ips.index_type_desc,
    ROUND(ips.avg_fragmentation_in_percent, 1) AS fragmentation_pct,
    ips.page_count,
    CASE
        WHEN ips.avg_fragmentation_in_percent < 10 THEN '✅ OK — no action needed'
        WHEN ips.avg_fragmentation_in_percent < 30 THEN '🟡 REORGANIZE recommended'
        ELSE '🔴 REBUILD recommended'
    END AS recommendation,
    CASE
        WHEN ips.avg_fragmentation_in_percent >= 30
        THEN CONCAT('ALTER INDEX [', i.name, '] ON [', OBJECT_NAME(ips.object_id), '] REBUILD WITH (ONLINE = ON);')
        WHEN ips.avg_fragmentation_in_percent >= 10
        THEN CONCAT('ALTER INDEX [', i.name, '] ON [', OBJECT_NAME(ips.object_id), '] REORGANIZE;')
        ELSE 'No action needed'
    END AS fix_script
FROM sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL, 'SAMPLED') ips
JOIN sys.indexes i ON ips.object_id = i.object_id AND ips.index_id = i.index_id
WHERE ips.page_count > 100  -- ignore tiny indexes
  AND i.name IS NOT NULL
ORDER BY ips.avg_fragmentation_in_percent DESC;



================================================================================

  الملف 09: scripts/docker/docker-compose.monitoring.yml — Docker Stack

================================================================================


version: '3.8'

# ============================================================
# Enterprise Monitoring Stack
# Covers: Load Testing Metrics, APM, Logs, Alerts
# Usage: docker-compose -f docker-compose.monitoring.yml up -d
# ============================================================

services:

  # ─────────────────────────────────────────
  # TIME-SERIES DB for k6 metrics
  # ─────────────────────────────────────────
  influxdb:
    image: influxdb:1.8
    container_name: qa_influxdb
    ports:
      - "8086:8086"
    environment:
      INFLUXDB_DB: k6
      INFLUXDB_ADMIN_USER: admin
      INFLUXDB_ADMIN_PASSWORD: ${INFLUXDB_PASSWORD:-changeme}
      INFLUXDB_HTTP_AUTH_ENABLED: "true"
    volumes:
      - influxdb_data:/var/lib/influxdb
    healthcheck:
      test: ["CMD", "curl", "-f", "http://localhost:8086/ping"]
      interval: 10s
      timeout: 5s
      retries: 10
    restart: unless-stopped

  # ─────────────────────────────────────────
  # VISUALIZATION & DASHBOARDS
  # ─────────────────────────────────────────
  grafana:
    image: grafana/grafana:latest
    container_name: qa_grafana
    ports:
      - "3000:3000"
    environment:
      GF_SECURITY_ADMIN_USER: admin
      GF_SECURITY_ADMIN_PASSWORD: ${GRAFANA_PASSWORD:-changeme}
      GF_USERS_ALLOW_SIGN_UP: "false"
      GF_SERVER_ROOT_URL: http://localhost:3000
      GF_INSTALL_PLUGINS: grafana-clock-panel,grafana-simple-json-datasource
    volumes:
      - grafana_data:/var/lib/grafana
      - ./grafana/dashboards:/etc/grafana/provisioning/dashboards
      - ./grafana/datasources:/etc/grafana/provisioning/datasources
    depends_on:
      influxdb:
        condition: service_healthy
    restart: unless-stopped

  # ─────────────────────────────────────────
  # PROMETHEUS (for app metrics)
  # ─────────────────────────────────────────
  prometheus:
    image: prom/prometheus:latest
    container_name: qa_prometheus
    ports:
      - "9090:9090"
    volumes:
      - ./prometheus/prometheus.yml:/etc/prometheus/prometheus.yml:ro
      - prometheus_data:/prometheus
    command:
      - '--config.file=/etc/prometheus/prometheus.yml'
      - '--storage.tsdb.retention.time=30d'
    restart: unless-stopped

  # ─────────────────────────────────────────
  # STRUCTURED LOG AGGREGATION
  # ─────────────────────────────────────────
  seq:
    image: datalust/seq:latest
    container_name: qa_seq
    ports:
      - "5341:80"
    environment:
      ACCEPT_EULA: "Y"
      SEQ_FIRSTRUN_ADMINPASSWORDHASH: ${SEQ_PASSWORD_HASH:-}
    volumes:
      - seq_data:/data
    restart: unless-stopped

  # ─────────────────────────────────────────
  # DISTRIBUTED TRACING
  # ─────────────────────────────────────────
  jaeger:
    image: jaegertracing/all-in-one:latest
    container_name: qa_jaeger
    ports:
      - "6831:6831/udp"   # Jaeger compact thrift
      - "16686:16686"     # Jaeger UI
      - "4317:4317"       # OTLP gRPC
      - "4318:4318"       # OTLP HTTP
    environment:
      COLLECTOR_OTLP_ENABLED: "true"
    restart: unless-stopped

  # ─────────────────────────────────────────
  # ALERT MANAGER
  # ─────────────────────────────────────────
  alertmanager:
    image: prom/alertmanager:latest
    container_name: qa_alertmanager
    ports:
      - "9093:9093"
    volumes:
      - ./alertmanager/alertmanager.yml:/etc/alertmanager/alertmanager.yml:ro
    restart: unless-stopped

volumes:
  influxdb_data:
  grafana_data:
  prometheus_data:
  seq_data:

networks:
  default:
    name: qa_monitoring_net

# ─────────────────────────────────────────
# USAGE COMMANDS:
# ─────────────────────────────────────────
# Start stack:
#   docker-compose -f docker-compose.monitoring.yml up -d
#
# Run k6 with metrics export:
#   k6 run --out influxdb=http://localhost:8086/k6 load-test.js
#
# Access Grafana:
#   http://localhost:3000 (admin/changeme)
#
# Access Jaeger traces:
#   http://localhost:16686
#
# Access Seq logs:
#   http://localhost:5341
#
# Stop stack:
#   docker-compose -f docker-compose.monitoring.yml down
#
# Stop and remove volumes:
#   docker-compose -f docker-compose.monitoring.yml down -v

