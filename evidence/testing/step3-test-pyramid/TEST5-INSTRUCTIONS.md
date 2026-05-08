# 🎯 TEST #5: Endpoint Isolation - Execution Instructions

**Purpose:** Test each API endpoint in isolation to identify the top-3 slowest endpoints
**Duration:** ~2 minutes
**Status:** ⏳ READY TO RUN

---

## 📋 Pre-Test Checklist

### Infrastructure
- [ ] SQL Server running
- [ ] Backend API running (https://localhost:44368)
- [ ] Docker monitoring stack running (optional for metrics)

### Verification Commands
```bash
# 1. Check backend is responding
curl -k https://localhost:44368/api/abp/application-configuration

# 2. Check SQL Server
docker ps | grep sql
# OR
Get-Service | Where-Object {$_.Name -like "*SQL*"}

# 3. Check k6 is installed
k6 version
```

---

## 🚀 Test Execution

### Step 1: Run the Test
```bash
# Basic run (console output only)
k6 run tests/performance/endpoint-isolation.js

# With JSON output for analysis
k6 run tests/performance/endpoint-isolation.js \
  --out json=evidence/testing/step3-test-pyramid/test5-endpoint-isolation-results.json

# With InfluxDB output (if monitoring stack is running)
k6 run tests/performance/endpoint-isolation.js \
  --out influxdb=http://localhost:8086/k6 \
  --out json=evidence/testing/step3-test-pyramid/test5-endpoint-isolation-results.json
```

### Step 2: Save Output
```bash
# Redirect console output to file
k6 run tests/performance/endpoint-isolation.js \
  --out json=evidence/testing/step3-test-pyramid/test5-endpoint-isolation-results.json \
  > evidence/testing/step3-test-pyramid/test5-endpoint-isolation-output.txt 2>&1
```

---

## 📊 What This Test Does

### Endpoints Tested (11 total)

**Framework Endpoints (2):**
1. `app-config` - Application configuration
2. `localization` - Localization resources

**BlogPost Endpoints (2):**
3. `blogpost-list` - List all blog posts
4. `blogpost-detail` - Get single blog post

**BlogCategory Endpoints (1):**
5. `category-list` - List all categories

**BlogTag Endpoints (1):**
6. `tag-list` - List all tags

**MediaFile Endpoints (1):**
7. `media-list` - List all media files

**SiteSettings Endpoints (1):**
8. `settings-get` - Get site settings

**Identity Endpoints (2):**
9. `users-list` - List all users
10. `roles-list` - List all roles

**Tenancy Endpoints (1):**
11. `tenants-list` - List all tenants

### Test Configuration
- **Virtual Users:** 5 concurrent users
- **Duration:** 2 minutes
- **Strategy:** Each VU tests all endpoints in sequence
- **Metrics:** Latency tracked per endpoint with tags

---

## 🎯 Success Criteria

### Thresholds
- ✅ `endpoint_latency p(95) < 1000ms` - 95th percentile under 1 second
- ✅ `endpoint_errors rate < 0.05` - Less than 5% errors
- ✅ `http_req_duration p(95) < 1000ms` - Overall latency under 1 second

### Expected Outcomes
1. **All endpoints respond successfully** (status 200-299)
2. **Identify top-3 slowest endpoints** for optimization
3. **Baseline latency established** for each endpoint
4. **No authentication failures**

---

## 📈 Expected Results

Based on previous tests, we expect:

| Endpoint Category | Expected p95 Latency |
|-------------------|---------------------|
| **Framework** (app-config, localization) | 50-100ms |
| **Simple Lists** (categories, tags) | 50-150ms |
| **Complex Lists** (blog posts, media) | 100-300ms |
| **Detail Views** (single blog post) | 50-100ms |
| **Identity** (users, roles) | 100-200ms |
| **Tenancy** (tenants) | 50-150ms |

**Likely Slowest Endpoints:**
1. BlogPost List (with content, tags, categories)
2. Media File List (with file metadata)
3. Users List (with roles, permissions)

---

## 🔍 Analysis After Test

### Step 1: Review Console Output
Look for:
- ✅ Authentication successful
- ✅ All endpoints tested
- ❌ Any failed requests
- 📊 Summary metrics

### Step 2: Identify Slowest Endpoints
From the JSON results, extract latency by endpoint:
```bash
# If you have jq installed
cat evidence/testing/step3-test-pyramid/test5-endpoint-isolation-results.json | \
  jq -r 'select(.type=="Point" and .metric=="endpoint_latency") | 
         [.data.tags.endpoint, .data.value] | @tsv' | \
  sort -k2 -nr | head -10
```

### Step 3: Document Top-3 Slowest
Create a summary:
```
Top-3 Slowest Endpoints:
1. [endpoint-name]: [p95-latency]ms
2. [endpoint-name]: [p95-latency]ms
3. [endpoint-name]: [p95-latency]ms
```

---

## 📋 Post-Test Actions

### 1. Create Completion Report
```bash
evidence/testing/step3-test-pyramid/TEST5-COMPLETION-REPORT.md
```

Include:
- ✅ Test results summary
- 📊 Latency breakdown by endpoint
- 🐌 Top-3 slowest endpoints
- 💡 Optimization recommendations
- ✅ Pass/Fail verdict

### 2. Update QA Status
Update `.agent/QA-STATUS.md`:
- Progress: 4/8 → 5/8 Foundation Tests (62.5%)
- Add Test #5 results

### 3. Proceed to Test #6
If Test #5 passes, proceed to:
**Test #6: Soak/Endurance Test (1-4 hours)**

---

## 🔧 Troubleshooting

### If authentication fails:
```bash
# Test auth manually
curl -k -X POST https://localhost:44368/connect/token \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "grant_type=password&username=admin&password=1q2w3E*&client_id=SaasDemo_App&scope=SaasDemo offline_access"
```

### If endpoints return 404:
- Check backend is running
- Verify endpoint URLs in script match your API routes
- Check ABP Framework version compatibility

### If endpoints return 401:
- Token may have expired (test runs 2 minutes, token valid for 1 hour)
- Check token is being passed correctly in headers

---

## 📊 Test Pyramid Progress

**Phase 1: Foundation Tests (8 tests)**
- ✅ Test #1: Smoke Test
- ✅ Test #2: Functional API (Newman)
- ✅ Test #3: Load Baseline
- ✅ Test #4: Load Sustained
- ⏳ Test #5: Endpoint Isolation ← **CURRENT**
- ⬜ Test #6: Soak/Endurance
- ⬜ Test #7: DB Health
- ⬜ Test #8: Frontend Perf

**Progress:** 4/8 → 5/8 (62.5%)

---

**Status:** 🟡 **READY TO EXECUTE**
**Estimated Time:** 2-3 minutes
**Next Test:** Test #6 (Soak/Endurance)
