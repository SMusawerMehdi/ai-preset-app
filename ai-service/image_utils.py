import cv2
import numpy as np
import httpx
import base64


async def download_image(url: str) -> np.ndarray:
    """Download image from URL and decode into OpenCV BGR matrix."""
    async with httpx.AsyncClient(timeout=30.0) as client:
        response = await client.get(url)
        response.raise_for_status()

    img_array = np.frombuffer(response.content, dtype=np.uint8)
    img = cv2.imdecode(img_array, cv2.IMREAD_COLOR)

    if img is None:
        raise ValueError(f"Could not decode image from URL: {url}")

    return img


def encode_image_to_base64(img: np.ndarray) -> str:
    """Encode OpenCV image to base64 JPEG string."""
    _, buffer = cv2.imencode(".jpg", img, [cv2.IMWRITE_JPEG_QUALITY, 90])
    return "data:image/jpeg;base64," + base64.b64encode(buffer).decode("utf-8")
