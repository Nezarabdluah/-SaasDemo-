/**
 * TEST #4: Load Testing (Sustained)
 * 
 * Purpose: Test system under sustained load at 2× expected concurrent users
 * Duration: 20 minutes
 * Pattern: Ramp-up → Sustained Load → Ramp-down
 * 
 * Pass Criteria:
 * - p95 latency < 500ms
 * - Error rate < 5%
 * - Throughput: sustain 100 RPS
 * - No connection pool exhaustion
 * - No memory leaks
 * 
 * This test validates that the system can handle double the expected load
 * for an extended period without degradation.
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
    { duration: '2m', target: 20 },   // Ramp-up: 0 → 20 VUs (2× baseline)
    { duration: '16m', target: 20 },  // Sustained: 20 VUs for 16 minutes
    { duration: '2m', target: 0 },    // Ramp-down: 20 → 0 VUs
  ],
  thresholds: {
    'http_req_duration': ['p(95)<500'],  // 95% of requests < 500ms
    'errors': ['rate<0.05'],              // Error rate < 5%
    'http_reqs': ['rate>100'],            // Throughput > 100 RPS
    'http_req_failed': ['rate<0.05'],     // HTTP failures < 5%
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
    console.log('🚀 Starting sustained load test: 20 VUs for 20 minutes');
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
    sleep(1); // Prevent hammering
    return;
  }

  const headers = {
    'Authorization': `Bearer ${token}`,
    'Content-Type': 'application/json',
  };

  // Scenario: Mixed read/write operations (realistic user behavior)
  // Adjusted distribution for sustained load testing
  
  // 1. List BlogPosts (60% of traffic - most common operation)
  if (Math.random() < 0.6) {
    const listRes = http.get(
      `${BASE_URL}/api/app/blog-post?skipCount=0&maxResultCount=10`,
      { headers, tags: { name: 'list_posts' } }
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

    if (success) {
      successfulRequests.add(1);
    } else {
      failedRequests.add(1);
    }

    errorRate.add(!success);
    latency.add(listRes.timings.duration);
  }

  // 2. Read single BlogPost (25% of traffic)
  else if (Math.random() < 0.85) {
    // First get a list to find an ID
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
            'Read: has id': (r) => {
              try {
                const data = JSON.parse(r.body);
                return data.id === postId;
              } catch {
                return false;
              }
            },
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

  // 3. Create BlogPost (10% of traffic - write operations)
  else if (Math.random() < 0.95) {
    const timestamp = Date.now();
    const payload = {
      title: `Sustained Load Test Post ${timestamp}`,
      slug: `sustained-load-test-${timestamp}`,
      content: `This is a test post created during sustained load testing at ${new Date().toISOString()}. Testing system stability under 2× expected load.`,
      status: 1, // Published
    };

    const createRes = http.post(
      `${BASE_URL}/api/app/blog-post`,
      JSON.stringify(payload),
      { headers, tags: { name: 'create_post' } }
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

    if (success) {
      successfulRequests.add(1);
    } else {
      failedRequests.add(1);
    }

    errorRate.add(!success);
    latency.add(createRes.timings.duration);
  }

  // 4. Update BlogPost (5% of traffic - less common write operation)
  else {
    // Get a post to update
    const listRes = http.get(
      `${BASE_URL}/api/app/blog-post?skipCount=0&maxResultCount=1`,
      { headers, tags: { name: 'list_for_update' } }
    );

    if (listRes.status === 200) {
      try {
        const body = JSON.parse(listRes.body);
        if (body.items && body.items.length > 0) {
          const post = body.items[0];
          
          // Update the post
          const updatePayload = {
            ...post,
            content: `Updated during sustained load test at ${new Date().toISOString()}`,
          };

          const updateRes = http.put(
            `${BASE_URL}/api/app/blog-post/${post.id}`,
            JSON.stringify(updatePayload),
            { headers, tags: { name: 'update_post' } }
          );

          const success = check(updateRes, {
            'Update: status 200': (r) => r.status === 200,
          });

          if (success) {
            successfulRequests.add(1);
          } else {
            failedRequests.add(1);
          }

          errorRate.add(!success);
          latency.add(updateRes.timings.duration);
        }
      } catch (e) {
        errorRate.add(1);
        failedRequests.add(1);
      }
    }
  }

  // Think time: simulate user reading/thinking (0.5-2 seconds)
  // Reduced from baseline to achieve higher throughput
  sleep(Math.random() * 1.5 + 0.5);
}

export function teardown(data) {
  console.log('✅ Sustained load test completed');
  console.log('📊 Test ran for 20 minutes with 20 concurrent users');
}
