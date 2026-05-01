# Instructions

- Following Playwright test failed.
- Explain why, be concise, respect Playwright best practices.
- Provide a snippet of code with the fix, if possible.

# Test info

- Name: smoke.spec.ts >> P1 — Critical UI Flows (BlogPosts) >> Should render Quill editor on create page
- Location: tests\e2e\smoke.spec.ts:50:7

# Error details

```
Error: page.goto: net::ERR_CONNECTION_REFUSED at http://localhost:4200/blogs
Call log:
  - navigating to "http://localhost:4200/blogs", waiting until "load"

```

# Test source

```ts
  1  | import { test, expect } from '@playwright/test';
  2  | 
  3  | test.beforeEach(async ({ page }) => {
> 4  |   await page.goto('/blogs');
     |              ^ Error: page.goto: net::ERR_CONNECTION_REFUSED at http://localhost:4200/blogs
  5  |   await page.waitForLoadState('networkidle');
  6  |   if (page.url().includes('Login')) {
  7  |       await page.fill('input[type="text"]', 'admin');
  8  |       await page.fill('input[type="password"]', '1q2w3E*');
  9  |       await page.click('button[type="submit"], button[value="Login"]');
  10 |       await page.waitForURL('http://localhost:4200/**');
  11 |       await page.goto('/blogs'); // ensure we are back at the right page
  12 |   }
  13 | });
  14 | 
  15 | // P0: Authentication & Access
  16 | test.describe('P0 — Auth & Basic Navigation', () => {
  17 |   test('Should load blog list page', async ({ page }) => {
  18 |     // Navigate to blog list
  19 |     await page.goto('/blogs');
  20 |     
  21 |     // Check if the page title is correct or list is rendered
  22 |     await expect(page.locator('app-blog-list')).toBeVisible();
  23 |   });
  24 | 
  25 |   test('Should navigate to blog detail', async ({ page }) => {
  26 |     await page.goto('/blogs');
  27 |     
  28 |     // Wait for items to load and click the first one if available
  29 |     const firstPost = page.locator('app-blog-list a').first();
  30 |     if (await firstPost.isVisible()) {
  31 |       await firstPost.click();
  32 |       await expect(page.locator('app-blog-detail')).toBeVisible();
  33 |     }
  34 |   });
  35 | });
  36 | 
  37 | // P1: Critical UI Flows
  38 | test.describe('P1 — Critical UI Flows (BlogPosts)', () => {
  39 |   test('Create button should navigate to create page', async ({ page }) => {
  40 |     await page.goto('/blogs');
  41 |     
  42 |     // Assuming there is a "Create" button for admins
  43 |     const createBtn = page.getByRole('button', { name: /create/i });
  44 |     if (await createBtn.isVisible()) {
  45 |       await createBtn.click();
  46 |       await expect(page.locator('app-blog-create')).toBeVisible();
  47 |     }
  48 |   });
  49 | 
  50 |   test('Should render Quill editor on create page', async ({ page }) => {
  51 |     await page.goto('/blogs/create');
  52 |     
  53 |     // Check if quill editor is initialized
  54 |     await expect(page.locator('.ql-editor')).toBeVisible();
  55 |   });
  56 | });
  57 | 
```