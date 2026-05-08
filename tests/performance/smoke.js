// ============================================================
// SaasDemo — Smoke Test (Test #1 of 20)
// Purpose: Basic health check — "Does the system respond?"
// Duration: 1 minute with 3 virtual users
// Run: k6 run --out influxdb=http://localhost:8086/k6 tests/performance/smoke.js
// ============================================================

import http from 'k6/http';
import { check, sleep, group } from 'k6';
import { Rate, Trend, Counter } from 'k6/metrics';

// ─────────────────────────────────────────
// CONFIGURATION
// ─────────────────────────────────────────
const BASE_URL = __ENV.BASE_URL || 'https://localhost:44368';

// ─────────────────────────────────────────
// CUSTOM METRICS
// ─────────────────────────────────────────
const errorRate = new Rate('custom_error_rate');
const slowRequests = new Counter('slow_requests_over_1s');

// ─────────────────────────────────────────
// TEST CONFIGURATION (Smoke Test)
// ─────────────────────────────────────────
export const options = {
  vus: 3,
  duration: '1m',
  insecureSkipTLSVerify: true,  // Skip SSL verification for dev
  thresholds: {
    http_req_failed: ['rate<0.01'],      // < 1% errors
    http_req_duration: ['p(95)<500'],    // p95 under 500ms
    custom_error_rate: ['rate<0.01'],
  },
};

// ─────────────────────────────────────────
// SHARED HEADERS
// ─────────────────────────────────────────
const HEADERS = {
  'Accept': 'application/json',
  'Accept-Encoding': 'gzip, deflate',
};

// ─────────────────────────────────────────
// MAIN TEST FUNCTION
// ─────────────────────────────────────────
export default function () {

  group('API Check', () => {
    const res = http.get(`${BASE_URL}/api/abp/application-configuration`, { 
      headers: HEADERS, 
      tags: { name: 'api-config' } 
    });
    
    const passed = check(res, {
      'api-config: status 200': (r) => r.status === 200,
      'api-config: response fast': (r) => r.timings.duration < 500,
    });

    errorRate.add(!passed);
    if (res.timings.duration > 1000) {
      slowRequests.add(1);
    }
  });

  sleep(0.5);

  group('API — Anonymous Endpoints', () => {
    const endpoints = [
      { name: 'api-definition', url: '/api/abp/api-definition' },
    ];

    for (const ep of endpoints) {
      const res = http.get(`${BASE_URL}${ep.url}`, {
        headers: HEADERS,
        tags: { name: ep.name },
      });

      const passed = check(res, {
        [`${ep.name}: success`]: (r) => r.status === 200,
        [`${ep.name}: not empty`]: (r) => r.body && r.body.length > 0,
        [`${ep.name}: valid json`]: (r) => {
          try { 
            JSON.parse(r.body); 
            return true; 
          } catch (e) { 
            return false; 
          }
        },
      });

      errorRate.add(!passed);
      if (res.timings.duration > 1000) {
        slowRequests.add(1, { endpoint: ep.name });
      }
    }
  });

  sleep(1);
}

// ─────────────────────────────────────────
// SETUP — Runs once before test
// ─────────────────────────────────────────
export function setup() {
  console.log(`🚀 Starting Smoke Test against: ${BASE_URL}`);
  console.log(`📊 Test will run for 1 minute with 3 virtual users`);
  
  const res = http.get(`${BASE_URL}/api/abp/application-configuration`);
  if (res.status !== 200) {
    console.error(`❌ API check failed! Status: ${res.status}`);
    console.error(`Response: ${res.body}`);
    throw new Error('API check failed. Aborting test.');
  }
  
  console.log('✅ API check passed. Starting test...');
  return { startTime: new Date().toISOString() };
}

// ─────────────────────────────────────────
// TEARDOWN — Runs once after test  
// ─────────────────────────────────────────
export function teardown(data) {
  console.log(`\n📊 Test Summary:`);
  console.log(`   Started:  ${data.startTime}`);
  console.log(`   Ended:    ${new Date().toISOString()}`);
  console.log(`\n✅ Check Grafana for detailed metrics: http://localhost:3000`);
}

// ─────────────────────────────────────────
// RESULT INTERPRETATION GUIDE
// ─────────────────────────────────────────
// After test, check these metrics in Grafana:
//
// ✅ PASS Criteria:
//   - http_req_failed < 1%
//   - p95 latency < 500ms
//   - No slow_requests (> 1s)
//
// ⚠️ WARNING:
//   - p95 between 500-1000ms → needs optimization
//   - error rate 1-5% → investigate logs
//
// ❌ FAIL:
//   - p95 > 1000ms → critical performance issue
//   - error rate > 5% → system unstable
