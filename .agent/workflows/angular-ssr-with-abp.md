---
description: Angular SSR investigation with ABP Framework - Known limitations and workarounds
---

# Angular SSR with ABP Framework

## ⛔ Current Status: BLOCKED
ABP Lepton-X theme v4.3 is **fundamentally incompatible** with Angular SSR.

## Root Cause Analysis

### Problem 1: Direct DOM Manipulation
ABP Lepton-X services use raw `document` APIs during Angular DI initialization:
- `StyleContentStrategy.createElement()` → calls `document.createElement()`
- `DocumentDirHandlerService.setBodyDir()` → writes `document.body.dir`
- `parseTenantFromUrl()` → reads `document.location.href`

These crash because Node.js has no DOM.

### Problem 2: localStorage at Module Level
ABP OAuth module assigns `const oAuthStorage = localStorage` at the **top level** of a vendor module — before any polyfill can intercept it.

## What We Tried (and Why It Failed)

### Attempt 1: Inline Polyfills in server.ts
```typescript
// In server.ts, before imports
(global as any).localStorage = mockStorage;
```
❌ **Failed**: Webpack hoists `import` statements above inline code, so ABP vendor code executes first.

### Attempt 2: Separate polyfills.server.ts
```typescript
// polyfills.server.ts imported first in server.ts
import './polyfills.server';
```
❌ **Failed**: Same hoisting problem. All `import` statements evaluate before side-effect code.

### Attempt 3: External server-launcher.js
```javascript
// Plain JS file that injects globals BEFORE requiring Webpack bundle
global.localStorage = mockStorage;
require('./dist/SaasDemo/server/main.js');
```
✅ **Partially worked**: Fixed `localStorage` crash, but ABP still crashes during Angular's render pipeline with `document.createElement is not a function`.

### Attempt 4: domino Full DOM Emulation
```javascript
const domino = require('domino');
global.document = domino.createWindow(template).document;
```
❌ **Failed**: ABP services use Angular's injected `DOCUMENT` token during rendering, which provides a limited server-side document that doesn't support full DOM operations.

## Working Alternative: CSR + Dynamic Meta Tags

Instead of SSR, use Angular's `Title` and `Meta` services in components:

```typescript
import { Title, Meta } from '@angular/platform-browser';

// In your component
this.titleService.setTitle(`${post.title} | SaasDemo Blog`);
this.metaService.updateTag({ property: 'og:title', content: post.title });
this.metaService.updateTag({ property: 'og:description', content: post.shortDescription });
this.metaService.updateTag({ property: 'og:image', content: post.featuredImageUrl });
```

**Why this works**: Googlebot 2024+ fully executes JavaScript, so dynamic Meta Tags are indexed correctly.

## Additional Gotchas Discovered

1. **`ng build` ≠ Server Build**: `npm run build` only compiles browser. Server needs `ng run ProjectName:server`.
2. **Angular 20 CommonEngine**: Requires `allowedHosts: ['localhost']` in constructor.
3. **Node.js v24 fetch**: Built-in `fetch` (undici) ignores `NODE_TLS_REJECT_UNAUTHORIZED` set in JS. Must set as OS env variable before Node starts: `set NODE_TLS_REJECT_UNAUTHORIZED=0&& node app.js`

## When to Revisit
- When ABP releases Lepton-X with SSR support (check ABP changelog)
- When ABP migrates to Angular 20+ `@angular/ssr` native support
- Consider a "headless" approach: separate public-facing app without ABP theme for SSR
