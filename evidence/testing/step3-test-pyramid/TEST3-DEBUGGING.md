# 🔧 TEST #3: Load Baseline - Debugging Session

**Date:** 2026-05-04
**Issue:** 100% authentication failures in load test
**Status:** 🔧 IN PROGRESS

---

## 🔴 Problem Description

### Symptoms (from screenshots):
1. **Test ran successfully** but with 100% errors
2. **1,336,062 iterations** completed
3. **Custom errors: 100%** - "No auth token available"
4. **Thresholds:**
   - ✓ `rate<0.01` passed (rate=0)
   - ✗ `rate>50` failed (rate=0.00332)
   - ✗ `errors < 100.00%` failed (100% errors)

### Root Cause:
The `setup()` function in `load-baseline.js` was not correctly formatting the authentication request body. It was sending a JavaScript object instead of URL-encoded form data.

---

## 🔧 Fixes Applied

### Fix #1: Correct Form Encoding
**File:** `tests/performance/load-baseline.js`

**Before:**
```javascript
const loginRes = http.post(
  `${BASE_URL}/connect/token`,
  credentials,  // ❌ Sending object directly
  { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }
);
```

**After:**
```javascript
// Convert credentials object to URL-encoded string
const formBody = Object.keys(credentials)
  .map(key => encodeURIComponent(key) + '=' + encodeURIComponent(credentials[key]))
  .join('&');

const loginRes = http.post(
  `${BASE_URL}/connect/token`,
  formBody,  // ✅ Sending properly encoded form data
  { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }
);
```

### Fix #2: Better Error Logging
Added detailed logging to help debug authentication issues:
```javascript
console.log(`🔐 Login attempt: ${loginRes.status}`);
if (loginRes.status === 200) {
  console.log('✅ Authentication successful - Token received');
} else {
  console.error('❌ Authentication failed:', loginRes.status);
  console.error('Response:', loginRes.body);
}
```

### Fix #3: Prevent Hammering on Auth Failure
Added sleep when no token is available:
```javascript
if (!token) {
  console.error('❌ No auth token available - skipping iteration');
  errorRate.add(1);
  sleep(1); // Prevent hammering
  return;
}
```

---

## 🧪 Testing Plan

### Step 1: Test Authentication Separately
Created `test-auth.js` to verify authentication works:
```bash
k6 run tests/performance/test-auth.js --iterations 1
```

**Expected Output:**
```
✅ SUCCESS! Token received
Token type: Bearer
Expires in: 3600 seconds
Token (first 50 chars): eyJhbGciOiJSUzI1NiIsImtpZCI6...
```

### Step 2: Re-run Load Baseline Test
Once authentication is confirmed working:
```bash
k6 run tests/performance/load-baseline.js
```

**Expected Results:**
- ✅ Authentication successful in setup()
- ✅ Error rate < 1%
- ✅ p95 latency < 200ms
- ✅ Throughput > 50 RPS

---

## 📋 Pre-Test Checklist

Before running the test, verify:

- [ ] **Backend is running**
  ```bash
  curl -k https://localhost:44368/api/abp/application-configuration
  ```
  Should return 200 OK

- [ ] **SQL Server is running**
  ```bash
  docker ps | grep sql
  ```
  Should show SQL Server container

- [ ] **Credentials are correct**
  - Username: `admin`
  - Password: `1q2w3E*`
  - Client ID: `SaasDemo_App`

- [ ] **SSL certificate accepted**
  k6 uses `--insecure-skip-tls-verify` flag if needed

---

## 🎯 Success Criteria

### Authentication Test (test-auth.js):
- ✅ Status 200
- ✅ Token received
- ✅ Token is valid JWT format

### Load Baseline Test (load-baseline.js):
- ✅ Setup function succeeds
- ✅ Token is passed to all iterations
- ✅ Error rate < 1%
- ✅ p95 latency < 200ms
- ✅ Throughput > 50 RPS
- ✅ All checks pass

---

## 📊 Expected Metrics

Based on previous smoke test (STEP 2), we expect:

| Metric | Expected Value | Baseline (Smoke Test) |
|--------|----------------|----------------------|
| **p95 Latency** | < 200ms | 92ms ✅ |
| **Error Rate** | < 1% | 0% ✅ |
| **Throughput** | > 50 RPS | 3.72 RPS (3 VUs) |
| **Success Rate** | > 99% | 100% ✅ |

With 10 VUs, we expect:
- Throughput: ~12-15 RPS (3.72 × 3.3)
- p95 Latency: 100-150ms (slight increase under load)
- Error Rate: < 0.5%

---

## 🔍 Troubleshooting Guide

### If authentication still fails:

1. **Check backend logs** for authentication errors
2. **Verify credentials** in `appsettings.json`
3. **Test with curl**:
   ```bash
   curl -k -X POST https://localhost:44368/connect/token \
     -H "Content-Type: application/x-www-form-urlencoded" \
     -d "grant_type=password&username=admin&password=1q2w3E*&client_id=SaasDemo_App&scope=SaasDemo offline_access"
   ```
4. **Check for rate limiting** on auth endpoint
5. **Verify SSL certificate** is not blocking requests

### If load test fails after auth succeeds:

1. **Check connection pool** - may need to increase `MaxPoolSize`
2. **Monitor SQL Server** - check for THREADPOOL waits
3. **Check memory** - look for memory leaks
4. **Review backend logs** - look for exceptions

---

## 📁 Files Modified

1. ✅ `tests/performance/load-baseline.js` - Fixed authentication
2. ✅ `tests/performance/test-auth.js` - Created auth test
3. ✅ `evidence/testing/step3-test-pyramid/TEST3-DEBUGGING.md` - This file

---

## 🎯 Next Steps

1. ✅ Fixes applied to `load-baseline.js`
2. ⏳ Run `test-auth.js` to verify authentication
3. ⏳ Run `load-baseline.js` with fixed authentication
4. ⏳ Document results in `TEST3-REPORT.md`
5. ⏳ Proceed to Test #4 (Load Sustained)

---

**Status:** 🟡 **READY FOR TESTING**
**Action Required:** Run authentication test and verify fix

