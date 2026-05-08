/**
 * TEST #10: Spike Test (Netflix-style)
 * 
 * Purpose: Test system behavior under sudden extreme traffic burst (10× spike)
 * Duration: 10 minutes
 * Pattern: 20 VUs → 300 VUs (sudden) → 20 VUs
 * 
 * Pass Criteria:
 * - System survives the spike without crashing
 * - Recovery time < 60 seconds after spike ends
 * - No cascading failures
 * - Error rate during spike < 10%
 * - System returns to normal performance after spike
 * 
 * This test simulates viral content, flash sales, or DDoS-like traffic patterns.
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
    { duration: '2m', target: 20 },      // Baseline: normal traffic
    { duration: '30s', target: 300 },    // SPIKE: 10× sudden burst!
    { duration: '3m', target: 300 },     // Hold the spike
    { duration: '30s', target: 20 },     // Drop back to normal
    { duration: '3m', target: 20 },      // Observe recovery
    { duration: '1m', target: 0 },       // Ramp down
  ],
  thresholds: {
    'http_req_duration': ['p(95)<1500'],  // 95% of requests < 1500ms (relaxed during spike)
    'errors': ['rate<0.10'],               // Error rate < 10% (acceptable during spike)
    'http_req_failed': ['rate<0.10'],      // HTTP failures < 10%
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
    console.log('🚀 Starting spike test: 20 → 300 VUs (sudden burst)');
    console.log('⚡ WARNING: This simulates viral traffic or flash sale');
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

  // Spike test scenario: Simulate viral content access
  // Heavy read traffic (90% reads, 10% writes)
  
  // 1. List BlogPosts (80% of traffic - most common during viral spike)
  if (Math.random() < 0.8) {
    const listRes = http.get(
      `${BASE_URL}/api/app/blog-post?skipCount=0&maxResultCount=20`,
      { headers, tags: { name: 'list_posts' } }
    );

    const success = check(listRes, {
      'List: status 200': (r) => r.status === 200,
      'List: response time OK': (r) => r.timings.duration < 3000, // 3s timeout during spike
    });

    if (success) {
      successfulRequests.add(1);
    } else {
      failedRequests.add(1);
    }

    errorRate.add(!success);
    latency.add(listRes.timings.duration);
  }

  // 2. Read single BlogPost (15% of traffic - viral post access)
  else if (Math.random() < 0.95) {
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
            'Read: response time OK': (r) => r.timings.duration < 3000,
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

  // 3. Create BlogPost (5% of traffic - minimal writes during spike)
  else {
    const timestamp = Date.now();
    const vuid = __VU;
    const payload = {
      title: `Spike Test Post ${timestamp}-${vuid}`,
      slug: `spike-test-${timestamp}-${vuid}`,
      content: `This is a spike test post created at ${new Date().toISOString()} by VU ${vuid}. Testing sudden 10× traffic burst.`,
      status: 1, // Published
    };

    const createRes = http.post(
      `${BASE_URL}/api/app/blog-post`,
      JSON.stringify(payload),
      { headers, tags: { name: 'create_post' } }
    );

    const success = check(createRes, {
      'Create: status 200/201': (r) => r.status === 200 || r.status === 201,
      'Create: response time OK': (r) => r.timings.duration < 5000, // 5s timeout for writes during spike
    });

    if (success) {
      successfulRequests.add(1);
    } else {
      failedRequests.add(1);
    }

    errorRate.add(!success);
    latency.add(createRes.timings.duration);
  }

  // Very short think time during spike (0.1-0.5 second)
  sleep(Math.random() * 0.4 + 0.1);
}

export function teardown(data) {
  console.log('✅ Spike test completed');
  console.log('⚡ Test simulated sudden 10× traffic burst');
  console.log('🔍 Review metrics to assess spike resilience');
  console.log('');
  console.log('Key Questions to Answer:');
  console.log('1. Did the system survive the spike without crashing?');
  console.log('2. What was the recovery time after spike ended?');
  console.log('3. Were there any cascading failures?');
  console.log('4. Did error rate return to normal after spike?');
  console.log('5. Was there any data loss or corruption?');
}
