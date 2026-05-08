# 🧪 Test #6: Soak/Endurance Test - Execution Guide

**Test Type:** Endurance/Stability Test
**Duration:** 1 hour (60 minutes)
**Purpose:** Detect memory leaks and long-term stability issues
**Status:** ⏳ READY TO EXECUTE

---

## 📋 Pre-Execution Checklist

### 1. Verify System Status
- [ ] Backend API is running (https://localhost:44368)
- [ ] SQL Server is running
- [ ] Docker monitoring stack is running (optional but recommended)
- [ ] No other heavy processes running
- [ ] Sufficient disk space for logs

### 2. Open Monitoring Tools (Recommended)
- [ ] Task Manager → Performance tab (monitor RAM/CPU)
- [ ] SQL Server Management Studio → Activity Monitor
- [ ] Grafana dashboard (http://localhost:3000) if available

---

## 🚀 Execution Steps

### Step 1: Verify Backend is Running
```powershell
# Test backend health
curl https://localhost:44368/health -k
```

**Expected:** Status 200 OK

### Step 2: Execute Soak Test
```powershell
# Navigate to project root
cd E:\مشروع تجريبي

# Run the test (will take 1 HOUR)
k6 run tests/performance/soak-test.js --out json=evidence/testing/step3-test-pyramid/test6-soak-results.json > evidence/testing/step3-test-pyramid/test6-soak-output.txt 2>&1
```

### Step 3: Monitor During Execution (Every 15 minutes)

**Create monitoring log file:**
```powershell
# Create monitoring log
New-Item -Path evidence/testing/step3-test-pyramid/test6-monitoring-log.txt -ItemType File -Force
```

**Record these metrics every 15 minutes:**
```
Time: [HH:MM]
- Backend Memory: [X] MB
- SQL Server Memory: [X] MB
- CPU Usage: [X]%
- Active Connections: [X]
- p95 Latency (from k6 output): [X] ms
- Error Rate: [X]%
```

---

## 📊 What to Watch For

### 🔴 RED FLAGS (Stop test if you see these):
1. **Memory Growth:** Backend memory increases > 500MB
2. **Error Spike:** Error rate suddenly jumps to > 5%
3. **System Freeze:** System becomes unresponsive
4. **Disk Full:** Transaction log fills disk

### 🟡 YELLOW FLAGS (Monitor closely):
1. **Gradual Memory Increase:** +10% every 15 minutes
2. **Latency Creep:** p95 increases steadily
3. **Connection Pool Growth:** Active connections keep increasing

### 🟢 GREEN SIGNALS (Good):
1. **Stable Memory:** ±5% variation
2. **Stable Latency:** p95 stays within 50-150ms range
3. **Low Error Rate:** < 1%
4. **Stable Connections:** Connection count stable

---

## ⏱️ Timeline

```
00:00 - 05:00  Ramp-up phase (0 → 15 VUs)
05:00 - 55:00  Sustained load (15 VUs constant) ← MONITOR THIS
55:00 - 60:00  Ramp-down phase (15 → 0 VUs)
```

---

## 📈 Expected Metrics

### Healthy System:
- **Iterations:** ~2,700 - 3,000
- **Requests:** ~30,000 - 35,000
- **p95 Latency:** 100-200ms (stable)
- **Error Rate:** < 1%
- **Memory:** Stable (no growth)

### Unhealthy System (Memory Leak):
- **p95 Latency:** Starts at 100ms, ends at 500ms+ (growing)
- **Error Rate:** Starts at 0%, ends at 5%+ (growing)
- **Memory:** Grows continuously

---

## 🛑 How to Stop Test Early

If you need to stop the test:
```powershell
# Press Ctrl+C in the terminal running k6
# Or close the terminal window
```

---

## ✅ Post-Execution Analysis

After test completes, run:
```powershell
# View summary
Get-Content evidence/testing/step3-test-pyramid/test6-soak-output.txt -Tail 50

# Check for errors
Select-String -Path evidence/testing/step3-test-pyramid/test6-soak-output.txt -Pattern "error|fail|timeout" -CaseSensitive:False
```

---

## 📝 Success Criteria

Test PASSES if:
- ✅ p95 latency remains stable (no upward trend)
- ✅ Error rate < 1% throughout
- ✅ No memory growth > 10%
- ✅ No connection pool exhaustion
- ✅ System responsive throughout

Test FAILS if:
- ❌ p95 latency increases > 50% from start to end
- ❌ Error rate > 5% at any point
- ❌ Memory grows > 20%
- ❌ System becomes unresponsive

---

## 🔧 Troubleshooting

### Issue: Test fails to start
**Solution:** Verify backend is running and accessible

### Issue: High error rate immediately
**Solution:** Check authentication token, verify API endpoints

### Issue: Memory grows rapidly
**Solution:** This indicates a memory leak - STOP test and investigate

### Issue: Test runs too slow
**Solution:** Close other applications, check system resources

---

## 📁 Output Files

After completion, you will have:
- 	est6-soak-output.txt - Console output with metrics
- 	est6-soak-results.json - Detailed JSON results
- 	est6-monitoring-log.txt - Your manual monitoring notes

---

## 🚀 Ready to Execute?

**Command to run:**
```powershell
k6 run tests/performance/soak-test.js --out json=evidence/testing/step3-test-pyramid/test6-soak-results.json > evidence/testing/step3-test-pyramid/test6-soak-output.txt 2>&1
```

**Estimated completion time:** 1 hour from now

**Next test after this:** Test #7 - DB Health Check

---

**Good luck! 🍀**
