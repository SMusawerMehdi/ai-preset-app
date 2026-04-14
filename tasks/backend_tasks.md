# Backend Tasks

> **Spec:** `docs/PROJECT_SPEC.md`

---

## API Project Setup

- [ ] Create .NET 8 Web API project (`/backend`) with Minimal APIs.
- [ ] Add packages: EF Core, SQL Server, Cloudinary SDK, JWT Bearer.
- [ ] Configure `appsettings.json` with sections: `ConnectionStrings`, `Cloudinary`, `Jwt`, `PythonService:BaseUrl`.
- [ ] Set up CORS, JWT auth middleware, and EF Core `AppDbContext`.
- [ ] Add `GET /api/health` endpoint returning `200 OK`.

**Done when:** Project builds and the health endpoint responds.

---

## Image Upload Handling

- [ ] Create `IImageStorageService` with method `UploadImageAsync(Stream, string) → string (URL)`.
- [ ] Implement using Cloudinary SDK.
- [ ] Validate file type by reading magic bytes (JPEG, PNG, WebP only).
- [ ] Reject files over 10 MB.
- [ ] Register service in DI.

**Done when:** A test image uploads to Cloudinary and returns a valid URL.

---

## Style Match Endpoint

- [ ] Create `POST /api/style/match` accepting `multipart/form-data` with `referenceImage` and `targetImage`.
- [ ] Validate both files (type + size) — return `400` on failure.
- [ ] Upload both to Cloudinary via `IImageStorageService`.
- [ ] Call Python service: `POST {PythonService:BaseUrl}/process` with `{ referenceUrl, targetUrl }`.
- [ ] Set 30s timeout on the Python call.
- [ ] Return `200` with `{ previewImageUrl, preset: { parameters, referenceImageUrl } }`.
- [ ] Return `503` if Python service unreachable, `422` if processing fails.

**Done when:** Endpoint accepts images, calls Python service, returns preset + preview. Error codes work.

---

## Mock Processing (for dev without Python service)

- [ ] Create a fallback mock when `PythonService:BaseUrl` is not configured.
- [ ] Mock returns a hardcoded preset JSON and uses the target image URL as the preview.
- [ ] Allows frontend and backend development to proceed without the AI service running.

**Done when:** Backend runs standalone and returns mock data on style match requests.
