// ============================================================
// TEST #5: ENDPOINT ISOLATION
// Purpose: Test each API endpoint in isolation to identify slowest endpoints
// Target: Identify top-3 slowest endpoints
// ============================================================

import http from 'k6/http';
import { check, sleep } from 'k6';
import { Trend, Rate } from 'k6/metrics';

// ─────────────────────────────────────────
// CONFIGURATION
// ─────────────────────────────────────────
const BASE_URL = __ENV.BASE_URL || 'https://localhost:44368';

// Custom metrics for each endpoint
const endpointLatency = new Trend('endpoint_latency', true);
const endpointErrors = new Rate('endpoint_errors');

// ─────────────────────────────────────────
// TEST CONFIGURATION
// ─────────────────────────────────────────
export const options = {
  scenarios: {
    // Test each endpoint with 5 VUs for 2 minutes
    isolation_test: {
      executor: 'constant-vus',
      vus: 5,
      duration: '2m',
    },
  },
  thresholds: {
    'endpoint_latency': ['p(95)<1000'], // Allow up to 1s for slowest endpoints
    'endpoint_errors': ['rate<0.05'],   // < 5% errors
    'http_req_duration': ['p(95)<1000'],
  },
  insecureSkipTLSVerify: true,
};

// ─────────────────────────────────────────
// ENDPOINTS TO TEST
// ─────────────────────────────────────────
const ENDPOINTS = [
  // ABP Framework endpoints
  {
    name: 'app-config',
    method: 'GET',
    url: '/api/abp/application-configuration',
    requiresAuth: false,
    category: 'Framework',
  },
  {
    name: 'localization',
    method: 'GET',
    url: '/api/abp/application-localization?cultureName=en&onlyDynamics=false',
    requiresAuth: false,
    category: 'Framework',
  },
  
  // BlogPost endpoints
  {
    name: 'blogpost-list',
    method: 'GET',
    url: '/api/app/blog-post',
    requiresAuth: true,
    category: 'BlogPost',
  },
  {
    name: 'blogpost-detail',
    method: 'GET',
    url: '/api/app/blog-post/{{id}}',
    requiresAuth: true,
    category: 'BlogPost',
    needsId: true,
  },
  
  // BlogCategory endpoints
  {
    name: 'category-list',
    method: 'GET',
    url: '/api/app/blog-category',
    requiresAuth: true,
    category: 'BlogCategory',
  },
  
  // BlogTag endpoints
  {
    name: 'tag-list',
    method: 'GET',
    url: '/api/app/blog-tag',
    requiresAuth: true,
    category: 'BlogTag',
  },
  
  // MediaFile endpoints
  {
    name: 'media-list',
    method: 'GET',
    url: '/api/app/media-file',
    requiresAuth: true,
    category: 'MediaFile',
  },
  
  // SiteSettings endpoints
  {
    name: 'settings-get',
    method: 'GET',
    url: '/api/app/site-settings',
    requiresAuth: true,
    category: 'SiteSettings',
  },
  
  // Identity endpoints
  {
    name: 'users-list',
    method: 'GET',
    url: '/api/identity/users',
    requiresAuth: true,
    category: 'Identity',
  },
  {
    name: 'roles-list',
    method: 'GET',
    url: '/api/identity/roles',
    requiresAuth: true,
    category: 'Identity',
  },
  
  // Tenant endpoints
  {
    name: 'tenants-list',
    method: 'GET',
    url: '/api/multi-tenancy/tenants',
    requiresAuth: true,
    category: 'Tenancy',
  },
];

// ─────────────────────────────────────────
// SETUP - Get authentication token
// ─────────────────────────────────────────
export function setup() {
  console.log('🔐 Authenticating...');
  
  const credentials = {
    grant_type: 'password',
    username: 'admin',
    password: '1q2w3E*',
    client_id: 'SaasDemo_App',
    scope: 'SaasDemo offline_access',
  };

  const formBody = Object.keys(credentials)
    .map(key => encodeURIComponent(key) + '=' + encodeURIComponent(credentials[key]))
    .join('&');

  const loginRes = http.post(
    `${BASE_URL}/connect/token`,
    formBody,
    {
      headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
      tags: { name: 'auth' },
    }
  );

  if (loginRes.status === 200) {
    const token = loginRes.json('access_token');
    console.log('✅ Authentication successful');
    
    // Get a BlogPost ID for detail endpoint
    const headers = {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json',
    };
    
    const listRes = http.get(`${BASE_URL}/api/app/blog-post`, { headers });
    let blogPostId = null;
    
    if (listRes.status === 200) {
      const data = listRes.json();
      if (data.items && data.items.length > 0) {
        blogPostId = data.items[0].id;
        console.log(`✅ Found BlogPost ID: ${blogPostId}`);
      }
    }
    
    return { 
      token,
      blogPostId,
      startTime: new Date().toISOString(),
    };
  } else {
    console.error('❌ Authentication failed:', loginRes.status);
    throw new Error('Authentication failed');
  }
}

// ─────────────────────────────────────────
// MAIN TEST FUNCTION
// ─────────────────────────────────────────
export default function(data) {
  const { token, blogPostId } = data;
  
  // Test each endpoint
  for (const endpoint of ENDPOINTS) {
    // Skip if requires ID but we don't have one
    if (endpoint.needsId && !blogPostId) {
      continue;
    }
    
    // Prepare URL
    let url = endpoint.url;
    if (endpoint.needsId && blogPostId) {
      url = url.replace('{{id}}', blogPostId);
    }
    
    // Prepare headers
    const headers = {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
    };
    
    if (endpoint.requiresAuth && token) {
      headers['Authorization'] = `Bearer ${token}`;
    }
    
    // Make request
    const startTime = Date.now();
    const res = http.request(
      endpoint.method,
      `${BASE_URL}${url}`,
      null,
      {
        headers,
        tags: { 
          name: endpoint.name,
          category: endpoint.category,
        },
      }
    );
    const duration = Date.now() - startTime;
    
    // Record metrics
    endpointLatency.add(duration, { 
      endpoint: endpoint.name,
      category: endpoint.category,
    });
    
    // Check response
    const success = check(res, {
      [`${endpoint.name}: status OK`]: (r) => r.status >= 200 && r.status < 300,
      [`${endpoint.name}: has body`]: (r) => r.body && r.body.length > 0,
    });
    
    if (!success) {
      endpointErrors.add(1, { endpoint: endpoint.name });
      console.error(`❌ ${endpoint.name} failed: ${res.status}`);
    }
    
    // Small delay between endpoints
    sleep(0.1);
  }
  
  // Sleep before next iteration
  sleep(1);
}

// ─────────────────────────────────────────
// TEARDOWN - Print summary
// ─────────────────────────────────────────
export function teardown(data) {
  console.log('');
  console.log('📊 Endpoint Isolation Test Completed');
  console.log(`Started: ${data.startTime}`);
  console.log(`Ended: ${new Date().toISOString()}`);
  console.log('');
  console.log('🔍 Check results for slowest endpoints:');
  console.log('   - Review endpoint_latency metric by endpoint tag');
  console.log('   - Identify top-3 slowest endpoints');
  console.log('   - Focus optimization efforts on these endpoints');
}
