import http from 'k6/http';
import { check, sleep } from 'k6';
import { Rate, Trend } from 'k6/metrics';

// Custom metrics
const errorRate = new Rate('errors');
const apiDuration = new Trend('api_duration');

// Test configuration
export const options = {
  stages: [
    { duration: '2m', target: 20 },    // Ramp up to 20 users
    { duration: '5m', target: 20 },    // Stay at 20 users
    { duration: '2m', target: 50 },    // Scale to 50 users
    { duration: '5m', target: 50 },    // Sustain 50 users
    { duration: '2m', target: 0 },     // Ramp down
  ],
  thresholds: {
    'http_req_failed': ['rate<0.05'],           // < 5% errors
    'http_req_duration': ['p(95)<500'],         // p95 < 500ms
    'http_req_duration{name:auth}': ['p(95)<2000'],     // Auth can be slower
    'http_req_duration{name:list}': ['p(95)<300'],      // List should be fast
    'http_req_duration{name:create}': ['p(95)<1000'],   // Create moderate
    'http_req_duration{name:read}': ['p(95)<200'],      // Read should be fast
    'errors': ['rate<0.05'],                    // < 5% error rate
  },
};

const BASE_URL = 'https://localhost:44368';
let authToken = null;

// Setup: Get authentication token once per VU
export function setup() {
  const loginRes = http.post(
    `${BASE_URL}/connect/token`,
    {
      grant_type: 'password',
      username: 'admin',
      password: '1q2w3E*',
      client_id: 'SaasDemo_App',
      scope: 'SaasDemo offline_access'
    },
    {
      headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
      tags: { name: 'auth' }
    }
  );

  if (loginRes.status === 200) {
    const token = loginRes.json('access_token');
    console.log('✅ Authentication successful');
    return { token: token };
  } else {
    console.error('❌ Authentication failed:', loginRes.status);
    return { token: null };
  }
}

// Main test scenario
export default function (data) {
  if (!data.token) {
    console.error('No auth token available, skipping test');
    return;
  }

  const headers = {
    'Authorization': `Bearer ${data.token}`,
    'Content-Type': 'application/json',
  };

  // Test 1: List BlogPosts (most common operation)
  const listRes = http.get(
    `${BASE_URL}/api/app/blog-post?skipCount=0&maxResultCount=10`,
    { 
      headers: headers,
      tags: { name: 'list' }
    }
  );

  const listCheck = check(listRes, {
    'list: status 200': (r) => r.status === 200,
    'list: has items': (r) => r.json('items') !== undefined,
    'list: response time OK': (r) => r.timings.duration < 500,
  });

  errorRate.add(!listCheck);
  apiDuration.add(listRes.timings.duration, { endpoint: 'list' });

  sleep(1);

  // Test 2: Create BlogPost (write operation)
  const createPayload = JSON.stringify({
    title: `Load Test Post ${Date.now()}`,
    slug: `load-test-${Date.now()}`,
    content: 'This is a load test post created by k6',
    status: 1  // Published
  });

  const createRes = http.post(
    `${BASE_URL}/api/app/blog-post`,
    createPayload,
    { 
      headers: headers,
      tags: { name: 'create' }
    }
  );

  const createCheck = check(createRes, {
    'create: status 200': (r) => r.status === 200,
    'create: has id': (r) => r.json('id') !== undefined,
    'create: response time OK': (r) => r.timings.duration < 1500,
  });

  errorRate.add(!createCheck);
  apiDuration.add(createRes.timings.duration, { endpoint: 'create' });

  let postId = null;
  if (createRes.status === 200) {
    postId = createRes.json('id');
  }

  sleep(1);

  // Test 3: Read specific BlogPost (if created successfully)
  if (postId) {
    const readRes = http.get(
      `${BASE_URL}/api/app/blog-post/${postId}`,
      { 
        headers: headers,
        tags: { name: 'read' }
      }
    );

    const readCheck = check(readRes, {
      'read: status 200': (r) => r.status === 200,
      'read: correct id': (r) => r.json('id') === postId,
      'read: response time OK': (r) => r.timings.duration < 300,
    });

    errorRate.add(!readCheck);
    apiDuration.add(readRes.timings.duration, { endpoint: 'read' });

    sleep(1);

    // Test 4: Delete BlogPost (cleanup)
    const deleteRes = http.del(
      `${BASE_URL}/api/app/blog-post/${postId}`,
      null,
      { 
        headers: headers,
        tags: { name: 'delete' }
      }
    );

    const deleteCheck = check(deleteRes, {
      'delete: status 204': (r) => r.status === 204,
      'delete: response time OK': (r) => r.timings.duration < 500,
    });

    errorRate.add(!deleteCheck);
    apiDuration.add(deleteRes.timings.duration, { endpoint: 'delete' });
  }

  sleep(2); // Think time between iterations
}

// Teardown: Summary
export function teardown(data) {
  console.log('🏁 Load test completed');
}
