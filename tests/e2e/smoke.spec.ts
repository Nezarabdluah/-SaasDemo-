import { test, expect } from '@playwright/test';

test.beforeEach(async ({ page }) => {
  await page.goto('/blogs');
  await page.waitForLoadState('networkidle');
  if (page.url().includes('Login')) {
      await page.fill('input[type="text"]', 'admin');
      await page.fill('input[type="password"]', '1q2w3E*');
      await page.click('button[type="submit"], button[value="Login"]');
      await page.waitForURL('http://localhost:4200/**');
      await page.goto('/blogs'); // ensure we are back at the right page
  }
});

// P0: Authentication & Access
test.describe('P0 — Auth & Basic Navigation', () => {
  test('Should load blog list page', async ({ page }) => {
    // Navigate to blog list
    await page.goto('/blogs');
    
    // Check if the page title is correct or list is rendered
    await expect(page.locator('app-blog-list')).toBeVisible();
  });

  test('Should navigate to blog detail', async ({ page }) => {
    await page.goto('/blogs');
    
    // Wait for items to load and click the first one if available
    const firstPost = page.locator('app-blog-list a').first();
    if (await firstPost.isVisible()) {
      await firstPost.click();
      await expect(page.locator('app-blog-detail')).toBeVisible();
    }
  });
});

// P1: Critical UI Flows
test.describe('P1 — Critical UI Flows (BlogPosts)', () => {
  test('Create button should navigate to create page', async ({ page }) => {
    await page.goto('/blogs');
    
    // Assuming there is a "Create" button for admins
    const createBtn = page.getByRole('button', { name: /create/i });
    if (await createBtn.isVisible()) {
      await createBtn.click();
      await expect(page.locator('app-blog-create')).toBeVisible();
    }
  });

  test('Should render Quill editor on create page', async ({ page }) => {
    await page.goto('/blogs/create');
    
    // Check if quill editor is initialized
    await expect(page.locator('.ql-editor')).toBeVisible();
  });
});
