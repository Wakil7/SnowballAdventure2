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
        return # keep existing GUID if present
    
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
# 1. AURORA NIGHT BACKGROUNDS (1024x512)
# -------------------------------------------------------------
def generate_aurora_backgrounds():
    W, H = 1024, 512

    # A) bg_aurora_sky (Deep midnight indigo with emerald/cyan aurora waves)
    img = Image.new("RGBA", (W, H), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)

    # Base sky gradient: dark indigo top to dark teal horizon
    top_c = (8, 10, 28, 255)
    mid_c = (15, 25, 52, 255)
    bot_c = (12, 45, 68, 255)
    for y in range(H):
        if y < H * 0.6:
            t = y / (H * 0.6)
            c = lerp_color(top_c, mid_c, t)
        else:
            t = (y - H * 0.6) / (H * 0.4)
            c = lerp_color(mid_c, bot_c, t)
        draw.line([(0, y), (W, y)], fill=c)

    # Glowing stars
    random.seed(42)
    for _ in range(140):
        sx = random.randint(0, W - 1)
        sy = random.randint(5, int(H * 0.75))
        rad = random.choice([1, 1, 1, 2])
        col = (random.randint(220, 255), random.randint(235, 255), 255, random.randint(140, 240))
        draw.ellipse([sx - rad, sy - rad, sx + rad, sy + rad], fill=col)

    # Swirling Aurora Borealis ribbons (sine-wave ribbons, perfectly seamless horizontally)
    aurora_layer = Image.new("RGBA", (W, H), (0, 0, 0, 0))
    a_draw = ImageDraw.Draw(aurora_layer)
    for x in range(W):
        # Wave 1: Emerald ribbon (2 periods across W)
        y1 = 120 + int(35 * math.sin(x * 4 * math.pi / W) + 18 * math.cos(x * 6 * math.pi / W))
        # Wave 2: Cyan-Turquoise ribbon
        y2 = 165 + int(42 * math.sin(x * 4 * math.pi / W + 1.2))
        # Draw vertical translucent ray curtains
        for dy in range(-25, 55):
            alpha = max(0, int(110 * (1.0 - abs(dy) / 45.0)))
            a_draw.point((x, y1 + dy), fill=(35, 245, 160, alpha))
            alpha2 = max(0, int(90 * (1.0 - abs(dy) / 40.0)))
            a_draw.point((x, y2 + dy), fill=(20, 210, 255, alpha2))

    # Blur aurora slightly for soft luminous glow
    aurora_blurred = aurora_layer.filter(ImageFilter.GaussianBlur(radius=3))
    img.alpha_composite(aurora_blurred)

    # Luminous crescent moon in top-right
    m_draw = ImageDraw.Draw(img)
    mx, my = 820, 75
    # Outer moon glow
    for r in range(45, 28, -2):
        m_draw.ellipse([mx - r, my - r, mx + r, my + r], fill=(210, 250, 240, int(4 * (45 - r))))
    # Moon disc
    m_draw.ellipse([mx - 28, my - 28, mx + 28, my + 28], fill=(245, 255, 240, 245))
    # Moon shadow to create crescent
    m_draw.ellipse([mx - 18, my - 34, mx + 38, my + 22], fill=top_c)

    save_and_sync(img, "bg_aurora_sky.png", wrap_repeat=True)

    # B) bg_aurora_mountains (Glacial mountain silhouettes with cyan aurora rim-lights)
    img = Image.new("RGBA", (W, H), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    # Seamless peaks starting and ending at y=290
    peaks = [
        (0, H), (0, 290), (120, 160), (240, 270), (380, 120), (520, 240),
        (660, 140), (800, 260), (920, 175), (1024, 290), (1024, H)
    ]
    draw.polygon(peaks, fill=(24, 34, 58, 255))

    # Aurora cyan rim light on left faces of peaks
    rim_lines = [
        [(120, 160), (240, 270)],
        [(380, 120), (520, 240)],
        [(660, 140), (800, 260)],
        [(920, 175), (1024, 290)]
    ]
    for line in rim_lines:
        draw.line(line, fill=(40, 210, 200, 180), width=3)

    # Crisp glowing snow crests on the peaks
    snow_peaks = [
        [(120, 160), (80, 210), (120, 200), (160, 210)],
        [(380, 120), (320, 180), (380, 170), (440, 180)],
        [(660, 140), (600, 195), (660, 185), (715, 195)],
        [(920, 175), (870, 225), (920, 215), (970, 225)]
    ]
    for sp in snow_peaks:
        draw.polygon(sp, fill=(215, 245, 255, 240), outline=(255, 255, 255, 255))

    save_and_sync(img, "bg_aurora_mountains.png", wrap_repeat=True)

    # C) bg_aurora_pines (Night forest with bioluminescent turquoise frost on branches)
    img = Image.new("RGBA", (W, H), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    hill_pts = [(0, H)]
    for x in range(0, W + 1, 16):
        y = int(350 + 32 * math.sin(x * 4 * math.pi / W))
        hill_pts.append((x, y))
    hill_pts.append((W, H))
    draw.polygon(hill_pts, fill=(18, 30, 48, 255))

    # Pine trees with glowing frosty needles
    for x in range(16, W, 44):
        by = int(350 + 32 * math.sin(x * 4 * math.pi / W))
        # Silhouette pine
        draw.polygon([(x - 20, by), (x, by - 68), (x + 20, by)], fill=(12, 22, 36, 255))
        # Bioluminescent snow on branches
        draw.polygon([(x - 12, by - 32), (x, by - 68), (x + 12, by - 32)], fill=(120, 230, 220, 220))
        draw.line([(x, by - 68), (x, by - 40)], fill=(230, 255, 255, 255), width=2)

    save_and_sync(img, "bg_aurora_pines.png", wrap_repeat=True)

# -------------------------------------------------------------
# 2. GROUND & PLATFORM SPRITES FOR LEVELS 11-20
# -------------------------------------------------------------
def generate_world2_terrain():
    # A) ground_frost_deep (128x128 tileable)
    # Deep midnight-slate glacial bedrock with glowing cyan crystal specks and pure snow on top
    img = Image.new("RGBA", (128, 128), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)

    # Bedrock body (y=38..128)
    draw.rectangle([0, 38, 128, 128], fill=(32, 40, 58, 255))
    # Embedded glowing cyan crystal veins
    for px, py, pw, ph in [(12, 54, 20, 10), (70, 68, 26, 12), (32, 92, 22, 14), (92, 102, 18, 10), (102, 46, 16, 8)]:
        draw.rounded_rectangle([px, py, px+pw, py+ph], radius=3, fill=(45, 65, 88, 255), outline=(25, 32, 48, 255), width=1)
        # Crystal shimmer dot inside rock
        draw.ellipse([px + pw//2 - 2, py + ph//2 - 2, px + pw//2 + 2, py + ph//2 + 2], fill=(60, 220, 240, 200))

    # Permafrost transition seam
    draw.rectangle([0, 32, 128, 44], fill=(70, 125, 165, 255))

    # Crisp winter snow blanket on top (y=0..36)
    draw.rectangle([0, 0, 128, 36], fill=(240, 248, 255, 255))
    # Pure white top surface rim
    draw.rectangle([0, 0, 128, 8], fill=(255, 255, 255, 255))
    # Scalloped frost ripples along snow bottom
    for x in range(-8, 136, 24):
        draw.ellipse([x, 26, x + 20, 40], fill=(240, 248, 255, 255))

    # Aurora sparkle dots on snow
    for sx, sy in [(18, 12), (52, 18), (86, 10), (114, 16), (36, 24), (98, 22)]:
        draw.ellipse([sx-2, sy-2, sx+2, sy+2], fill=(140, 230, 255, 255))
        draw.ellipse([sx-1, sy-1, sx+1, sy+1], fill=(255, 255, 255, 255))

    save_and_sync(img, "ground_frost_deep.png", wrap_repeat=True)

    # B) platform_crystal (128x32)
    # Faceted cyan ice crystal platform with glowing edge highlights and top snow
    img = Image.new("RGBA", (128, 32), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    # Crystal slab body
    draw.rounded_rectangle([2, 4, 126, 28], radius=6, fill=(80, 180, 230, 220), outline=(200, 245, 255, 255), width=2)
    # Facet cuts
    for x in [32, 64, 96]:
        draw.line([(x, 6), (x - 8, 26)], fill=(50, 140, 190, 200), width=2)
        draw.line([(x + 1, 6), (x - 7, 26)], fill=(220, 250, 255, 180), width=1)
    # Snow layer on top surface
    draw.rounded_rectangle([4, 2, 124, 10], radius=3, fill=(245, 252, 255, 255))
    draw.line([(6, 3), (122, 3)], fill=(255, 255, 255, 255), width=2)

    save_and_sync(img, "platform_crystal.png", wrap_repeat=False)

# -------------------------------------------------------------
# 3. NEW OBSTACLES & CHALLENGE SPRITES
# -------------------------------------------------------------
def generate_world2_obstacles():
    # A) frost_trampoline (128x64) - Mega Bouncy Pad / Super Mushroom
    img = Image.new("RGBA", (128, 64), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    # Heavy frosted base with bolts
    draw.rounded_rectangle([28, 50, 100, 62], radius=4, fill=(45, 60, 85, 255), outline=(130, 160, 200, 255), width=2)
    draw.ellipse([34, 53, 40, 59], fill=(210, 230, 255, 255))
    draw.ellipse([88, 53, 94, 59], fill=(210, 230, 255, 255))
    # Dual pneumatic high-energy springs
    for x in [44, 76]:
        for y in [34, 40, 46]:
            draw.ellipse([x - 8, y, x + 8, y + 6], fill=(160, 195, 230, 255), outline=(70, 95, 130, 255), width=2)
    # Mega bouncy trampoline pad (Glowing turquoise & violet dome)
    draw.chord([10, 8, 118, 52], start=180, end=360, fill=(30, 210, 190, 255), outline=(220, 255, 250, 255), width=3)
    draw.ellipse([18, 22, 110, 36], fill=(20, 160, 150, 255))
    # Glowing yellow star in center of pad
    cx, cy, r_out, r_in = 64, 20, 11, 5
    star_pts = []
    for i in range(10):
        angle = i * math.pi / 5 - math.pi / 2
        r = r_out if i % 2 == 0 else r_in
        star_pts.append((cx + r * math.cos(angle), cy + r * math.sin(angle)))
    draw.polygon(star_pts, fill=(255, 235, 60, 255), outline=(255, 255, 200, 255), width=1)
    # Upward velocity chevrons
    draw.polygon([(64, 4), (56, 12), (72, 12)], fill=(255, 255, 255, 255))

    save_and_sync(img, "frost_trampoline.png", wrap_repeat=False)

    # B) moving_ice_platform (128x32) - Floating motorized ice raft
    img = Image.new("RGBA", (128, 32), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    # Bottom mechanical hover chassis
    draw.rounded_rectangle([12, 14, 116, 30], radius=5, fill=(35, 48, 70, 255), outline=(90, 120, 160, 255), width=2)
    # Glowing cyan thruster vents
    for tx in [30, 64, 98]:
        draw.ellipse([tx - 8, 23, tx + 8, 29], fill=(0, 240, 255, 240), outline=(255, 255, 255, 255), width=1)
    # Top ice platform block
    draw.rounded_rectangle([4, 2, 124, 18], radius=5, fill=(150, 220, 250, 240), outline=(230, 250, 255, 255), width=2)
    # Pure snow cushion
    draw.rounded_rectangle([6, 2, 122, 9], radius=3, fill=(255, 255, 255, 255))

    save_and_sync(img, "moving_ice_platform.png", wrap_repeat=False)

    # C) swinging_frost_mace (96x96) - Spiked pendulum ice ball
    img = Image.new("RGBA", (96, 96), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    cx, cy = 48, 48
    # 8 crystalline ice spikes pointing outward
    for i in range(8):
        ang = i * math.pi / 4
        tip_x = cx + 44 * math.cos(ang)
        tip_y = cy + 44 * math.sin(ang)
        b1_x = cx + 24 * math.cos(ang - 0.28)
        b1_y = cy + 24 * math.sin(ang - 0.28)
        b2_x = cx + 24 * math.cos(ang + 0.28)
        b2_y = cy + 24 * math.sin(ang + 0.28)
        draw.polygon([(b1_x, b1_y), (tip_x, tip_y), (b2_x, b2_y)], fill=(160, 235, 255, 240), outline=(245, 255, 255, 255), width=1)
        # Spike highlight
        draw.line([(cx + 24 * math.cos(ang), cy + 24 * math.sin(ang)), (tip_x, tip_y)], fill=(255, 255, 255, 220), width=2)

    # Central frosted heavy iron orb
    draw.ellipse([cx - 26, cy - 26, cx + 26, cy + 26], fill=(55, 75, 105, 255), outline=(180, 220, 245, 255), width=3)
    # Inner ice crystal core with glow
    draw.ellipse([cx - 16, cy - 16, cx + 16, cy + 16], fill=(0, 210, 255, 255), outline=(255, 255, 255, 255), width=2)
    draw.ellipse([cx - 8, cy - 8, cx + 8, cy + 8], fill=(230, 255, 255, 255))

    save_and_sync(img, "swinging_frost_mace.png", wrap_repeat=False)

    # D) frost_chain_link (16x32) - Frosted chain for pendulum
    img = Image.new("RGBA", (16, 32), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    draw.rounded_rectangle([3, 2, 13, 30], radius=5, fill=(60, 85, 115, 255), outline=(190, 225, 250, 255), width=2)
    draw.rounded_rectangle([6, 7, 10, 25], radius=2, fill=(0, 0, 0, 0))
    draw.line([(4, 4), (4, 28)], fill=(255, 255, 255, 180), width=1)

    save_and_sync(img, "frost_chain_link.png", wrap_repeat=False)

    # E) frost_portal_blue (Entrance vortex - 96x128)
    img = Image.new("RGBA", (96, 128), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    cx, cy = 48, 64
    # Swirling outer aura
    for r in range(46, 20, -4):
        alpha = int(180 * (r / 46.0))
        draw.ellipse([cx - r*0.75, cy - r, cx + r*0.75, cy + r], outline=(30, 190, 255, alpha), width=3)
    # Portal frame arches
    draw.arc([10, 12, 86, 116], start=30, end=150, fill=(180, 240, 255, 255), width=4)
    draw.arc([10, 12, 86, 116], start=210, end=330, fill=(180, 240, 255, 255), width=4)
    # Swirling spiral rings
    for ang_deg in range(0, 360, 45):
        rad = math.radians(ang_deg)
        x1 = cx + 22 * math.cos(rad)
        y1 = cy + 32 * math.sin(rad)
        x2 = cx + 8 * math.cos(rad + 1.2)
        y2 = cy + 12 * math.sin(rad + 1.2)
        draw.line([(x1, y1), (x2, y2)], fill=(200, 245, 255, 220), width=2)
    # Bright center core
    draw.ellipse([cx - 8, cy - 12, cx + 8, cy + 12], fill=(255, 255, 255, 255))

    save_and_sync(img, "frost_portal_blue.png", wrap_repeat=False)

    # F) frost_portal_orange (Exit vortex - 96x128)
    img = Image.new("RGBA", (96, 128), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    # Swirling outer aura in amber/golden-rose
    for r in range(46, 20, -4):
        alpha = int(180 * (r / 46.0))
        draw.ellipse([cx - r*0.75, cy - r, cx + r*0.75, cy + r], outline=(255, 140, 40, alpha), width=3)
    # Portal frame arches
    draw.arc([10, 12, 86, 116], start=30, end=150, fill=(255, 220, 160, 255), width=4)
    draw.arc([10, 12, 86, 116], start=210, end=330, fill=(255, 220, 160, 255), width=4)
    for ang_deg in range(0, 360, 45):
        rad = math.radians(ang_deg)
        x1 = cx + 22 * math.cos(rad)
        y1 = cy + 32 * math.sin(rad)
        x2 = cx + 8 * math.cos(rad + 1.2)
        y2 = cy + 12 * math.sin(rad + 1.2)
        draw.line([(x1, y1), (x2, y2)], fill=(255, 240, 200, 220), width=2)
    draw.ellipse([cx - 8, cy - 12, cx + 8, cy + 12], fill=(255, 255, 255, 255))

    save_and_sync(img, "frost_portal_orange.png", wrap_repeat=False)

if __name__ == "__main__":
    print("=== Generating World 2 Assets (Levels 11-20) ===")
    generate_aurora_backgrounds()
    generate_world2_terrain()
    generate_world2_obstacles()
    print("=== All World 2 Assets Generated & Synced Successfully! ===")
