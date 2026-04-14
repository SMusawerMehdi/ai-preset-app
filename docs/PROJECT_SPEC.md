# AI Preset Generator — Project Specification

> Version: 1.0 (MVP)
> Last Updated: 2026-04-14

---

## 1. Product Overview

### What It Does

AI Preset Generator is a web application that analyzes the visual style of a reference photograph (color grading, contrast, tone, saturation) and generates a reusable editing preset that can be applied to any target image. Users upload a reference image whose "look" they want to replicate and a target image they want to transform — the system produces a preset that bridges the gap between the two.

### Core Problem

Photographers and content creators spend significant time manually recreating color grades and editing styles from images they admire. Existing tools require deep knowledge of editing parameters (curves, HSL, split toning) to reverse-engineer a look. This app automates that process: point at a style, apply it to your photo, and save the result as a reusable preset.

### Main User Flow

1. User uploads a **reference image** (the style they want).
2. User uploads a **target image** (the photo they want to edit).
3. The system analyzes both images, computes the style difference, and generates a **preset** — a set of numeric adjustments (brightness, contrast, saturation, color curves, white balance, etc.).
4. The transformed target image is returned as a preview.
5. User saves the preset to their library and organizes it into collections.

---

## 2. MVP Features

### 2.1 Style Matching

- Upload a reference image and a target image.
- The backend extracts color/tone properties from both images using OpenCV.
- A preset (set of adjustment parameters) is computed to transform the target toward the reference style.
- A preview of the transformed image is returned to the user.

### 2.2 Preset Saving

- Save a generated preset with a user-defined name and optional description.
- Each preset stores the full set of adjustment parameters as structured JSON.
- Presets are tied to the authenticated user's account.
- Users can view, rename, delete, and re-apply saved presets to new images.

### 2.3 Collections Management

- Create named collections (e.g., "Warm Tones", "Film Look", "Client: Wedding").
- Add or remove presets from collections.
- View all presets within a collection.
- Delete collections (presets remain in the user's library).

---

## 3. User Flow

```
Step 1: Upload Reference Image
  └─> User selects or drags a reference photo into the upload zone.

Step 2: Upload Target Image
  └─> User selects or drags the target photo they want to transform.

Step 3: Generate Preset
  └─> User clicks "Match Style".
  └─> Frontend sends both images to the backend API.
  └─> Backend forwards images to the Python processing service.
  └─> Python analyzes both images, computes adjustment parameters.
  └─> Backend receives the preset data and a preview image URL.
  └─> Frontend displays the before/after preview and preset details.

Step 4: Save Preset
  └─> User names the preset (e.g., "Golden Hour Warm").
  └─> User clicks "Save Preset".
  └─> Preset parameters are persisted to the database.

Step 5: Organize into Collection
  └─> User opens their preset library.
  └─> User creates or selects a collection.
  └─> User adds the preset to the collection.

Step 6: Re-apply Preset
  └─> User selects a saved preset from their library.
  └─> User uploads a new target image.
  └─> System applies the saved preset and returns the result.
```

---

## 4. Technical Architecture

```
┌─────────────┐       ┌──────────────────┐       ┌───────────────────┐
│   Frontend   │──────>│   Backend API    │──────>│  Python Service   │
│  React/Vite  │<──────│  .NET 8 Web API  │<──────│  OpenCV + NumPy   │
└─────────────┘       └──────────────────┘       └───────────────────┘
                              │                          │
                              v                          v
                       ┌──────────┐              ┌──────────────┐
                       │  SQL DB  │              │  Cloudinary  │
                       │ (Data)   │              │  (Images)    │
                       └──────────┘              └──────────────┘
```

### 4.1 Frontend — React (Vite)

- **Framework:** React 18+ with Vite as the build tool.
- **Language:** TypeScript.
- **Styling:** Tailwind CSS.
- **State Management:** React Context + useReducer for global state; local state for forms.
- **HTTP Client:** Axios.
- **Key Pages:**
  - `/` — Landing / Upload page (reference + target image upload).
  - `/result` — Before/after preview with save option.
  - `/presets` — User's preset library.
  - `/collections` — Collection management.

### 4.2 Backend — .NET 8 Web API

- **Framework:** ASP.NET Core 8 Minimal APIs.
- **Language:** C# 12.
- **ORM:** Entity Framework Core 8 with SQL Server.
- **Authentication:** JWT Bearer tokens (ASP.NET Identity).
- **Image Upload Handling:** Multipart form data, forwarded to Cloudinary for storage.
- **Communication with Python Service:** HTTP calls via `HttpClient` to the Python processing endpoint.
- **Responsibilities:**
  - Receive image uploads from the frontend.
  - Upload images to Cloudinary and obtain URLs.
  - Forward image URLs to the Python service for processing.
  - Persist presets and collections to the database.
  - Serve preset/collection data to the frontend.

### 4.3 AI Processing — Python (OpenCV)

- **Framework:** FastAPI.
- **Libraries:** OpenCV, NumPy, scikit-image.
- **Responsibilities:**
  - Receive reference and target image URLs.
  - Download and decode images.
  - Extract style features: histogram distributions (per channel), average luminance, saturation profile, contrast ratio, white balance temperature estimate.
  - Compute a transformation preset (adjustment deltas) that maps the target's style toward the reference.
  - Apply the preset to the target image to generate a preview.
  - Upload the preview image to Cloudinary.
  - Return the preset parameters and preview URL to the .NET backend.

### 4.4 Storage

- **Cloudinary** — All image storage (reference images, target images, preview outputs). Images are referenced by URL; no local file storage.
- **SQL Server** — All structured data: users, presets, collections, relationships.

---

## 5. Core Concepts

### What Is a "Preset"?

A preset is a structured set of numeric image adjustment parameters derived from comparing a reference image to a target image. It captures the style transformation needed to make the target look like the reference. Once saved, a preset can be re-applied to any new image.

### Preset JSON Structure

```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "name": "Golden Hour Warm",
  "description": "Warm golden tones with lifted shadows",
  "createdAt": "2026-04-14T10:30:00Z",
  "referenceImageUrl": "https://res.cloudinary.com/demo/image/upload/v1/ref_abc123.jpg",
  "parameters": {
    "brightness": 12,
    "contrast": 8,
    "saturation": 15,
    "temperature": 18,
    "tint": 4,
    "highlights": -10,
    "shadows": 20,
    "whites": 5,
    "blacks": -8,
    "vibrance": 10,
    "curves": {
      "red": [[0, 5], [64, 70], [128, 135], [192, 195], [255, 250]],
      "green": [[0, 3], [64, 62], [128, 130], [192, 190], [255, 248]],
      "blue": [[0, 0], [64, 55], [128, 118], [192, 185], [255, 240]]
    },
    "splitToning": {
      "highlightHue": 45,
      "highlightSaturation": 20,
      "shadowHue": 220,
      "shadowSaturation": 10
    }
  }
}
```

All numeric values represent deltas from a neutral baseline (0 = no change). Curves are arrays of `[input, output]` control points per RGB channel.

---

## 6. API Design (High Level)

Base URL: `/api`

### 6.1 Style Matching

| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/style/match` | Upload reference + target images, receive preset + preview |

**Request:** `multipart/form-data`

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `referenceImage` | file | Yes | The style source image |
| `targetImage` | file | Yes | The image to transform |

**Response:** `200 OK`

```json
{
  "previewImageUrl": "https://res.cloudinary.com/demo/image/upload/v1/preview_xyz.jpg",
  "preset": {
    "parameters": { ... },
    "referenceImageUrl": "https://..."
  }
}
```

### 6.2 Presets

| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/presets` | Save a new preset |
| `GET` | `/api/presets` | List all presets for the authenticated user |
| `GET` | `/api/presets/{id}` | Get a single preset by ID |
| `PUT` | `/api/presets/{id}` | Update preset name/description |
| `DELETE` | `/api/presets/{id}` | Delete a preset |

**POST /api/presets — Request Body:**

```json
{
  "name": "Golden Hour Warm",
  "description": "Warm golden tones with lifted shadows",
  "referenceImageUrl": "https://...",
  "parameters": { ... }
}
```

**GET /api/presets — Response:**

```json
{
  "items": [
    {
      "id": "550e8400-...",
      "name": "Golden Hour Warm",
      "description": "...",
      "createdAt": "2026-04-14T10:30:00Z",
      "referenceImageUrl": "https://..."
    }
  ],
  "total": 1
}
```

### 6.3 Collections

| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/collections` | Create a new collection |
| `GET` | `/api/collections` | List all collections for the authenticated user |
| `GET` | `/api/collections/{id}` | Get collection details with its presets |
| `PUT` | `/api/collections/{id}` | Update collection name |
| `DELETE` | `/api/collections/{id}` | Delete a collection (presets are not deleted) |
| `POST` | `/api/collections/{id}/presets` | Add a preset to a collection |
| `DELETE` | `/api/collections/{id}/presets/{presetId}` | Remove a preset from a collection |

---

## 7. Database Design

### 7.1 Entity: User

| Column | Type | Constraints |
|--------|------|-------------|
| `Id` | `UNIQUEIDENTIFIER` | PK, default `NEWID()` |
| `Email` | `NVARCHAR(256)` | NOT NULL, UNIQUE |
| `PasswordHash` | `NVARCHAR(MAX)` | NOT NULL |
| `DisplayName` | `NVARCHAR(100)` | NOT NULL |
| `CreatedAt` | `DATETIME2` | NOT NULL, default `GETUTCDATE()` |

### 7.2 Entity: Preset

| Column | Type | Constraints |
|--------|------|-------------|
| `Id` | `UNIQUEIDENTIFIER` | PK, default `NEWID()` |
| `UserId` | `UNIQUEIDENTIFIER` | FK → User.Id, NOT NULL |
| `Name` | `NVARCHAR(200)` | NOT NULL |
| `Description` | `NVARCHAR(500)` | NULL |
| `ReferenceImageUrl` | `NVARCHAR(2048)` | NULL |
| `Parameters` | `NVARCHAR(MAX)` | NOT NULL (JSON string) |
| `CreatedAt` | `DATETIME2` | NOT NULL, default `GETUTCDATE()` |
| `UpdatedAt` | `DATETIME2` | NOT NULL, default `GETUTCDATE()` |

**Index:** `IX_Preset_UserId` on `UserId`.

### 7.3 Entity: Collection

| Column | Type | Constraints |
|--------|------|-------------|
| `Id` | `UNIQUEIDENTIFIER` | PK, default `NEWID()` |
| `UserId` | `UNIQUEIDENTIFIER` | FK → User.Id, NOT NULL |
| `Name` | `NVARCHAR(200)` | NOT NULL |
| `CreatedAt` | `DATETIME2` | NOT NULL, default `GETUTCDATE()` |

**Index:** `IX_Collection_UserId` on `UserId`.

### 7.4 Entity: CollectionPreset (Join Table)

| Column | Type | Constraints |
|--------|------|-------------|
| `CollectionId` | `UNIQUEIDENTIFIER` | PK (composite), FK → Collection.Id |
| `PresetId` | `UNIQUEIDENTIFIER` | PK (composite), FK → Preset.Id |
| `AddedAt` | `DATETIME2` | NOT NULL, default `GETUTCDATE()` |

**Cascade Rules:**
- Deleting a `User` cascades to their `Presets` and `Collections`.
- Deleting a `Collection` cascades to `CollectionPreset` rows (presets themselves remain).
- Deleting a `Preset` cascades to `CollectionPreset` rows.

### ER Diagram

```
┌──────────┐       ┌──────────────┐       ┌──────────────────┐
│   User   │──1:N──│   Preset     │──M:N──│   Collection     │
│          │       │              │       │                  │
│ Id (PK)  │       │ Id (PK)      │       │ Id (PK)          │
│ Email    │       │ UserId (FK)  │       │ UserId (FK)      │
│ ...      │       │ Name         │       │ Name             │
└──────────┘       │ Parameters   │       └──────────────────┘
                   └──────────────┘
                          │
                   CollectionPreset
                   (CollectionId, PresetId)
```

---

## 8. Processing Flow

```
1. Frontend
   │  User uploads referenceImage + targetImage
   │  POST /api/style/match (multipart/form-data)
   v
2. .NET Backend
   │  a. Validate file types (JPEG, PNG, WebP) and size (max 10 MB each).
   │  b. Upload both images to Cloudinary.
   │  c. Obtain Cloudinary URLs for reference and target.
   │  d. POST to Python service: { referenceUrl, targetUrl }
   v
3. Python Service (FastAPI)
   │  a. Download reference and target images from Cloudinary URLs.
   │  b. Decode images into OpenCV BGR matrices.
   │  c. Convert both to LAB color space.
   │  d. Compute per-channel statistics (mean, std dev) for both images.
   │  e. Apply histogram matching / color transfer algorithm:
   │       - For each channel: target_normalized = (target - target_mean) * (ref_std / target_std) + ref_mean
   │  f. Derive preset parameters from the transformation deltas.
   │  g. Apply the preset to the target image to produce a preview.
   │  h. Encode the preview as JPEG and upload to Cloudinary.
   │  i. Return JSON: { previewUrl, parameters }
   v
4. .NET Backend
   │  a. Receive preset parameters and preview URL from Python.
   │  b. Construct response payload.
   │  c. Return 200 OK with preset data and preview URL to frontend.
   v
5. Frontend
   │  a. Display side-by-side before/after comparison.
   │  b. Show preset parameter summary.
   │  c. Enable "Save Preset" action.
```

**Error Handling at Each Stage:**

| Stage | Error | Response |
|-------|-------|----------|
| Backend validation | Invalid file type or size exceeded | `400 Bad Request` with details |
| Cloudinary upload | Upload failure | `502 Bad Gateway` — retry once, then fail |
| Python service | Unreachable or timeout (30s) | `503 Service Unavailable` |
| Python processing | Image analysis failure | `422 Unprocessable Entity` |

---

## 9. Non-Functional Requirements

### 9.1 Performance

- **Style matching response time:** Under 10 seconds for two 5 MP images (end-to-end, including upload and processing).
- **API response time (CRUD):** Under 200ms for preset/collection read/write operations.
- **Image upload size limit:** 10 MB per image.
- **Supported formats:** JPEG, PNG, WebP.
- **Concurrent processing:** Python service must handle at least 10 concurrent style-match requests.

### 9.2 Scalability

- **Stateless backend:** .NET API is stateless; horizontally scalable behind a load balancer.
- **Python service:** Deploy as a separate container; scale independently based on processing demand.
- **Database:** Start with a single SQL Server instance. Add read replicas if read load grows.
- **Image storage:** Cloudinary handles CDN and transformation offloading; no custom scaling needed.

### 9.3 Security

- **Authentication:** JWT tokens with short expiry (15 min access, 7 day refresh).
- **Authorization:** Users can only access their own presets and collections. Enforce at the query level (`WHERE UserId = @currentUser`).
- **Input validation:** Validate all file uploads (MIME type check, file header verification, size limit). Reject anything that is not a valid image.
- **CORS:** Restrict to the frontend domain only.
- **HTTPS:** All traffic over TLS in production.
- **Rate limiting:** 20 style-match requests per user per hour. 100 API calls per user per minute for CRUD operations.
- **Secrets management:** API keys (Cloudinary, JWT signing key) stored in environment variables or a secrets vault — never in source code.

---

## 10. Out of Scope (MVP)

The following features are explicitly **not** included in the MVP release:

- **Mobile applications** — iOS and Android native apps.
- **Payment / subscription system** — Monetization, paywalls, or premium tiers.
- **Advanced AI model training** — Custom neural style transfer or fine-tuned models.
- **Social features** — Following users, liking presets, commenting.
- **Batch processing** — Applying a preset to multiple images at once.
- **Export to editing software** — Generating Lightroom `.xmp` or Photoshop `.acv` files.
- **Real-time collaboration** — Shared collections with multiple users.
- **Preset versioning** — Tracking changes to a preset over time.
- **Image editing UI** — In-app sliders or manual parameter tuning.

---

## 11. Future Enhancements

### Phase 2: Intelligence

- **AI-based style detection** — Automatically tag presets with style labels (e.g., "moody", "film", "high-key") using a classification model.
- **Smart recommendations** — Suggest presets based on the target image content and lighting conditions.
- **Neural style transfer** — Offer a deep-learning-based option for more complex style matching beyond histogram-level adjustments.

### Phase 3: Community

- **Preset marketplace** — Users publish presets for others to browse, download, or purchase.
- **Social sharing** — Share before/after comparisons and presets via public links or social media.
- **User profiles** — Public creator profiles showcasing published presets and collections.

### Phase 4: Platform

- **Export integrations** — Generate Lightroom, Capture One, and Photoshop preset files.
- **Mobile apps** — Native iOS and Android clients.
- **Batch processing** — Apply a preset to an entire photo set in one operation.
- **API access** — Public API for third-party integrations.

---

## Appendix: Technology Versions

| Component | Technology | Version |
|-----------|-----------|---------|
| Backend | ASP.NET Core | 8.x |
| Language (Backend) | C# | 12 |
| ORM | Entity Framework Core | 8.x |
| Database | SQL Server | 2022 |
| Frontend | React | 18.x |
| Build Tool | Vite | 5.x |
| Language (Frontend) | TypeScript | 5.x |
| Styling | Tailwind CSS | 3.x |
| AI Service | Python | 3.11+ |
| AI Framework | FastAPI | 0.100+ |
| Image Processing | OpenCV | 4.x |
| Image Storage | Cloudinary | Latest SDK |
