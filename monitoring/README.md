# 📊 SaasDemo Monitoring Stack

Enterprise-grade monitoring infrastructure for performance testing, observability, and production readiness.

---

## 🏗️ Stack Components

| Service | Port | Purpose | Access |
|---------|------|---------|--------|
| **Grafana** | 3000 | Dashboards & Visualization | http://localhost:3000 |
| **InfluxDB** | 8086 | k6 Metrics Storage | http://localhost:8086 |
| **Prometheus** | 9090 | App Metrics Scraping | http://localhost:9090 |
| **Seq** | 5341 | Structured Logs (.NET) | http://localhost:5341 |
| **Jaeger** | 16686 | Distributed Tracing | http://localhost:16686 |

---

## 🚀 Quick Start

### 1. Prerequisites
- Docker Desktop installed and running
- k6 installed: `choco install k6` (Windows) or `brew install k6` (Mac)

### 2. Start Monitoring Stack
```bash
# From project root
docker-compose -f docker-compose.monitoring.yml up -d

# Verify all services are running
docker-compose -f docker-compose.monitoring.yml ps
```

### 3. Access Grafana
1. Open: http://localhost:3000
2. Login: `admin` / `admin123`
3. Add InfluxDB datasource (already provisioned)
4. Import k6 dashboard (ID: 2587)

### 4. Run First Performance Test
```bash
# Make sure ASP.NET Core backend is running on port 44300
cd aspnet-core
dotnet run --project src/SaasDemo.HttpApi.Host

# In another terminal, run smoke test
k6 run --out influxdb=http://localhost:8086/k6 tests/performance/smoke.js
```

### 5. View Results in Grafana
- Navigate to Dashboards → k6 Load Testing Results
- Check the Four Golden Signals:
  - **Latency:** p50, p95, p99
  - **Traffic:** Requests per second
  - **Errors:** Error rate %
  - **Saturation:** (will be added when app is instrumented)

---

## 📋 Available Performance Tests

| Test | File | VUs | Duration | Purpose |
|------|------|-----|----------|---------|
| **Smoke** | `smoke.js` | 3 | 1 min | Basic health check |
| **Load** | `load.js` | 20-100 | 20 min | Expected production load |
| **Stress** | `stress.js` | 50-300 | 25 min | Find breaking point |
| **Spike** | `spike.js` | 20-300 | 12 min | Sudden traffic burst |
| **Soak** | `soak.js` | 50 | 3 hours | Memory leak detection |

---

## 🎯 Four Golden Signals

### 1. Latency
**What to measure:** Response time percentiles (p50, p95, p99)

**Targets:**
- p95 < 200ms → ✅ Good
- p95 < 500ms → ⚠️ Acceptable
- p95 > 1000ms → ❌ Critical

**Where to check:** Grafana → k6 Dashboard → HTTP Request Duration

### 2. Traffic
**What to measure:** Requests per second (RPS), concurrent users

**Targets:**
- Baseline: Document current capacity
- Goal: Handle 2× expected load without degradation

**Where to check:** Grafana → k6 Dashboard → Virtual Users & RPS

### 3. Errors
**What to measure:** HTTP 4xx/5xx rate, exceptions

**Targets:**
- < 0.1% → ✅ Excellent
- < 1% → ⚠️ Acceptable
- > 5% → ❌ Critical

**Where to check:** Grafana → k6 Dashboard → Error Rate

### 4. Saturation
**What to measure:** CPU%, RAM%, DB connections, thread pool

**Targets:**
- < 70% → ✅ Healthy
- 70-85% → ⚠️ Warning
- > 85% → ❌ Critical

**Where to check:** 
- Windows: Task Manager / Performance Monitor
- SQL Server: Run `tests/database/connection-pool-health.sql`

---

## 🔧 Troubleshooting

### Docker containers won't start
```bash
# Check if ports are already in use
netstat -ano | findstr "3000 8086 9090"

# Stop and remove all containers
docker-compose -f docker-compose.monitoring.yml down -v

# Restart
docker-compose -f docker-compose.monitoring.yml up -d
```

### k6 can't connect to InfluxDB
```bash
# Verify InfluxDB is healthy
curl http://localhost:8086/ping

# Check InfluxDB logs
docker logs saasdemo_influxdb

# Recreate database
docker exec -it saasdemo_influxdb influx -execute "CREATE DATABASE k6"
```

### Grafana shows "No data"
1. Verify k6 test ran successfully
2. Check InfluxDB has data: http://localhost:8086
3. Verify datasource connection in Grafana
4. Check time range in dashboard (last 15 minutes)

---

## 📚 Next Steps

1. **Instrument ASP.NET Core** with OpenTelemetry
2. **Add custom metrics** for business logic
3. **Setup alerts** for SLO breaches
4. **Create custom dashboards** for specific features
5. **Run full 20-test suite** (Foundation → Stress → Chaos)

---

## 🔗 References

- [k6 Documentation](https://k6.io/docs/)
- [Grafana Dashboards](https://grafana.com/grafana/dashboards/)
- [InfluxDB Query Language](https://docs.influxdata.com/influxdb/v1.8/query_language/)
- [Four Golden Signals (Google SRE)](https://sre.google/sre-book/monitoring-distributed-systems/)

---

**Last Updated:** 2026-05-03
