import os
import uuid
from PIL import Image, ImageDraw

OUTPUT_DIR = r"d:\Games\Snowball Adventure 2\Assets\Resources\Sprites"
TEMPLATE_META = r"d:\Games\Snowball Adventure 2\Assets\Resources\Sprites\ui_btn_blue.png.meta"

w, h = 360, 76
img = Image.new("RGBA", (w, h), (0, 0, 0, 0))
draw = ImageDraw.Draw(img)

# Outer drop shadow
draw.rounded_rectangle([4, 7, w - 4, h - 2], radius=22, fill=(0, 20, 50, 80))

# Outer icy blue glow border
draw.rounded_rectangle([3, 4, w - 3, h - 4], radius=22, fill=(35, 135, 230, 200), outline=(220, 245, 255, 240), width=4)

# Inner frosted glass body
draw.rounded_rectangle([7, 8, w - 7, h - 8], radius=18, fill=(45, 150, 240, 220))

# Top glossy highlight
draw.rounded_rectangle([10, 10, w - 10, h // 2], radius=14, fill=(130, 215, 255, 130))

# Inner subtle border
draw.rounded_rectangle([8, 9, w - 8, h - 9], radius=17, outline=(255, 255, 255, 100), width=2)

# Save sprite
spr_path = os.path.join(OUTPUT_DIR, "ui_touch_spacebar.png")
img.save(spr_path, "PNG")

# Create .meta file
meta_path = os.path.join(OUTPUT_DIR, "ui_touch_spacebar.png.meta")
if not os.path.exists(meta_path):
    with open(TEMPLATE_META, "r", encoding="utf-8") as f:
        template_lines = f.readlines()
    guid = uuid.uuid4().hex
    lines = []
    for line in template_lines:
        if line.startswith("guid:"):
            lines.append(f"guid: {guid}\n")
        else:
            lines.append(line)
    with open(meta_path, "w", encoding="utf-8") as out:
        out.writelines(lines)

print("Generated ui_touch_spacebar.png and meta successfully!")
