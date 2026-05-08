/**
 * Quick Authentication Test
 * Purpose: Verify that authentication works before running load tests
 */

import http from 'k6/http';

// Disable SSL verification for localhost testing
export const options = {
  insecureSkipTLSVerify: true,
};

const BASE_URL = 'https://localhost:44368';

const credentials = {
  grant_type: 'password',
  username: 'admin',
  password: '1q2w3E*',
  client_id: 'SaasDemo_App',
  scope: 'SaasDemo offline_access',
};

export default function () {
  // Convert credentials to URL-encoded format
  const formBody = Object.keys(credentials)
    .map(key => encodeURIComponent(key) + '=' + encodeURIComponent(credentials[key]))
    .join('&');

  console.log('🔐 Attempting authentication...');
  console.log('URL:', `${BASE_URL}/connect/token`);
  console.log('Body:', formBody);

  const loginRes = http.post(
    `${BASE_URL}/connect/token`,
    formBody,
    {
      headers: { 
        'Content-Type': 'application/x-www-form-urlencoded',
      },
    }
  );

  console.log('📊 Response Status:', loginRes.status);
  console.log('📊 Response Body:', loginRes.body);

  if (loginRes.status === 200) {
    try {
      const body = JSON.parse(loginRes.body);
      console.log('✅ SUCCESS! Token received');
      console.log('Token type:', body.token_type);
      console.log('Expires in:', body.expires_in, 'seconds');
      console.log('Token (first 50 chars):', body.access_token.substring(0, 50) + '...');
    } catch (e) {
      console.error('❌ Failed to parse response:', e);
    }
  } else {
    console.error('❌ Authentication failed!');
    console.error('Status:', loginRes.status);
    console.error('Body:', loginRes.body);
  }
}
