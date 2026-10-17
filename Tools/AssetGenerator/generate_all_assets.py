import math
import wave
import struct
import os
from PIL import Image, ImageDraw, ImageFont, ImageFilter

OUTPUT_SPRITES = r"D:\Games\Snowball Adventure 2\Assets\Sprites"
OUTPUT_RESOURCES_SPRITES = r"D:\Games\Snowball Adventure 2\Assets\Resources\Sprites"
OUTPUT_AUDIO = r"D:\Games\Snowball Adventure 2\Assets\Audio"

os.makedirs(OUTPUT_SPRITES, exist_ok=True)
os.makedirs(OUTPUT_RESOURCES_SPRITES, exist_ok=True)
os.makedirs(OUTPUT_AUDIO, exist_ok=True)

# -------------------------------------------------------------
# HELPER DRAWING FUNCTIONS
# -------------------------------------------------------------

def lerp_color(c1, c2, t):
    return tuple(int(a + (b - a) * t) for a, b in zip(c1, c2))

def draw_radial_gradient(draw, center, radius, inner_color, outer_color):
    cx, cy = center
    for r in range(radius, 0, -1):
        t = r / radius
        col = lerp_color(inner_color, outer_color, t)
        draw.ellipse([cx - r, cy - r, cx + r, cy + r], fill=col)

def draw_smooth_circle(draw, bbox, fill, outline=None, width=1):
    draw.ellipse(bbox, fill=fill, outline=outline, width=width)

def draw_sparkle(draw, cx, cy, size, color):
    draw.polygon([(cx, cy - size), (cx + 1, cy), (cx, cy + size), (cx - 1, cy)], fill=color)
    draw.polygon([(cx - size, cy), (cx, cy + 1), (cx + size, cy), (cx - 1, cy)], fill=color)
    draw.ellipse([cx - 1, cy - 1, cx + 1, cy + 1], fill=(255, 255, 255, 255))

# -------------------------------------------------------------
# 1. 10 SNOWBALL CHARACTERS (128x128 high-res sprites)
# -------------------------------------------------------------
characters = [
    {
        "id": "char_0_frosty",
        "name": "Frosty Classic",
        "base_col": (240, 248, 255, 255),
        "shade_col": (180, 215, 245, 255),
        "accent_col": (40, 140, 240, 255),
        "eye_col": (30, 40, 60, 255),
        "hat": "beanie",
    },
    {
        "id": "char_1_pebble",
        "name": "Pebble Snow",
        "base_col": (220, 230, 230, 255),
        "shade_col": (150, 165, 175, 255),
        "accent_col": (90, 110, 120, 255),
        "eye_col": (40, 45, 50, 255),
        "hat": "pebble",
    },
    {
        "id": "char_2_minty",
        "name": "Minty Swirl",
        "base_col": (220, 255, 240, 255),
        "shade_col": (140, 220, 195, 255),
        "accent_col": (30, 190, 150, 255),
        "eye_col": (20, 60, 50, 255),
        "hat": "swirl",
    },
    {
        "id": "char_3_glacier",
        "name": "Crystal Glacier",
        "base_col": (200, 245, 255, 255),
        "shade_col": (120, 195, 240, 255),
        "accent_col": (70, 220, 255, 255),
        "eye_col": (10, 50, 90, 255),
        "hat": "ice_crown",
    },
    {
        "id": "char_4_berry",
        "name": "Berry Sorbet",
        "base_col": (255, 225, 240, 255),
        "shade_col": (245, 150, 195, 255),
        "accent_col": (230, 60, 130, 255),
        "eye_col": (70, 20, 50, 255),
        "hat": "cherry",
    },
    {
        "id": "char_5_midas",
        "name": "Golden Nugget",
        "base_col": (255, 248, 210, 255),
        "shade_col": (235, 195, 90, 255),
        "accent_col": (255, 215, 0, 255),
        "eye_col": (60, 45, 10, 255),
        "hat": "gold_crown",
    },
    {
        "id": "char_6_aurora",
        "name": "Aurora Star",
        "base_col": (230, 225, 255, 255),
        "shade_col": (175, 140, 240, 255),
        "accent_col": (150, 90, 240, 255),
        "eye_col": (40, 20, 70, 255),
        "hat": "star",
    },
    {
        "id": "char_7_blaze",
        "name": "Inferno Spark",
        "base_col": (255, 235, 215, 255),
        "shade_col": (245, 140, 80, 255),
        "accent_col": (240, 80, 30, 255),
        "eye_col": (80, 25, 10, 255),
        "hat": "flame",
    },
    {
        "id": "char_8_cyber",
        "name": "Cyber Neon",
        "base_col": (215, 245, 255, 255),
        "shade_col": (100, 170, 220, 255),
        "accent_col": (0, 240, 255, 255),
        "eye_col": (0, 240, 255, 255),
        "hat": "visor",
    },
    {
        "id": "char_9_cosmo",
        "name": "Cosmic Nebula",
        "base_col": (235, 228, 255, 255),
        "shade_col": (110, 80, 180, 255),
        "accent_col": (200, 110, 255, 255),
        "eye_col": (25, 12, 50, 255),
        "hat": "orbit_ring",
    },
]

def generate_characters():
    S = 128
    for ch in characters:
        img = Image.new("RGBA", (S, S), (0, 0, 0, 0))
        draw = ImageDraw.Draw(img)

        # Exact mathematical center and radius for 100% balanced rotation
        cx, cy, r = 64, 64, 54

        # 1. Base solid spherical silhouette (guarantees perfect circle centered at 64, 64 with radius 54)
        draw.ellipse([cx - r, cy - r, cx + r, cy + r], fill=ch["shade_col"])

        # 2. Smooth internal radial shading towards top-left light source
        # offset_t smoothly transitions from 0 at outer boundary to ~8px at the brightest center
        for step in range(r - 1, 0, -1):
            t = (r - step) / r
            col = lerp_color(ch["shade_col"], ch["base_col"], t)
            offset_t = int(t * 8)
            draw.ellipse([cx - step - offset_t, cy - step - offset_t,
                          cx + step - offset_t, cy + step - offset_t], fill=col)

        # 3. Outer smooth rim (perfect circle centered at 64, 64)
        draw.ellipse([cx - r, cy - r, cx + r, cy + r],
                     outline=(max(0, ch["shade_col"][0] - 25),
                              max(0, ch["shade_col"][1] - 25),
                              max(0, ch["shade_col"][2] - 25), 200), width=2)

        hat = ch["hat"]

        # 3. Soft snow texture specks inside sphere
        if hat == "visor":
            specks = [(40, 94), (88, 94), (64, 100)]
            for sx, sy in specks:
                draw.ellipse([sx - 2, sy - 2, sx + 2, sy + 2], fill=(255, 255, 255, 120))
        elif hat == "orbit_ring":
            draw_sparkle(draw, 42, 90, 3, (255, 255, 255, 220))
            draw_sparkle(draw, 86, 90, 3, (255, 255, 255, 220))
            draw_sparkle(draw, 64, 97, 2, (255, 240, 180, 220))
            draw.ellipse([34, 60, 36, 62], fill=(255, 255, 255, 180))
            draw.ellipse([94, 62, 96, 64], fill=(255, 255, 255, 180))
        else:
            specks = [(45, 82), (83, 85), (64, 94), (35, 60), (93, 62)]
            for sx, sy in specks:
                draw.ellipse([sx - 2, sy - 2, sx + 2, sy + 2], fill=(255, 255, 255, 120))

        # 4. Glossy highlight reflection (crescent shine at top-left)
        if hat == "visor":
            # Reposition highlight neatly above the cyber visor
            draw.ellipse([32, 18, 52, 36], fill=(255, 255, 255, 160))
            draw.ellipse([36, 22, 46, 31], fill=(255, 255, 255, 240))
        else:
            draw.ellipse([26, 26, 56, 48], fill=(255, 255, 255, 150))
            draw.ellipse([32, 30, 46, 42], fill=(255, 255, 255, 220))

        # 5. Accessories / Hats fitted smoothly to spherical contour
        if hat == "beanie":
            # Cozy winter beanie cuff wrapping top of sphere
            draw.arc([cx - r + 3, cy - r + 3, cx + r - 3, cy + r - 3], start=210, end=330, fill=ch["accent_col"], width=14)
            # White fluffy trim
            draw.arc([cx - r + 4, cy - r + 4, cx + r - 4, cy + r - 4], start=215, end=325, fill=(245, 250, 255, 255), width=6)
            # Pompom at top center
            draw.ellipse([58, 8, 70, 20], fill=(255, 255, 255, 255), outline=ch["accent_col"], width=1)
        elif hat == "pebble":
            draw.ellipse([56, 14, 72, 26], fill=(100, 120, 130, 255), outline=(70, 85, 95, 255), width=2)
            draw.ellipse([60, 16, 68, 22], fill=(160, 210, 160, 220))
        elif hat == "swirl":
            draw.arc([48, 14, 80, 38], start=190, end=350, fill=ch["accent_col"], width=5)
            draw.ellipse([60, 12, 68, 20], fill=ch["accent_col"])
        elif hat == "ice_crown":
            pts = [(44, 30), (52, 14), (58, 24), (64, 10), (70, 24), (76, 14), (84, 30)]
            draw.polygon(pts, fill=(180, 235, 255, 230), outline=(255, 255, 255, 255))
        elif hat == "cherry":
            draw.line([(64, 24), (68, 12)], fill=(80, 120, 50, 255), width=2)
            draw.ellipse([58, 18, 70, 30], fill=(220, 30, 70, 255), outline=(255, 255, 255, 180), width=1)
        elif hat == "gold_crown":
            pts = [(44, 28), (50, 14), (56, 22), (64, 10), (72, 22), (78, 14), (84, 28)]
            draw.polygon(pts, fill=(255, 215, 0, 255), outline=(200, 150, 10, 255))
            draw.ellipse([61, 20, 67, 26], fill=(230, 30, 60, 255))
        elif hat == "star":
            draw.ellipse([58, 12, 70, 24], fill=(255, 240, 100, 255), outline=(240, 180, 20, 255), width=1)
            draw.line([(64, 8), (64, 28)], fill=(255, 255, 255, 255), width=2)
            draw.line([(54, 18), (74, 18)], fill=(255, 255, 255, 255), width=2)
        elif hat == "flame":
            pts = [(54, 26), (58, 14), (64, 8), (70, 14), (74, 26)]
            draw.polygon(pts, fill=(255, 130, 20, 240))
            pts_inner = [(58, 26), (61, 18), (64, 12), (67, 18), (70, 26)]
            draw.polygon(pts_inner, fill=(255, 235, 80, 255))
        elif hat == "orbit_ring":
            # Planetary Orbit Ring (Floating celestial Saturn halo)
            halo_img = Image.new("RGBA", (S, S), (0, 0, 0, 0))
            hdraw = ImageDraw.Draw(halo_img)

            hx, hy = 64, 27
            # Outer glow
            hdraw.ellipse([hx - 44, hy - 11, hx + 44, hy + 11], outline=(210, 120, 255, 190), width=4)
            # Bright core ring
            hdraw.ellipse([hx - 43, hy - 10, hx + 43, hy + 10], outline=(255, 245, 255, 255), width=2)
            # Inner gold shimmer
            hdraw.ellipse([hx - 41, hy - 8, hx + 41, hy + 8], outline=(255, 220, 140, 210), width=1)

            # Mini Orbiting Moon on the right of the halo
            mx, my = hx + 39, hy - 4
            hdraw.ellipse([mx - 7, my - 7, mx + 7, my + 7], fill=(255, 235, 140, 255), outline=(220, 160, 40, 255), width=1)
            hdraw.ellipse([mx - 5, my - 5, mx + 1, my + 1], fill=(255, 255, 230, 255))
            hdraw.ellipse([mx + 1, my + 1, mx + 4, my + 4], fill=(225, 190, 90, 255))

            # Orbiting starlet on the left
            sx, sy = hx - 36, hy + 4
            draw_sparkle(hdraw, sx, sy, 3, (255, 245, 180, 255))

            # Rotate halo by -14 degrees for a natural planetary angle
            halo_rot = halo_img.rotate(-14, resample=Image.Resampling.BILINEAR, center=(64, 64))
            img.alpha_composite(halo_rot)

            # Golden Crescent Moon & Star Forehead Emblem
            draw.arc([57, 18, 71, 32], start=40, end=275, fill=(255, 215, 60, 255), width=3)
            draw.arc([58, 19, 70, 31], start=45, end=270, fill=(255, 245, 160, 255), width=1)
            draw_sparkle(draw, 64, 38, 3, (255, 235, 140, 255))
        elif hat == "visor":
            # High-tech Cyber Headset & Visor
            accent_cyan = ch["accent_col"]
            bright_cyan = (150, 255, 255, 255)
            accent_pink = (255, 45, 140, 255)
            dark_metal = (16, 22, 36, 255)

            # Lower cheek cyber circuits
            draw.line([(28, 76), (22, 76), (18, 86)], fill=(0, 220, 255, 160), width=2)
            draw.ellipse([16, 84, 20, 88], fill=accent_cyan)
            draw.line([(100, 76), (106, 76), (110, 86)], fill=(0, 220, 255, 160), width=2)
            draw.ellipse([108, 84, 112, 88], fill=accent_cyan)

            # Headband across top
            draw.arc([16, 16, 112, 112], start=225, end=315, fill=dark_metal, width=7)
            draw.arc([17, 17, 111, 111], start=230, end=310, fill=accent_cyan, width=2)

            # Top Central Cyber Crest / Fin (stays within y=8)
            draw.polygon([(61, 18), (67, 18), (65, 9), (63, 9)], fill=dark_metal, outline=accent_cyan)
            draw.ellipse([62, 7, 66, 11], fill=bright_cyan)

            # Left Ear Node (Headphone cup)
            draw.rounded_rectangle([13, 50, 25, 74], radius=5, fill=dark_metal, outline=accent_cyan, width=2)
            draw.ellipse([16, 56, 22, 68], fill=accent_pink)
            draw.ellipse([18, 60, 20, 64], fill=(255, 200, 230, 255))

            # Right Ear Node (Headphone cup)
            draw.rounded_rectangle([103, 50, 115, 74], radius=5, fill=dark_metal, outline=accent_cyan, width=2)
            draw.ellipse([106, 56, 112, 68], fill=accent_pink)
            draw.ellipse([108, 60, 110, 64], fill=(255, 200, 230, 255))

        # 6. Face Details (Eyes, Cheeks, Mouth)
        if ch["hat"] == "orbit_ring":
            # Starlight Rosy Cheeks
            draw.ellipse([35, 71, 49, 81], fill=(255, 115, 180, 110))
            draw.ellipse([79, 71, 93, 81], fill=(255, 115, 180, 110))
            draw_sparkle(draw, 42, 76, 2, (255, 255, 255, 240))
            draw_sparkle(draw, 86, 76, 2, (255, 255, 255, 240))

            # Big Gorgeous Anime Cosmic Eyes (Amethyst Violet with Galaxy Star Catchlights)
            for ex in [51, 77]:
                draw.ellipse([ex - 6, 52, ex + 6, 70], fill=(25, 12, 50, 255))
                draw.ellipse([ex - 5, 57, ex + 5, 69], fill=(145, 70, 225, 255))
                draw.ellipse([ex - 4, 62, ex + 4, 68], fill=(245, 175, 255, 255))
                draw.ellipse([ex - 2, 64, ex + 2, 67], fill=(255, 230, 160, 255))
                draw.ellipse([ex - 4, 54, ex + 1, 61], fill=(255, 255, 255, 255))
                draw_sparkle(draw, ex + 2, 64, 2, (255, 255, 255, 240))

            # Cute smile
            draw.arc([56, 69, 72, 79], start=10, end=170, fill=(35, 18, 65, 255), width=2)
        elif ch["hat"] != "visor":
            # Rosy Cheeks (symmetric about x = 64)
            draw.ellipse([36, 70, 50, 80], fill=(255, 130, 150, 80))
            draw.ellipse([78, 70, 92, 80], fill=(255, 130, 150, 80))

            # Left Eye
            draw.ellipse([45, 52, 57, 70], fill=ch["eye_col"])
            draw.ellipse([47, 54, 52, 61], fill=(255, 255, 255, 245))  # primary catchlight
            draw.ellipse([52, 63, 55, 66], fill=(255, 255, 255, 180))

            # Right Eye
            draw.ellipse([71, 52, 83, 70], fill=ch["eye_col"])
            draw.ellipse([73, 54, 78, 61], fill=(255, 255, 255, 245))
            draw.ellipse([78, 63, 81, 66], fill=(255, 255, 255, 180))

            # Cute smile
            draw.arc([55, 68, 73, 78], start=10, end=170, fill=ch["eye_col"], width=2)
        else:
            accent_cyan = ch["accent_col"]
            bright_cyan = (150, 255, 255, 255)
            accent_pink = (255, 45, 140, 255)

            # Cyber Visor (Futuristic aerodynamic wrap-around)
            visor_poly = [
                (25, 53),
                (38, 45),
                (64, 43),
                (90, 45),
                (103, 53),
                (97, 72),
                (64, 75),
                (31, 72)
            ]
            draw.polygon(visor_poly, fill=(12, 16, 28, 252), outline=accent_cyan)

            # Visor bottom neon bevel
            draw.line([(31, 72), (64, 75), (97, 72)], fill=accent_cyan, width=2)

            # Visor gloss reflection streak on top-left
            draw.polygon([(33, 48), (48, 46), (44, 53), (30, 54)], fill=(255, 255, 255, 140))

            # Left Digital LED Eye
            draw.rounded_rectangle([39, 51, 55, 67], radius=4, fill=(0, 215, 255, 240), outline=bright_cyan, width=1)
            draw.rectangle([42, 53, 46, 58], fill=(255, 255, 255, 255))
            draw.line([40, 56, 54, 56], fill=(10, 30, 50, 150), width=1)
            draw.line([40, 61, 54, 61], fill=(10, 30, 50, 150), width=1)

            # Right Digital LED Eye
            draw.rounded_rectangle([73, 51, 89, 67], radius=4, fill=(0, 215, 255, 240), outline=bright_cyan, width=1)
            draw.rectangle([76, 53, 80, 58], fill=(255, 255, 255, 255))
            draw.line([74, 56, 88, 56], fill=(10, 30, 50, 150), width=1)
            draw.line([74, 61, 88, 61], fill=(10, 30, 50, 150), width=1)

            # Center Mini HUD targeting reticle & audio bars
            draw.line([(62, 57), (62, 63)], fill=accent_pink, width=1)
            draw.line([(64, 54), (64, 66)], fill=bright_cyan, width=1)
            draw.line([(66, 57), (66, 63)], fill=accent_pink, width=1)

            # Cute cyber smile below visor
            draw.arc([56, 78, 72, 87], start=20, end=160, fill=(20, 45, 75, 255), width=2)

            # Cyber Cheek Marks (glowing dual neon dashes)
            draw.line([(34, 79), (46, 79)], fill=accent_pink, width=2)
            draw.line([(37, 83), (43, 83)], fill=accent_cyan, width=2)
            draw.line([(82, 79), (94, 79)], fill=accent_pink, width=2)
            draw.line([(85, 83), (91, 83)], fill=accent_cyan, width=2)

        filepath = os.path.join(OUTPUT_SPRITES, f"{ch['id']}.png")
        img.save(filepath, "PNG")
        res_filepath = os.path.join(OUTPUT_RESOURCES_SPRITES, f"{ch['id']}.png")
        img.save(res_filepath, "PNG")
        print(f"Generated {filepath} and {res_filepath}")

# -------------------------------------------------------------
# 2. COLLECTIBLE COIN (64x64) - Star embossed gold coin
# -------------------------------------------------------------
def generate_coin():
    S = 64
    img = Image.new("RGBA", (S, S), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)

    # Shadow
    draw.ellipse([10, 12, 54, 56], fill=(140, 90, 10, 100))
    # Outer gold rim
    draw.ellipse([8, 8, 56, 56], fill=(255, 215, 0, 255), outline=(200, 140, 10, 255), width=2)
    # Inner gradient rim
    draw.ellipse([12, 12, 52, 52], fill=(255, 235, 80, 255), outline=(240, 180, 20, 255), width=2)
    # Center embossed star
    star_pts = []
    cx, cy, r_out, r_in = 32, 32, 14, 6
    for i in range(10):
        angle = i * math.pi / 5 - math.pi / 2
        r = r_out if i % 2 == 0 else r_in
        star_pts.append((cx + r * math.cos(angle), cy + r * math.sin(angle)))
    draw.polygon(star_pts, fill=(255, 175, 10, 255), outline=(255, 255, 180, 255), width=1)

    # Specular shine
    draw.ellipse([16, 14, 26, 22], fill=(255, 255, 255, 220))
    draw.line([(44, 16), (48, 20)], fill=(255, 255, 255, 200), width=2)

    filepath = os.path.join(OUTPUT_SPRITES, "coin_gold.png")
    img.save(filepath, "PNG")
    print(f"Generated {filepath}")

# -------------------------------------------------------------
# 3. LEVEL 1 OBSTACLES (6 types)
# -------------------------------------------------------------
def generate_obstacles():
    # A) Ice Spikes (128x64)
    img = Image.new("RGBA", (128, 64), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    # Draw 4 sharp icy crystal spikes
    spike_configs = [
        [(8, 62), (20, 8), (34, 62)],
        [(30, 62), (48, 2), (66, 62)],
        [(60, 62), (80, 12), (98, 62)],
        [(92, 62), (108, 18), (122, 62)]
    ]
    for pts in spike_configs:
        draw.polygon(pts, fill=(160, 225, 255, 230), outline=(230, 250, 255, 255), width=1)
        # Shading half
        mid_x = pts[1][0]
        shade_pts = [(pts[0][0], pts[0][1]), (mid_x, pts[1][1]), (mid_x, 62)]
        draw.polygon(shade_pts, fill=(110, 185, 235, 180))
        # Glisten highlight
        draw.line([(mid_x, pts[1][1] + 2), (mid_x, 56)], fill=(255, 255, 255, 220), width=2)
    # Frosty base
    draw.rounded_rectangle([4, 56, 124, 63], radius=3, fill=(210, 240, 255, 240))
    img.save(os.path.join(OUTPUT_SPRITES, "ice_spikes.png"), "PNG")

    # B) Bounce Mushroom / Spring (96x80)
    img = Image.new("RGBA", (96, 80), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    # Spring stem (metallic coils)
    for y in [44, 52, 60, 68]:
        draw.ellipse([34, y, 62, y + 8], fill=(180, 200, 220, 255), outline=(100, 120, 140, 255), width=2)
    # Stem base
    draw.rounded_rectangle([24, 70, 72, 78], radius=3, fill=(120, 140, 160, 255), outline=(80, 95, 110, 255), width=2)
    # Spring bouncy cap (vibrant cyan-purple mushroom cap with glowing spots)
    draw.chord([8, 12, 88, 64], start=180, end=360, fill=(40, 180, 240, 255), outline=(20, 120, 180, 255), width=2)
    draw.ellipse([14, 34, 82, 48], fill=(30, 150, 210, 255))
    # Glowing yellow spots
    for cx, cy, rad in [(32, 24, 6), (48, 18, 8), (66, 24, 6), (48, 30, 5)]:
        draw.ellipse([cx - rad, cy - rad, cx + rad, cy + rad], fill=(255, 240, 100, 255), outline=(255, 255, 200, 255), width=1)
    # Upward bounce indicator arrows
    draw.polygon([(48, 3), (42, 10), (54, 10)], fill=(255, 255, 255, 240))
    img.save(os.path.join(OUTPUT_SPRITES, "bounce_spring.png"), "PNG")

    # C) Crumbling Ice Platform (128x40)
    img = Image.new("RGBA", (128, 40), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    # Translucent frost block
    draw.rounded_rectangle([2, 4, 126, 36], radius=6, fill=(170, 225, 245, 210), outline=(240, 250, 255, 255), width=2)
    # Fluffy snow frosting on top
    draw.rounded_rectangle([2, 2, 126, 12], radius=4, fill=(245, 250, 255, 255))
    # Cracks & fractures
    cracks = [
        [(24, 6), (36, 18), (30, 28), (42, 34)],
        [(64, 4), (58, 14), (70, 22), (66, 34)],
        [(96, 6), (90, 16), (102, 26), (98, 34)]
    ]
    for line in cracks:
        draw.line(line, fill=(100, 170, 210, 230), width=2)
        draw.line([(p[0]+1, p[1]) for p in line], fill=(255, 255, 255, 180), width=1)
    img.save(os.path.join(OUTPUT_SPRITES, "crumbling_ice.png"), "PNG")

    # D) Moving Frost Saw / Cutter (80x80)
    img = Image.new("RGBA", (80, 80), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    # 8 sharp triangular ice blade teeth
    cx, cy, r_out, r_in = 40, 40, 38, 26
    teeth_pts = []
    for i in range(16):
        ang = i * math.pi / 8
        r = r_out if i % 2 == 0 else r_in
        teeth_pts.append((cx + r * math.cos(ang), cy + r * math.sin(ang)))
    draw.polygon(teeth_pts, fill=(140, 215, 250, 240), outline=(240, 252, 255, 255), width=2)
    # Center metallic hub with frost gem
    draw.ellipse([22, 22, 58, 58], fill=(70, 130, 180, 255), outline=(210, 240, 255, 255), width=2)
    draw.ellipse([30, 30, 50, 50], fill=(20, 200, 255, 255), outline=(255, 255, 255, 255), width=2)
    img.save(os.path.join(OUTPUT_SPRITES, "frost_saw.png"), "PNG")

    # E) Falling Icicle (40x80)
    img = Image.new("RGBA", (40, 80), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    # Crystal icicle hanging down
    pts = [(6, 4), (34, 4), (24, 76), (16, 76)]
    draw.polygon(pts, fill=(175, 230, 255, 230), outline=(245, 252, 255, 255), width=2)
    # Shading on left
    draw.polygon([(6, 4), (20, 4), (20, 76), (16, 76)], fill=(120, 190, 235, 180))
    # Bright shine on right
    draw.line([(24, 6), (22, 70)], fill=(255, 255, 255, 220), width=2)
    # Ceiling mount base
    draw.rounded_rectangle([2, 2, 38, 10], radius=3, fill=(220, 245, 255, 255))
    img.save(os.path.join(OUTPUT_SPRITES, "falling_icicle.png"), "PNG")

    # F) Floating Ice Floe / Raft (128x48)
    img = Image.new("RGBA", (128, 48), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    # Underwater ice base
    draw.polygon([(10, 20), (24, 44), (104, 44), (118, 20)], fill=(70, 150, 200, 210), outline=(50, 120, 170, 255), width=1)
    # Top floating ice pack
    draw.rounded_rectangle([4, 6, 124, 26], radius=6, fill=(200, 240, 255, 255), outline=(255, 255, 255, 255), width=2)
    # Snow layer on top
    draw.rounded_rectangle([6, 4, 122, 14], radius=4, fill=(255, 255, 255, 255))
    img.save(os.path.join(OUTPUT_SPRITES, "ice_floe.png"), "PNG")

    # G) Wind Draft Streamer (64x128) - Vertical air geyser
    img = Image.new("RGBA", (64, 128), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    # Swirling upward wind curls
    for y_offset in [10, 50, 90]:
        draw.arc([10, y_offset, 54, y_offset + 30], start=180, end=360, fill=(180, 235, 255, 180), width=3)
        draw.polygon([(32, y_offset - 4), (24, y_offset + 6), (40, y_offset + 6)], fill=(220, 248, 255, 230))
    # Vertical wind flow lines
    draw.line([(18, 12), (18, 116)], fill=(160, 225, 255, 120), width=2)
    draw.line([(32, 4), (32, 124)], fill=(230, 250, 255, 180), width=2)
    draw.line([(46, 12), (46, 116)], fill=(160, 225, 255, 120), width=2)
    img.save(os.path.join(OUTPUT_SPRITES, "wind_draft.png"), "PNG")

    # H) Blinking Phase Ice Platform (128x36)
    img = Image.new("RGBA", (128, 36), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    # Glowing crystal glass block
    draw.rounded_rectangle([2, 2, 126, 34], radius=6, fill=(130, 220, 255, 190), outline=(230, 255, 255, 255), width=2)
    # Diamond energy core
    for cx in [32, 64, 96]:
        pts = [(cx, 8), (cx + 10, 18), (cx, 28), (cx - 10, 18)]
        draw.polygon(pts, fill=(255, 255, 255, 240), outline=(100, 200, 255, 255), width=1)
    # Top frost sheen
    draw.line([(8, 4), (120, 4)], fill=(255, 255, 255, 240), width=2)
    img.save(os.path.join(OUTPUT_SPRITES, "blinking_ice.png"), "PNG")

    # I) Speed Boost Ramp (128x32) - Golden Chevron Booster
    img = Image.new("RGBA", (128, 32), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    draw.rounded_rectangle([2, 4, 126, 28], radius=4, fill=(40, 60, 80, 240), outline=(255, 215, 0, 255), width=2)
    # 4 golden forward-pointing acceleration chevrons
    for ch_x in [24, 52, 80, 108]:
        pts = [(ch_x, 8), (ch_x + 12, 16), (ch_x, 24), (ch_x - 4, 24), (ch_x + 6, 16), (ch_x - 4, 8)]
        draw.polygon(pts, fill=(255, 220, 40, 255), outline=(255, 255, 180, 255), width=1)
    img.save(os.path.join(OUTPUT_SPRITES, "speed_boost.png"), "PNG")

    # J) Rotating Frost Bar (160x32) - Spiked obstacle arm
    img = Image.new("RGBA", (160, 32), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    # Ice beam
    draw.rounded_rectangle([16, 8, 144, 24], radius=4, fill=(150, 220, 245, 230), outline=(240, 252, 255, 255), width=2)
    # Left spike tip
    draw.polygon([(16, 6), (2, 16), (16, 26)], fill=(200, 240, 255, 255), outline=(100, 180, 230, 255), width=1)
    # Right spike tip
    draw.polygon([(144, 6), (158, 16), (144, 26)], fill=(200, 240, 255, 255), outline=(100, 180, 230, 255), width=1)
    # Center rotation hub
    draw.ellipse([70, 6, 90, 26], fill=(50, 100, 140, 255), outline=(255, 255, 255, 255), width=2)
    draw.ellipse([76, 12, 84, 20], fill=(0, 230, 255, 255))
    img.save(os.path.join(OUTPUT_SPRITES, "frost_bar.png"), "PNG")

    print("Generated all obstacle sprites!")

# -------------------------------------------------------------
# 4. TERRAIN TILES & PLATFORMS
# -------------------------------------------------------------
def generate_terrain():
    # 1. Ground Snow Top (128x128 tileable)
    img = Image.new("RGBA", (128, 128), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    # Frozen soil body (bottom 70% of tile)
    draw.rectangle([0, 38, 128, 128], fill=(68, 52, 74, 255))
    # Rock pebbles in soil
    for px, py, pw, ph in [(16, 55, 18, 10), (75, 70, 22, 14), (35, 95, 20, 12), (95, 105, 16, 10), (105, 45, 14, 8)]:
        draw.rounded_rectangle([px, py, px+pw, py+ph], radius=3, fill=(95, 80, 105, 255), outline=(50, 38, 55, 255), width=1)
    # Soil transition frost / ice seam
    draw.rectangle([0, 32, 128, 44], fill=(135, 170, 210, 255))
    # Thick fluffy winter snow blanket on top (fills y=0..36)
    draw.rectangle([0, 0, 128, 36], fill=(245, 250, 255, 255))
    # Top highlight sheen (pure crisp white top rim where ball rolls)
    draw.rectangle([0, 0, 128, 8], fill=(255, 255, 255, 255))
    # Puffy scalloped frost ripples along the bottom of the snow blanket
    for x in range(-8, 136, 24):
        draw.ellipse([x, 26, x + 20, 40], fill=(245, 250, 255, 255))
    # Ice sparkle dots embedded in snow
    for sx, sy in [(16, 12), (48, 18), (82, 10), (112, 16), (32, 24), (96, 22)]:
        draw.ellipse([sx-2, sy-2, sx+2, sy+2], fill=(190, 230, 255, 255))
        draw.ellipse([sx-1, sy-1, sx+1, sy+1], fill=(255, 255, 255, 255))
    img.save(os.path.join(OUTPUT_SPRITES, "ground_snow_block.png"), "PNG")

    # 2. Frozen Ice Block (128x128)
    img = Image.new("RGBA", (128, 128), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    draw.rounded_rectangle([2, 2, 126, 126], radius=10, fill=(140, 210, 245, 230), outline=(230, 250, 255, 255), width=3)
    # Bevel lines
    draw.line([(4, 4), (124, 4)], fill=(255, 255, 255, 240), width=3)
    draw.line([(4, 4), (4, 124)], fill=(255, 255, 255, 220), width=3)
    draw.line([(4, 124), (124, 124)], fill=(100, 170, 210, 255), width=3)
    draw.line([(124, 4), (124, 124)], fill=(100, 170, 210, 255), width=3)
    # Subtle internal frost pattern
    draw.line([(30, 20), (50, 45), (40, 70), (70, 95)], fill=(200, 240, 255, 160), width=2)
    draw.line([(85, 30), (75, 55), (95, 80)], fill=(200, 240, 255, 140), width=2)
    img.save(os.path.join(OUTPUT_SPRITES, "ice_platform_block.png"), "PNG")

    # 3. Frosty Wooden Platform (128x32)
    img = Image.new("RGBA", (128, 32), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    # Wood plank body (bottom)
    draw.rounded_rectangle([0, 8, 128, 32], radius=3, fill=(135, 90, 55, 255), outline=(90, 55, 30, 255), width=2)
    # Wood grain lines
    draw.line([(4, 18), (124, 18)], fill=(110, 70, 40, 255), width=1)
    # Iron rivets/nails
    draw.ellipse([8, 16, 14, 22], fill=(190, 200, 210, 255))
    draw.ellipse([114, 16, 120, 22], fill=(190, 200, 210, 255))
    # Snow blanket covering the top surface (y=0..10)
    draw.rectangle([0, 0, 128, 10], fill=(245, 250, 255, 255))
    # Crisp white top sheen
    draw.rectangle([0, 0, 128, 4], fill=(255, 255, 255, 255))
    # Soft scalloped snow ripples draping over the wood
    for x in range(-4, 132, 16):
        draw.ellipse([x, 6, x + 14, 13], fill=(245, 250, 255, 255))
    img.save(os.path.join(OUTPUT_SPRITES, "wooden_frost_platform.png"), "PNG")

    # 4. Pine Tree (128x192) - Gorgeous winter scenery prop
    img = Image.new("RGBA", (128, 192), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    # Trunk
    draw.rounded_rectangle([56, 150, 72, 186], radius=3, fill=(110, 75, 50, 255), outline=(70, 45, 30, 255), width=2)
    # 3 Foliage Tiers (Winter Spruce)
    tiers = [
        ([(20, 154), (64, 100), (108, 154)], (35, 95, 75, 255)),
        ([(28, 114), (64, 68), (100, 114)], (45, 115, 90, 255)),
        ([(38, 76), (64, 28), (90, 76)], (55, 135, 105, 255))
    ]
    for pts, col in tiers:
        draw.polygon(pts, fill=col, outline=(25, 70, 55, 255), width=2)
        # Snow cap on each tier
        snow_pts = [
            (pts[0][0], pts[0][1]),
            (pts[1][0], pts[1][1]),
            (pts[2][0], pts[2][1]),
            (pts[2][0] - 12, pts[2][1] - 8),
            (pts[1][0], pts[1][1] + 16),
            (pts[0][0] + 12, pts[0][1] - 8)
        ]
        draw.polygon(snow_pts, fill=(245, 252, 255, 255), outline=(210, 235, 250, 255), width=1)
    img.save(os.path.join(OUTPUT_SPRITES, "winter_pine_tree.png"), "PNG")
    print("Generated terrain & scenery sprites!")

# -------------------------------------------------------------
# 5. GOAL & CHECKPOINT
# -------------------------------------------------------------
def generate_goals():
    # 1. Victory Igloo / Goal Arch (160x120)
    img = Image.new("RGBA", (160, 120), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    # Igloo dome
    draw.chord([10, 10, 150, 150], start=180, end=360, fill=(225, 245, 255, 255), outline=(170, 215, 245, 255), width=3)
    # Igloo ice block seams
    draw.arc([24, 24, 136, 136], start=185, end=355, fill=(180, 220, 245, 255), width=2)
    draw.arc([42, 42, 118, 118], start=190, end=350, fill=(180, 220, 245, 255), width=2)
    # Entrance tunnel arch
    draw.chord([52, 46, 108, 120], start=180, end=360, fill=(40, 80, 120, 255), outline=(200, 235, 255, 255), width=4)
    # Golden star badge over entrance
    star_pts = []
    cx, cy, r_out, r_in = 80, 34, 14, 6
    for i in range(10):
        angle = i * math.pi / 5 - math.pi / 2
        r = r_out if i % 2 == 0 else r_in
        star_pts.append((cx + r * math.cos(angle), cy + r * math.sin(angle)))
    draw.polygon(star_pts, fill=(255, 215, 0, 255), outline=(255, 255, 255, 255), width=2)
    # Festive ribbon wreath
    draw.arc([68, 22, 92, 46], start=0, end=360, fill=(230, 40, 70, 255), width=3)
    img.save(os.path.join(OUTPUT_SPRITES, "level_goal_arch.png"), "PNG")

    # 2. Checkpoint Flag (64x96)
    for state, col in [("idle", (200, 220, 240, 255)), ("active", (50, 205, 90, 255))]:
        img = Image.new("RGBA", (64, 96), (0, 0, 0, 0))
        draw = ImageDraw.Draw(img)
        # Flag Pole
        draw.rounded_rectangle([14, 8, 20, 92], radius=2, fill=(150, 160, 175, 255), outline=(100, 110, 125, 255), width=1)
        # Gold finial on top
        draw.ellipse([12, 4, 22, 14], fill=(255, 215, 0, 255), outline=(200, 150, 10, 255), width=1)
        # Flag Banner
        flag_pts = [(20, 14), (58, 28), (20, 44)]
        draw.polygon(flag_pts, fill=col, outline=(255, 255, 255, 240), width=2)
        # Flag star icon
        draw.ellipse([26, 24, 34, 32], fill=(255, 255, 255, 255))
        img.save(os.path.join(OUTPUT_SPRITES, f"checkpoint_flag_{state}.png"), "PNG")
    print("Generated goal & checkpoint sprites!")

# -------------------------------------------------------------
# -------------------------------------------------------------
# 6. PARALLAX BACKGROUND LAYERS (1024x512 seamless high-res)
# -------------------------------------------------------------
def generate_backgrounds():
    W, H = 1024, 512

    # 1. Sky Gradient with aurora & moon (1024x512)
    img = Image.new("RGBA", (W, H), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    top_col = (14, 26, 56, 255)       # Deep twilight navy
    mid_col = (38, 74, 128, 255)      # Royal winter dusk blue
    bot_col = (115, 172, 222, 255)    # Frosty cyan horizon
    for y in range(H):
        if y < H * 0.55:
            t = y / (H * 0.55)
            col = lerp_color(top_col, mid_col, t)
        else:
            t = (y - H * 0.55) / (H * 0.45)
            col = lerp_color(mid_col, bot_col, t)
        draw.line([(0, y), (W, y)], fill=col)

    # Distant glowing moon
    draw.ellipse([780, 50, 890, 160], fill=(255, 255, 235, 240))
    draw.ellipse([770, 40, 900, 170], outline=(255, 255, 255, 45), width=6)
    # Subtle sparkling stars in sky
    import random
    random.seed(101)
    for _ in range(80):
        sx = random.randint(0, W)
        sy = random.randint(10, int(H * 0.6))
        srad = random.choice([1, 1, 2])
        draw.ellipse([sx - srad, sy - srad, sx + srad, sy + srad], fill=(255, 255, 255, random.randint(120, 220)))

    img.save(os.path.join(OUTPUT_SPRITES, "bg_sky.png"), "PNG")

    # 2. Distant Snow Mountains (seamless horizontally, transparent top)
    img = Image.new("RGBA", (W, H), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    # Ensure start (x=0) and end (x=W) heights match exactly for seamless tiling: y=280
    mountain_pts = [
        (0, H), (0, 280), (140, 150), (280, 260), (450, 100),
        (620, 230), (780, 130), (920, 250), (1024, 280), (1024, H)
    ]
    draw.polygon(mountain_pts, fill=(75, 110, 165, 255))

    # Snow caps on the mountain peaks
    snow_caps = [
        [(140, 150), (90, 205), (140, 195), (190, 205)],
        [(450, 100), (380, 175), (450, 160), (520, 175)],
        [(780, 130), (720, 190), (780, 180), (840, 190)]
    ]
    for sc in snow_caps:
        draw.polygon(sc, fill=(235, 248, 255, 240))

    img.save(os.path.join(OUTPUT_SPRITES, "bg_mountains.png"), "PNG")

    # 3. Middle Snowy Pine Hills (seamless horizontally, transparent top)
    img = Image.new("RGBA", (W, H), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    hill_pts = [(0, H)]
    for x in range(0, W + 1, 16):
        # 2 full sine waves so left and right match perfectly
        y = int(340 + 35 * math.sin(x * 4 * math.pi / W))
        hill_pts.append((x, y))
    hill_pts.append((W, H))
    draw.polygon(hill_pts, fill=(105, 155, 198, 255))

    # Row of frosted pine trees along the hill crest
    for x in range(16, W, 48):
        by = int(340 + 35 * math.sin(x * 4 * math.pi / W))
        # Pine triangle
        draw.polygon([(x - 22, by), (x, by - 65), (x + 22, by)], fill=(45, 95, 125, 255))
        # Snow cap
        draw.polygon([(x - 12, by - 30), (x, by - 65), (x + 12, by - 30)], fill=(225, 245, 255, 255))

    img.save(os.path.join(OUTPUT_SPRITES, "bg_pines.png"), "PNG")

    # 4. Snowflake particle
    img = Image.new("RGBA", (32, 32), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    cx, cy = 16, 16
    for i in range(6):
        ang = i * math.pi / 3
        x2 = cx + 12 * math.cos(ang)
        y2 = cy + 12 * math.sin(ang)
        draw.line([(cx, cy), (x2, y2)], fill=(255, 255, 255, 240), width=2)
        # Branches
        bx = cx + 8 * math.cos(ang)
        by = cy + 8 * math.sin(ang)
        for offset in [-0.5, 0.5]:
            bx2 = bx + 4 * math.cos(ang + offset)
            by2 = by + 4 * math.sin(ang + offset)
            draw.line([(bx, by), (bx2, by2)], fill=(255, 255, 255, 220), width=1)
    img.save(os.path.join(OUTPUT_SPRITES, "particle_snowflake.png"), "PNG")
    print("Generated background layers!")

# -------------------------------------------------------------
# 7. UI ELEMENTS, BUTTONS, BADGES, AND TITLE LOGO
# -------------------------------------------------------------
def generate_ui():
    # 1. UI Buttons (Green Play, Blue Action, Red/Orange Back, Yellow Special)
    buttons = [
        ("ui_btn_green", (40, 180, 90, 255), (70, 220, 120, 255), (20, 120, 60, 255)),
        ("ui_btn_blue", (35, 135, 230, 255), (80, 185, 255, 255), (15, 85, 175, 255)),
        ("ui_btn_yellow", (245, 175, 20, 255), (255, 220, 70, 255), (180, 120, 10, 255)),
        ("ui_btn_red", (230, 60, 60, 255), (255, 110, 110, 255), (160, 30, 30, 255)),
        ("ui_btn_wood", (150, 105, 65, 255), (190, 140, 90, 255), (100, 65, 35, 255)),
    ]
    for name, base, light, dark in buttons:
        img = Image.new("RGBA", (192, 64), (0, 0, 0, 0))
        draw = ImageDraw.Draw(img)
        # Outer drop shadow
        draw.rounded_rectangle([4, 8, 188, 62], radius=16, fill=(0, 0, 0, 60))
        # Dark border
        draw.rounded_rectangle([4, 4, 188, 58], radius=16, fill=dark)
        # Main base
        draw.rounded_rectangle([6, 6, 186, 54], radius=14, fill=base)
        # Highlight top shine
        draw.rounded_rectangle([8, 8, 184, 30], radius=12, fill=light)
        img.save(os.path.join(OUTPUT_SPRITES, f"{name}.png"), "PNG")

    # 2. Vibrant 3D Level Card (180x104)
    w_card, h_card = 180, 104
    img = Image.new("RGBA", (w_card, h_card), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    # Shadow
    draw.rounded_rectangle([4, 6, w_card-4, h_card-2], radius=18, fill=(0, 25, 55, 90))
    # 3D bottom bevel
    draw.rounded_rectangle([3, 4, w_card-3, h_card-4], radius=18, fill=(16, 75, 150, 255))
    # Vibrant Blue Face
    draw.rounded_rectangle([3, 3, w_card-3, h_card-10], radius=18, fill=(35, 140, 230, 255), outline=(145, 220, 255, 255), width=3)
    # Top glossy shine
    draw.rounded_rectangle([7, 6, w_card-7, h_card//2 - 4], radius=14, fill=(115, 200, 255, 145))
    # Inner bottom tray for coins
    draw.rounded_rectangle([14, h_card-35, w_card-14, h_card-14], radius=10, fill=(12, 60, 125, 200), outline=(50, 150, 240, 180), width=1)
    img.save(os.path.join(OUTPUT_SPRITES, "ui_level_card.png"), "PNG")

    # 3. Frosted Twilight Level Card Locked (180x104)
    img = Image.new("RGBA", (w_card, h_card), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    draw.rounded_rectangle([4, 6, w_card-4, h_card-2], radius=18, fill=(0, 20, 40, 75))
    draw.rounded_rectangle([3, 4, w_card-3, h_card-4], radius=18, fill=(38, 58, 82, 255))
    draw.rounded_rectangle([3, 3, w_card-3, h_card-10], radius=18, fill=(72, 102, 136, 255), outline=(125, 160, 195, 255), width=2)
    draw.rounded_rectangle([6, 6, w_card-6, h_card-13], radius=15, fill=(100, 128, 158, 255))
    draw.rounded_rectangle([8, 8, w_card-8, h_card//2 - 4], radius=12, fill=(165, 190, 218, 95))
    img.save(os.path.join(OUTPUT_SPRITES, "ui_level_card_locked.png"), "PNG")

    # 4. Lock Icon (64x64)
    img = Image.new("RGBA", (64, 64), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    # Shackle
    draw.arc([18, 8, 46, 36], start=180, end=360, fill=(160, 175, 195, 255), width=6)
    # Padlock body
    draw.rounded_rectangle([14, 26, 50, 56], radius=6, fill=(245, 180, 30, 255), outline=(190, 130, 10, 255), width=2)
    # Keyhole
    draw.ellipse([28, 34, 36, 42], fill=(60, 40, 10, 255))
    draw.polygon([(30, 40), (34, 40), (36, 50), (28, 50)], fill=(60, 40, 10, 255))
    img.save(os.path.join(OUTPUT_SPRITES, "ui_lock.png"), "PNG")

    # 5. Star Icon (64x64)
    img = Image.new("RGBA", (64, 64), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    star_pts = []
    cx, cy, r_out, r_in = 32, 32, 26, 11
    for i in range(10):
        angle = i * math.pi / 5 - math.pi / 2
        r = r_out if i % 2 == 0 else r_in
        star_pts.append((cx + r * math.cos(angle), cy + r * math.sin(angle)))
    draw.polygon(star_pts, fill=(255, 215, 0, 255), outline=(215, 150, 10, 255), width=2)
    # Highlight
    draw.polygon([(cx, cy - r_out), (star_pts[1][0], star_pts[1][1]), (cx, cy)], fill=(255, 255, 180, 255))
    img.save(os.path.join(OUTPUT_SPRITES, "ui_star.png"), "PNG")

    # 6. Touch Controls (Round Left, Right, Jump buttons: 96x96)
    for arrow, ang_deg in [("left", 180), ("right", 0), ("jump", 270)]:
        img = Image.new("RGBA", (96, 96), (0, 0, 0, 0))
        draw = ImageDraw.Draw(img)
        # Glass ring
        draw.ellipse([4, 6, 92, 94], fill=(0, 20, 50, 60))
        draw.ellipse([4, 4, 92, 92], fill=(40, 140, 230, 200), outline=(220, 245, 255, 240), width=4)
        draw.ellipse([8, 8, 88, 88], outline=(255, 255, 255, 120), width=2)
        # Arrow icon
        cx, cy = 48, 48
        ang = math.radians(ang_deg)
        cos_a, sin_a = math.cos(ang), math.sin(ang)
        def rotate(x, y):
            dx, dy = x - 48, y - 48
            return (cx + dx * cos_a - dy * sin_a, cy + dx * sin_a + dy * cos_a)
        # Base arrow points pointing right (0 deg)
        base_pts = [(36, 32), (58, 48), (36, 64), (42, 48)]
        pts = [rotate(px, py) for px, py in base_pts]
        draw.polygon(pts, fill=(255, 255, 255, 245))
        img.save(os.path.join(OUTPUT_SPRITES, f"ui_touch_{arrow}.png"), "PNG")

    # 7. Frosted Dialog Panel (512x384)
    img = Image.new("RGBA", (512, 384), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    # Outer drop shadow
    draw.rounded_rectangle([10, 14, 502, 378], radius=24, fill=(0, 15, 35, 120))
    # Outer icy blue frame
    draw.rounded_rectangle([6, 6, 506, 374], radius=24, fill=(50, 110, 175, 255), outline=(210, 240, 255, 255), width=4)
    # Frosted glass inner
    draw.rounded_rectangle([14, 14, 498, 366], radius=20, fill=(230, 242, 255, 245))
    # Header ribbon bar
    draw.rounded_rectangle([20, 20, 492, 70], radius=14, fill=(40, 130, 210, 255), outline=(255, 255, 255, 200), width=2)
    # Corner snowflake decorations
    for cx, cy in [(40, 45), (472, 45)]:
        draw.ellipse([cx-8, cy-8, cx+8, cy+8], fill=(255, 255, 255, 220))
    img.save(os.path.join(OUTPUT_SPRITES, "ui_dialog_panel.png"), "PNG")

    # 8. Title Banner Logo (600x160)
    img = Image.new("RGBA", (600, 160), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    # Big glowing icy title banner
    draw.rounded_rectangle([10, 16, 590, 148], radius=28, fill=(0, 25, 60, 140))
    draw.rounded_rectangle([6, 8, 594, 142], radius=28, fill=(30, 90, 160, 255), outline=(220, 245, 255, 255), width=5)
    draw.rounded_rectangle([12, 14, 588, 136], radius=24, fill=(50, 135, 215, 255))
    # Top frost shine
    draw.rounded_rectangle([16, 18, 584, 60], radius=16, fill=(120, 190, 255, 180))
    # Snow drippings from the top banner
    for x in range(30, 570, 45):
        draw.ellipse([x - 12, 10, x + 12, 38], fill=(255, 255, 255, 255))

    # Text drawing fallback using default font or drawn shapes
    try:
        font_text = ImageFont.truetype("arialbd.ttf", 37)
        font_2 = ImageFont.truetype("arialbd.ttf", 55)
    except Exception:
        font_text = font_2 = ImageFont.load_default()

    bbox1 = font_text.getbbox("SNOWBALL ADVENTURE")
    w1 = bbox1[2] - bbox1[0]
    bbox2 = font_2.getbbox("2")
    w2 = bbox2[2] - bbox2[0]

    gap = 18
    total_w = w1 + gap + w2
    start_x = (600 - total_w) // 2 - 8
    y_text = 52

    # Draw "SNOWBALL ADVENTURE" text with 3D drop shadow
    text1 = "SNOWBALL ADVENTURE"
    draw.text((start_x + 3, y_text + 4), text1, font=font_text, fill=(10, 35, 75, 255))
    draw.text((start_x, y_text), text1, font=font_text, fill=(255, 255, 255, 255))

    # Draw giant golden "2" with star
    x2 = start_x + w1 + gap
    y2 = y_text - 10
    draw.text((x2 + 3, y2 + 4), "2", font=font_2, fill=(180, 100, 10, 255))
    draw.text((x2, y2), "2", font=font_2, fill=(255, 215, 0, 255))

    # Sparkling star next to "2"
    cx, cy = x2 + w2 + 14, y2 + 13
    draw.line([(cx-8, cy), (cx+8, cy)], fill=(255, 255, 255, 255), width=3)
    draw.line([(cx, cy-8), (cx, cy+8)], fill=(255, 255, 255, 255), width=3)
    draw.ellipse([cx-3, cy-3, cx+3, cy+3], fill=(255, 255, 255, 255))

    img.save(os.path.join(OUTPUT_SPRITES, "logo_title.png"), "PNG")
    print("Generated all UI sprites!")

# -------------------------------------------------------------
# 8. SOUND EFFECTS GENERATOR (44.1 kHz WAV files)
# -------------------------------------------------------------
def generate_sfx():
    SAMPLE_RATE = 44100

    def write_wav(filename, samples):
        filepath = os.path.join(OUTPUT_AUDIO, filename)
        with wave.open(filepath, "w") as wav_file:
            wav_file.setnchannels(1)       # Mono
            wav_file.setsampwidth(2)      # 16-bit
            wav_file.setframerate(SAMPLE_RATE)
            packed = bytearray()
            for s in samples:
                val = int(max(-1.0, min(1.0, s)) * 32767.0)
                packed.extend(struct.pack("<h", val))
            wav_file.writeframes(packed)
        print(f"Generated audio: {filepath}")

    # 1. SFX Jump (Upward bouncy pitch sweep)
    dur = 0.22
    total_samples = int(SAMPLE_RATE * dur)
    samples = []
    for i in range(total_samples):
        t = i / SAMPLE_RATE
        env = math.exp(-t * 9.0)
        freq = 240.0 + 400.0 * (t / dur)
        s = 0.6 * math.sin(2 * math.pi * freq * t) * env
        samples.append(s)
    write_wav("sfx_jump.wav", samples)

    # 2. SFX Coin (High double chime: B5 to E6)
    dur = 0.35
    total_samples = int(SAMPLE_RATE * dur)
    samples = []
    for i in range(total_samples):
        t = i / SAMPLE_RATE
        if t < 0.12:
            freq = 987.77 # B5
            env = math.exp(-t * 18.0)
            s = 0.5 * math.sin(2 * math.pi * freq * t) * env
        else:
            t2 = t - 0.12
            freq = 1318.51 # E6
            env = math.exp(-t2 * 12.0)
            # Add harmonic shine
            s = 0.5 * math.sin(2 * math.pi * freq * t2) * env + 0.2 * math.sin(4 * math.pi * freq * t2) * env
        samples.append(s)
    write_wav("sfx_coin.wav", samples)

    # 3. SFX Spring Bounce (Super cartoon spring boing)
    dur = 0.45
    total_samples = int(SAMPLE_RATE * dur)
    samples = []
    for i in range(total_samples):
        t = i / SAMPLE_RATE
        env = math.exp(-t * 6.0)
        # Modulated spring frequency
        freq = 300.0 + 250.0 * math.sin(2 * math.pi * 18.0 * t) + 150.0 * (t / dur)
        s = 0.7 * math.sin(2 * math.pi * freq * t) * env
        samples.append(s)
    write_wav("sfx_spring.wav", samples)

    # 4. SFX Crumble / Ice Crack (Crunchy noise burst)
    import random
    dur = 0.3
    total_samples = int(SAMPLE_RATE * dur)
    samples = []
    random.seed(42)
    for i in range(total_samples):
        t = i / SAMPLE_RATE
        env = math.exp(-t * 12.0)
        noise = (random.random() * 2.0 - 1.0)
        s = (0.4 * noise + 0.3 * math.sin(2 * math.pi * 320.0 * t)) * env
        samples.append(s)
    write_wav("sfx_crumble.wav", samples)

    # 5. SFX Pop / Hazard hit (Soft snow puff / balloon pop)
    dur = 0.25
    total_samples = int(SAMPLE_RATE * dur)
    samples = []
    for i in range(total_samples):
        t = i / SAMPLE_RATE
        env = math.exp(-t * 20.0)
        freq = 480.0 * math.exp(-t * 15.0)
        s = 0.8 * math.sin(2 * math.pi * freq * t) * env
        samples.append(s)
    write_wav("sfx_pop.wav", samples)

    # 6. SFX Checkpoint (Triumphant 2-tone chime)
    dur = 0.4
    total_samples = int(SAMPLE_RATE * dur)
    samples = []
    for i in range(total_samples):
        t = i / SAMPLE_RATE
        if t < 0.18:
            freq = 659.25 # E5
            env = math.exp(-t * 10.0)
            s = 0.5 * math.sin(2 * math.pi * freq * t) * env
        else:
            t2 = t - 0.18
            freq = 880.0 # A5
            env = math.exp(-t2 * 8.0)
            s = 0.5 * math.sin(2 * math.pi * freq * t2) * env
        samples.append(s)
    write_wav("sfx_checkpoint.wav", samples)

    # 7. SFX Victory Fanfare (Bright arpeggio)
    dur = 0.8
    total_samples = int(SAMPLE_RATE * dur)
    samples = []
    notes = [523.25, 659.25, 783.99, 1046.50] # C5, E5, G5, C6
    for i in range(total_samples):
        t = i / SAMPLE_RATE
        idx = min(3, int(t / 0.18))
        freq = notes[idx]
        note_t = t - idx * 0.18
        env = math.exp(-note_t * 6.0)
        s = 0.5 * math.sin(2 * math.pi * freq * note_t) * env
        samples.append(s)
    write_wav("sfx_victory.wav", samples)

    # 8. SFX Button Click
    dur = 0.08
    total_samples = int(SAMPLE_RATE * dur)
    samples = []
    for i in range(total_samples):
        t = i / SAMPLE_RATE
        env = math.exp(-t * 50.0)
        s = 0.6 * math.sin(2 * math.pi * 900.0 * t) * env
        samples.append(s)
    write_wav("sfx_click.wav", samples)

if __name__ == "__main__":
    print("=== Starting Snowball Adventure 2 Asset Generation ===")
    generate_characters()
    generate_coin()
    generate_obstacles()
    generate_terrain()
    generate_goals()
    generate_backgrounds()
    generate_ui()
    generate_sfx()

    # Automatically synchronize all sprites and audio to Assets/Resources
    import shutil
    res_sprites = r"D:\Games\Snowball Adventure 2\Assets\Resources\Sprites"
    res_audio = r"D:\Games\Snowball Adventure 2\Assets\Resources\Audio"
    os.makedirs(res_sprites, exist_ok=True)
    os.makedirs(res_audio, exist_ok=True)
    for f in os.listdir(OUTPUT_SPRITES):
        if f.endswith(".png"):
            shutil.copy2(os.path.join(OUTPUT_SPRITES, f), os.path.join(res_sprites, f))
    for f in os.listdir(OUTPUT_AUDIO):
        if f.endswith(".wav"):
            shutil.copy2(os.path.join(OUTPUT_AUDIO, f), os.path.join(res_audio, f))
    print("=== Synced all assets to Assets/Resources! ===")
    print("=== All Assets Successfully Generated! ===")
