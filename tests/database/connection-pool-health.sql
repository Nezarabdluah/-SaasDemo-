-- ============================================================
-- SaasDemo — Connection Pool Health Check
-- Purpose: Monitor SQL Server connection pool during load tests
-- Run: sqlcmd -S localhost -d SaasDemo -i connection-pool-health.sql
-- ============================================================

PRINT '========================================';
PRINT 'CONNECTION POOL HEALTH CHECK';
PRINT 'Timestamp: ' + CONVERT(VARCHAR, GETDATE(), 120);
PRINT '========================================';
PRINT '';

-- ============================================================
-- SECTION 1: CURRENT CONNECTION STATE
-- ============================================================
PRINT '--- SECTION 1: Current Connection State ---';
SELECT 
    DB_NAME(database_id) AS database_name,
    COUNT(*) AS total_connections,
    SUM(CASE WHEN status = 'sleeping' THEN 1 ELSE 0 END) AS idle_connections,
    SUM(CASE WHEN status = 'running'  THEN 1 ELSE 0 END) AS active_connections,
    SUM(CASE WHEN wait_time > 5000    THEN 1 ELSE 0 END) AS stalled_5s_plus,
    SUM(CASE WHEN wait_time > 30000   THEN 1 ELSE 0 END) AS stalled_30s_plus,
    MAX(wait_time) AS max_wait_ms,
    MIN(login_time) AS oldest_connection
FROM sys.dm_exec_sessions
WHERE is_user_process = 1
GROUP BY database_id
ORDER BY total_connections DESC;

PRINT '';
PRINT '✅ Healthy: idle ≈ pool size, active = current load, stalled = 0';
PRINT '❌ Danger: stalled > 0, or total approaching max_pool_size';
PRINT '';

-- ============================================================
-- SECTION 2: THREADPOOL WAIT (Connection Exhaustion Signal)
-- ============================================================
PRINT '--- SECTION 2: THREADPOOL Wait Stats ---';
SELECT 
    wait_type,
    waiting_tasks_count,
    wait_time_ms,
    max_wait_time_ms,
    CASE 
        WHEN wait_type = 'THREADPOOL' AND waiting_tasks_count > 1000 
        THEN '🔴 CRITICAL: Connection pool exhausted!'
        WHEN wait_type = 'THREADPOOL' AND waiting_tasks_count > 100 
        THEN '⚠️ WARNING: High connection pressure'
        ELSE '✅ OK'
    END AS status
FROM sys.dm_os_wait_stats
WHERE wait_type IN ('THREADPOOL', 'RESOURCE_SEMAPHORE', 'ASYNC_NETWORK_IO')
ORDER BY wait_time_ms DESC;

PRINT '';
PRINT 'THREADPOOL count > 1,000 → Connection pool exhausted (increase Max Pool Size)';
PRINT 'RESOURCE_SEMAPHORE → Memory grant waits';
PRINT 'ASYNC_NETWORK_IO → Client reading slowly (app-side issue)';
PRINT '';

-- ============================================================
-- SECTION 3: TOP 5 SLOWEST QUERIES (During Load Test)
-- ============================================================
PRINT '--- SECTION 3: Top 5 Slowest Queries ---';
SELECT TOP 5
    qs.execution_count,
    ROUND(qs.total_elapsed_time / qs.execution_count / 1000.0, 2) AS avg_duration_ms,
    ROUND(qs.total_logical_reads / qs.execution_count, 0) AS avg_logical_reads,
    SUBSTRING(
        st.text, 
        (qs.statement_start_offset / 2) + 1,
        ((CASE qs.statement_end_offset 
          WHEN -1 THEN DATALENGTH(st.text)
          ELSE qs.statement_end_offset END 
          - qs.statement_start_offset) / 2) + 1
    ) AS query_text
FROM sys.dm_exec_query_stats qs
CROSS APPLY sys.dm_exec_sql_text(qs.sql_handle) st
WHERE qs.execution_count > 5
ORDER BY avg_duration_ms DESC;

PRINT '';
PRINT 'Target: avg_duration_ms < 100ms for read queries';
PRINT '';

-- ============================================================
-- SECTION 4: CURRENTLY RUNNING REQUESTS (> 1 second)
-- ============================================================
PRINT '--- SECTION 4: Long-Running Requests ---';
SELECT 
    r.session_id,
    r.status,
    r.wait_type,
    r.wait_time / 1000 AS wait_time_sec,
    r.total_elapsed_time / 1000 AS running_sec,
    r.cpu_time / 1000 AS cpu_sec,
    r.logical_reads,
    DB_NAME(r.database_id) AS database_name,
    SUBSTRING(st.text, 1, 100) AS query_preview
FROM sys.dm_exec_requests r
CROSS APPLY sys.dm_exec_sql_text(r.sql_handle) st
WHERE r.session_id > 50
  AND r.total_elapsed_time > 1000
ORDER BY r.total_elapsed_time DESC;

PRINT '';
PRINT 'If this list is long during load test → queries need optimization';
PRINT '';

-- ============================================================
-- SECTION 5: TRANSACTION LOG HEALTH
-- ============================================================
PRINT '--- SECTION 5: Transaction Log Status ---';
SELECT 
    d.name AS database_name,
    d.recovery_model_desc,
    mf.size * 8 / 1024 AS log_size_mb,
    ROUND(100.0 * FILEPROPERTY(d.name, 'SpaceUsed') / mf.size, 2) AS log_used_pct,
    log_reuse_wait_desc AS cannot_shrink_until,
    CASE 
        WHEN FILEPROPERTY(d.name, 'SpaceUsed') * 100.0 / mf.size > 80
        THEN '🔴 Log > 80% full — immediate action needed'
        WHEN mf.size * 8 / 1024 > 2000
        THEN '⚠️ Log file large — consider shrinking'
        ELSE '✅ Log size OK'
    END AS status
FROM sys.databases d
JOIN sys.master_files mf ON d.database_id = mf.database_id AND mf.type = 1
WHERE d.name = 'SaasDemo';

PRINT '';
PRINT 'Target: log_used_pct < 50%, log_size_mb < 2× data size';
PRINT '';

PRINT '========================================';
PRINT 'END OF HEALTH CHECK';
PRINT '========================================';
