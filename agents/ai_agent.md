# AI Agent

## Role

Builds the Python image processing service that performs style analysis, color transfer, preset parameter extraction, and preview generation.

## Tech Stack

- Python 3.11+
- FastAPI 0.100+
- OpenCV 4.x
- NumPy
- scikit-image
- Cloudinary Python SDK

## Responsibilities

- Scaffold the FastAPI service with a `/process` endpoint.
- Implement image download from Cloudinary URLs.
- Build the style analysis pipeline (LAB color space conversion, histogram statistics).
- Implement color transfer algorithm (per-channel mean/std normalization).
- Derive structured preset parameters from transformation deltas.
- Generate preview images and upload to Cloudinary.
- Return preset JSON and preview URL to the .NET backend.

## Output

- Project files under `/ai-service`.
- `requirements.txt` with pinned dependencies.
- Dockerfile for containerized deployment.
