using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SnowballAdventure
{
    public class MainMenuManager : MonoBehaviour
    {
        public static MainMenuManager Instance { get; private set; }

        [Header("Menu Panels")]
        public GameObject titlePanel;
        public GameObject levelSelectPanel;
        public GameObject characterSelectPanel;

        [Header("Level Select UI")]
        public Transform levelGridContainer;
        public GameObject levelButtonPrefab;
        public Text totalStarsCoinsText;
        public Text currentWorldTitleText;
        public Text worldPageIndicatorText;
        public Button prevWorldButton;
        public Button nextWorldButton;
        public int currentWorldTab = 0;

        [Header("Character Select UI")]
        public Transform characterListContainer;
        public GameObject characterCardPrefab;
        public Image activePreviewImage;
        public Text activeNameText;
        public Text activeDescText;
        public RectTransform heroStageBallRt;
        public RectTransform heroAuraRingRt;
        public Text heroTitleText;
        public Text heroSpecialPerkText;
        public RectTransform speedBarFill;
        public Text speedValText;
        public RectTransform controlBarFill;
        public Text controlValText;
        public RectTransform bounceBarFill;
        public Text bounceValText;
        public Button heroActionButton;
        public Image heroActionBtnImg;
        public Text heroActionBtnText;
        public Text unlockedCountText;
        public int inspectedCharIndex = 0;

        [Header("Play Flow State")]
        public bool isPlayFlow = false;
        public Text startButtonText;
        public Text characterPanelTitleText;
        public Image activeTitlePreviewImage;

        [Header("Audio Toggles")]
        public Image soundToggleIcon;

        [Header("Settings & Controls Popups")]
        public GameObject settingsPanel;
        public GameObject controlsPanel;
        public Text soundSettingText;
        public Image soundSettingBtnImg;
        public Text musicSettingText;
        public Image musicSettingBtnImg;

        // World Names (clean names without brackets)
        public static readonly string[] WorldNames = new string[]
        {
            "❄️ FROST PEAKS",
            "🏔️ GLACIAL ABYSS",
            "✨ COSMIC SUMMIT"
        };

        // Level Names for 30 levels
        public static readonly string[] LevelNames = new string[]
        {
            "Frosty Foothills", "Snowdrift Slopes", "Glacier Run", "Pebble Pathway", "Icicle Gorge",
            "Crystal Springs", "Minty Ridge", "Frozen Lake", "Blizzard Peak", "Summit Gateway",
            "Sub-Zero Chasm", "Avalanche Basin", "Berry Orchard", "Ice Floe Canal", "Echoing Caverns",
            "Midas Vault", "Permafrost Pass", "Shattered Peaks", "Northern Aurora", "Glacial Heart",
            "Celestial Ascent", "Blazing Hearth", "Solar Flare Gap", "Cosmic Gateway", "Cyber Grid",
            "Neon Spire", "Superluminal Run", "Orbital Relay", "Supernova Ridge", "Infinity Apex"
        };

        public struct CharacterStats
        {
            public float speed;
            public float control;
            public float bounce;
            public string perk;
        }

        public static CharacterStats GetStats(int id)
        {
            switch (id)
            {
                case 0: return new CharacterStats { speed = 7.5f, control = 8.0f, bounce = 7.0f, perk = "❄️ Fresh Powder Glide" };
                case 1: return new CharacterStats { speed = 6.5f, control = 9.5f, bounce = 5.5f, perk = "🪨 Heavy Stone Core" };
                case 2: return new CharacterStats { speed = 8.0f, control = 7.5f, bounce = 8.0f, perk = "🍬 Sweet Sugar Rush" };
                case 3: return new CharacterStats { speed = 8.5f, control = 8.5f, bounce = 6.5f, perk = "💎 Sub-Zero Glacier Armor" };
                case 4: return new CharacterStats { speed = 7.5f, control = 8.0f, bounce = 8.8f, perk = "🍓 Super Berry Spring" };
                case 5: return new CharacterStats { speed = 8.8f, control = 9.0f, bounce = 7.5f, perk = "🪙 Fortune Coin Magnet" };
                case 6: return new CharacterStats { speed = 9.2f, control = 8.2f, bounce = 8.5f, perk = "🌌 Polar Aurora Glow" };
                case 7: return new CharacterStats { speed = 9.6f, control = 7.2f, bounce = 8.2f, perk = "🔥 Thermal Boost Momentum" };
                case 8: return new CharacterStats { speed = 9.6f, control = 9.2f, bounce = 8.6f, perk = "⚡ Synthwave Overdrive" };
                case 9: return new CharacterStats { speed = 10.0f, control = 9.8f, bounce = 9.5f, perk = "🪐 Graviton Singularity" };
                default: return new CharacterStats { speed = 7.0f, control = 7.0f, bounce = 7.0f, perk = "❄️ Standard Roll" };
            }
        }

        private Vector2 initialTitlePreviewPos;
        private bool hasInitialTitlePos = false;
        private Vector2 initialHeroStageBallPos;
        private bool hasInitialHeroStagePos = false;

        private void Awake()
        {
            Instance = this;
            SoundManager.EnsureInstance();
        }

        private void Start()
        {
            GameDataManager.Load();
            inspectedCharIndex = Mathf.Clamp(GameDataManager.Data.selectedCharacter, 0, GameDataManager.Characters.Length - 1);
            ShowTitlePanel();
            UpdateSoundIcon();
            SoundManager.EnsureInstance().PlayMenuMusic();
        }

        private void Update()
        {
            // Title screen ball animation
            if (activeTitlePreviewImage != null && titlePanel != null && titlePanel.activeSelf)
            {
                RectTransform rt = activeTitlePreviewImage.rectTransform;
                if (!hasInitialTitlePos)
                {
                    initialTitlePreviewPos = rt.anchoredPosition;
                    hasInitialTitlePos = true;
                }
                float floatOffset = Mathf.Sin(Time.time * 2.8f) * 6f;
                rt.anchoredPosition = new Vector2(initialTitlePreviewPos.x, initialTitlePreviewPos.y + floatOffset);
                float squish = 1f + Mathf.Sin(Time.time * 3.5f) * 0.035f;
                rt.localScale = new Vector3(squish, 2f - squish, 1f);
            }

            // Hero spotlight stage animation (floating & breathing & rotating aura)
            if (characterSelectPanel != null && characterSelectPanel.activeSelf)
            {
                if (heroStageBallRt != null)
                {
                    if (!hasInitialHeroStagePos)
                    {
                        initialHeroStageBallPos = heroStageBallRt.anchoredPosition;
                        hasInitialHeroStagePos = true;
                    }
                    float floatOffset = Mathf.Sin(Time.time * 2.6f) * 7f;
                    heroStageBallRt.anchoredPosition = new Vector2(initialHeroStageBallPos.x, initialHeroStageBallPos.y + floatOffset);
                    float squish = 1f + Mathf.Sin(Time.time * 3.2f) * 0.03f;
                    heroStageBallRt.localScale = new Vector3(squish, 2f - squish, 1f);
                }

                if (heroAuraRingRt != null)
                {
                    heroAuraRingRt.Rotate(Vector3.forward, -12f * Time.deltaTime);
                }
            }
        }

        public void ShowTitlePanel()
        {
            isPlayFlow = false;
            if (titlePanel != null) titlePanel.SetActive(true);
            if (levelSelectPanel != null) levelSelectPanel.SetActive(false);
            if (characterSelectPanel != null) characterSelectPanel.SetActive(false);
            if (settingsPanel != null) settingsPanel.SetActive(false);
            if (controlsPanel != null) controlsPanel.SetActive(false);
            if (activeTitlePreviewImage != null)
            {
                activeTitlePreviewImage.sprite = Resources.Load<Sprite>("Sprites/char_0_frosty");
            }
        }

        public void OpenLevelSelect()
        {
            SoundManager.EnsureInstance().PlayClick();
            if (titlePanel != null) titlePanel.SetActive(false);
            if (characterSelectPanel != null) characterSelectPanel.SetActive(false);
            if (levelSelectPanel != null)
            {
                levelSelectPanel.SetActive(true);
                // Auto switch to world tab containing the highest unlocked level
                int unlockedWorld = Mathf.Clamp((GameDataManager.Data.maxUnlockedLevel - 1) / 10, 0, 2);
                SelectWorldTab(unlockedWorld);
            }
        }

        public void NextWorld()
        {
            if (currentWorldTab < 2)
            {
                SelectWorldTab(currentWorldTab + 1);
            }
        }

        public void PrevWorld()
        {
            if (currentWorldTab > 0)
            {
                SelectWorldTab(currentWorldTab - 1);
            }
        }

        public void SelectWorldTab(int tabIndex)
        {
            currentWorldTab = Mathf.Clamp(tabIndex, 0, 2);
            SoundManager.EnsureInstance().PlayClick();

            if (currentWorldTitleText != null && currentWorldTab < WorldNames.Length)
            {
                currentWorldTitleText.text = WorldNames[currentWorldTab];
            }

            if (worldPageIndicatorText != null)
            {
                string dots = "";
                for (int i = 0; i < 3; i++)
                {
                    dots += (i == currentWorldTab) ? "<color=#40e0ff>●</color>   " : "<color=#507090>○</color>   ";
                }
                worldPageIndicatorText.text = dots.Trim();
            }

            // Update Left and Right arrow button states
            // Left button disabled on World 1 (levels 1 - 10)
            // Right button disabled on World 3 (levels 21 - 30)
            if (prevWorldButton != null)
            {
                bool canPrev = currentWorldTab > 0;
                prevWorldButton.interactable = canPrev;
                if (prevWorldButton.image != null)
                {
                    prevWorldButton.image.color = canPrev ? Color.white : new Color(0.4f, 0.5f, 0.65f, 0.35f);
                }
                var pTxt = prevWorldButton.GetComponentInChildren<Text>();
                if (pTxt != null)
                {
                    pTxt.color = canPrev ? Color.white : new Color(0.7f, 0.75f, 0.85f, 0.35f);
                }
            }

            if (nextWorldButton != null)
            {
                bool canNext = currentWorldTab < 2;
                nextWorldButton.interactable = canNext;
                if (nextWorldButton.image != null)
                {
                    nextWorldButton.image.color = canNext ? Color.white : new Color(0.4f, 0.5f, 0.65f, 0.35f);
                }
                var nTxt = nextWorldButton.GetComponentInChildren<Text>();
                if (nTxt != null)
                {
                    nTxt.color = canNext ? Color.white : new Color(0.7f, 0.75f, 0.85f, 0.35f);
                }
            }

            PopulateLevelGrid();
        }

        public void OpenCharacterSelect()
        {
            SoundManager.EnsureInstance().PlayClick();
            if (titlePanel != null) titlePanel.SetActive(false);
            if (levelSelectPanel != null) levelSelectPanel.SetActive(false);
            if (characterSelectPanel != null)
            {
                characterSelectPanel.SetActive(true);
                inspectedCharIndex = Mathf.Clamp(GameDataManager.Data.selectedCharacter, 0, GameDataManager.Characters.Length - 1);
                SelectHeroInDossier(inspectedCharIndex);
                PopulateCharacterList();
                UpdateCharacterPanelUI();
            }
        }

        public void UpdateCharacterPanelUI()
        {
            if (characterPanelTitleText != null)
            {
                characterPanelTitleText.text = isPlayFlow 
                    ? $"LEVEL {GameDataManager.Data.selectedLevel} - CHOOSE HERO" 
                    : "SNOWBALL HERO ROSTER";
            }
        }

        public void OnPlayClicked()
        {
            SoundManager.EnsureInstance().PlayClick();
            isPlayFlow = true;
            OpenLevelSelect();
        }

        public void OnLevelSelected(int level)
        {
            if (!GameDataManager.IsLevelUnlocked(level)) return;

            SoundManager.EnsureInstance().PlayClick();
            GameDataManager.Data.selectedLevel = level;
            GameDataManager.Save();

            // Transition to Hero Selection Stage
            OpenCharacterSelect();
        }

        public void SelectHeroInDossier(int charIndex)
        {
            inspectedCharIndex = Mathf.Clamp(charIndex, 0, GameDataManager.Characters.Length - 1);
            CharacterDef ch = GameDataManager.Characters[inspectedCharIndex];
            bool unlocked = GameDataManager.IsCharacterUnlocked(inspectedCharIndex);
            bool isEquipped = (GameDataManager.Data.selectedCharacter == inspectedCharIndex);

            // 1. Stage Avatar
            if (activePreviewImage != null)
            {
                activePreviewImage.sprite = Resources.Load<Sprite>(ch.spriteResource);
                activePreviewImage.color = unlocked ? Color.white : new Color(0.35f, 0.35f, 0.45f, 0.85f);
            }

            // 2. Name & Title & Description
            if (activeNameText != null)
            {
                activeNameText.text = ch.name.ToUpper();
            }
            if (heroTitleText != null)
            {
                heroTitleText.text = $"★ {ch.title.ToUpper()}";
            }
            if (activeDescText != null)
            {
                activeDescText.text = ch.description;
            }

            // 3. Stats & Perk
            CharacterStats stats = GetStats(inspectedCharIndex);
            if (speedBarFill != null) speedBarFill.sizeDelta = new Vector2(216f * (stats.speed / 10f), 14f);
            if (speedValText != null) speedValText.text = $"{stats.speed:0.0}";

            if (controlBarFill != null) controlBarFill.sizeDelta = new Vector2(216f * (stats.control / 10f), 14f);
            if (controlValText != null) controlValText.text = $"{stats.control:0.0}";

            if (bounceBarFill != null) bounceBarFill.sizeDelta = new Vector2(216f * (stats.bounce / 10f), 14f);
            if (bounceValText != null) bounceValText.text = $"{stats.bounce:0.0}";

            if (heroSpecialPerkText != null)
            {
                heroSpecialPerkText.text = $"SPECIAL: {stats.perk}";
            }

            // 4. Equip / Action Button
            Sprite greenBtn = Resources.Load<Sprite>("Sprites/ui_btn_green");
            Sprite blueBtn = Resources.Load<Sprite>("Sprites/ui_btn_blue");
            Sprite redBtn = Resources.Load<Sprite>("Sprites/ui_btn_red");

            if (heroActionButton != null && heroActionBtnText != null && heroActionBtnImg != null)
            {
                if (!unlocked)
                {
                    heroActionButton.interactable = false;
                    heroActionBtnText.text = $"UNLOCKS AT LEVEL {ch.unlockLevel}";
                    heroActionBtnImg.sprite = redBtn;
                }
                else if (isPlayFlow)
                {
                    heroActionButton.interactable = true;
                    heroActionBtnText.text = isEquipped 
                        ? $"PLAY LEVEL {GameDataManager.Data.selectedLevel}" 
                        : $"EQUIP & PLAY LEVEL {GameDataManager.Data.selectedLevel}";
                    heroActionBtnImg.sprite = greenBtn;
                }
                else
                {
                    if (isEquipped)
                    {
                        heroActionButton.interactable = false;
                        heroActionBtnText.text = "EQUIPPED";
                        heroActionBtnImg.sprite = greenBtn;
                    }
                    else
                    {
                        heroActionButton.interactable = true;
                        heroActionBtnText.text = "EQUIP HERO BALL";
                        heroActionBtnImg.sprite = blueBtn;
                    }
                }
            }

            PopulateCharacterList();
        }

        public void OnHeroActionClicked()
        {
            if (!GameDataManager.IsCharacterUnlocked(inspectedCharIndex)) return;

            SoundManager.EnsureInstance().PlayClick();
            GameDataManager.Data.selectedCharacter = inspectedCharIndex;
            GameDataManager.Save();

            if (isPlayFlow)
            {
                OnStartGameClicked();
            }
            else
            {
                SelectHeroInDossier(inspectedCharIndex);
            }
        }

        public void OnCharacterBackClicked()
        {
            SoundManager.EnsureInstance().PlayClick();
            if (isPlayFlow)
            {
                OpenLevelSelect();
            }
            else
            {
                ShowTitlePanel();
            }
        }

        public void OnStartGameClicked()
        {
            SoundManager.EnsureInstance().PlayClick();
            GameDataManager.Save();
            SceneManager.LoadScene("GameScene");
        }

        public void ToggleSound()
        {
            bool current = GameDataManager.Data.isSfxEnabled;
            SoundManager.EnsureInstance().SetSfxEnabled(!current);
            SoundManager.EnsureInstance().SetMusicEnabled(!current);
            UpdateSoundIcon();
            SoundManager.EnsureInstance().PlayClick();
        }

        private void UpdateSoundIcon()
        {
            if (soundToggleIcon != null)
            {
                soundToggleIcon.color = GameDataManager.Data.isSfxEnabled ? Color.white : new Color(0.6f, 0.6f, 0.6f, 0.7f);
            }
        }

        public void OnQuitClicked()
        {
            SoundManager.EnsureInstance().PlayClick();
            Application.Quit();
        }

        private void PopulateLevelGrid()
        {
            if (levelGridContainer == null) return;

            foreach (Transform child in levelGridContainer)
            {
                Destroy(child.gameObject);
            }

            Sprite cardUnlocked = Resources.Load<Sprite>("Sprites/ui_card_glass_unlocked");
            Sprite cardLocked = Resources.Load<Sprite>("Sprites/ui_card_glass_locked");
            Sprite lockIcon = Resources.Load<Sprite>("Sprites/ui_lock");
            Sprite starIcon = Resources.Load<Sprite>("Sprites/ui_star");
            Sprite coinIcon = Resources.Load<Sprite>("Sprites/coin_gold");
            Font defaultFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf") ?? Resources.GetBuiltinResource<Font>("Arial.ttf");

            // Compute total coins & stars across all 30 levels
            int totalStars = 0;
            int totalCoins = 0;
            for (int i = 1; i <= GameDataManager.TOTAL_LEVELS; i++)
            {
                int c = (GameDataManager.Data.levelCoins != null && i < GameDataManager.Data.levelCoins.Length)
                    ? GameDataManager.Data.levelCoins[i] : 0;
                totalCoins += c;
                if (c >= 30) totalStars += 3;
                else if (c >= 20) totalStars += 2;
                else if (c > 0) totalStars += 1;
            }

            if (totalStarsCoinsText != null)
            {
                totalStarsCoinsText.text = $"★ {totalStars}/90   🪙 {totalCoins}/900";
            }

            int startLevel = currentWorldTab * 10 + 1;
            int endLevel = startLevel + 9;

            for (int levelNum = startLevel; levelNum <= endLevel; levelNum++)
            {
                int currentLvl = levelNum;
                bool unlocked = GameDataManager.IsLevelUnlocked(currentLvl);
                int coins = (GameDataManager.Data.levelCoins != null && currentLvl < GameDataManager.Data.levelCoins.Length)
                    ? GameDataManager.Data.levelCoins[currentLvl] : 0;

                int stars = 0;
                if (coins >= 30) stars = 3;
                else if (coins >= 20) stars = 2;
                else if (coins > 0) stars = 1;

                GameObject cardObj = new GameObject($"LevelCard_{currentLvl}", typeof(RectTransform));
                cardObj.transform.SetParent(levelGridContainer, false);

                RectTransform rt = cardObj.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(195, 185);

                Image img = cardObj.AddComponent<Image>();
                img.sprite = unlocked ? cardUnlocked : cardLocked;

                Button btn = cardObj.AddComponent<Button>();
                btn.interactable = unlocked;
                if (unlocked)
                {
                    btn.onClick.AddListener(() => OnLevelSelected(currentLvl));
                }

                if (unlocked)
                {
                    // 1. Level Number Tag
                    GameObject tagObj = new GameObject("LevelTag", typeof(RectTransform));
                    tagObj.transform.SetParent(cardObj.transform, false);
                    Text tagTxt = tagObj.AddComponent<Text>();
                    tagTxt.text = $"LEVEL {currentLvl:D2}";
                    tagTxt.font = defaultFont;
                    tagTxt.fontSize = 24;
                    tagTxt.fontStyle = FontStyle.Bold;
                    tagTxt.alignment = TextAnchor.MiddleCenter;
                    tagTxt.color = Color.white;

                    Outline tagOut = tagObj.AddComponent<Outline>();
                    tagOut.effectColor = new Color(0.04f, 0.15f, 0.35f, 1f);
                    tagOut.effectDistance = new Vector2(1.5f, -1.5f);

                    RectTransform tagRt = tagObj.GetComponent<RectTransform>();
                    tagRt.anchoredPosition = new Vector2(0, 48);
                    tagRt.sizeDelta = new Vector2(170, 28);

                    // 2. Stars Row (3 Stars) - Enlarged and moved down into middle section
                    GameObject starRow = new GameObject("StarsRow", typeof(RectTransform));
                    starRow.transform.SetParent(cardObj.transform, false);
                    RectTransform starRowRt = starRow.GetComponent<RectTransform>();
                    starRowRt.anchoredPosition = new Vector2(0, -16);
                    starRowRt.sizeDelta = new Vector2(130, 36);

                    HorizontalLayoutGroup shlg = starRow.AddComponent<HorizontalLayoutGroup>();
                    shlg.childAlignment = TextAnchor.MiddleCenter;
                    shlg.spacing = 10;
                    shlg.childControlWidth = false;
                    shlg.childControlHeight = false;

                    for (int s = 0; s < 3; s++)
                    {
                        GameObject sObj = new GameObject($"Star_{s}", typeof(RectTransform));
                        sObj.transform.SetParent(starRow.transform, false);
                        Image sImg = sObj.AddComponent<Image>();
                        sImg.sprite = starIcon;
                        sImg.color = (s < stars) ? Color.white : new Color(0.2f, 0.35f, 0.5f, 0.45f);
                        RectTransform sRt = sObj.GetComponent<RectTransform>();
                        sRt.sizeDelta = new Vector2(34, 34);
                    }

                    // 3. Coin Tray Container - Enlarged and fitted squarely inside bottom box
                    GameObject coinTray = new GameObject("CoinTray", typeof(RectTransform));
                    coinTray.transform.SetParent(cardObj.transform, false);
                    RectTransform cTrayRt = coinTray.GetComponent<RectTransform>();
                    cTrayRt.anchoredPosition = new Vector2(0, -60);
                    cTrayRt.sizeDelta = new Vector2(150, 26);

                    HorizontalLayoutGroup hlg = coinTray.AddComponent<HorizontalLayoutGroup>();
                    hlg.childAlignment = TextAnchor.MiddleCenter;
                    hlg.spacing = 6f;
                    hlg.childControlWidth = false;
                    hlg.childControlHeight = false;

                    if (coinIcon != null)
                    {
                        GameObject iconObj = new GameObject("Icon", typeof(RectTransform));
                        iconObj.transform.SetParent(coinTray.transform, false);
                        Image iconImg = iconObj.AddComponent<Image>();
                        iconImg.sprite = coinIcon;
                        RectTransform iconRt = iconObj.GetComponent<RectTransform>();
                        iconRt.sizeDelta = new Vector2(20, 20);
                    }

                    GameObject coinTxtObj = new GameObject("CoinText", typeof(RectTransform));
                    coinTxtObj.transform.SetParent(coinTray.transform, false);
                    Text coinTxt = coinTxtObj.AddComponent<Text>();
                    coinTxt.text = $"{coins}/30";
                    coinTxt.font = defaultFont;
                    coinTxt.fontSize = 17;
                    coinTxt.fontStyle = FontStyle.Bold;
                    coinTxt.alignment = TextAnchor.MiddleCenter;
                    coinTxt.color = (coins >= 30) ? new Color(1f, 0.9f, 0.25f, 1f) : new Color(0.85f, 0.94f, 1f, 1f);

                    Outline coinOutline = coinTxtObj.AddComponent<Outline>();
                    coinOutline.effectColor = new Color(0.04f, 0.15f, 0.35f, 1f);
                    coinOutline.effectDistance = new Vector2(1f, -1f);

                    RectTransform coinRt = coinTxtObj.GetComponent<RectTransform>();
                    coinRt.sizeDelta = new Vector2(65, 22);
                }
                else
                {
                    // Locked Card Visuals
                    GameObject lockObj = new GameObject("Lock", typeof(RectTransform));
                    lockObj.transform.SetParent(cardObj.transform, false);
                    Image lockImg = lockObj.AddComponent<Image>();
                    lockImg.sprite = lockIcon;
                    RectTransform lockRt = lockObj.GetComponent<RectTransform>();
                    lockRt.sizeDelta = new Vector2(38, 38);
                    lockRt.anchoredPosition = new Vector2(0, 16);

                    GameObject tagObj = new GameObject("LevelTag", typeof(RectTransform));
                    tagObj.transform.SetParent(cardObj.transform, false);
                    Text tagTxt = tagObj.AddComponent<Text>();
                    tagTxt.text = $"LEVEL {currentLvl:D2}";
                    tagTxt.font = defaultFont;
                    tagTxt.fontSize = 17;
                    tagTxt.fontStyle = FontStyle.Bold;
                    tagTxt.alignment = TextAnchor.MiddleCenter;
                    tagTxt.color = new Color(0.5f, 0.62f, 0.75f, 0.8f);
                    RectTransform tagRt = tagObj.GetComponent<RectTransform>();
                    tagRt.anchoredPosition = new Vector2(0, 58);
                    tagRt.sizeDelta = new Vector2(170, 24);

                    GameObject reqObj = new GameObject("ReqText", typeof(RectTransform));
                    reqObj.transform.SetParent(cardObj.transform, false);
                    Text reqTxt = reqObj.AddComponent<Text>();
                    reqTxt.text = $"Clear Level {currentLvl - 1}";
                    reqTxt.font = defaultFont;
                    reqTxt.fontSize = 12;
                    reqTxt.alignment = TextAnchor.MiddleCenter;
                    reqTxt.color = new Color(0.7f, 0.4f, 0.35f, 0.9f);
                    RectTransform reqRt = reqObj.GetComponent<RectTransform>();
                    reqRt.anchoredPosition = new Vector2(0, -60);
                    reqRt.sizeDelta = new Vector2(150, 20);
                }
            }
        }

        private void PopulateCharacterList()
        {
            if (characterListContainer == null) return;

            foreach (Transform child in characterListContainer)
            {
                Destroy(child.gameObject);
            }

            Sprite cardNormal = Resources.Load<Sprite>("Sprites/ui_dock_card_normal");
            Sprite cardSelected = Resources.Load<Sprite>("Sprites/ui_dock_card_selected");
            Sprite lockIcon = Resources.Load<Sprite>("Sprites/ui_lock");
            Font defaultFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf") ?? Resources.GetBuiltinResource<Font>("Arial.ttf");

            int unlockedCount = 0;
            for (int i = 0; i < GameDataManager.Characters.Length; i++)
            {
                if (GameDataManager.IsCharacterUnlocked(i)) unlockedCount++;
            }
            if (unlockedCountText != null)
            {
                unlockedCountText.text = $"{unlockedCount} / 10 UNLOCKED";
            }

            for (int i = 0; i < GameDataManager.Characters.Length; i++)
            {
                int index = i;
                CharacterDef ch = GameDataManager.Characters[index];
                bool unlocked = GameDataManager.IsCharacterUnlocked(index);
                bool isInspected = (inspectedCharIndex == index);
                bool isEquipped = (GameDataManager.Data.selectedCharacter == index);

                GameObject cardObj = new GameObject($"CharDockCard_{index}", typeof(RectTransform));
                cardObj.transform.SetParent(characterListContainer, false);

                RectTransform rt = cardObj.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(100, 126);

                Image bgImg = cardObj.AddComponent<Image>();
                bgImg.sprite = (isInspected || isEquipped) ? cardSelected : cardNormal;

                Button btn = cardObj.AddComponent<Button>();
                btn.onClick.AddListener(() =>
                {
                    SoundManager.EnsureInstance().PlayClick();
                    SelectHeroInDossier(index);
                });

                // Mini Avatar Icon
                GameObject avatarObj = new GameObject("Avatar", typeof(RectTransform));
                avatarObj.transform.SetParent(cardObj.transform, false);
                Image avatarImg = avatarObj.AddComponent<Image>();
                avatarImg.sprite = Resources.Load<Sprite>(ch.spriteResource);
                if (!unlocked) avatarImg.color = new Color(0.25f, 0.25f, 0.35f, 0.65f);
                RectTransform aRt = avatarObj.GetComponent<RectTransform>();
                aRt.anchoredPosition = new Vector2(0, 15);
                aRt.sizeDelta = new Vector2(56, 56);

                // Name label
                GameObject nameObj = new GameObject("Name", typeof(RectTransform));
                nameObj.transform.SetParent(cardObj.transform, false);
                Text nameTxt = nameObj.AddComponent<Text>();
                nameTxt.text = ch.name;
                nameTxt.font = defaultFont;
                nameTxt.fontSize = 13;
                nameTxt.fontStyle = FontStyle.Bold;
                nameTxt.alignment = TextAnchor.MiddleCenter;
                nameTxt.color = unlocked ? Color.white : new Color(0.5f, 0.55f, 0.65f, 1f);
                RectTransform nameRt = nameObj.GetComponent<RectTransform>();
                nameRt.anchoredPosition = new Vector2(0, -26);
                nameRt.sizeDelta = new Vector2(92, 18);

                // Status Pill / Badge
                GameObject subObj = new GameObject("Status", typeof(RectTransform));
                subObj.transform.SetParent(cardObj.transform, false);
                Text subTxt = subObj.AddComponent<Text>();
                subTxt.font = defaultFont;
                subTxt.fontSize = 10;
                subTxt.fontStyle = FontStyle.Bold;
                subTxt.alignment = TextAnchor.MiddleCenter;

                if (!unlocked)
                {
                    subTxt.text = $"LV {ch.unlockLevel}";
                    subTxt.color = new Color(1f, 0.45f, 0.4f, 1f);
                }
                else if (isEquipped)
                {
                    subTxt.text = "★ EQUIPPED";
                    subTxt.color = new Color(0.2f, 1f, 0.5f, 1f);
                }
                else
                {
                    subTxt.text = "UNLOCKED";
                    subTxt.color = new Color(0.5f, 0.85f, 1f, 0.85f);
                }

                RectTransform subRt = subObj.GetComponent<RectTransform>();
                subRt.anchoredPosition = new Vector2(0, -45);
                subRt.sizeDelta = new Vector2(92, 16);

                if (!unlocked)
                {
                    GameObject lObj = new GameObject("Lock", typeof(RectTransform));
                    lObj.transform.SetParent(cardObj.transform, false);
                    Image lImg = lObj.AddComponent<Image>();
                    lImg.sprite = lockIcon;
                    RectTransform lRt = lObj.GetComponent<RectTransform>();
                    lRt.anchoredPosition = new Vector2(24, 34);
                    lRt.sizeDelta = new Vector2(22, 22);
                }
            }
        }

        // ----------------------------------------------------
        // SETTINGS & CONTROLS POPUPS
        // ----------------------------------------------------
        public void OpenSettings()
        {
            SoundManager.EnsureInstance().PlayClick();
            if (settingsPanel != null)
            {
                settingsPanel.SetActive(true);
                UpdateSettingsUI();
            }
        }

        public void CloseSettings()
        {
            SoundManager.EnsureInstance().PlayClick();
            if (settingsPanel != null) settingsPanel.SetActive(false);
        }

        public void ToggleSoundFromSettings()
        {
            bool nextState = !GameDataManager.Data.isSfxEnabled;
            SoundManager.EnsureInstance().SetSfxEnabled(nextState);
            UpdateSettingsUI();
            if (nextState) SoundManager.EnsureInstance().PlayClick();
        }

        public void ToggleMusicFromSettings()
        {
            bool nextState = !GameDataManager.Data.isMusicEnabled;
            SoundManager.EnsureInstance().SetMusicEnabled(nextState);
            UpdateSettingsUI();
            SoundManager.EnsureInstance().PlayClick();
        }

        public void UpdateSettingsUI()
        {
            if (soundSettingText != null)
            {
                soundSettingText.text = GameDataManager.Data.isSfxEnabled ? "SOUND: ON" : "SOUND: OFF";
            }
            if (soundSettingBtnImg != null)
            {
                soundSettingBtnImg.sprite = Resources.Load<Sprite>(GameDataManager.Data.isSfxEnabled ? "Sprites/ui_btn_green" : "Sprites/ui_btn_red");
            }

            if (musicSettingText != null)
            {
                musicSettingText.text = GameDataManager.Data.isMusicEnabled ? "MUSIC: ON" : "MUSIC: OFF";
            }
            if (musicSettingBtnImg != null)
            {
                musicSettingBtnImg.sprite = Resources.Load<Sprite>(GameDataManager.Data.isMusicEnabled ? "Sprites/ui_btn_green" : "Sprites/ui_btn_red");
            }
        }

        public void OpenControls()
        {
            SoundManager.EnsureInstance().PlayClick();
            if (controlsPanel != null) controlsPanel.SetActive(true);
        }

        public void CloseControls()
        {
            SoundManager.EnsureInstance().PlayClick();
            if (controlsPanel != null) controlsPanel.SetActive(false);
        }
    }
}
