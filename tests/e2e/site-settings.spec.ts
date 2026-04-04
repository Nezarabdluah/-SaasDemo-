// ============================================
// Feature: Site Settings (إعدادات الموقع)
// Tested by: QA Agent — 2026-04-04
// ============================================

import { test, expect } from '@playwright/test';

test.use({ headless: false, video: 'on', screenshot: 'on' });

const BASE_URL = 'http://localhost:4200';
const LOGIN_URL = `${BASE_URL}/account/login`;
const SETTINGS_URL = `${BASE_URL}/settings`;

test.describe('Site Settings (إعدادات الموقع)', () => {

  test.beforeEach(async ({ page }) => {
    // Login as admin
    await page.goto(LOGIN_URL);
    await page.getByLabel('Username or email address').fill('admin');
    await page.getByLabel('Password').fill('1q2w3E*');
    await page.getByRole('button', { name: 'Log in' }).click();
    await page.waitForURL('**/');
    // Navigate to settings
    await page.goto(SETTINGS_URL);
    await page.waitForLoadState('networkidle');
  });

  test('T1.PageLoad.AllSectionsVisible', async ({ page }) => {
    // Arrange & Act — page already loaded in beforeEach

    // Assert — verify all 3 sections
    await expect(page.locator('text=المعلومات الأساسية')).toBeVisible();
    await expect(page.locator('text=روابط التواصل الاجتماعي')).toBeVisible();
    await expect(page.locator('text=إعدادات البريد الإلكتروني')).toBeVisible();
    await expect(page.getByRole('button', { name: /حفظ التغييرات/ })).toBeVisible();
  });

  test('T2.HappyPath.SaveSettingsSuccessfully', async ({ page }) => {
    // Arrange
    const siteNameField = page.locator('#siteName');

    // Act
    await siteNameField.clear();
    await siteNameField.fill('SaasDemo Production');
    await page.getByRole('button', { name: /حفظ التغييرات/ }).click();

    // Assert — success toast
    await expect(page.locator('.toast-success, .abp-toast-success, [class*="success"]')).toBeVisible({ timeout: 5000 });
  });

  test('T3.Validation.EmptyRequiredFieldShowsError', async ({ page }) => {
    // Arrange
    const siteNameField = page.locator('#siteName');

    // Act
    await siteNameField.clear();
    await siteNameField.blur();

    // Assert — validation message or disabled button
    const validationMsg = page.locator('text=يرجى إدخال اسم الموقع');
    const saveBtn = page.getByRole('button', { name: /حفظ التغييرات/ });

    const hasValidation = await validationMsg.isVisible().catch(() => false);
    const isDisabled = await saveBtn.isDisabled().catch(() => false);

    expect(hasValidation || isDisabled).toBeTruthy();
  });

  test('T4.SocialLinks.FillFacebookUrl', async ({ page }) => {
    // Arrange
    const fbField = page.locator('input[formControlName="facebookUrl"]');

    // Act
    await fbField.clear();
    await fbField.fill('https://facebook.com/saasdemo');

    // Assert
    await expect(fbField).toHaveValue('https://facebook.com/saasdemo');
  });

  test('T5.EmailSettings.FillSmtpFields', async ({ page }) => {
    // Arrange
    const hostField = page.locator('#smtpHost');
    const portField = page.locator('#smtpPort');

    // Act
    await hostField.clear();
    await hostField.fill('smtp.gmail.com');
    await portField.clear();
    await portField.fill('587');

    // Assert
    await expect(hostField).toHaveValue('smtp.gmail.com');
    await expect(portField).toHaveValue('587');
  });

  test('T6.Persistence.DataSurvivesReload', async ({ page }) => {
    // Arrange — fill and save
    const siteNameField = page.locator('#siteName');
    await siteNameField.clear();
    await siteNameField.fill('Persistence Check');
    await page.getByRole('button', { name: /حفظ التغييرات/ }).click();
    await page.waitForTimeout(2000);

    // Act — reload
    await page.goto(SETTINGS_URL);
    await page.waitForLoadState('networkidle');

    // Assert — value persisted
    await expect(page.locator('#siteName')).toHaveValue('Persistence Check');
  });

});
