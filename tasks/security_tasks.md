# Security Tasks

> **Spec:** `docs/PROJECT_SPEC.md`

---

## File Upload Security

- [ ] File type validated by magic bytes, not just MIME type or extension.
- [ ] File size enforced server-side.
- [ ] No path traversal in file handling.
- [ ] No files stored on local filesystem.

---

## API & Auth Security

- [ ] JWT uses strong signing (RS256/HS256), not hardcoded secret.
- [ ] Token expiry enforced (15 min access, 7 day refresh).
- [ ] All endpoints require auth (except health check).
- [ ] Users cannot access other users' data (no IDOR).
- [ ] CORS restricted to frontend domain only.
- [ ] Rate limiting active (20 style-match/hr, 100 CRUD/min).
- [ ] No secrets in source code or committed config files.

---

## Python Service Security

- [ ] Image URLs validated before download (no SSRF).
- [ ] No `subprocess` calls with user input.
- [ ] Cloudinary credentials from environment only.
- [ ] Error responses don't leak internal details.

---

**Done when:** All checks pass. Any findings documented with severity and fix.
