# ✅ Test #8: Frontend Performance (Lighthouse) - INSTRUCTIONS

**Test Date:** 2026-05-04
**Test Type:** Frontend Performance Audit
**Tool:** Google Lighthouse (Chrome DevTools)
**Status:** ⏳ READY TO EXECUTE

---

## 📋 Prerequisites

✅ **Angular App Running:** http://localhost:4200
- The Angular development server is already running
- You can access the app in your browser

---

## 🚀 How to Execute (Chrome DevTools Method - RECOMMENDED)

### Step 1: Open Chrome Browser
1. Open **Google Chrome** browser
2. Navigate to: **http://localhost:4200**
3. Wait for the page to fully load

### Step 2: Open DevTools
1. Press **F12** (or Right-click → Inspect)
2. Click on the **"Lighthouse"** tab
   - If you don't see it, click the **»** icon and select Lighthouse

### Step 3: Configure Lighthouse
Select the following categories:
- ✅ **Performance**
- ✅ **Accessibility**
- ✅ **Best Practices**
- ✅ **SEO**

Device: **Desktop** (default)

### Step 4: Run Analysis
1. Click **"Analyze page load"** button
2. Wait ~30-60 seconds for the analysis to complete
3. Lighthouse will reload the page and measure metrics

### Step 5: Review Results
After analysis completes, you'll see 4 scores (0-100):

#### 🎯 Target Scores (Pass Criteria):
| Category | Target | Status |
|----------|--------|--------|
| **Performance** | ≥ 90 | ⏳ |
| **Accessibility** | ≥ 95 | ⏳ |
| **Best Practices** | ≥ 90 | ⏳ |
| **SEO** | ≥ 95 | ⏳ |

### Step 6: Save Report
1. Click the **"⚙️"** icon (top-right of Lighthouse panel)
2. Select **"Save as HTML"**
3. Save to: **evidence/testing/step3-test-pyramid/test8-lighthouse-report.html**

### Step 7: Take Screenshot
1. Take a screenshot of the Lighthouse scores
2. Save to: **evidence/testing/step3-test-pyramid/test8-lighthouse-screenshot.png**

---

## 📊 What to Look For

### Performance Metrics:
- **First Contentful Paint (FCP):** < 1.8s ✅
- **Largest Contentful Paint (LCP):** < 2.5s ✅
- **Total Blocking Time (TBT):** < 200ms ✅
- **Cumulative Layout Shift (CLS):** < 0.1 ✅
- **Speed Index:** < 3.4s ✅

### Common Issues to Check:
- ❌ **Large bundle sizes** → Consider lazy loading
- ❌ **Unoptimized images** → Use WebP, lazy loading
- ❌ **Missing alt text** → Add to all images
- ❌ **Missing meta tags** → Add description, keywords
- ❌ **Console errors** → Fix JavaScript errors

---

## 🔍 Alternative Method: CLI (If DevTools Fails)

If Chrome DevTools doesn't work, try this PowerShell command:

\\\powershell
# Run Lighthouse CLI (already installed)
lighthouse http://localhost:4200 \
  --output html json \
  --output-path evidence/testing/step3-test-pyramid/test8-lighthouse \
  --chrome-flags=\"--ignore-certificate-errors\" \
  --only-categories=performance,accessibility,best-practices,seo
\\\

This will create:
- \	est8-lighthouse.report.html\ (visual report)
- \	est8-lighthouse.report.json\ (raw data)

---

## 📝 After Execution

Once you have the results, share:
1. ✅ Screenshot of the 4 scores
2. ✅ HTML report file (if saved)
3. ✅ Any critical issues found

I will then:
- Analyze the results
- Create completion report
- Provide recommendations
- Update progress tracking

---

## 🎯 Expected Results

Based on the project setup (Angular 20 + ABP Framework):

### Likely Scores:
- **Performance:** 70-85 (Angular dev mode is slower)
- **Accessibility:** 85-95 (ABP has good defaults)
- **Best Practices:** 90-100 (Modern Angular)
- **SEO:** 80-95 (Depends on meta tags)

### Known Issues (Dev Mode):
- ⚠️ **Dev mode is NOT optimized** - Production build will be much faster
- ⚠️ **Source maps included** - Increases bundle size
- ⚠️ **No minification** - Code is readable but larger

**Note:** For accurate performance testing, we should test the **production build** (\
g build --configuration production\), but for this QA phase, dev mode is acceptable to identify structural issues.

---

## 🚀 Ready to Execute!

**Current Status:**
- ✅ Angular app running on http://localhost:4200
- ✅ Chrome browser available
- ✅ Lighthouse installed
- ⏳ Waiting for manual execution

**Next:** Open Chrome, run Lighthouse, and share the results!

---

**Framework:** Enterprise DevOps & QA Master Skill v3.2
**Phase:** STEP 3 - Test Pyramid (Test #8 of 20)
**Progress:** 35% → 40% (after completion)
