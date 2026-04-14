from fastapi import APIRouter, HTTPException
from models import ProcessRequest, ProcessResponse
from image_utils import download_image, encode_image_to_base64
from style_transfer import apply_color_transfer, extract_preset_parameters

router = APIRouter()


@router.post("/process", response_model=ProcessResponse)
async def process_style_match(request: ProcessRequest):
    """Download both images, apply color transfer, return preset + preview."""

    # Download images
    try:
        reference = await download_image(request.referenceImageUrl)
    except Exception as e:
        raise HTTPException(status_code=400, detail=f"Failed to download reference image: {e}")

    try:
        target = await download_image(request.targetImageUrl)
    except Exception as e:
        raise HTTPException(status_code=400, detail=f"Failed to download target image: {e}")

    # Process
    try:
        result_image = apply_color_transfer(reference, target)
        preset = extract_preset_parameters(reference, target)
        processed_url = encode_image_to_base64(result_image)
    except Exception as e:
        raise HTTPException(status_code=422, detail=f"Processing failed: {e}")

    return ProcessResponse(
        processedImageUrl=processed_url,
        preset=preset,
    )
