# SLO/SLI/SLA Framework — SaasDemo

**Created:** 2026-05-08
**Based on:** Google SRE Standard (devops-qa-enterprise-full.md STEP 5)

---

## Service: BlogPost API (`/api/app/blog-post`)

### SLOs (Service Level Objectives)

```yaml
service: blogpost-api
tier: T2-High

slos:
  availability:
    target: 99.9%                    # "three nines" = 8.7h downtime/year
    measurement: success_rate_over_30d
    current_baseline: 99.99%         # from Test #4 (Load Sustained)

  latency:
    target: 95% of requests < 200ms
    measurement: p95_over_1h
    current_baseline: 111ms          # from Test #4 (p95)
    warning_threshold: 500ms
    critical_threshold: 2000ms

  throughput:
    target: sustain 20 RPS without degradation
    measurement: sustained_load_test_20min
    current_baseline: 17.19 RPS      # from Test #4

error_budget:
  monthly_allowance: 0.1%            # = 43.8 minutes/month
  alert_at: 50%_consumed             # alert when 21.9 min burned
  freeze_deploys_at: 90%_consumed    # stop deploys at 39.4 min burned
```

### SLIs (Service Level Indicators)

| SLI | Source | Query/Method |
|-----|--------|-------------|
| **Availability** | k6 / Prometheus | `http_req_failed` rate |
| **Latency (p95)** | k6 / InfluxDB | `percentile(http_req_duration, 95)` |
| **Throughput** | k6 | `http_reqs` rate per second |
| **Error Rate** | k6 | `http_req_failed` / `http_reqs` |
| **Saturation** | SQL DMV | `sys.dm_exec_sessions` count vs Max Pool |

### SLA (for future production — NOT active yet)

| Metric | SLA Target | Penalty |
|--------|-----------|---------|
| Monthly Uptime | ≥ 99.5% | — (internal SaaS, no customer SLA yet) |
| Incident Response | < 4 hours for SEV-1 | — |
| Data Recovery (RPO) | < 24 hours | — |

---

## ⚠️ Current Gaps vs SLO

| SLO | Target | Current | Gap |
|-----|--------|---------|-----|
| Latency p95 | < 200ms | 471ms (Load Baseline) | 🔴 **2.4× over target** |
| Availability under spike | > 90% | 63.5% (Spike Test) | 🔴 **26.5% gap** |
| Breakpoint capacity | > 50 VUs | 9 VUs | 🔴 **5.5× under target** |
| Error rate under stress | < 5% | 36.5% (Spike) | 🔴 **7.3× over target** |

---

## 🎯 Error Budget Consumption (Simulated)

Based on test results, if current performance were in production:
- **Normal load:** 0% error budget consumed ✅
- **2× load (sustained):** 0.01% consumed ✅
- **10× spike:** Would consume **entire month's error budget in ~2 hours** 🔴

**Recommendation:** System is NOT ready for production spikes. Must fix P0 performance issues before defining production SLAs.
