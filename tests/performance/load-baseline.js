/**
 * TEST #3: Load Testing (Baseline)
 * 
 * Purpose: Establish baseline performance under normal expected load
 * Duration: 5 minutes
 * Pattern: Ramp-up → Steady → Ramp-down
 * 
 * Pass Criteria:
 * - p95 latency < 200ms
 * - Error rate < 1%
 * - Throughput: sustain 50 RPS
 * - No connection pool exhaustion
 */

import http from 'k6/http';
import { check, sleep } from 'k6';
import { Rate, Trend } from 'k6/metrics';

// Custom metrics
const errorRate = new Rate('errors');
const latency = new Trend('latency');

// Test configuration
export const options = {
  insecureSkipTLSVerify: true,  // Disable SSL verification for localhost
  stages: [
    { duration: '1m', target: 10 },   // Ramp-up: 0 → 10 VUs
    { duration: '3m', target: 10 },   // Steady: 10 VUs for 3 minutes
    { duration: '1m', target: 0 },    // Ramp-down: 10 → 0 VUs
  ],
  thresholds: {
    'http_req_duration': ['p(95)<200'],  // 95% of requests < 200ms
    'errors': ['rate<0.01'],              // Error rate < 1%
    'http_reqs': ['rate>50'],             // Throughput > 50 RPS
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
  // Convert credentials object to URL-encoded string
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
    sleep(1); // Prevent hammering
    return;
  }

  const headers = {
    'Authorization': `Bearer ${token}`,
    'Content-Type': 'application/json',
  };

  // Scenario: Mixed read/write operations (realistic user behavior)
  
  // 1. List BlogPosts (70% of traffic - most common operation)
  if (Math.random() < 0.7) {
    const listRes = http.get(
      `${BASE_URL}/api/app/blog-post?skipCount=0&maxResultCount=10`,
      { headers }
    );

    const success = check(listRes, {
      'List: status 200': (r) => r.status === 200,
      'List: has items': (r) => {
        try {
          const body = JSON.parse(r.body);
          return Array.isArray(body.items);
        } catch {
          return false;
        }
      },
    });

    errorRate.add(!success);
    latency.add(listRes.timings.duration);
  }

  // 2. Read single BlogPost (20% of traffic)
  else if (Math.random() < 0.9) {
    // First get a list to find an ID
    const listRes = http.get(
      `${BASE_URL}/api/app/blog-post?skipCount=0&maxResultCount=1`,
      { headers }
    );

    if (listRes.status === 200) {
      try {
        const body = JSON.parse(listRes.body);
        if (body.items && body.items.length > 0) {
          const postId = body.items[0].id;

          const readRes = http.get(
            `${BASE_URL}/api/app/blog-post/${postId}`,
            { headers }
          );

          const success = check(readRes, {
            'Read: status 200': (r) => r.status === 200,
            'Read: has id': (r) => {
              try {
                const data = JSON.parse(r.body);
                return data.id === postId;
              } catch {
                return false;
              }
            },
          });

          errorRate.add(!success);
          latency.add(readRes.timings.duration);
        }
      } catch (e) {
        errorRate.add(1);
      }
    }
  }

  // 3. Create BlogPost (10% of traffic - write operations)
  else {
    const timestamp = Date.now();
    const payload = {
      title: `Load Test Post ${timestamp}`,
      slug: `load-test-post-${timestamp}`,
      content: `This is a test post created during load testing at ${new Date().toISOString()}`,
      status: 1, // Published
    };

    const createRes = http.post(
      `${BASE_URL}/api/app/blog-post`,
      JSON.stringify(payload),
      { headers }
    );

    const success = check(createRes, {
      'Create: status 200/201': (r) => r.status === 200 || r.status === 201,
      'Create: has id': (r) => {
        try {
          const body = JSON.parse(r.body);
          return body.id !== undefined;
        } catch {
          return false;
        }
      },
    });

    errorRate.add(!success);
    latency.add(createRes.timings.duration);
  }

  // Think time: simulate user reading/thinking (1-3 seconds)
  sleep(Math.random() * 2 + 1);
}

export function teardown(data) {
  console.log('✅ Load test completed');
}
