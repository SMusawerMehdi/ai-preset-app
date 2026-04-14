# Security Tasks

- [ ] Verify file type checked by magic bytes, not just extension
- [ ] Verify file size enforced server-side
- [ ] Verify no files stored on local filesystem
- [ ] Verify JWT auth on all endpoints (except health check)
- [ ] Verify users cannot access other users' data
- [ ] Verify CORS restricted to frontend domain
- [ ] Verify no secrets hardcoded in source code
- [ ] Verify Python service validates URLs before downloading (no SSRF)
- [ ] Verify error responses don't leak internal details
