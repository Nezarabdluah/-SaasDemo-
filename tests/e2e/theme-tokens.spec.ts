// ============================================
// Feature: Theme Tokens System (Deep Override)
// Tested by: Universal Playwright QA Agent — 2026-04-04
// ============================================

import { test, expect } from '@playwright/test';

test.use({ headless: false }); // Must run headed as per project guidelines

test.describe('Theme Tokens System', () => {

  test.beforeEach(async ({ page }) => {
    // Navigate to local site
    await page.goto('http://localhost:4200/settings');
    
    // Login handle if redirected to auth server
    if (page.url().includes('Account/Login')) {
      await page.locator('input[name="LoginInput.UserNameOrEmailAddress"]').fill('admin');
      await page.locator('input[name="LoginInput.Password"]').fill('1q2w3E*');
      await page.getByRole('button', { name: 'Login' }).click();
      await page.waitForTimeout(2000);
    }
  });

  test('Primary Color Deep Override', async ({ page }) => {
    // Arrange: Go to settings and set a bright pink color
    await page.goto('http://localhost:4200/settings');
    
    // Find our new text input bound to primaryColor and fill it
    // Using nth(0) assuming it's the first text input matching the placeholder #000000
    const primaryColorInput = page.getByPlaceholder('#000000').first();
    await primaryColorInput.fill('');
    await primaryColorInput.fill('#ff00ff');
    
    // Act: Save settings
    await page.getByRole('button', { name: 'حفظ التغييرات' }).click();
    await page.waitForTimeout(1500); // give time for toaster and Angular logic
    
    // Assert: Navigate to Blogs page to check global styling
    await page.goto('http://localhost:4200/blogs');
    
    // Check if the primary button has the injected style
    const newBlogBtn = page.getByRole('button', { name: 'New Blog Post' });
    await expect(newBlogBtn).toBeVisible();
    
    // Assert CSS computed values
    const btnColor = await newBlogBtn.evaluate((el) => {
        return window.getComputedStyle(el).backgroundColor;
    });
    
    // #ff00ff in RGB is rgb(255, 0, 255)
    expect(btnColor).toBe('rgb(255, 0, 255)');
    
    // Check active sidebar
    const activeSidebarItem = page.locator('.lpx-menu-item-link.selected');
    const sidebarColor = await activeSidebarItem.evaluate((el) => {
        return window.getComputedStyle(el).color;
    });
    
    // Pink color overrides text
    expect(sidebarColor).toBe('rgb(255, 0, 255)');
    
    // Take evidence screenshot
    await page.screenshot({ path: 'evidence/testing/theme-tokens/final-override-proof.png' });
  });

});
