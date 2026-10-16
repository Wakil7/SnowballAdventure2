using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SnowballAdventure
{
    public class MainMenuBuilder : MonoBehaviour
    {
        private void Awake()
        {
            BuildMainMenuScene();
        }

        public void BuildMainMenuScene()
        {
            if (FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                GameObject es = new GameObject("EventSystem");
                es.AddComponent<UnityEngine.EventSystems.EventSystem>();
#if ENABLE_INPUT_SYSTEM
                es.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
#elif ENABLE_LEGACY_INPUT_MANAGER
                es.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
#else
                System.Type inputSystemModule = System.Type.GetType("UnityEngine.InputSystem.UI.InputSystemUIInputModule, Unity.InputSystem");
                if (inputSystemModule != null)
                {
                    es.AddComponent(inputSystemModule);
                }
                else
                {
                    es.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
                }
#endif
            }

            // 1. Camera & Lighting Setup
            SetupCamera();

            // 2. Parallax Backdrop & Falling Snow
            SetupBackdrop();

            // 3. UI Canvas & Panels
            BuildCanvas();
        }

        private void SetupCamera()
        {
            Camera cam = Camera.main;
            if (cam == null)
            {
                GameObject camObj = new GameObject("Main Camera");
                cam = camObj.AddComponent<Camera>();
                camObj.tag = "MainCamera";
            }

            cam.orthographic = true;
            cam.orthographicSize = 5f;
            cam.backgroundColor = new Color(0.1f, 0.18f, 0.32f, 1f);
        }

        private void SetupBackdrop()
        {
            Camera cam = Camera.main;
            if (cam == null) return;

            Sprite auroraSpr = Resources.Load<Sprite>("Sprites/bg_menu_aurora");
            if (auroraSpr != null)
            {
                GameObject bgObj = new GameObject("MenuCameraBackdrop");
                bgObj.transform.SetParent(cam.transform, false);
                bgObj.transform.localPosition = new Vector3(0f, 0f, 20f);
                bgObj.transform.localScale = new Vector3(20f, 11.25f, 1f);

                SpriteRenderer bgRend = bgObj.AddComponent<SpriteRenderer>();
                bgRend.sprite = auroraSpr;
                bgRend.sortingOrder = -50;
            }
        }

        private void BuildCanvas()
        {
            GameObject canvasObj = new GameObject("MainMenu_Canvas", typeof(RectTransform));
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280, 720);
            scaler.matchWidthOrHeight = 0.5f;
            canvasObj.AddComponent<GraphicRaycaster>();

            // Fullscreen Attractive Winter Aurora Background
            GameObject bgObj = new GameObject("Menu_CanvasBackground", typeof(RectTransform));
            bgObj.transform.SetParent(canvasObj.transform, false);
            bgObj.transform.SetAsFirstSibling();
            RectTransform bgRt = bgObj.GetComponent<RectTransform>();
            bgRt.anchorMin = Vector2.zero;
            bgRt.anchorMax = Vector2.one;
            bgRt.offsetMin = Vector2.zero;
            bgRt.offsetMax = Vector2.zero;

            Image bgImg = bgObj.AddComponent<Image>();
            bgImg.sprite = Resources.Load<Sprite>("Sprites/bg_menu_aurora");
            bgImg.type = Image.Type.Simple;
            bgImg.preserveAspect = false;

            // Subtle 15% vignette overlay to make UI buttons pop crisply
            GameObject vigObj = new GameObject("Menu_VignetteOverlay", typeof(RectTransform));
            vigObj.transform.SetParent(canvasObj.transform, false);
            RectTransform vigRt = vigObj.GetComponent<RectTransform>();
            vigRt.anchorMin = Vector2.zero;
            vigRt.anchorMax = Vector2.one;
            vigRt.offsetMin = Vector2.zero;
            vigRt.offsetMax = Vector2.zero;
            Image vigImg = vigObj.AddComponent<Image>();
            vigImg.color = new Color(0.02f, 0.05f, 0.12f, 0.15f);
            vigImg.raycastTarget = false;

            Font defaultFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf") ?? Resources.GetBuiltinResource<Font>("Arial.ttf");

            // Create Manager
            MainMenuManager manager = canvasObj.AddComponent<MainMenuManager>();

            // 1. Title Panel
            GameObject titlePanel = BuildTitlePanel(canvasObj.transform, defaultFont, manager);

            // 2. Level Select Panel
            GameObject levelPanel = BuildLevelSelectPanel(canvasObj.transform, defaultFont, manager);

            // 3. Character Select Panel
            GameObject charPanel = BuildCharacterSelectPanel(canvasObj.transform, defaultFont, manager);

            // 4. Settings Panel
            GameObject settingsPanel = BuildSettingsPanel(canvasObj.transform, defaultFont, manager);

            // 5. Controls Panel
            GameObject controlsPanel = BuildControlsPanel(canvasObj.transform, defaultFont, manager);

            manager.titlePanel = titlePanel;
            manager.levelSelectPanel = levelPanel;
            manager.characterSelectPanel = charPanel;
            manager.settingsPanel = settingsPanel;
            manager.controlsPanel = controlsPanel;
        }

        private GameObject BuildTitlePanel(Transform canvas, Font font, MainMenuManager manager)
        {
            GameObject panel = new GameObject("TitlePanel", typeof(RectTransform));
            panel.transform.SetParent(canvas, false);
            RectTransform rt = panel.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            // Logo Banner
            GameObject logoObj = new GameObject("TitleLogo", typeof(RectTransform));
            logoObj.transform.SetParent(panel.transform, false);
            Image logoImg = logoObj.AddComponent<Image>();
            logoImg.sprite = Resources.Load<Sprite>("Sprites/logo_title");
            logoImg.preserveAspect = true;
            RectTransform lRt = logoObj.GetComponent<RectTransform>();
            lRt.anchorMin = new Vector2(0.5f, 1f);
            lRt.anchorMax = new Vector2(0.5f, 1f);
            lRt.pivot = new Vector2(0.5f, 1f);
            lRt.anchoredPosition = new Vector2(0, -35);
            lRt.sizeDelta = new Vector2(580, 125);

            // Snowball Display (always the original first character Frosty)
            GameObject previewObj = new GameObject("ActiveBallPreview", typeof(RectTransform));
            previewObj.transform.SetParent(panel.transform, false);
            Image prevImg = previewObj.AddComponent<Image>();
            prevImg.sprite = Resources.Load<Sprite>("Sprites/char_0_frosty");
            manager.activeTitlePreviewImage = prevImg;
            RectTransform pRt = previewObj.GetComponent<RectTransform>();
            pRt.anchoredPosition = new Vector2(0, 50);
            pRt.sizeDelta = new Vector2(120, 120);

            // Menu Buttons Container
            GameObject btnContainer = new GameObject("ButtonContainer", typeof(RectTransform));
            btnContainer.transform.SetParent(panel.transform, false);
            RectTransform bcRt = btnContainer.GetComponent<RectTransform>();
            bcRt.anchoredPosition = new Vector2(0, -150);
            bcRt.sizeDelta = new Vector2(300, 220);

            // PLAY Button
            CreateMenuButton(btnContainer.transform, "PlayBtn", "PLAY ▶", font, Resources.Load<Sprite>("Sprites/ui_btn_green"), new Vector2(0, 65), new Vector2(240, 56), 24,
                () => manager.OnPlayClicked());

            // SETTINGS Button (replaces Levels Button)
            CreateMenuButton(btnContainer.transform, "SettingsBtn", "SETTINGS ⚙", font, Resources.Load<Sprite>("Sprites/ui_btn_blue"), new Vector2(0, 0), new Vector2(240, 52), 20,
                () => manager.OpenSettings());

            // CONTROLS Button (replaces Characters Button)
            CreateMenuButton(btnContainer.transform, "ControlsBtn", "CONTROLS 🎮", font, Resources.Load<Sprite>("Sprites/ui_btn_yellow"), new Vector2(0, -65), new Vector2(240, 52), 20,
                () => manager.OpenControls());

            // QUIT Button (Bottom Right)
            CreateMenuButton(panel.transform, "QuitBtn", "EXIT", font, Resources.Load<Sprite>("Sprites/ui_btn_red"), new Vector2(-100, 58), new Vector2(130, 48), 18,
                () => manager.OnQuitClicked(), new Vector2(1, 0), new Vector2(1, 0));

            return panel;
        }

        private GameObject BuildLevelSelectPanel(Transform canvas, Font font, MainMenuManager manager)
        {
            GameObject panel = new GameObject("LevelSelectPanel", typeof(RectTransform));
            panel.transform.SetParent(canvas, false);
            RectTransform rt = panel.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            Image darkBg = panel.AddComponent<Image>();
            darkBg.color = new Color(0, 0.08f, 0.2f, 0.88f);

            // Dialog Frame
            GameObject dialog = new GameObject("Dialog", typeof(RectTransform));
            dialog.transform.SetParent(panel.transform, false);
            RectTransform dRt = dialog.GetComponent<RectTransform>();
            dRt.sizeDelta = new Vector2(980, 620);
            Image dImg = dialog.AddComponent<Image>();
            dImg.sprite = Resources.Load<Sprite>("Sprites/ui_dialog_panel");

            // Header Title (centered in the blue ribbon)
            GameObject title = new GameObject("Title", typeof(RectTransform));
            title.transform.SetParent(dialog.transform, false);
            Text tTxt = title.AddComponent<Text>();
            tTxt.text = "SELECT LEVEL (1 - 30)";
            tTxt.font = font;
            tTxt.fontSize = 28;
            tTxt.fontStyle = FontStyle.Bold;
            tTxt.alignment = TextAnchor.MiddleCenter;
            tTxt.color = Color.white;
            RectTransform tRt = title.GetComponent<RectTransform>();
            tRt.anchorMin = new Vector2(0, 1);
            tRt.anchorMax = new Vector2(1, 1);
            tRt.pivot = new Vector2(0.5f, 0.5f);
            tRt.anchoredPosition = new Vector2(0, -56);
            tRt.sizeDelta = new Vector2(0, 48);

            // 30 Levels Grid Container (6 columns x 5 rows)
            GameObject grid = new GameObject("LevelGrid", typeof(RectTransform));
            grid.transform.SetParent(dialog.transform, false);
            RectTransform gRt = grid.GetComponent<RectTransform>();
            gRt.anchoredPosition = new Vector2(0, 10);
            gRt.sizeDelta = new Vector2(880, 420);

            GridLayoutGroup glg = grid.AddComponent<GridLayoutGroup>();
            glg.cellSize = new Vector2(125, 72);
            glg.spacing = new Vector2(16, 8);
            glg.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            glg.constraintCount = 6;
            glg.childAlignment = TextAnchor.MiddleCenter;

            manager.levelGridContainer = grid.transform;

            // BACK Button (comfortably lifted above bottom border)
            CreateMenuButton(dialog.transform, "BackBtn", "← BACK", font, Resources.Load<Sprite>("Sprites/ui_btn_blue"), new Vector2(0, -240), new Vector2(180, 46), 18,
                () => manager.ShowTitlePanel());

            panel.SetActive(false);
            return panel;
        }

        private GameObject BuildCharacterSelectPanel(Transform canvas, Font font, MainMenuManager manager)
        {
            GameObject panel = new GameObject("CharacterSelectPanel", typeof(RectTransform));
            panel.transform.SetParent(canvas, false);
            RectTransform rt = panel.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            Image darkBg = panel.AddComponent<Image>();
            darkBg.color = new Color(0, 0.08f, 0.2f, 0.88f);

            // Dialog Frame
            GameObject dialog = new GameObject("Dialog", typeof(RectTransform));
            dialog.transform.SetParent(panel.transform, false);
            RectTransform dRt = dialog.GetComponent<RectTransform>();
            dRt.sizeDelta = new Vector2(1040, 620);
            Image dImg = dialog.AddComponent<Image>();
            dImg.sprite = Resources.Load<Sprite>("Sprites/ui_dialog_panel");

            // Header Title (centered in the blue ribbon)
            GameObject title = new GameObject("Title", typeof(RectTransform));
            title.transform.SetParent(dialog.transform, false);
            Text tTxt = title.AddComponent<Text>();
            tTxt.text = "SNOWBALL ROSTER (10 CHARACTERS)";
            tTxt.font = font;
            tTxt.fontSize = 28;
            tTxt.fontStyle = FontStyle.Bold;
            tTxt.alignment = TextAnchor.MiddleCenter;
            tTxt.color = Color.white;
            RectTransform tRt = title.GetComponent<RectTransform>();
            tRt.anchorMin = new Vector2(0, 1);
            tRt.anchorMax = new Vector2(1, 1);
            tRt.pivot = new Vector2(0.5f, 0.5f);
            tRt.anchoredPosition = new Vector2(0, -56);
            tRt.sizeDelta = new Vector2(0, 48);
            manager.characterPanelTitleText = tTxt;

            // Left Side: Active Character Preview Showcase
            GameObject showcase = new GameObject("Showcase", typeof(RectTransform));
            showcase.transform.SetParent(dialog.transform, false);
            RectTransform scRt = showcase.GetComponent<RectTransform>();
            scRt.anchoredPosition = new Vector2(-280, 5);
            scRt.sizeDelta = new Vector2(300, 420);

            GameObject prevImgObj = new GameObject("ShowcaseAvatar", typeof(RectTransform));
            prevImgObj.transform.SetParent(showcase.transform, false);
            Image scImg = prevImgObj.AddComponent<Image>();
            scImg.sprite = GameDataManager.GetActiveCharacterSprite();
            RectTransform scImgRt = prevImgObj.GetComponent<RectTransform>();
            scImgRt.anchoredPosition = new Vector2(0, 80);
            scImgRt.sizeDelta = new Vector2(160, 160);
            manager.activePreviewImage = scImg;

            GameObject scNameObj = new GameObject("ShowcaseName", typeof(RectTransform));
            scNameObj.transform.SetParent(showcase.transform, false);
            Text scName = scNameObj.AddComponent<Text>();
            scName.text = "Frosty Classic";
            scName.font = font;
            scName.fontSize = 22;
            scName.fontStyle = FontStyle.Bold;
            scName.alignment = TextAnchor.MiddleCenter;
            scName.color = new Color(0.08f, 0.18f, 0.38f, 1f);
            RectTransform scNameRt = scNameObj.GetComponent<RectTransform>();
            scNameRt.anchoredPosition = new Vector2(0, -25);
            scNameRt.sizeDelta = new Vector2(280, 35);
            manager.activeNameText = scName;

            GameObject scDescObj = new GameObject("ShowcaseDesc", typeof(RectTransform));
            scDescObj.transform.SetParent(showcase.transform, false);
            Text scDesc = scDescObj.AddComponent<Text>();
            scDesc.text = "The cheerful original adventurer! Well-balanced and loves rolling on fresh powder.";
            scDesc.font = font;
            scDesc.fontSize = 15;
            scDesc.alignment = TextAnchor.MiddleCenter;
            scDesc.color = new Color(0.18f, 0.28f, 0.44f, 1f);
            RectTransform scDescRt = scDescObj.GetComponent<RectTransform>();
            scDescRt.anchoredPosition = new Vector2(0, -90);
            scDescRt.sizeDelta = new Vector2(270, 80);
            manager.activeDescText = scDesc;

            // Right Side: Scrollable Roster of 10 Characters
            GameObject rosterObj = new GameObject("RosterGrid", typeof(RectTransform));
            rosterObj.transform.SetParent(dialog.transform, false);
            RectTransform rRt = rosterObj.GetComponent<RectTransform>();
            rRt.anchoredPosition = new Vector2(160, 5);
            rRt.sizeDelta = new Vector2(600, 430);

            GridLayoutGroup rGlg = rosterObj.AddComponent<GridLayoutGroup>();
            rGlg.cellSize = new Vector2(285, 75);
            rGlg.spacing = new Vector2(15, 10);
            rGlg.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            rGlg.constraintCount = 2;
            rGlg.childAlignment = TextAnchor.UpperLeft;

            manager.characterListContainer = rosterObj.transform;

            // BACK Button (returns to Level Select if in play flow, or Title if browsing)
            CreateMenuButton(dialog.transform, "BackBtn", "← BACK", font, Resources.Load<Sprite>("Sprites/ui_btn_blue"), new Vector2(-130, -240), new Vector2(180, 46), 18,
                () => manager.OnCharacterBackClicked());

            // START GAME Button (launches GameScene with chosen level and character)
            GameObject startBtn = CreateMenuButton(dialog.transform, "StartBtn", "PLAY LEVEL 1 ▶", font, Resources.Load<Sprite>("Sprites/ui_btn_green"), new Vector2(130, -240), new Vector2(240, 46), 18,
                () => manager.OnStartGameClicked());
            manager.startButtonText = startBtn.GetComponentInChildren<Text>();

            panel.SetActive(false);
            return panel;
        }

        private GameObject BuildSettingsPanel(Transform canvas, Font font, MainMenuManager manager)
        {
            GameObject panel = new GameObject("SettingsPanel", typeof(RectTransform));
            panel.transform.SetParent(canvas, false);
            RectTransform rt = panel.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            Image darkBg = panel.AddComponent<Image>();
            darkBg.color = new Color(0, 0.08f, 0.2f, 0.88f);

            // Dialog Frame
            GameObject dialog = new GameObject("Dialog", typeof(RectTransform));
            dialog.transform.SetParent(panel.transform, false);
            RectTransform dRt = dialog.GetComponent<RectTransform>();
            dRt.sizeDelta = new Vector2(520, 420);
            Image dImg = dialog.AddComponent<Image>();
            dImg.sprite = Resources.Load<Sprite>("Sprites/ui_dialog_panel");

            // Header Title (centered in the top blue ribbon)
            GameObject title = new GameObject("Title", typeof(RectTransform));
            title.transform.SetParent(dialog.transform, false);
            Text tTxt = title.AddComponent<Text>();
            tTxt.text = "SETTINGS";
            tTxt.font = font;
            tTxt.fontSize = 28;
            tTxt.fontStyle = FontStyle.Bold;
            tTxt.alignment = TextAnchor.MiddleCenter;
            tTxt.color = Color.white;
            RectTransform tRt = title.GetComponent<RectTransform>();
            tRt.anchorMin = new Vector2(0, 1);
            tRt.anchorMax = new Vector2(1, 1);
            tRt.pivot = new Vector2(0.5f, 0.5f);
            tRt.anchoredPosition = new Vector2(0, -50);
            tRt.sizeDelta = new Vector2(0, 48);

            // Subtitle on frosted body (dark blue text)
            GameObject subObj = new GameObject("Subtitle", typeof(RectTransform));
            subObj.transform.SetParent(dialog.transform, false);
            Text subTxt = subObj.AddComponent<Text>();
            subTxt.text = "Audio & Sound Preferences";
            subTxt.font = font;
            subTxt.fontSize = 18;
            subTxt.fontStyle = FontStyle.Bold;
            subTxt.alignment = TextAnchor.MiddleCenter;
            subTxt.color = new Color(0.08f, 0.18f, 0.38f, 1f);
            RectTransform sRt = subObj.GetComponent<RectTransform>();
            sRt.anchoredPosition = new Vector2(0, 60);
            sRt.sizeDelta = new Vector2(400, 30);

            // Sound Toggle Button (controls SFX only)
            bool sfxOn = GameDataManager.Data.isSfxEnabled;
            GameObject soundBtn = CreateMenuButton(dialog.transform, "SoundToggleBtn", sfxOn ? "SOUND: ON" : "SOUND: OFF", font,
                Resources.Load<Sprite>(sfxOn ? "Sprites/ui_btn_green" : "Sprites/ui_btn_red"),
                new Vector2(0, 10), new Vector2(260, 52), 20,
                () => manager.ToggleSoundFromSettings());
            manager.soundSettingBtnImg = soundBtn.GetComponent<Image>();
            manager.soundSettingText = soundBtn.GetComponentInChildren<Text>();

            // Music Toggle Button (controls Background Music only)
            bool musicOn = GameDataManager.Data.isMusicEnabled;
            GameObject musicBtn = CreateMenuButton(dialog.transform, "MusicToggleBtn", musicOn ? "MUSIC: ON" : "MUSIC: OFF", font,
                Resources.Load<Sprite>(musicOn ? "Sprites/ui_btn_green" : "Sprites/ui_btn_red"),
                new Vector2(0, -55), new Vector2(260, 52), 20,
                () => manager.ToggleMusicFromSettings());
            manager.musicSettingBtnImg = musicBtn.GetComponent<Image>();
            manager.musicSettingText = musicBtn.GetComponentInChildren<Text>();

            // Description note (dark blue text)
            GameObject noteObj = new GameObject("Note", typeof(RectTransform));
            noteObj.transform.SetParent(dialog.transform, false);
            Text noteTxt = noteObj.AddComponent<Text>();
            noteTxt.text = "Sound: SFX & impacts • Music: Background soundtrack";
            noteTxt.font = font;
            noteTxt.fontSize = 13;
            noteTxt.fontStyle = FontStyle.Bold;
            noteTxt.alignment = TextAnchor.MiddleCenter;
            noteTxt.color = new Color(0.2f, 0.32f, 0.48f, 1f);
            RectTransform noteRt = noteObj.GetComponent<RectTransform>();
            noteRt.anchoredPosition = new Vector2(0, -112);
            noteRt.sizeDelta = new Vector2(450, 28);

            // BACK Button
            CreateMenuButton(dialog.transform, "BackBtn", "← BACK", font, Resources.Load<Sprite>("Sprites/ui_btn_blue"),
                new Vector2(0, -150), new Vector2(180, 46), 18,
                () => manager.CloseSettings());

            panel.SetActive(false);
            return panel;
        }

        private GameObject BuildControlsPanel(Transform canvas, Font font, MainMenuManager manager)
        {
            GameObject panel = new GameObject("ControlsPanel", typeof(RectTransform));
            panel.transform.SetParent(canvas, false);
            RectTransform rt = panel.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            Image darkBg = panel.AddComponent<Image>();
            darkBg.color = new Color(0, 0.08f, 0.2f, 0.88f);

            // Dialog Frame
            GameObject dialog = new GameObject("Dialog", typeof(RectTransform));
            dialog.transform.SetParent(panel.transform, false);
            RectTransform dRt = dialog.GetComponent<RectTransform>();
            dRt.sizeDelta = new Vector2(660, 530);
            Image dImg = dialog.AddComponent<Image>();
            dImg.sprite = Resources.Load<Sprite>("Sprites/ui_dialog_panel");

            // Header Title (centered in the top blue ribbon)
            GameObject title = new GameObject("Title", typeof(RectTransform));
            title.transform.SetParent(dialog.transform, false);
            Text tTxt = title.AddComponent<Text>();
            tTxt.text = "GAME CONTROLS";
            tTxt.font = font;
            tTxt.fontSize = 28;
            tTxt.fontStyle = FontStyle.Bold;
            tTxt.alignment = TextAnchor.MiddleCenter;
            tTxt.color = Color.white;
            RectTransform tRt = title.GetComponent<RectTransform>();
            tRt.anchorMin = new Vector2(0, 1);
            tRt.anchorMax = new Vector2(1, 1);
            tRt.pivot = new Vector2(0.5f, 0.5f);
            tRt.anchoredPosition = new Vector2(0, -54);
            tRt.sizeDelta = new Vector2(0, 48);

            // Content text on frosted body (dark blue text)
            GameObject contentObj = new GameObject("Content", typeof(RectTransform));
            contentObj.transform.SetParent(dialog.transform, false);
            Text cTxt = contentObj.AddComponent<Text>();
            cTxt.font = font;
            cTxt.fontSize = 15;
            cTxt.lineSpacing = 1.25f;
            cTxt.alignment = TextAnchor.UpperLeft;
            cTxt.color = new Color(0.08f, 0.18f, 0.38f, 1f);
            cTxt.text =
                "KEYBOARD (PC):\n" +
                "  • Move Left / Right : A / D  or  Left / Right Arrow Keys\n" +
                "  • Jump / Leap : W  or  Up Arrow Key  or  Spacebar\n" +
                "  • Pause Game : Escape  or  P\n\n" +
                "TOUCH (MOBILE):\n" +
                "  • Move Left / Right : Left & Right On-screen Arrows\n" +
                "  • Jump : Round Jump Button (Bottom Right)\n" +
                "  • Pause : Pause Button (Top Right)\n\n" +
                "GAME OBJECTIVES:\n" +
                "  • Collect Coins to unlock cool Snowball Characters!\n" +
                "  • Dodge Spikes, Pitfalls, and hazardous traps.\n" +
                "  • Reach the Checkered Finish Flag to complete each level!\n" +
                "  • 3 Lives per run + 1 Last Chance Ad Revive!";

            RectTransform cRt = contentObj.GetComponent<RectTransform>();
            cRt.anchoredPosition = new Vector2(0, -6);
            cRt.sizeDelta = new Vector2(560, 330);

            // BACK Button
            CreateMenuButton(dialog.transform, "BackBtn", "← BACK", font, Resources.Load<Sprite>("Sprites/ui_btn_blue"),
                new Vector2(0, -205), new Vector2(180, 46), 18,
                () => manager.CloseControls());

            panel.SetActive(false);
            return panel;
        }

        private GameObject CreateMenuButton(Transform parent, string name, string text, Font font, Sprite spr, Vector2 pos, Vector2 size, int fontSize, UnityEngine.Events.UnityAction onClick, Vector2? anchor = null, Vector2? pivot = null)
        {
            GameObject btnObj = new GameObject(name, typeof(RectTransform));
            btnObj.transform.SetParent(parent, false);

            RectTransform rt = btnObj.GetComponent<RectTransform>();
            if (anchor.HasValue)
            {
                rt.anchorMin = anchor.Value;
                rt.anchorMax = anchor.Value;
            }
            if (pivot.HasValue) rt.pivot = pivot.Value;
            rt.anchoredPosition = pos;
            rt.sizeDelta = size;

            Image img = btnObj.AddComponent<Image>();
            img.sprite = spr;

            Button btn = btnObj.AddComponent<Button>();
            if (onClick != null) btn.onClick.AddListener(onClick);

            GameObject txtObj = new GameObject("Text", typeof(RectTransform));
            txtObj.transform.SetParent(btnObj.transform, false);
            Text txt = txtObj.AddComponent<Text>();
            txt.text = text;
            txt.font = font;
            txt.fontSize = fontSize;
            txt.fontStyle = FontStyle.Bold;
            txt.alignment = TextAnchor.MiddleCenter;
            txt.color = Color.white;

            RectTransform txtRt = txtObj.GetComponent<RectTransform>();
            txtRt.anchorMin = Vector2.zero;
            txtRt.anchorMax = Vector2.one;
            txtRt.offsetMin = Vector2.zero;
            txtRt.offsetMax = Vector2.zero;

            return btnObj;
        }
    }
}
