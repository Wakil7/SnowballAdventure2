import os
import hashlib

def deterministic_guid(path):
    return hashlib.md5(path.replace("\\", "/").lower().encode("utf-8")).hexdigest()

SAMPLE_SCENE_PATH = r"D:\Games\Snowball Adventure 2\Assets\Scenes\SampleScene.unity"
with open(SAMPLE_SCENE_PATH, "r", encoding="utf-8") as f:
    sample_content = f.read()

# Split sample scene to keep header up to transforms
# GUID of MainMenuBuilder: db741f34afaff46aa02f3db4bc449af7
# GUID of Level1Builder: fd257039fbf703258186670cc7e06fe6

def make_scene(builder_name, script_guid, output_path):
    custom_obj = f"""--- !u!1 &900000001
GameObject:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  serializedVersion: 6
  m_Component:
  - component: {{fileID: 900000002}}
  - component: {{fileID: 900000003}}
  m_Layer: 0
  m_Name: {builder_name}
  m_TagString: Untagged
  m_Icon: {{fileID: 0}}
  m_NavMeshLayer: 0
  m_StaticEditorFlags: 0
  m_IsActive: 1
--- !u!4 &900000002
Transform:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  m_GameObject: {{fileID: 900000001}}
  m_LocalRotation: {{x: 0, y: 0, z: 0, w: 1}}
  m_LocalPosition: {{x: 0, y: 0, z: 0}}
  m_LocalScale: {{x: 1, y: 1, z: 1}}
  m_ConstrainProportionsScale: 0
  m_Children: []
  m_Father: {{fileID: 0}}
  m_RootOrder: 2
  m_LocalEulerAnglesHint: {{x: 0, y: 0, z: 0}}
--- !u!114 &900000003
MonoBehaviour:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  m_GameObject: {{fileID: 900000001}}
  m_Enabled: 1
  m_EditorHideFlags: 0
  m_Script: {{fileID: 11500000, guid: {script_guid}, type: 3}}
  m_Name: 
  m_EditorClassIdentifier: 
"""
    content = sample_content.strip() + "\n" + custom_obj
    with open(output_path, "w", encoding="utf-8") as f:
        f.write(content)

    # Make .meta file
    rel_path = os.path.relpath(output_path, r"D:\Games\Snowball Adventure 2").replace("\\", "/")
    guid = deterministic_guid(rel_path)
    meta_content = f"""fileFormatVersion: 2
guid: {guid}
DefaultImporter:
  externalObjects: {{}}
  userData: 
  assetBundleName: 
  assetBundleVariant: 
"""
    with open(output_path + ".meta", "w", encoding="utf-8") as f:
        f.write(meta_content)

make_scene("MainMenuController", "db741f34afaff46aa02f3db4bc449af7", r"D:\Games\Snowball Adventure 2\Assets\Scenes\MainMenuScene.unity")
make_scene("Level1Controller", "fd257039fbf703258186670cc7e06fe6", r"D:\Games\Snowball Adventure 2\Assets\Scenes\GameScene.unity")
print("Scenes successfully created!")
