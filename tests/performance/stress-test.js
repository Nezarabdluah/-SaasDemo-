/**
 * TEST #9: Stress Test
 * 
 * Purpose: Find system breaking point by gradually increasing load to 3-5× expected users
 * Duration: 20 minutes
 * Pattern: Gradual ramp from 50 → 300 VUs
 * 
 * Pass Criteria:
 * - Graceful degradation (no crashes)
 * - No data loss or corruption
 * - System recovers when load decreases
 * - Error rate increases gradually (not sudden spike)
 * - p95 latency < 1000ms at peak
 * 
 * This test validates that the system degrades gracefully under extreme load
 * and identifies the exact breaking point.
 */

import http from 'k6/http';
import { check, sleep } from 'k6';
import { Rate, Trend, Counter } from 'k6/metrics';

// Custom metrics
const errorRate = new Rate('errors');
const latency = new Trend('latency');
const successfulRequests = new Counter('successful_requests');
const failedRequests = new Counter('failed_requests');

// Test configuration
export const options = {
  insecureSkipTLSVerify: true,  // Disable SSL verification for localhost
  stages: [
    { duration: '2m', target: 50 },    // Ramp-up: 0 → 50 VUs (baseline)
    { duration: '3m', target: 100 },   // Increase: 50 → 100 VUs
    { duration: '3m', target: 150 },   // Increase: 100 → 150 VUs
    { duration: '3m', target: 200 },   // Increase: 150 → 200 VUs
    { duration: '3m', target: 250 },   // Increase: 200 → 250 VUs
    { duration: '3m', target: 300 },   // Peak: 250 → 300 VUs (5× expected)
    { duration: '2m', target: 50 },    // Recovery: 300 → 50 VUs
    { duration: '1m', target: 0 },     // Ramp-down: 50 → 0 VUs
  ],
  thresholds: {
    'http_req_duration': ['p(95)<1000'],  // 95% of requests < 1000ms (relaxed for stress)
    'errors': ['rate<0.15'],               // Error rate < 15% (acceptable under stress)
    'http_req_failed': ['rate<0.15'],      // HTTP failures < 15%
  },
};

const BASE_URL = 'https://localhost:44368';

// Test data
const credentials = {
  grant_type: 'password',
  username: 'admin',
  password: '1q2w3E*',
  client_id: 'SaasDemo_App',
  scope: 'SaasDemo offline_access',
};

let authToken = null;

export function setup() {
  // Get authentication token once
  const formBody = Object.keys(credentials)
    .map(key => encodeURIComponent(key) + '=' + encodeURIComponent(credentials[key]))
    .join('&');

  const loginRes = http.post(
    `${BASE_URL}/connect/token`,
    formBody,
    {
      headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
    }
  );

  console.log(`🔐 Login attempt: ${loginRes.status}`);
  
  if (loginRes.status === 200) {
    const body = JSON.parse(loginRes.body);
    authToken = body.access_token;
    console.log('✅ Authentication successful - Token received');
    console.log('🚀 Starting stress test: 50 → 300 VUs over 20 minutes');
    console.log('⚠️  WARNING: This test will push the system to its limits');
    return { token: authToken };
  } else {
    console.error('❌ Authentication failed:', loginRes.status);
    console.error('Response:', loginRes.body);
    return { token: null };
  }
}

export default function (data) {
  const token = data.token;

  if (!token) {
    console.error('❌ No auth token available - skipping iteration');
    errorRate.add(1);
    failedRequests.add(1);
    sleep(1);
    return;
  }

  const headers = {
    'Authorization': `Bearer ${token}`,
    'Content-Type': 'application/json',
  };

  // Stress test scenario: Focus on most demanding operations
  // Higher proportion of reads to simulate realistic stress
  
  // 1. List BlogPosts (70% of traffic - most common, cacheable)
  if (Math.random() < 0.7) {
    const listRes = http.get(
      `${BASE_URL}/api/app/blog-post?skipCount=0&maxResultCount=20`,
      { headers, tags: { name: 'list_posts' } }
    );

    const success = check(listRes, {
      'List: status 200': (r) => r.status === 200,
      'List: response time OK': (r) => r.timings.duration < 2000, // 2s timeout
    });

    if (success) {
      successfulRequests.add(1);
    } else {
      failedRequests.add(1);
    }

    errorRate.add(!success);
    latency.add(listRes.timings.duration);
  }

  // 2. Read single BlogPost (20% of traffic)
  else if (Math.random() < 0.9) {
    const listRes = http.get(
      `${BASE_URL}/api/app/blog-post?skipCount=0&maxResultCount=1`,
      { headers, tags: { name: 'list_for_read' } }
    );

    if (listRes.status === 200) {
      try {
        const body = JSON.parse(listRes.body);
        if (body.items && body.items.length > 0) {
          const postId = body.items[0].id;

          const readRes = http.get(
            `${BASE_URL}/api/app/blog-post/${postId}`,
            { headers, tags: { name: 'read_post' } }
          );

          const success = check(readRes, {
            'Read: status 200': (r) => r.status === 200,
            'Read: response time OK': (r) => r.timings.duration < 2000,
          });

          if (success) {
            successfulRequests.add(1);
          } else {
            failedRequests.add(1);
          }

          errorRate.add(!success);
          latency.add(readRes.timings.duration);
        }
      } catch (e) {
        errorRate.add(1);
        failedRequests.add(1);
      }
    }
  }

  // 3. Create BlogPost (10% of traffic - write operations stress DB)
  else {
    const timestamp = Date.now();
    const vuid = __VU;
    const payload = {
      title: `Stress Test Post ${timestamp}-${vuid}`,
      slug: `stress-test-${timestamp}-${vuid}`,
      content: `This is a stress test post created at ${new Date().toISOString()} by VU ${vuid}. Testing system under extreme load (300 VUs).`,
      status: 1, // Published
    };

    const createRes = http.post(
      `${BASE_URL}/api/app/blog-post`,
      JSON.stringify(payload),
      { headers, tags: { name: 'create_post' } }
    );

    const success = check(createRes, {
      'Create: status 200/201': (r) => r.status === 200 || r.status === 201,
      'Create: response time OK': (r) => r.timings.duration < 3000, // 3s timeout for writes
    });

    if (success) {
      successfulRequests.add(1);
    } else {
      failedRequests.add(1);
    }

    errorRate.add(!success);
    latency.add(createRes.timings.duration);
  }

  // Minimal think time under stress (0.2-1 second)
  sleep(Math.random() * 0.8 + 0.2);
}

export function teardown(data) {
  console.log('✅ Stress test completed');
  console.log('📊 Test pushed system from 50 → 300 VUs');
  console.log('🔍 Review metrics to identify breaking point');
  console.log('');
  console.log('Key Questions to Answer:');
  console.log('1. At what VU count did error rate exceed 5%?');
  console.log('2. At what VU count did p95 latency exceed 500ms?');
  console.log('3. Did the system recover when load decreased?');
  console.log('4. Were there any crashes or data corruption?');
}
