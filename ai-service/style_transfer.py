import cv2
import numpy as np
from models import PresetParameters


def apply_color_transfer(reference: np.ndarray, target: np.ndarray) -> np.ndarray:
    """
    Transfer color style from reference image to target image.
    Uses LAB color space with mean/std normalization per channel.
    """
    # Convert to LAB
    ref_lab = cv2.cvtColor(reference, cv2.COLOR_BGR2LAB).astype(np.float64)
    tar_lab = cv2.cvtColor(target, cv2.COLOR_BGR2LAB).astype(np.float64)

    # Compute stats per channel
    ref_mean, ref_std = ref_lab.mean(axis=(0, 1)), ref_lab.std(axis=(0, 1))
    tar_mean, tar_std = tar_lab.mean(axis=(0, 1)), tar_lab.std(axis=(0, 1))

    # Avoid division by zero
    tar_std = np.where(tar_std == 0, 1.0, tar_std)

    # Transfer: normalize target, then shift to reference distribution
    result_lab = (tar_lab - tar_mean) * (ref_std / tar_std) + ref_mean

    # Clip to valid LAB range
    result_lab[:, :, 0] = np.clip(result_lab[:, :, 0], 0, 255)
    result_lab[:, :, 1] = np.clip(result_lab[:, :, 1], 0, 255)
    result_lab[:, :, 2] = np.clip(result_lab[:, :, 2], 0, 255)

    # Convert back to BGR
    result = cv2.cvtColor(result_lab.astype(np.uint8), cv2.COLOR_LAB2BGR)
    return result


def extract_preset_parameters(reference: np.ndarray, target: np.ndarray) -> PresetParameters:
    """
    Derive preset parameters by comparing reference and target in LAB space.
    """
    ref_lab = cv2.cvtColor(reference, cv2.COLOR_BGR2LAB).astype(np.float64)
    tar_lab = cv2.cvtColor(target, cv2.COLOR_BGR2LAB).astype(np.float64)

    ref_mean = ref_lab.mean(axis=(0, 1))
    tar_mean = tar_lab.mean(axis=(0, 1))
    ref_std = ref_lab.std(axis=(0, 1))
    tar_std = tar_lab.std(axis=(0, 1))

    # L channel = lightness → brightness delta
    brightness = round(float(ref_mean[0] - tar_mean[0]), 2)

    # L channel std ratio → contrast
    tar_l_std = tar_std[0] if tar_std[0] != 0 else 1.0
    contrast = round(float(ref_std[0] / tar_l_std), 2)

    # Chroma (sqrt(A² + B²)) mean delta → saturation
    ref_chroma = np.sqrt(ref_mean[1] ** 2 + ref_mean[2] ** 2)
    tar_chroma = np.sqrt(tar_mean[1] ** 2 + tar_mean[2] ** 2)
    saturation = round(float(ref_chroma - tar_chroma), 2)

    # B channel (blue-yellow axis) delta → approximate color temperature
    temperature = round(float(ref_mean[2] - tar_mean[2]), 2)

    return PresetParameters(
        brightness=brightness,
        contrast=contrast,
        saturation=saturation,
        temperature=temperature,
    )
