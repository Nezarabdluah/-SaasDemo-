-- ============================================================
-- Test #7: Database Health Check
-- Enterprise SQL Server Performance Diagnostic
-- Run AFTER load tests to identify bottlenecks
-- ============================================================

PRINT '========================================';
PRINT 'Test #7: Database Health Check';
PRINT 'Date: ' + CONVERT(VARCHAR, GETDATE(), 120);
PRINT '========================================';
PRINT '';

-- ============================================================
-- SECTION 1: QUICK HEALTH CHECK
-- ============================================================
PRINT '=== 1. QUICK HEALTH CHECK ===';
PRINT '';

SELECT 
    @@SERVERNAME AS server_name,
    DB_NAME() AS current_database,
    GETDATE() AS check_time,
    (SELECT COUNT(*) FROM sys.dm_exec_sessions WHERE is_user_process = 1) AS active_user_connections;

PRINT '';

-- ============================================================
-- SECTION 2: CONNECTION POOL DIAGNOSTICS
-- ============================================================
PRINT '=== 2. CONNECTION POOL HEALTH ===';
PRINT '';

SELECT 
    DB_NAME(database_id) AS database_name,
    COUNT(*) AS total_connections,
    SUM(CASE WHEN status = 'sleeping' THEN 1 ELSE 0 END) AS idle,
    SUM(CASE WHEN status = 'running'  THEN 1 ELSE 0 END) AS active
FROM sys.dm_exec_sessions
WHERE is_user_process = 1
GROUP BY database_id
ORDER BY total_connections DESC;

PRINT '';

-- ============================================================
-- SECTION 3: CRITICAL WAIT STATS
-- ============================================================
PRINT '=== 3. TOP WAIT TYPES ===';
PRINT '';

SELECT TOP 10
    wait_type,
    waiting_tasks_count,
    wait_time_ms,
    ROUND(100.0 * wait_time_ms / SUM(wait_time_ms) OVER(), 2) AS pct_of_all_waits,
    CASE wait_type
        WHEN 'THREADPOOL'          THEN 'CRITICAL: Connection pool exhausted!'
        WHEN 'RESOURCE_SEMAPHORE'  THEN 'WARNING: Memory grant waits'
        WHEN 'ASYNC_NETWORK_IO'    THEN 'INFO: Client reading slowly'
        WHEN 'LCK_M_X'            THEN 'CRITICAL: Exclusive lock waits'
        WHEN 'PAGEIOLATCH_SH'      THEN 'WARNING: Disk I/O'
        ELSE 'Normal'
    END AS diagnosis
FROM sys.dm_os_wait_stats
WHERE wait_type NOT IN (
    'SLEEP_TASK', 'REQUEST_FOR_DEADLOCK_SEARCH', 'RESOURCE_QUEUE',
    'SERVER_IDLE_CHECK', 'CLR_AUTO_EVENT', 'WAITFOR', 'BROKER_TO_FLUSH'
)
ORDER BY wait_time_ms DESC;

PRINT '';

-- ============================================================
-- SECTION 4: TOP 10 SLOWEST QUERIES
-- ============================================================
PRINT '=== 4. TOP 10 SLOWEST QUERIES ===';
PRINT '';

SELECT TOP 10
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

-- ============================================================
-- SECTION 5: MISSING INDEXES
-- ============================================================
PRINT '=== 5. MISSING INDEX SUGGESTIONS ===';
PRINT '';

SELECT TOP 10
    ROUND(
        migs.avg_total_user_cost * migs.avg_user_impact * (migs.user_seeks + migs.user_scans),
        0
    ) AS impact_score,
    migs.user_seeks,
    migs.user_scans,
    migs.avg_user_impact AS pct_improvement_estimate,
    mid.statement AS table_name,
    mid.equality_columns,
    mid.inequality_columns
FROM sys.dm_db_missing_index_groups mig
JOIN sys.dm_db_missing_index_group_stats migs ON mig.index_group_handle = migs.group_handle
JOIN sys.dm_db_missing_index_details mid ON mig.index_handle = mid.index_handle
WHERE mid.database_id = DB_ID()
ORDER BY impact_score DESC;

PRINT '';

-- ============================================================
-- SECTION 6: TRANSACTION LOG HEALTH
-- ============================================================
PRINT '=== 6. TRANSACTION LOG STATUS ===';
PRINT '';

SELECT 
    d.name AS database_name,
    d.recovery_model_desc,
    mf.size * 8 / 1024 AS log_size_mb,
    ROUND(100.0 * FILEPROPERTY(mf.name, 'SpaceUsed') / mf.size, 2) AS log_used_pct,
    log_reuse_wait_desc AS cannot_shrink_until
FROM sys.databases d
JOIN sys.master_files mf ON d.database_id = mf.database_id AND mf.type = 1
WHERE d.name = DB_NAME();

PRINT '';

-- ============================================================
-- SECTION 7: DATABASE SIZE
-- ============================================================
PRINT '=== 7. DATABASE SIZE ===';
PRINT '';

EXEC sp_spaceused;

PRINT '';
PRINT '========================================';
PRINT 'Test #7: Database Health Check COMPLETE';
PRINT '========================================';
