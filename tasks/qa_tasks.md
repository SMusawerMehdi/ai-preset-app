# QA Tasks

> **Spec:** `docs/PROJECT_SPEC.md`

---

## Backend Tests

- [ ] Test file validation: valid types pass, invalid types return `400`, oversized files return `400`.
- [ ] Test `POST /api/style/match` happy path returns `200` with correct JSON shape.
- [ ] Test Python service unreachable returns `503`.
- [ ] Test Python processing failure returns `422`.

**Done when:** All backend test cases pass.

---

## Python Service Tests

- [ ] Test color transfer with known input/output image pairs.
- [ ] Test preset parameter JSON matches expected structure.
- [ ] Test error handling: invalid URLs, corrupted images.

**Done when:** All Python test cases pass.

---

## Frontend Tests

- [ ] Test upload page: file selection, validation errors, disabled button state.
- [ ] Test result page: image display, save preset flow.
- [ ] Test loading and error states.

**Done when:** All frontend test cases pass.

---

## End-to-End

- [ ] Upload two images → receive preview → save preset.
- [ ] Verify preset stored in database with correct data.
- [ ] Verify all three services communicate correctly.

**Done when:** Full flow works without errors.
