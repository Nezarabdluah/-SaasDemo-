# 🚀 Start Backend API - Instructions

**Purpose:** Start the ASP.NET Core backend before running Test #5
**Status:** ⏳ REQUIRED

---

## 🔧 Option 1: Start from Visual Studio

1. Open `aspnet-core/SaasDemo.sln` in Visual Studio
2. Set `SaasDemo.HttpApi.Host` as startup project
3. Press F5 or click "Run"
4. Wait for: `Now listening on: https://localhost:44368`

---

## 🔧 Option 2: Start from Command Line

### PowerShell:
```powershell
# Navigate to project
cd aspnet-core

# Run the backend
dotnet run --project src/SaasDemo.HttpApi.Host/SaasDemo.HttpApi.Host.csproj
```

### Expected Output:
```
[INF] Initialized all ABP modules.
[INF] Now listening on: https://localhost:44368
[INF] Application started. Press Ctrl+C to shut down.
```

---

## ✅ Verify Backend is Running

### Test 1: Health Check
```powershell
curl.exe -k https://localhost:44368/api/abp/application-configuration
```

**Expected:** JSON response with application configuration

### Test 2: Port Check
```powershell
Test-NetConnection -ComputerName localhost -Port 44368
```

**Expected:** `TcpTestSucceeded : True`

---

## 📊 Current System Status

- ✅ Docker Monitoring: Running (5/5 containers)
- ✅ SQL Server: Running
- ⏳ Backend API: **Needs to be started**

---

## 🎯 After Backend Starts

**Proceed to Test #5:**
```bash
k6 run tests/performance/endpoint-isolation.js \
  --out json=evidence/testing/step3-test-pyramid/test5-endpoint-isolation-results.json \
  > evidence/testing/step3-test-pyramid/test5-endpoint-isolation-output.txt 2>&1
```

---

**Next:** Once backend is running, execute Test #5 (2 minutes)
