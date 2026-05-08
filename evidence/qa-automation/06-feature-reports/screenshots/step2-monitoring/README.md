# 📸 STEP 2 Monitoring Stack — Screenshots

This folder contains evidence screenshots for STEP 2 of the Enterprise QA Framework.

## 📋 Screenshot Checklist

### Phase 1: Monitoring Stack (No SQL Server Required)
- [ ] 01-docker-containers.png — Docker Desktop showing all 5 containers
- [ ] 02-grafana-login.png — Grafana login page
- [ ] 03-grafana-home.png — Grafana home dashboard
- [ ] 04-influxdb-datasource.png — InfluxDB datasource configuration
- [ ] 05-prometheus-ui.png — Prometheus web UI
- [ ] 06-jaeger-ui.png — Jaeger tracing UI
- [ ] 07-seq-ui.png — Seq logs UI
- [ ] 08-sql-server-stopped.png — SQL Server service status (stopped)

### Phase 2: Backend & Testing (Requires SQL Server)
- [ ] 09-sql-server-running.png — SQL Server service status (running)
- [ ] 10-backend-startup.png — ASP.NET Core backend console
- [ ] 11-swagger-ui.png — Swagger API documentation
- [ ] 12-health-endpoint.png — Health check endpoint response
- [ ] 13-k6-test-running.png — k6 test execution console
- [ ] 14-influxdb-metrics.png — InfluxDB metrics query
- [ ] 15-grafana-import.png — Grafana dashboard import
- [ ] 16-grafana-live-metrics.png — k6 dashboard during test
- [ ] 17-grafana-test-summary.png — k6 dashboard after test
- [ ] 18-test-results-table.png — Final test results summary

## 🎯 Current Status

**Phase 1:** ✅ Ready to capture (monitoring stack is running)
**Phase 2:** ⏸️ Waiting for SQL Server to start

## 📝 Instructions

1. Open the services in your browser
2. Take screenshots as per the guide in step2-screenshot-guide.md
3. Save screenshots with the exact filenames listed above
4. Check off each screenshot in this README

## 🔗 Service URLs

- Grafana: http://localhost:3000 (admin/admin123)
- Prometheus: http://localhost:9090
- Jaeger: http://localhost:16686
- Seq: http://localhost:5341
- InfluxDB: http://localhost:8086

