import math
import os
import random
import uuid
import shutil
from PIL import Image, ImageDraw, ImageFilter

OUTPUT_SPRITES = r"D:\Games\Snowball Adventure 2\Assets\Sprites"
OUTPUT_RESOURCES = r"D:\Games\Snowball Adventure 2\Assets\Resources\Sprites"

os.makedirs(OUTPUT_SPRITES, exist_ok=True)
os.makedirs(OUTPUT_RESOURCES, exist_ok=True)

def lerp_color(c1, c2, t):
    return tuple(int(a + (b - a) * t) for a, b in zip(c1, c2))

def create_meta_file(filepath, wrap_repeat=False):
    meta_path = filepath + ".meta"
    if os.path.exists(meta_path):
        return
    
    guid = uuid.uuid4().hex
    wrap_val = 1 if wrap_repeat else 0
    meta_content = """fileFormatVersion: 2
guid: {GUID}
TextureImporter:
  internalIDToNameTable: []
  externalObjects: {}
  serializedVersion: 13
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
    flipGreenChannel: 0
  isReadable: 0
  streamingMipmaps: 0
  streamingMipmapsPriority: 0
  vTOnly: 0
  ignoreMipmapLimit: 0
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
    wrapU: {WRAP_VAL}
    wrapV: {WRAP_VAL}
    wrapW: {WRAP_VAL}
  nPOTScale: 0
  lightmap: 0
  compressionQuality: 50
  spriteMode: 1
  spriteExtrude: 1
  spriteMeshType: 0
  alignment: 0
  spritePivot: {x: 0.5, y: 0.5}
  spritePixelsToUnits: 100
  spriteBorder: {x: 0, y: 0, z: 0, w: 0}
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
  platformSettings:
  - serializedVersion: 3
    buildTarget: DefaultTexturePlatform
    maxTextureSize: 2048
    resizeAlgorithm: 0
    textureFormat: -1
    textureCompression: 1
    compressionQuality: 50
    crunchedCompression: 0
    allowsAlphaSplitting: 0
    overridden: 0
    ignorePlatformSupport: 0
    androidETC2FallbackOverride: 0
    forceMaximumCompressionQuality_BC6H_BC7: 0
  userData: 
  assetBundleName: 
  assetBundleVariant: 
""".replace("{GUID}", guid).replace("{WRAP_VAL}", str(wrap_val))
    with open(meta_path, "w", encoding="utf-8") as f:
        f.write(meta_content)

def save_and_sync(img, filename, wrap_repeat=False):
    p1 = os.path.join(OUTPUT_SPRITES, filename)
    img.save(p1, "PNG")
    create_meta_file(p1, wrap_repeat=wrap_repeat)
    
    p2 = os.path.join(OUTPUT_RESOURCES, filename)
    shutil.copy2(p1, p2)
    shutil.copy2(p1 + ".meta", p2 + ".meta")
    print(f"Saved & synced {filename}")

# -------------------------------------------------------------
# 1. COSMIC POLAR BACKGROUNDS (1024x512)
# -------------------------------------------------------------
def generate_cosmic_backgrounds():
    W, H = 1024, 512

    # A) bg_cosmic_sky (Deep violet/magenta cosmic starlight sky with ringed planet)
    img = Image.new("RGBA", (W, H), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)

    top_c = (12, 6, 32, 255)
    mid_c = (35, 15, 60, 255)
    bot_c = (40, 25, 80, 255)
    for y in range(H):
        if y < H * 0.5:
            t = y / (H * 0.5)
            c = lerp_color(top_c, mid_c, t)
        else:
            t = (y - H * 0.5) / (H * 0.5)
            c = lerp_color(mid_c, bot_c, t)
        draw.line([(0, y), (W, y)], fill=c)

    # Cosmic nebulae clouds (soft magenta & cyan swirls)
    nebula_layer = Image.new("RGBA", (W, H), (0, 0, 0, 0))
    n_draw = ImageDraw.Draw(nebula_layer)
    for x in range(W):
        y1 = 110 + int(40 * math.sin(x * 4 * math.pi / W) + 20 * math.cos(x * 2 * math.pi / W))
        y2 = 170 + int(35 * math.sin(x * 4 * math.pi / W + 2.0))
        for dy in range(-35, 45):
            alpha1 = max(0, int(85 * (1.0 - abs(dy) / 40.0)))
            n_draw.point((x, y1 + dy), fill=(230, 60, 160, alpha1))
            alpha2 = max(0, int(95 * (1.0 - abs(dy) / 40.0)))
            n_draw.point((x, y2 + dy), fill=(70, 160, 255, alpha2))

    nebula_blurred = nebula_layer.filter(ImageFilter.GaussianBlur(radius=5))
    img.alpha_composite(nebula_blurred)

    # Sparkling starry clusters
    random.seed(999)
    for _ in range(180):
        sx = random.randint(0, W - 1)
        sy = random.randint(5, int(H * 0.75))
        rad = random.choice([1, 1, 1, 2, 2])
        col = (random.randint(220, 255), random.randint(220, 255), 255, random.randint(150, 255))
        draw.ellipse([sx - rad, sy - rad, sx + rad, sy + rad], fill=col)

    # Giant Ringed Cosmic Frost Planet (Upper Right)
    px, py = 790, 85
    prad = 42
    # Planet body
    draw.ellipse([px - prad, py - prad, px + prad, py + prad], fill=(215, 235, 255, 240), outline=(255, 255, 255, 255), width=2)
    # Surface ice bands
    draw.arc([px - prad, py - prad, px + prad, py + prad], start=10, end=170, fill=(160, 195, 240, 220), width=6)
    draw.arc([px - prad, py - prad, px + prad, py + prad], start=200, end=340, fill=(140, 180, 235, 220), width=5)
    # Shimmering planetary rings
    draw.arc([px - 85, py - 18, px + 85, py + 18], start=160, end=380, fill=(240, 210, 255, 230), width=4)
    draw.arc([px - 95, py - 22, px + 95, py + 22], start=160, end=380, fill=(180, 230, 255, 160), width=2)

    save_and_sync(img, "bg_cosmic_sky.png", wrap_repeat=True)

    # B) bg_cosmic_mountains (Starlight crystalline jagged mountain ridges)
    img = Image.new("RGBA", (W, H), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    # Seamless peaks starting and ending at y=285
    peaks = [
        (0, H), (0, 285), (130, 145), (260, 255), (410, 110), (560, 230),
        (700, 130), (840, 245), (940, 165), (1024, 285), (1024, H)
    ]
    draw.polygon(peaks, fill=(38, 28, 62, 255))

    # Glowing violet & cyan facet rim-lights
    for p1, p2 in [((130, 145), (260, 255)), ((410, 110), (560, 230)), ((700, 130), (840, 245))]:
        draw.line([p1, p2], fill=(210, 120, 255, 200), width=3)

    # Crystal snow crests on the peaks
    snow_peaks = [
        [(130, 145), (85, 195), (130, 185), (175, 195)],
        [(410, 110), (350, 170), (410, 160), (470, 170)],
        [(700, 130), (640, 185), (700, 175), (760, 185)],
        [(940, 165), (890, 215), (940, 205), (990, 215)]
    ]
    for sp in snow_peaks:
        draw.polygon(sp, fill=(245, 240, 255, 245), outline=(255, 255, 255, 255))

    save_and_sync(img, "bg_cosmic_mountains.png", wrap_repeat=True)

    # C) bg_cosmic_pines (Crystallized purple/cyan pine ridge)
    img = Image.new("RGBA", (W, H), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    hill_pts = [(0, H)]
    for x in range(0, W + 1, 16):
        y = int(350 + 30 * math.sin(x * 4 * math.pi / W))
        hill_pts.append((x, y))
    hill_pts.append((W, H))
    draw.polygon(hill_pts, fill=(28, 18, 48, 255))

    # Frosted starlight pines
    for x in range(16, W, 42):
        by = int(350 + 30 * math.sin(x * 4 * math.pi / W))
        draw.polygon([(x - 18, by), (x, by - 66), (x + 18, by)], fill=(18, 10, 32, 255))
        # Starlight silver snow on branches
        draw.polygon([(x - 10, by - 30), (x, by - 66), (x + 10, by - 30)], fill=(220, 200, 255, 220))
        draw.line([(x, by - 66), (x, by - 38)], fill=(255, 255, 255, 255), width=2)

    save_and_sync(img, "bg_cosmic_pines.png", wrap_repeat=True)

# -------------------------------------------------------------
# 2. GROUND & PLATFORM SPRITES FOR WORLD 3
# -------------------------------------------------------------
def generate_world3_terrain():
    # A) ground_cosmic_frost (128x128 tileable)
    # Dark cosmic obsidian with starlight crystal facets and sparkling snow on top
    img = Image.new("RGBA", (128, 128), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)

    # Bedrock body (y=38..128) in deep starlight violet-obsidian
    draw.rectangle([0, 38, 128, 128], fill=(36, 26, 52, 255))
    # Embedded glowing star runes and magenta/cyan crystals
    for px, py, pw, ph in [(14, 52, 22, 12), (72, 66, 24, 14), (36, 94, 20, 12), (94, 104, 16, 10), (104, 48, 16, 8)]:
        draw.rounded_rectangle([px, py, px+pw, py+ph], radius=3, fill=(55, 40, 75, 255), outline=(25, 15, 38, 255), width=1)
        # Starlight core dot inside rock
        draw.ellipse([px + pw//2 - 2, py + ph//2 - 2, px + pw//2 + 2, py + ph//2 + 2], fill=(210, 110, 255, 220))

    # Crystalline transition seam
    draw.rectangle([0, 32, 128, 44], fill=(120, 90, 170, 255))

    # Iridescent cosmic snow blanket on top (y=0..36)
    draw.rectangle([0, 0, 128, 36], fill=(245, 240, 255, 255))
    draw.rectangle([0, 0, 128, 8], fill=(255, 255, 255, 255))
    for x in range(-8, 136, 24):
        draw.ellipse([x, 26, x + 20, 40], fill=(245, 240, 255, 255))

    # Diamond starlight specks
    for sx, sy in [(16, 12), (50, 18), (84, 10), (112, 16), (34, 24), (96, 22)]:
        draw.ellipse([sx-2, sy-2, sx+2, sy+2], fill=(210, 180, 255, 255))
        draw.ellipse([sx-1, sy-1, sx+1, sy+1], fill=(255, 255, 255, 255))

    save_and_sync(img, "ground_cosmic_frost.png", wrap_repeat=True)

    # B) platform_cosmic (128x32)
    # Starlight rune platform with purple-cyan neon rim and top snow
    img = Image.new("RGBA", (128, 32), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    draw.rounded_rectangle([2, 4, 126, 28], radius=6, fill=(90, 60, 130, 230), outline=(220, 180, 255, 255), width=2)
    # Glowing rune lines
    for x in [32, 64, 96]:
        draw.line([(x, 6), (x, 26)], fill=(240, 120, 255, 220), width=2)
    # Pure snow cushion on top
    draw.rounded_rectangle([4, 2, 124, 10], radius=3, fill=(250, 245, 255, 255))
    draw.line([(6, 3), (122, 3)], fill=(255, 255, 255, 255), width=2)

    save_and_sync(img, "platform_cosmic.png", wrap_repeat=False)

# -------------------------------------------------------------
# 3. NEW WORLD 3 OBSTACLES & CHALLENGES
# -------------------------------------------------------------
def generate_world3_obstacles():
    # A) gravity_orb (64x64) - Mid-Air Floating Gravity Re-Bounce Orb
    img = Image.new("RGBA", (64, 64), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    cx, cy = 32, 32
    # Outer pulsating aura rings
    draw.ellipse([cx - 28, cy - 28, cx + 28, cy + 28], outline=(180, 80, 255, 120), width=3)
    draw.ellipse([cx - 24, cy - 24, cx + 24, cy + 24], outline=(80, 210, 255, 160), width=3)
    # Core energy sphere
    draw.ellipse([cx - 18, cy - 18, cx + 18, cy + 18], fill=(130, 60, 240, 240), outline=(255, 255, 255, 255), width=2)
    # Radiant center star
    star_pts = []
    r_out, r_in = 12, 5
    for i in range(8):
        ang = i * math.pi / 4 - math.pi / 2
        r = r_out if i % 2 == 0 else r_in
        star_pts.append((cx + r * math.cos(ang), cy + r * math.sin(ang)))
    draw.polygon(star_pts, fill=(255, 240, 120, 255), outline=(255, 255, 255, 255), width=1)
    save_and_sync(img, "gravity_orb.png", wrap_repeat=False)

    # B) frost_laser_beam (32x128) - Pulsing vertical beam
    img = Image.new("RGBA", (32, 128), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    # Outer glow
    draw.line([(16, 0), (16, 128)], fill=(120, 210, 255, 140), width=18)
    draw.line([(16, 0), (16, 128)], fill=(180, 240, 255, 200), width=10)
    # Core blazing white beam
    draw.line([(16, 0), (16, 128)], fill=(255, 255, 255, 255), width=4)
    save_and_sync(img, "frost_laser_beam.png", wrap_repeat=False)

    # C) frost_laser_emitter (48x48) - Monolith base
    img = Image.new("RGBA", (48, 48), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    draw.rounded_rectangle([4, 4, 44, 44], radius=6, fill=(50, 40, 75, 255), outline=(160, 210, 255, 255), width=2)
    draw.ellipse([14, 14, 34, 34], fill=(0, 220, 255, 240), outline=(255, 255, 255, 255), width=2)
    draw.ellipse([20, 20, 28, 28], fill=(255, 255, 255, 255))
    save_and_sync(img, "frost_laser_emitter.png", wrap_repeat=False)

    # D) crushing_ice_pillar (80x120) - Ceiling Thwomp Smasher
    img = Image.new("RGBA", (80, 120), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    # Body
    draw.rounded_rectangle([6, 4, 74, 96], radius=8, fill=(65, 85, 120, 255), outline=(200, 230, 255, 255), width=3)
    # Angry frost eyes
    draw.ellipse([18, 26, 32, 44], fill=(255, 60, 80, 255), outline=(255, 255, 255, 255), width=2)
    draw.ellipse([48, 26, 62, 44], fill=(255, 60, 80, 255), outline=(255, 255, 255, 255), width=2)
    draw.ellipse([22, 32, 28, 38], fill=(255, 255, 255, 255))
    draw.ellipse([52, 32, 58, 38], fill=(255, 255, 255, 255))
    # Bottom spike teeth
    spikes = [
        [(8, 96), (20, 116), (32, 96)],
        [(32, 96), (40, 118), (48, 96)],
        [(48, 96), (60, 116), (72, 96)]
    ]
    for pts in spikes:
        draw.polygon(pts, fill=(180, 230, 255, 255), outline=(255, 255, 255, 255), width=2)
    save_and_sync(img, "crushing_ice_pillar.png", wrap_repeat=False)

    # E) frost_conveyor (128x32) - High-speed ice conveyor belt
    img = Image.new("RGBA", (128, 32), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    draw.rounded_rectangle([2, 4, 126, 28], radius=5, fill=(30, 45, 65, 255), outline=(0, 210, 255, 255), width=2)
    # Animated neon forward chevrons
    for cx in [20, 48, 76, 104]:
        pts = [(cx, 8), (cx + 12, 16), (cx, 24), (cx - 4, 24), (cx + 6, 16), (cx - 4, 8)]
        draw.polygon(pts, fill=(0, 240, 255, 255), outline=(255, 255, 255, 255), width=1)
    save_and_sync(img, "frost_conveyor.png", wrap_repeat=False)

    # F) gravity_flip_pad (96x32) - Antigravity Inversion Well
    img = Image.new("RGBA", (96, 32), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    draw.rounded_rectangle([4, 6, 92, 26], radius=6, fill=(70, 30, 110, 240), outline=(220, 120, 255, 255), width=2)
    # Upward / downward antigravity arrows
    for ax in [24, 48, 72]:
        draw.polygon([(ax, 8), (ax - 8, 20), (ax + 8, 20)], fill=(255, 140, 255, 255), outline=(255, 255, 255, 255), width=1)
    save_and_sync(img, "gravity_flip_pad.png", wrap_repeat=False)

if __name__ == "__main__":
    print("=== Generating World 3 Assets (Levels 21-30) ===")
    generate_cosmic_backgrounds()
    generate_world3_terrain()
    generate_world3_obstacles()
    print("=== All World 3 Assets Generated & Synced Successfully! ===")
