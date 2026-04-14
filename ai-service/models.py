from pydantic import BaseModel


class ProcessRequest(BaseModel):
    referenceImageUrl: str
    targetImageUrl: str


class PresetParameters(BaseModel):
    brightness: float
    contrast: float
    saturation: float
    temperature: float


class ProcessResponse(BaseModel):
    processedImageUrl: str
    preset: PresetParameters
