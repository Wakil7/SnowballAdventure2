import os
import shutil
import uuid
from PIL import Image

src_path = r"D:\Games\Snowball Adventure 2\Assets\Resources\Sprites\frost_conveyor.png"
dst_res_path = r"D:\Games\Snowball Adventure 2\Assets\Resources\Sprites\frost_conveyor_reverse.png"
dst_spr_path = r"D:\Games\Snowball Adventure 2\Assets\Sprites\frost_conveyor_reverse.png"

img = Image.open(src_path)
rev_img = img.transpose(Image.FLIP_LEFT_RIGHT)

rev_img.save(dst_res_path)
rev_img.save(dst_spr_path)

meta_src = src_path + ".meta"
with open(meta_src, "r") as f:
    meta_content = f.read()

# Replace guid with a new guid
new_guid1 = uuid.uuid4().hex
meta_dst1 = meta_content.replace("809902047542c4440a741c8597f11fc8", new_guid1)
with open(dst_res_path + ".meta", "w") as f:
    f.write(meta_dst1)

new_guid2 = uuid.uuid4().hex
meta_dst2 = meta_content.replace("809902047542c4440a741c8597f11fc8", new_guid2)
with open(dst_spr_path + ".meta", "w") as f:
    f.write(meta_dst2)

print("Generated frost_conveyor_reverse.png successfully!")
