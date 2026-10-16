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

        [Header("Character Select UI")]
        public Transform characterListContainer;
        public GameObject characterCardPrefab;
        public Image activePreviewImage;
        public Text activeNameText;
        public Text activeDescText;

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

        private void Awake()
        {
            Instance = this;
            SoundManager.EnsureInstance();
        }

        private Vector2 initialPreviewPos;
        private bool hasInitialPreviewPos = false;

        private void Start()
        {
            GameDataManager.Load();
            ShowTitlePanel();
            UpdateSoundIcon();
            SoundManager.EnsureInstance().PlayMenuMusic();
        }

        private void Update()
        {
            if (activeTitlePreviewImage != null && titlePanel != null && titlePanel.activeSelf)
            {
                RectTransform rt = activeTitlePreviewImage.rectTransform;
                if (!hasInitialPreviewPos)
                {
                    initialPreviewPos = rt.anchoredPosition;
                    hasInitialPreviewPos = true;
                }
                float floatOffset = Mathf.Sin(Time.time * 2.8f) * 6f;
                rt.anchoredPosition = new Vector2(initialPreviewPos.x, initialPreviewPos.y + floatOffset);
                float squish = 1f + Mathf.Sin(Time.time * 3.5f) * 0.035f;
                rt.localScale = new Vector3(squish, 2f - squish, 1f);
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
                PopulateLevelGrid();
            }
        }

        public void OpenCharacterSelect()
        {
            SoundManager.EnsureInstance().PlayClick();
            if (titlePanel != null) titlePanel.SetActive(false);
            if (levelSelectPanel != null) levelSelectPanel.SetActive(false);
            if (characterSelectPanel != null)
            {
                characterSelectPanel.SetActive(true);
                PopulateCharacterList();
                UpdateActiveCharacterPreview();
                UpdateCharacterPanelUI();
            }
        }

        public void UpdateCharacterPanelUI()
        {
            if (characterPanelTitleText != null)
            {
                characterPanelTitleText.text = isPlayFlow 
                    ? $"LEVEL {GameDataManager.Data.selectedLevel} - CHOOSE SNOWBALL" 
                    : "SNOWBALL ROSTER (10 CHARACTERS)";
            }

            if (startButtonText != null)
            {
                startButtonText.text = $"PLAY LEVEL {GameDataManager.Data.selectedLevel} ▶";
            }
        }

        public void OnPlayClicked()
        {
            SoundManager.EnsureInstance().PlayClick();
            // Step 1: Open Level Select first
            isPlayFlow = true;
            OpenLevelSelect();
        }

        public void OnLevelSelected(int level)
        {
            if (!GameDataManager.IsLevelUnlocked(level))
            {
                // Locked level
                return;
            }

            SoundManager.EnsureInstance().PlayClick();
            GameDataManager.Data.selectedLevel = level;
            GameDataManager.Save();

            // Step 2: Open Character Select next
            OpenCharacterSelect();
        }

        public void OnCharacterSelected(int charIndex)
        {
            if (!GameDataManager.IsCharacterUnlocked(charIndex))
            {
                return;
            }

            SoundManager.EnsureInstance().PlayClick();
            GameDataManager.Data.selectedCharacter = charIndex;
            GameDataManager.Save();
            UpdateActiveCharacterPreview();
            PopulateCharacterList(); // refresh selection borders
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
            // Step 3: Launch into GameScene
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

            // Clear old children if any
            foreach (Transform child in levelGridContainer)
            {
                Destroy(child.gameObject);
            }

            Sprite cardNormal = Resources.Load<Sprite>("Sprites/ui_level_card");
            Sprite cardLocked = Resources.Load<Sprite>("Sprites/ui_level_card_locked");
            Sprite lockIcon = Resources.Load<Sprite>("Sprites/ui_lock");
            Sprite coinIcon = Resources.Load<Sprite>("Sprites/coin_gold");
            Font defaultFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf") ?? Resources.GetBuiltinResource<Font>("Arial.ttf");

            for (int i = 1; i <= GameDataManager.TOTAL_LEVELS; i++)
            {
                int levelNum = i;
                bool unlocked = GameDataManager.IsLevelUnlocked(levelNum);
                int coins = GameDataManager.Data.levelCoins != null && levelNum < GameDataManager.Data.levelCoins.Length ? GameDataManager.Data.levelCoins[levelNum] : 0;

                GameObject btnObj = new GameObject($"LevelBtn_{levelNum}", typeof(RectTransform));
                btnObj.transform.SetParent(levelGridContainer, false);

                RectTransform rt = btnObj.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(125, 72);

                Image img = btnObj.AddComponent<Image>();
                img.sprite = unlocked ? cardNormal : cardLocked;

                Button btn = btnObj.AddComponent<Button>();
                btn.interactable = unlocked;
                if (unlocked)
                {
                    btn.onClick.AddListener(() => OnLevelSelected(levelNum));
                }

                if (unlocked)
                {
                    // 1. Level Number Text (Prominent, Bold, White with dark outline)
                    GameObject txtObj = new GameObject("Text", typeof(RectTransform));
                    txtObj.transform.SetParent(btnObj.transform, false);
                    Text txt = txtObj.AddComponent<Text>();
                    txt.text = levelNum.ToString();
                    txt.font = defaultFont;
                    txt.fontSize = 28;
                    txt.fontStyle = FontStyle.Bold;
                    txt.alignment = TextAnchor.MiddleCenter;
                    txt.color = Color.white;

                    Outline numOutline = txtObj.AddComponent<Outline>();
                    numOutline.effectColor = new Color(0.05f, 0.18f, 0.4f, 1f);
                    numOutline.effectDistance = new Vector2(1.5f, -1.5f);

                    RectTransform txtRt = txtObj.GetComponent<RectTransform>();
                    txtRt.anchorMin = new Vector2(0f, 0.32f);
                    txtRt.anchorMax = Vector2.one;
                    txtRt.offsetMin = Vector2.zero;
                    txtRt.offsetMax = new Vector2(0f, -4f);

                    // 2. Coins Display Tray Container (Centered mini coin + count)
                    GameObject coinTray = new GameObject("CoinTray", typeof(RectTransform));
                    coinTray.transform.SetParent(btnObj.transform, false);
                    RectTransform cTrayRt = coinTray.GetComponent<RectTransform>();
                    cTrayRt.anchorMin = new Vector2(0.08f, 0.04f);
                    cTrayRt.anchorMax = new Vector2(0.92f, 0.34f);
                    cTrayRt.offsetMin = Vector2.zero;
                    cTrayRt.offsetMax = Vector2.zero;

                    HorizontalLayoutGroup hlg = coinTray.AddComponent<HorizontalLayoutGroup>();
                    hlg.childAlignment = TextAnchor.MiddleCenter;
                    hlg.spacing = 4f;
                    hlg.childControlWidth = false;
                    hlg.childControlHeight = false;
                    hlg.childForceExpandWidth = false;
                    hlg.childForceExpandHeight = false;

                    // Mini Coin Icon
                    if (coinIcon != null)
                    {
                        GameObject iconObj = new GameObject("Icon", typeof(RectTransform));
                        iconObj.transform.SetParent(coinTray.transform, false);
                        Image iconImg = iconObj.AddComponent<Image>();
                        iconImg.sprite = coinIcon;
                        RectTransform iconRt = iconObj.GetComponent<RectTransform>();
                        iconRt.sizeDelta = new Vector2(14, 14);
                    }

                    // Coins Subtext (e.g. "30/30" or "16/30")
                    GameObject coinTxtObj = new GameObject("CoinText", typeof(RectTransform));
                    coinTxtObj.transform.SetParent(coinTray.transform, false);
                    Text coinTxt = coinTxtObj.AddComponent<Text>();
                    coinTxt.text = $"{coins}/30";
                    coinTxt.font = defaultFont;
                    coinTxt.fontSize = 13;
                    coinTxt.fontStyle = FontStyle.Bold;
                    coinTxt.alignment = TextAnchor.MiddleCenter;
                    coinTxt.color = (coins >= 30) ? new Color(1f, 0.9f, 0.25f, 1f) : new Color(0.85f, 0.94f, 1f, 1f);

                    Outline coinOutline = coinTxtObj.AddComponent<Outline>();
                    coinOutline.effectColor = new Color(0.04f, 0.15f, 0.35f, 1f);
                    coinOutline.effectDistance = new Vector2(1f, -1f);

                    RectTransform coinRt = coinTxtObj.GetComponent<RectTransform>();
                    coinRt.sizeDelta = new Vector2(46, 18);
                }
                else
                {
                    // Lock Icon
                    GameObject lockObj = new GameObject("Lock", typeof(RectTransform));
                    lockObj.transform.SetParent(btnObj.transform, false);
                    Image lockImg = lockObj.AddComponent<Image>();
                    lockImg.sprite = lockIcon;
                    RectTransform lockRt = lockObj.GetComponent<RectTransform>();
                    lockRt.sizeDelta = new Vector2(32, 32);
                    lockRt.anchoredPosition = new Vector2(0, 0);
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

            Sprite lockIcon = Resources.Load<Sprite>("Sprites/ui_lock");
            Sprite cardBg = Resources.Load<Sprite>("Sprites/ui_level_card");
            Sprite activeCardBg = Resources.Load<Sprite>("Sprites/ui_dialog_panel");

            for (int i = 0; i < GameDataManager.Characters.Length; i++)
            {
                int index = i;
                CharacterDef ch = GameDataManager.Characters[index];
                bool unlocked = GameDataManager.IsCharacterUnlocked(index);
                bool isSelected = (GameDataManager.Data.selectedCharacter == index);

                GameObject cardObj = new GameObject($"CharCard_{index}");
                cardObj.transform.SetParent(characterListContainer, false);

                RectTransform rt = cardObj.AddComponent<RectTransform>();
                rt.sizeDelta = new Vector2(280, 70);

                Image bgImg = cardObj.AddComponent<Image>();
                bgImg.sprite = isSelected ? activeCardBg : cardBg;
                bgImg.color = isSelected ? new Color(0.9f, 0.95f, 1f, 1f) : Color.white;

                Button btn = cardObj.AddComponent<Button>();
                btn.interactable = unlocked;
                btn.onClick.AddListener(() => OnCharacterSelected(index));

                // Avatar Icon
                GameObject avatarObj = new GameObject("Avatar");
                avatarObj.transform.SetParent(cardObj.transform, false);
                Image avatarImg = avatarObj.AddComponent<Image>();
                Sprite charSprite = Resources.Load<Sprite>(ch.spriteResource);
                avatarImg.sprite = charSprite;
                if (!unlocked) avatarImg.color = new Color(0.3f, 0.3f, 0.3f, 0.6f);
                RectTransform avatarRt = avatarObj.GetComponent<RectTransform>();
                avatarRt.anchorMin = new Vector2(0, 0.5f);
                avatarRt.anchorMax = new Vector2(0, 0.5f);
                avatarRt.pivot = new Vector2(0, 0.5f);
                avatarRt.anchoredPosition = new Vector2(10, 0);
                avatarRt.sizeDelta = new Vector2(50, 50);

                // Name & Info Text
                GameObject nameObj = new GameObject("Name");
                nameObj.transform.SetParent(cardObj.transform, false);
                Text nameTxt = nameObj.AddComponent<Text>();
                nameTxt.text = ch.name;
                nameTxt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf") ?? Resources.GetBuiltinResource<Font>("Arial.ttf");
                nameTxt.fontSize = 17;
                nameTxt.fontStyle = FontStyle.Bold;
                nameTxt.color = unlocked ? new Color(0.1f, 0.2f, 0.4f, 1f) : new Color(0.4f, 0.45f, 0.5f, 1f);
                RectTransform nameRt = nameObj.GetComponent<RectTransform>();
                nameRt.anchorMin = new Vector2(0, 0.5f);
                nameRt.anchorMax = new Vector2(1, 1f);
                nameRt.offsetMin = new Vector2(68, 0);
                nameRt.offsetMax = new Vector2(-10, -8);

                // Subtitle / Unlock requirement Text
                GameObject subObj = new GameObject("Subtitle");
                subObj.transform.SetParent(cardObj.transform, false);
                Text subTxt = subObj.AddComponent<Text>();
                subTxt.text = unlocked ? (isSelected ? "★ EQUIPPED" : ch.title) : $"Unlocks at Level {ch.unlockLevel}";
                subTxt.font = nameTxt.font;
                subTxt.fontSize = 12;
                subTxt.fontStyle = isSelected ? FontStyle.Bold : FontStyle.Normal;
                subTxt.color = isSelected ? new Color(0.1f, 0.65f, 0.3f, 1f) : (unlocked ? new Color(0.35f, 0.5f, 0.7f, 1f) : new Color(0.7f, 0.3f, 0.2f, 1f));
                RectTransform subRt = subObj.GetComponent<RectTransform>();
                subRt.anchorMin = new Vector2(0, 0f);
                subRt.anchorMax = new Vector2(1, 0.5f);
                subRt.offsetMin = new Vector2(68, 8);
                subRt.offsetMax = new Vector2(-10, 0);

                // Lock icon overlay if locked
                if (!unlocked)
                {
                    GameObject lockObj = new GameObject("Lock");
                    lockObj.transform.SetParent(cardObj.transform, false);
                    Image lImg = lockObj.AddComponent<Image>();
                    lImg.sprite = lockIcon;
                    RectTransform lRt = lockObj.GetComponent<RectTransform>();
                    lRt.anchorMin = new Vector2(1, 0.5f);
                    lRt.anchorMax = new Vector2(1, 0.5f);
                    lRt.pivot = new Vector2(1, 0.5f);
                    lRt.anchoredPosition = new Vector2(-12, 0);
                    lRt.sizeDelta = new Vector2(26, 26);
                }
            }
        }

        private void UpdateActiveCharacterPreview()
        {
            int index = Mathf.Clamp(GameDataManager.Data.selectedCharacter, 0, GameDataManager.Characters.Length - 1);
            CharacterDef ch = GameDataManager.Characters[index];

            if (activePreviewImage != null)
            {
                activePreviewImage.sprite = Resources.Load<Sprite>(ch.spriteResource);
            }
            if (activeNameText != null)
            {
                activeNameText.text = $"{ch.name} - {ch.title}";
            }
            if (activeDescText != null)
            {
                activeDescText.text = ch.description;
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
