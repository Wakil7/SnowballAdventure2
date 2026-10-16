using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SnowballAdventure.Editor
{
    public static class SnowballAdventureEditor
    {
        [MenuItem("Snowball Adventure 2/Scenes/Open Main Menu Scene", false, 1)]
        public static void OpenMainMenuScene()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene("Assets/Scenes/MainMenuScene.unity");
            }
        }

        [MenuItem("Snowball Adventure 2/Scenes/Open Game Scene (Level 1)", false, 2)]
        public static void OpenGameScene()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene("Assets/Scenes/GameScene.unity");
            }
        }

        [MenuItem("Snowball Adventure 2/Save Data/Reset Progress (Level 1 Default)", false, 20)]
        public static void ResetProgress()
        {
            string path = Path.Combine(Application.persistentDataPath, "snowball_adv2_save.json");
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            GameDataManager.Load();
            Debug.Log("<color=#40b0ff><b>[Snowball Adventure 2]</b> Progress reset to default (Level 1 unlocked, Character 0 equipped).</color>");
            EditorUtility.DisplayDialog("Save Data Reset", "Game progress has been reset to default state:\n• Level 1 Unlocked\n• Frosty Classic Equipped", "OK");
        }

        [MenuItem("Snowball Adventure 2/Save Data/Unlock All 30 Levels & 10 Characters (Dev Test)", false, 21)]
        public static void UnlockAllForTesting()
        {
            GameDataManager.Load();
            GameDataManager.Data.maxUnlockedLevel = 30;
            for (int i = 1; i <= 30; i++)
            {
                GameDataManager.Data.levelCoins[i] = 30;
            }
            GameDataManager.Save();
            Debug.Log("<color=#50e050><b>[Snowball Adventure 2]</b> All 30 levels and 10 characters unlocked for developer testing!</color>");
            EditorUtility.DisplayDialog("Dev Unlock Complete", "All 30 levels (with 30/30 coins) and all 10 snowball characters have been unlocked for testing!", "Awesome");
        }
    }
}
