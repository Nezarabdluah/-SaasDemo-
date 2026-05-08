# 🎯 Test #6: دليل التنفيذ السريع

## الخطوة 1️⃣: التحقق من الجاهزية

### تحقق من Backend:
```powershell
Test-NetConnection -ComputerName localhost -Port 44368
```
✅ يجب أن يكون: TcpTestSucceeded = True

### تحقق من SQL Server:
```powershell
Get-Service -Name MSSQLSERVER | Select-Object Status
```
✅ يجب أن يكون: Status = Running

### تحقق من Docker:
```powershell
docker ps
```
✅ يجب أن ترى 5 containers تعمل

---

## الخطوة 2️⃣: تشغيل الاختبار

### الأمر:
```bash
k6 run tests/performance/soak-test.js --out json=evidence/testing/step3-test-pyramid/test6-soak-results.json > evidence/testing/step3-test-pyramid/test6-soak-output.txt 2>&1
```

### المدة: ⏱️ ساعة كاملة (60 دقيقة)

---

## الخطوة 3️⃣: المراقبة أثناء التشغيل

### افتح هذه النوافذ:

1. **Grafana:** http://localhost:3000
   - راقب: Latency, Errors, Throughput

2. **Task Manager:**
   - راقب: استخدام الذاكرة للـ Backend
   - راقب: استخدام الذاكرة لـ SQL Server

3. **Terminal الخاص بـ k6:**
   - راقب: الأرقام المباشرة

---

## الخطوة 4️⃣: ما الذي تبحث عنه؟

### ✅ علامات النجاح:
- p95 latency مستقر (لا يزداد)
- Error rate < 1%
- الذاكرة مستقرة

### 🔴 علامات المشاكل:
- p95 latency يزداد مع الوقت → تسريب ذاكرة
- Error rate يزداد → مشكلة Connection Pool
- الذاكرة تنمو باستمرار → تسريب ذاكرة

---

## الخطوة 5️⃣: بعد انتهاء الاختبار

### راجع النتائج:
```bash
cat evidence/testing/step3-test-pyramid/test6-soak-output.txt | tail -50
```

### قارن:
- p95 في الدقيقة 10 مع p95 في الدقيقة 55
- إذا زاد > 20% → مشكلة!

---

## 🚀 جاهز للتنفيذ؟

1. تأكد من أن كل شيء يعمل
2. شغل الأمر أعلاه
3. راقب لمدة ساعة
4. حلل النتائج

**الاختبار التالي:** Test #7 - DB Health Check

---

**ملاحظة مهمة:** هذا الاختبار يستغرق ساعة كاملة. لا تقاطعه!
