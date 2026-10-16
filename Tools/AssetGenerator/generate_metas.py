import os
import hashlib

SPRITES_DIR = r"D:\Games\Snowball Adventure 2\Assets\Sprites"
AUDIO_DIR = r"D:\Games\Snowball Adventure 2\Assets\Audio"

def deterministic_guid(path):
    return hashlib.md5(path.replace("\\", "/").lower().encode("utf-8")).hexdigest()

SPRITE_META_TEMPLATE = """fileFormatVersion: 2
guid: {guid}
TextureImporter:
  internalIDToNameTable: []
  externalObjects: {{}}
  serializedVersion: 12
  mipmaps:
    mipMapMode: 0
    enableMipMap: 0
    sRGBTexture: 1
    linearTexture: 0
    fadeOut: 0
    borderMipMap: 0
    mipMapsPreserveCoverage: 0
    alphaTestReferenceValue: 0.5
    mipMapFadeDistanceStart: 1
    mipMapFadeDistanceEnd: 3
  bumpmap:
    convertToNormalMap: 0
    externalNormalMap: 0
    heightScale: 0.25
    normalMapFilter: 0
  isReadable: 0
  streamingMipmaps: 0
  streamingMipmapsPriority: 0
  vTOnly: 0
  grayScaleToAlpha: 0
  generateCubemap: 6
  cubemapConvolution: 0
  seamlessCubemap: 0
  textureFormat: 1
  maxTextureSize: 2048
  textureSettings:
    serializedVersion: 2
    filterMode: 1
    aniso: 1
    mipBias: 0
    wrapU: 1
    wrapV: 1
    wrapW: 1
  nPOTScale: 0
  lightmap: 0
  compressionQuality: 50
  spriteMode: 1
  spriteExtrude: 1
  spriteMeshType: 0
  alignment: 0
  spritePivot: {{x: 0.5, y: 0.5}}
  spritePixelsToUnits: 100
  spriteBorder: {{x: 0, y: 0, z: 0, w: 0}}
  spriteGenerateFallbackPhysicsShape: 1
  alphaUsage: 1
  alphaIsTransparency: 1
  spriteTessellationDetail: -1
  textureType: 8
  textureShape: 1
  singleChannelComponent: 0
  flipbookRows: 1
  flipbookColumns: 1
  maxTextureSizeSet: 0
  compressionQualitySet: 0
  textureFormatSet: 0
  ignorePngGamma: 0
  applyGammaDecoding: 0
  swizzle: 50462976
  cookieLightType: 0
  platformSettings: []
  userData: 
  assetBundleName: 
  assetBundleVariant: 
"""

AUDIO_META_TEMPLATE = """fileFormatVersion: 2
guid: {guid}
AudioImporter:
  externalObjects: {{}}
  serializedVersion: 6
  defaultSettings:
    loadType: 0
    sampleRateSetting: 0
    sampleRateOverride: 44100
    compressionFormat: 1
    quality: 1
    conversionMode: 0
  platformSettingOverrides: {{}}
  forceToMono: 0
  normalize: 1
  preloadAudioData: 1
  loadInBackground: 0
  ambisonic: 0
  3D: 0
  userData: 
  assetBundleName: 
  assetBundleVariant: 
"""

def make_sprite_meta(png_path):
    rel_path = os.path.relpath(png_path, r"D:\Games\Snowball Adventure 2").replace("\\", "/")
    guid = deterministic_guid(rel_path)
    content = SPRITE_META_TEMPLATE.format(guid=guid)
    with open(png_path + ".meta", "w", encoding="utf-8") as f:
        f.write(content)

def make_audio_meta(wav_path):
    rel_path = os.path.relpath(wav_path, r"D:\Games\Snowball Adventure 2").replace("\\", "/")
    guid = deterministic_guid(rel_path)
    content = AUDIO_META_TEMPLATE.format(guid=guid)
    with open(wav_path + ".meta", "w", encoding="utf-8") as f:
        f.write(content)

SCRIPT_META_TEMPLATE = """fileFormatVersion: 2
guid: {guid}
MonoImporter:
  externalObjects: {{}}
  serializedVersion: 2
  defaultReferences: []
  executionOrder: 0
  icon: {{instanceID: 0}}
  userData: 
  assetBundleName: 
  assetBundleVariant: 
"""

def make_script_meta(cs_path):
    rel_path = os.path.relpath(cs_path, r"D:\Games\Snowball Adventure 2").replace("\\", "/")
    guid = deterministic_guid(rel_path)
    content = SCRIPT_META_TEMPLATE.format(guid=guid)
    with open(cs_path + ".meta", "w", encoding="utf-8") as f:
        f.write(content)

SCRIPTS_DIR = r"D:\Games\Snowball Adventure 2\Assets\Scripts"
EDITOR_DIR = r"D:\Games\Snowball Adventure 2\Assets\Editor"
RESOURCES_DIR = r"D:\Games\Snowball Adventure 2\Assets\Resources"

for target_dir in [SPRITES_DIR, AUDIO_DIR, RESOURCES_DIR, SCRIPTS_DIR, EDITOR_DIR]:
    if os.path.exists(target_dir):
        for root, _, files in os.walk(target_dir):
            for f in files:
                p = os.path.join(root, f)
                if f.endswith(".png"):
                    make_sprite_meta(p)
                elif f.endswith(".wav"):
                    make_audio_meta(p)
                elif f.endswith(".cs"):
                    make_script_meta(p)

print("Meta files successfully generated!")
