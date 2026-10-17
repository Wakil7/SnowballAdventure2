using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SnowballAdventure
{
    [Serializable]
    public class CharacterDef
    {
        public int id;
        public string name;
        public string title;
        public int unlockLevel; // Character unlocked after reaching this level
        public string spriteResource;
        public string description;
    }

    [Serializable]
    public class PlayerProgressData
    {
        public int selectedCharacter = 0;
        public int selectedLevel = 1;
        public int maxUnlockedLevel = 1;
        public int[] levelCoins = new int[31]; // 1-indexed up to level 30
        public bool isMusicEnabled = true;
        public bool isSfxEnabled = true;

        public PlayerProgressData()
        {
            selectedCharacter = 0;
            selectedLevel = 1;
            maxUnlockedLevel = 1;
            levelCoins = new int[31];
            isMusicEnabled = true;
            isSfxEnabled = true;
        }
    }

    public static class GameDataManager
    {
        public const int TOTAL_LEVELS = 30;
        public const int TOTAL_CHARACTERS = 10;
        public const int COINS_PER_LEVEL = 30;

        private static PlayerProgressData _data;
        private static string SaveFilePath => Path.Combine(Application.persistentDataPath, "snowball_adv2_save.json");

        public static readonly CharacterDef[] Characters = new CharacterDef[]
        {
            new CharacterDef { id = 0, name = "Frosty", title = "Classic Snow", unlockLevel = 1, spriteResource = "Sprites/char_0_frosty", description = "The cheerful original adventurer! Well-balanced and loves rolling on fresh powder." },
            new CharacterDef { id = 1, name = "Rocky", title = "Pebble Snow", unlockLevel = 4, spriteResource = "Sprites/char_1_pebble", description = "Embedded with frosty river stones. Solid and determined! (Unlocked after Level 3)" },
            new CharacterDef { id = 2, name = "Minty", title = "Mint Swirl", unlockLevel = 7, spriteResource = "Sprites/char_2_minty", description = "Fresh winter mint flavor with sweet candy swirls. (Unlocked after Level 6)" },
            new CharacterDef { id = 3, name = "Glacier", title = "Ice Crystal", unlockLevel = 10, spriteResource = "Sprites/char_3_glacier", description = "Carved from ancient arctic glacier ice with dazzling reflections. (Unlocked after Level 9)" },
            new CharacterDef { id = 4, name = "Berry", title = "Strawberry Sorbet", unlockLevel = 13, spriteResource = "Sprites/char_4_berry", description = "Sweet pink snow sprinkled with winter berries. (Unlocked after Level 12)" },
            new CharacterDef { id = 5, name = "Midas", title = "Golden Nugget", unlockLevel = 16, spriteResource = "Sprites/char_5_midas", description = "A regal snowball dipped in pure shimmering gold! (Unlocked after Level 15)" },
            new CharacterDef { id = 6, name = "Aurora", title = "Polar Star", unlockLevel = 19, spriteResource = "Sprites/char_6_aurora", description = "Infused with the mystical glow of the Northern Lights. (Unlocked after Level 18)" },
            new CharacterDef { id = 7, name = "Blaze", title = "Inferno Spark", unlockLevel = 22, spriteResource = "Sprites/char_7_blaze", description = "Holds a warm hearth spark within, melting obstacles away! (Unlocked after Level 21)" },
            new CharacterDef { id = 8, name = "Cyber", title = "Cyber Neon", unlockLevel = 25, spriteResource = "Sprites/char_8_cyber", description = "High-tech synthwave snowball with a digital HUD visor. (Unlocked after Level 24)" },
            new CharacterDef { id = 9, name = "Cosmo", title = "Cosmic Nebula", unlockLevel = 28, spriteResource = "Sprites/char_9_cosmo", description = "A deep-space snowball surrounded by a miniature orbiting moon. (Unlocked after Level 27)" },
        };

        public static PlayerProgressData Data
        {
            get
            {
                if (_data == null) Load();
                return _data;
            }
        }

        public static void Load()
        {
            try
            {
                if (File.Exists(SaveFilePath))
                {
                    string json = File.ReadAllText(SaveFilePath);
                    _data = JsonUtility.FromJson<PlayerProgressData>(json);
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Failed to load save file, creating new data: " + ex.Message);
            }

            if (_data == null)
            {
                _data = new PlayerProgressData();
            }

            if (_data.levelCoins == null || _data.levelCoins.Length < 31)
            {
                int[] oldCoins = _data.levelCoins;
                _data.levelCoins = new int[31];
                if (oldCoins != null)
                {
                    Array.Copy(oldCoins, _data.levelCoins, Math.Min(oldCoins.Length, 31));
                }
            }
        }

        public static void Save()
        {
            try
            {
                if (_data == null) _data = new PlayerProgressData();
                string json = JsonUtility.ToJson(_data, true);
                File.WriteAllText(SaveFilePath, json);
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to save player progress: " + ex.Message);
            }
        }

        public static bool IsCharacterUnlocked(int charIndex)
        {
            if (charIndex < 0 || charIndex >= Characters.Length) return false;
            return Data.maxUnlockedLevel >= Characters[charIndex].unlockLevel;
        }

        public static bool IsLevelUnlocked(int level)
        {
            if (level <= 1) return true;
            if (level > TOTAL_LEVELS) return false;
            return level <= Data.maxUnlockedLevel;
        }

        public static CharacterDef GetCharacterUnlockedByLevel(int level)
        {
            int targetUnlockLevel = level + 1;
            for (int i = 0; i < Characters.Length; i++)
            {
                if (Characters[i].unlockLevel == targetUnlockLevel)
                {
                    return Characters[i];
                }
            }
            return null;
        }

        public static void CompleteLevel(int level, int coinsCollected)
        {
            if (level >= 1 && level <= TOTAL_LEVELS)
            {
                if (coinsCollected > Data.levelCoins[level])
                {
                    Data.levelCoins[level] = coinsCollected;
                }

                if (level == Data.maxUnlockedLevel && level < TOTAL_LEVELS)
                {
                    Data.maxUnlockedLevel = level + 1;
                }

                Save();
            }
        }

        public static Sprite GetActiveCharacterSprite()
        {
            int index = Mathf.Clamp(Data.selectedCharacter, 0, Characters.Length - 1);
            string res = Characters[index].spriteResource;
            Sprite s = Resources.Load<Sprite>(res);
            if (s == null)
            {
                s = Resources.Load<Sprite>("Sprites/char_0_frosty");
            }
            return s;
        }
    }
}
