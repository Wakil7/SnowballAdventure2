import uuid
from PIL import Image

src = r"C:\Users\HP2076TU\.gemini\antigravity-ide\brain\18a764b5-f3e5-486b-b6c4-25448a5b3ff8\main_menu_bg_1792177878403.jpg"
dest1 = r"d:\Games\Snowball Adventure 2\Assets\Resources\Sprites\bg_menu_aurora.png"
dest2 = r"d:\Games\Snowball Adventure 2\Assets\Sprites\bg_menu_aurora.png"

# Convert to PNG for crisp alpha / quality
im = Image.open(src)
im.save(dest1, "PNG")
im.save(dest2, "PNG")

with open(r"d:\Games\Snowball Adventure 2\Assets\Resources\Sprites\bg_aurora_sky.png.meta", "r") as f:
    template = f.read()

def write_meta(meta_path, guid):
    import re
    meta_content = re.sub(r"guid:\s*[0-9a-fA-F]+", f"guid: {guid}", template)
    with open(meta_path, "w") as f:
        f.write(meta_content)

write_meta(dest1 + ".meta", uuid.uuid4().hex)
write_meta(dest2 + ".meta", uuid.uuid4().hex)
print("Successfully generated bg_menu_aurora.png and .meta files!")
