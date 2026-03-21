import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'SaasDemo',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44368/',
    redirectUri: baseUrl,
    clientId: 'SaasDemo_App',
    responseType: 'code',
    scope: 'offline_access SaasDemo',
    requireHttps: true
  },
  apis: {
    default: {
      url: 'https://localhost:44368',
      rootNamespace: 'SaasDemo',
    },
  },
} as Environment;
