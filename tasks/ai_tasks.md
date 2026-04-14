# AI Tasks

> **Spec:** `docs/PROJECT_SPEC.md`

---

## Python Service Setup

- [ ] Create FastAPI project (`/ai-service`).
- [ ] Create `requirements.txt`: fastapi, uvicorn, opencv-python-headless, numpy, scikit-image, cloudinary, httpx.
- [ ] Add `GET /health` endpoint.
- [ ] Create `Dockerfile`.
- [ ] Load Cloudinary credentials from environment variables.

**Done when:** `uvicorn main:app` starts and health endpoint returns `200`.

---

## Style Transfer Processing

- [ ] Create `POST /process` accepting `{ referenceUrl, targetUrl }`.
- [ ] Download both images via `httpx`.
- [ ] Convert to LAB color space using OpenCV.
- [ ] Compute per-channel stats (mean, std dev) for L, A, B.
- [ ] Apply color transfer: `target_ch = (target_ch - target_mean) * (ref_std / target_std) + ref_mean`.
- [ ] Clip values and convert back to BGR.
- [ ] Derive preset parameters (brightness, contrast, saturation, temperature, tint, curves).
- [ ] Encode result as JPEG, upload preview to Cloudinary.
- [ ] Return `{ previewUrl, parameters }`.

**Done when:** Two test images produce a valid preset JSON and a working preview URL.
