# Frontend Tasks

> **Spec:** `docs/PROJECT_SPEC.md`

---

## Project Setup

- [ ] Scaffold Vite + React + TypeScript project (`/frontend`).
- [ ] Install Tailwind CSS, Axios, React Router.
- [ ] Set up routes: `/`, `/result`, `/presets`, `/collections`.
- [ ] Create Axios instance with base URL from env variable.
- [ ] Create layout component with navigation.

**Done when:** `npm run dev` starts the app and all routes render.

---

## Upload Page

- [ ] Build `/` page with two drop zones: "Reference Image" and "Target Image".
- [ ] Support drag-and-drop and click-to-browse.
- [ ] Show preview thumbnails after file selection.
- [ ] Validate file type and size (max 10 MB) client-side.
- [ ] "Match Style" button — disabled until both images selected.
- [ ] On click, POST to `/api/style/match` as `multipart/form-data`.
- [ ] Show loading spinner during processing.
- [ ] On success, navigate to `/result` with response data.
- [ ] On error, show user-friendly message.

**Done when:** User can select two images, submit, and land on the result page.

---

## Result Page

- [ ] Build `/result` page with side-by-side before/after images.
- [ ] Show preset parameter summary (brightness, contrast, saturation, etc.).
- [ ] "Save Preset" button opens modal with name (required) and description (optional).
- [ ] On save, POST to `/api/presets`.
- [ ] Show success confirmation.
- [ ] "Try Another" button navigates back to `/`.

**Done when:** User sees results, can save a preset, and start over.
