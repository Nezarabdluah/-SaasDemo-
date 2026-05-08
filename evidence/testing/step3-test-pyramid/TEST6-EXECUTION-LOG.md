# 🧪 Test #6: Soak/Endurance Test - Execution Log

**Test Started:** 2026-05-04 11:16:02
**Status:** ⏳ IN PROGRESS
**Expected Duration:** 60 minutes

---

## Execution Command

```bash
k6 run tests/performance/soak-test.js --out json=evidence/testing/step3-test-pyramid/test6-soak-results.json > evidence/testing/step3-test-pyramid/test6-soak-output.txt 2>&1
```

---

## Monitoring Checklist

### During Test (Every 10 minutes):

- [ ] 00:10 - Check p95 latency, memory usage
- [ ] 00:20 - Check p95 latency, memory usage
- [ ] 00:30 - Check p95 latency, memory usage
- [ ] 00:40 - Check p95 latency, memory usage
- [ ] 00:50 - Check p95 latency, memory usage
- [ ] 01:00 - Final check, test complete

### Metrics to Record:

| Time | p95 Latency | Error Rate | Backend Memory | SQL Memory | Notes |
|------|-------------|------------|----------------|------------|-------|
| 00:10 | ___ ms | ___% | ___ MB | ___ MB | |
| 00:20 | ___ ms | ___% | ___ MB | ___ MB | |
| 00:30 | ___ ms | ___% | ___ MB | ___ MB | |
| 00:40 | ___ ms | ___% | ___ MB | ___ MB | |
| 00:50 | ___ ms | ___% | ___ MB | ___ MB | |
| 01:00 | ___ ms | ___% | ___ MB | ___ MB | |

---

## Pass/Fail Criteria

- [ ] p95 latency stable (no upward trend > 20%)
- [ ] Error rate < 1% throughout
- [ ] Memory growth < 10%
- [ ] No connection pool exhaustion

---

## Notes

(Add observations here during the test)

---

**Test Completion Time:** ___________
**Final Status:** ⬜ PASSED / ⬜ FAILED
