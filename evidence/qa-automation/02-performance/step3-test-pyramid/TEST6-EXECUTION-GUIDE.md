# 🧪 Test #6: Soak/Endurance Test - Execution Guide

**Test Date:** 2026-05-04
**Status:** ⏳ READY TO EXECUTE
**Duration:** 1 hour (60 minutes)
**Purpose:** Detect memory leaks and stability issues

---

## 📋 Pre-Execution Checklist

### 1. Verify Backend is Running
```powershell
# Check if backend is running on port 44368
Test-NetConnection -ComputerName localhost -Port 44368
```

### 2. Verify SQL Server is Running
```powershell
# Check SQL Server service
Get-Service -Name MSSQLSERVER
```

### 3. Verify Docker Monitoring Stack
```powershell
docker ps --format 'table {{.Names}}\t{{.Status}}\t{{.Ports}}'
```

Expected: 5 containers running (Grafana, InfluxDB, Prometheus, Seq, Jaeger)

---

## 🚀 Execute Test #6

### Command:
```bash
k6 run tests/performance/soak-test.js --out json=evidence/testing/step3-test-pyramid/test6-soak-results.json > evidence/testing/step3-test-pyramid/test6-soak-output.txt 2>&1
```

### What to Expect:
- **Duration:** 60 minutes (1 hour)
- **VUs:** 15 concurrent users
- **Pattern:** 5min ramp-up → 50min sustained → 5min ramp-down
- **Requests:** ~27,000 total (estimated)

---

## 📊 Monitoring During Test

### Open These Dashboards:

1. **Grafana:** http://localhost:3000
   - Monitor: Latency trends, Error rates, Throughput

2. **Task Manager / Resource Monitor:**
   - Monitor: Backend memory usage
   - Monitor: SQL Server memory usage

3. **SQL Server Management Studio:**
   - Run this query every 10 minutes:
```sql
-- Check connection pool
SELECT COUNT(*) as total_connections,
       SUM(CASE WHEN status = 'sleeping' THEN 1 ELSE 0 END) as idle,
       SUM(CASE WHEN status = 'running' THEN 1 ELSE 0 END) as active
FROM sys.dm_exec_sessions
WHERE is_user_process = 1;

-- Check transaction log size
SELECT name, 
       size * 8 / 1024 AS log_size_mb,
       CAST(100.0 * FILEPROPERTY(name, 'SpaceUsed') / size AS DECIMAL(5,2)) AS log_used_pct
FROM sys.master_files
WHERE type = 1 AND database_id = DB_ID('SaasDemoDB');
```

---

## ✅ Pass Criteria

The test PASSES if:
- ✅ p95 latency remains stable (no upward trend)
- ✅ Error rate < 1% throughout
- ✅ Backend memory growth < 10%
- ✅ SQL Server memory stable
- ✅ Transaction log growth < 50%
- ✅ No connection pool exhaustion

---

## ⚠️ Warning Signs (Memory Leak Indicators)

Watch for these RED FLAGS:
- 🔴 p95 latency INCREASES steadily over time
- 🔴 Error rate INCREASES after X minutes
- 🔴 Backend memory grows continuously
- 🔴 Connection pool hits maximum
- 🔴 Transaction log grows excessively

---

## 📝 After Test Completion

1. **Review Results:**
```bash
# View summary
cat evidence/testing/step3-test-pyramid/test6-soak-output.txt | tail -50
```

2. **Check for Memory Leaks:**
   - Compare p95 latency at minute 10 vs minute 55
   - If increase > 20% → Memory leak suspected

3. **Analyze Grafana:**
   - Look for upward trends in latency
   - Check error rate stability

4. **Document Findings:**
   - Create TEST6-COMPLETION-REPORT.md
   - Include screenshots from Grafana
   - Note any anomalies

---

## 🎯 Next Steps After Test #6

If PASSED → Proceed to Test #7 (DB Health Check)
If FAILED → Investigate and fix issues before continuing

---

**Ready to Execute?** Run the command above and monitor for 1 hour.
