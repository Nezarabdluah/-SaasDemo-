/**
 * TEST #11 v2: Breakpoint Finder Test — POST P0 FIX
 * 
 * Purpose: Verify P0 fix and find new breaking point after:
 *   - N+1 query fix (batch loading in GetListAsync)
 *   - Connection pool increase (100 → 300)
 * 
 * Pattern: +10 VUs every 1 minute (per DevOps skill STEP 3)
 * Abort threshold: error rate > 15%
 * 
 * Expected result after fix: Breaking point > 100 VUs
 */

import http from 'k6/http';
import { check, sleep } from 'k6';
import { Rate, Trend, Counter } from 'k6/metrics';

// Custom metrics
const errorRate = new Rate('errors');
const latency = new Trend('latency');
const successfulRequests = new Counter('successful_requests');
const failedRequests = new Counter('failed_requests');

// Test configuration - incremental ramp
export const options = {
  insecureSkipTLSVerify: true,
  stages: [
    // Warm-up
    { duration: '1m', target: 10 },
    
    // Incremental increase: +10 VUs every minute
    { duration: '1m', target: 20 },
    { duration: '1m', target: 30 },
    { duration: '1m', target: 40 },
    { duration: '1m', target: 50 },
    { duration: '1m', target: 60 },
    { duration: '1m', target: 70 },
    { duration: '1m', target: 80 },
    { duration: '1m', target: 90 },
    { duration: '1m', target: 100 },
    { duration: '1m', target: 110 },
    { duration: '1m', target: 120 },
    { duration: '1m', target: 130 },
    { duration: '1m', target: 140 },
    { duration: '1m', target: 150 },
    { duration: '1m', target: 160 },
    { duration: '1m', target: 170 },
    { duration: '1m', target: 180 },
    { duration: '1m', target: 190 },
    { duration: '1m', target: 200 },
    
    // Push further if system is still stable
    { duration: '1m', target: 220 },
    { duration: '1m', target: 240 },
    { duration: '1m', target: 260 },
    { duration: '1m', target: 280 },
    { duration: '1m', target: 300 },
    
    // Ramp down
    { duration: '2m', target: 0 },
  ],
  thresholds: {
    'http_req_duration': ['p(95)<2000'],
    'errors': [
      { threshold: 'rate<0.15', abortOnFail: true },
    ],
    'http_req_failed': [
      { threshold: 'rate<0.15', abortOnFail: true },
    ],
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
    console.log('🔍 Starting Breakpoint Finder Test');
    console.log('📈 Load will increase by +10 VUs every minute');
    console.log('⚠️  Test will stop automatically if error rate > 15%');
    console.log('🎯 Goal: Find maximum sustainable load');
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

  // Realistic traffic mix: 70% reads, 30% writes
  
  // 1. List BlogPosts (70% of traffic)
  if (Math.random() < 0.7) {
    const listRes = http.get(
      `${BASE_URL}/api/app/blog-post?skipCount=0&maxResultCount=20`,
      { headers, tags: { name: 'list_posts' } }
    );

    const success = check(listRes, {
      'List: status 200': (r) => r.status === 200,
      'List: response time OK': (r) => r.timings.duration < 5000,
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
            'Read: response time OK': (r) => r.timings.duration < 5000,
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

  // 3. Create BlogPost (10% of traffic)
  else {
    const timestamp = Date.now();
    const vuid = __VU;
    const payload = {
      title: `Breakpoint Test Post ${timestamp}-${vuid}`,
      slug: `breakpoint-test-${timestamp}-${vuid}`,
      content: `This is a breakpoint test post created at ${new Date().toISOString()} by VU ${vuid}. Testing system capacity limits.`,
      status: 1, // Published
    };

    const createRes = http.post(
      `${BASE_URL}/api/app/blog-post`,
      JSON.stringify(payload),
      { headers, tags: { name: 'create_post' } }
    );

    const success = check(createRes, {
      'Create: status 200/201': (r) => r.status === 200 || r.status === 201,
      'Create: response time OK': (r) => r.timings.duration < 10000,
    });

    if (success) {
      successfulRequests.add(1);
    } else {
      failedRequests.add(1);
    }

    errorRate.add(!success);
    latency.add(createRes.timings.duration);
  }

  // Realistic think time (0.5-2 seconds)
  sleep(Math.random() * 1.5 + 0.5);
}

export function teardown(data) {
  console.log('✅ Breakpoint Finder Test completed');
  console.log('📊 Review metrics to identify breaking point');
  console.log('');
  console.log('Key Questions to Answer:');
  console.log('1. At what VU count did error rate exceed 15%?');
  console.log('2. What was the maximum sustainable throughput (req/s)?');
  console.log('3. What was p95 latency at breaking point?');
  console.log('4. Which component failed first (CPU, RAM, DB, connections)?');
  console.log('5. Was the failure graceful or catastrophic?');
}
