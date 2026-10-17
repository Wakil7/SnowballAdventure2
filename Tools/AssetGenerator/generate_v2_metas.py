import os
import uuid

SPRITES_DIR = r"d:\Games\Snowball Adventure 2\Assets\Resources\Sprites"
TEMPLATE_META = r"d:\Games\Snowball Adventure 2\Assets\Resources\Sprites\ui_btn_blue.png.meta"

with open(TEMPLATE_META, "r", encoding="utf-8") as f:
    template_lines = f.readlines()

new_sprites = [
    "ui_pedestal.png",
    "ui_aura_ring.png",
    "ui_world_tab_active.png",
    "ui_world_tab_inactive.png",
    "ui_card_glass_unlocked.png",
    "ui_card_glass_locked.png",
    "ui_dock_card_normal.png",
    "ui_dock_card_selected.png",
    "ui_stat_bar_bg.png",
    "ui_stat_bar_fill.png"
]

for spr in new_sprites:
    meta_path = os.path.join(SPRITES_DIR, f"{spr}.meta")
    if not os.path.exists(meta_path):
        guid = uuid.uuid4().hex
        lines = []
        for line in template_lines:
            if line.startswith("guid:"):
                lines.append(f"guid: {guid}\n")
            else:
                lines.append(line)
        with open(meta_path, "w", encoding="utf-8") as out:
            out.writelines(lines)
        print(f"Created meta for {spr} with guid {guid}")

print("Meta creation complete!")
