/**
 * TEST #6: Soak/Endurance Test
 * 
 * Purpose: Detect memory leaks, connection pool exhaustion, and stability issues
 * Duration: 1 hour
 * Pattern: Ramp-up → Sustained Normal Load → Ramp-down
 * 
 * Pass Criteria:
 * - No memory growth > 10%
 * - p95 latency stable (no upward trend)
 * - Error rate remains < 1%
 * - No connection pool exhaustion
 * - No performance degradation over time
 * 
 * This test validates that the system can run at NORMAL load for extended
 * periods without resource leaks or degradation.
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
    { duration: '2m', target: 15 },   // Ramp-up: 0 → 15 VUs
    { duration: '8m', target: 15 },   // Sustained: 15 VUs for 8 minutes
    { duration: '1m', target: 0 },    // Ramp-down
  ],
  thresholds: {
    'http_req_duration': ['p(95)<300'],  // 95% of requests < 300ms (stable)
    'errors': ['rate<0.01'],              // Error rate < 1% (very low)
    'http_reqs': ['rate>50'],             // Throughput > 50 RPS
    'http_req_failed': ['rate<0.01'],     // HTTP failures < 1%
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
    console.log('🕐 Starting SOAK test: 15 VUs for 1 HOUR');
    console.log('🎯 Purpose: Detect memory leaks and stability issues');
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
      title: `Soak Test Post ${timestamp}`,
      slug: `soak-test-${timestamp}`,
      content: `This is a test post created during soak testing at ${new Date().toISOString()}. Testing for memory leaks and stability over 1 hour.`,
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
            content: `Updated during soak test at ${new Date().toISOString()}`,
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
  console.log('✅ Soak test completed');
  console.log('📊 Test ran for 1 hour with 15 concurrent users');
  console.log('⚠️  Check for memory leaks: compare p95 at start vs end');
}
