import os
import math
from PIL import Image, ImageDraw, ImageFilter

OUTPUT_DIR = r"d:\Games\Snowball Adventure 2\Assets\Resources\Sprites"
os.makedirs(OUTPUT_DIR, exist_ok=True)

def create_pedestal():
    # 256x96 Glowing Crystalline Frost Pedestal
    w, h = 256, 96
    img = Image.new("RGBA", (w, h), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)

    # Ambient drop shadow / glow underneath
    shadow = Image.new("RGBA", (w, h), (0, 0, 0, 0))
    sdraw = ImageDraw.Draw(shadow)
    sdraw.ellipse([20, 48, w - 20, h - 8], fill=(0, 20, 50, 160))
    # Soft cyan ground glow
    sdraw.ellipse([30, 42, w - 30, h - 14], fill=(0, 180, 255, 90))
    shadow = shadow.filter(ImageFilter.GaussianBlur(8))
    img.alpha_composite(shadow)

    # Lower base tier
    draw.ellipse([24, 38, w - 24, h - 18], fill=(15, 45, 95, 230), outline=(50, 140, 220, 220), width=3)
    # Middle icy layer
    draw.ellipse([28, 28, w - 28, h - 28], fill=(25, 75, 140, 240), outline=(100, 200, 255, 240), width=3)
    # Upper reflective platform
    draw.ellipse([32, 16, w - 32, h - 42], fill=(45, 125, 205, 250), outline=(180, 235, 255, 255), width=4)
    # Top frosted ice surface
    draw.ellipse([38, 20, w - 38, h - 48], fill=(130, 210, 255, 240))
    # Core spotlight highlight
    draw.ellipse([55, 24, w - 55, h - 54], fill=(225, 248, 255, 220))

    img.save(os.path.join(OUTPUT_DIR, "ui_pedestal.png"), "PNG")

def create_aura_ring():
    # 256x256 Soft Celestial / Frost Aura Glow Ring
    w, h = 256, 256
    img = Image.new("RGBA", (w, h), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)

    # Outer aura pulses
    for r in range(120, 40, -10):
        alpha = int(35 * (1.0 - (r - 40) / 80.0))
        draw.ellipse([128 - r, 128 - r, 128 + r, 128 + r], fill=(40, 170, 255, alpha))

    # Inner bright ring
    ring_img = Image.new("RGBA", (w, h), (0, 0, 0, 0))
    rdraw = ImageDraw.Draw(ring_img)
    rdraw.ellipse([48, 48, 208, 208], outline=(140, 230, 255, 180), width=8)
    rdraw.ellipse([54, 54, 202, 202], outline=(255, 255, 255, 220), width=4)
    ring_img = ring_img.filter(ImageFilter.GaussianBlur(3))
    img.alpha_composite(ring_img)

    img.save(os.path.join(OUTPUT_DIR, "ui_aura_ring.png"), "PNG")

def create_world_tabs():
    w, h = 320, 68
    # 1. Active Tab: Vibrant Aurora Gradient with Glowing Bevel
    img_act = Image.new("RGBA", (w, h), (0, 0, 0, 0))
    draw_act = ImageDraw.Draw(img_act)
    # Shadow
    draw_act.rounded_rectangle([4, 8, w - 4, h - 2], radius=22, fill=(0, 20, 50, 120))
    # Glow Border
    draw_act.rounded_rectangle([2, 4, w - 2, h - 4], radius=22, fill=(20, 80, 150, 255), outline=(130, 230, 255, 255), width=3)
    # Vibrant Base
    draw_act.rounded_rectangle([5, 7, w - 5, h - 7], radius=19, fill=(30, 130, 225, 255))
    # Top Gloss
    draw_act.rounded_rectangle([8, 9, w - 8, h // 2], radius=16, fill=(110, 200, 255, 160))
    img_act.save(os.path.join(OUTPUT_DIR, "ui_world_tab_active.png"), "PNG")

    # 2. Inactive Tab: Sleek Translucent Dark Glass
    img_inact = Image.new("RGBA", (w, h), (0, 0, 0, 0))
    draw_inact = ImageDraw.Draw(img_inact)
    draw_inact.rounded_rectangle([4, 8, w - 4, h - 2], radius=22, fill=(0, 15, 35, 90))
    draw_inact.rounded_rectangle([2, 4, w - 2, h - 4], radius=22, fill=(15, 35, 65, 220), outline=(60, 110, 170, 180), width=2)
    draw_inact.rounded_rectangle([5, 7, w - 5, h - 7], radius=19, fill=(22, 50, 90, 200))
    draw_inact.rounded_rectangle([8, 9, w - 8, h // 2], radius=16, fill=(70, 130, 200, 70))
    img_inact.save(os.path.join(OUTPUT_DIR, "ui_world_tab_inactive.png"), "PNG")

def create_glass_level_cards():
    w, h = 220, 180
    # 1. Unlocked Glass Level Card
    img_unlocked = Image.new("RGBA", (w, h), (0, 0, 0, 0))
    draw_u = ImageDraw.Draw(img_unlocked)
    # Deep outer shadow
    draw_u.rounded_rectangle([4, 8, w - 4, h - 2], radius=24, fill=(0, 20, 45, 140))
    # Outer Bevel Rim
    draw_u.rounded_rectangle([2, 4, w - 2, h - 4], radius=24, fill=(18, 65, 125, 240), outline=(100, 205, 255, 240), width=3)
    # Dark Translucent Frosted Core
    draw_u.rounded_rectangle([6, 8, w - 6, h - 8], radius=20, fill=(14, 42, 82, 230))
    # Top Gloss Sheen
    draw_u.rounded_rectangle([10, 12, w - 10, h // 2 - 8], radius=16, fill=(70, 160, 235, 90))
    # Bottom Coin Tray Recess
    draw_u.rounded_rectangle([16, h - 48, w - 16, h - 14], radius=14, fill=(8, 25, 52, 240), outline=(40, 115, 195, 170), width=2)
    img_unlocked.save(os.path.join(OUTPUT_DIR, "ui_card_glass_unlocked.png"), "PNG")

    # 2. Locked Glass Level Card
    img_locked = Image.new("RGBA", (w, h), (0, 0, 0, 0))
    draw_l = ImageDraw.Draw(img_locked)
    draw_l.rounded_rectangle([4, 8, w - 4, h - 2], radius=24, fill=(0, 10, 25, 130))
    draw_l.rounded_rectangle([2, 4, w - 2, h - 4], radius=24, fill=(24, 38, 56, 230), outline=(60, 85, 115, 190), width=2)
    draw_l.rounded_rectangle([6, 8, w - 6, h - 8], radius=20, fill=(16, 26, 40, 225))
    draw_l.rounded_rectangle([10, 12, w - 10, h // 2 - 8], radius=16, fill=(45, 65, 90, 60))
    # Dimmed Bottom Tray
    draw_l.rounded_rectangle([16, h - 48, w - 16, h - 14], radius=14, fill=(10, 16, 26, 230), outline=(40, 58, 80, 140), width=1)
    img_locked.save(os.path.join(OUTPUT_DIR, "ui_card_glass_locked.png"), "PNG")

def create_dock_cards():
    w, h = 110, 130
    # 1. Normal Dock Card
    img_norm = Image.new("RGBA", (w, h), (0, 0, 0, 0))
    draw_n = ImageDraw.Draw(img_norm)
    draw_n.rounded_rectangle([3, 5, w - 3, h - 2], radius=18, fill=(0, 15, 35, 110))
    draw_n.rounded_rectangle([2, 3, w - 2, h - 4], radius=18, fill=(18, 48, 88, 230), outline=(70, 140, 215, 200), width=2)
    draw_n.rounded_rectangle([5, 6, w - 5, h - 7], radius=15, fill=(16, 40, 75, 220))
    draw_n.rounded_rectangle([8, 9, w - 8, 48], radius=12, fill=(60, 130, 210, 70))
    img_norm.save(os.path.join(OUTPUT_DIR, "ui_dock_card_normal.png"), "PNG")

    # 2. Selected / Equipped Dock Card (Glowing Gold & Cyan Gradient Accent)
    img_sel = Image.new("RGBA", (w, h), (0, 0, 0, 0))
    draw_s = ImageDraw.Draw(img_sel)
    # Bright Outer Glow
    draw_s.rounded_rectangle([1, 2, w - 1, h - 1], radius=20, fill=(20, 80, 150, 120))
    draw_s.rounded_rectangle([2, 3, w - 2, h - 4], radius=18, fill=(35, 120, 210, 255), outline=(255, 225, 75, 255), width=3)
    draw_s.rounded_rectangle([5, 6, w - 5, h - 7], radius=15, fill=(25, 95, 175, 240))
    draw_s.rounded_rectangle([8, 9, w - 8, 50], radius=12, fill=(130, 215, 255, 140))
    img_sel.save(os.path.join(OUTPUT_DIR, "ui_dock_card_selected.png"), "PNG")

def create_stat_bars():
    w, h = 240, 20
    # Stat Bar Track
    img_track = Image.new("RGBA", (w, h), (0, 0, 0, 0))
    draw_t = ImageDraw.Draw(img_track)
    draw_t.rounded_rectangle([0, 0, w, h], radius=10, fill=(8, 22, 44, 240), outline=(40, 95, 160, 200), width=2)
    img_track.save(os.path.join(OUTPUT_DIR, "ui_stat_bar_bg.png"), "PNG")

    # Stat Bar Fill (Vibrant Cyan-Yellow Gradient)
    img_fill = Image.new("RGBA", (w, h), (0, 0, 0, 0))
    draw_f = ImageDraw.Draw(img_fill)
    for x in range(w):
        t = x / float(w)
        r = int(30 * (1 - t) + 255 * t)
        g = int(210 * (1 - t) + 215 * t)
        b = int(255 * (1 - t) + 30 * t)
        draw_f.line([(x, 3), (x, h - 4)], fill=(r, g, b, 255))
    # Top highlight
    draw_f.rounded_rectangle([2, 2, w - 2, h // 2], radius=6, fill=(255, 255, 255, 110))
    img_fill.save(os.path.join(OUTPUT_DIR, "ui_stat_bar_fill.png"), "PNG")

if __name__ == "__main__":
    create_pedestal()
    create_aura_ring()
    create_world_tabs()
    create_glass_level_cards()
    create_dock_cards()
    create_stat_bars()
    print("All Modern Menu V2 UI assets generated successfully!")
