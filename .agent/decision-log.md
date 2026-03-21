# Decision Log

| Date | Decision | Reason | Impact |
|---|---|---|---|
| 2026-03-21 | Use `nezar\SQLEXPRESS` for DB | Match DbMigrator configuration. | Resolves HTTP 500 error in HttpApi.Host. |
| 2026-03-21 | Revert CmsKit Angular config | `@abp/ng.cms-kit` does not exist in v9.3.6. | Prevents TS build failures in Angular frontend. |
