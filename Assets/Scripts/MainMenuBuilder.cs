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
            bcRt.anchoredPosition = new Vector2(0, -155);
            bcRt.sizeDelta = new Vector2(340, 260);

            // PLAY Button (Enlarged)
            CreateMenuButton(btnContainer.transform, "PlayBtn", "PLAY", font, Resources.Load<Sprite>("Sprites/ui_btn_green"), new Vector2(0, 78), new Vector2(300, 68), 28,
                () => manager.OnPlayClicked());

            // SETTINGS Button (Enlarged)
            CreateMenuButton(btnContainer.transform, "SettingsBtn", "SETTINGS", font, Resources.Load<Sprite>("Sprites/ui_btn_blue"), new Vector2(0, 0), new Vector2(300, 62), 24,
                () => manager.OpenSettings());

            // CONTROLS Button (Enlarged)
            CreateMenuButton(btnContainer.transform, "ControlsBtn", "CONTROLS", font, Resources.Load<Sprite>("Sprites/ui_btn_yellow"), new Vector2(0, -78), new Vector2(300, 62), 24,
                () => manager.OpenControls());

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

            // Deep Midnight Aurora Frost Tint
            Image darkBg = panel.AddComponent<Image>();
            darkBg.color = new Color(0.01f, 0.04f, 0.10f, 0.94f);

            // 1. Top Header Bar
            GameObject header = new GameObject("HeaderBar", typeof(RectTransform));
            header.transform.SetParent(panel.transform, false);
            RectTransform hRt = header.GetComponent<RectTransform>();
            hRt.anchorMin = new Vector2(0, 1);
            hRt.anchorMax = new Vector2(1, 1);
            hRt.pivot = new Vector2(0.5f, 1);
            hRt.anchoredPosition = new Vector2(0, -10);
            hRt.sizeDelta = new Vector2(0, 70);

            // Top-Left Back Button
            CreateMenuButton(header.transform, "BackBtn", "BACK", font, Resources.Load<Sprite>("Sprites/ui_btn_blue"),
                new Vector2(90, -30), new Vector2(140, 44), 18, () => manager.ShowTitlePanel(),
                new Vector2(0, 0.5f), new Vector2(0.5f, 0.5f));

            // Center Title & Subtitle
            GameObject titleObj = new GameObject("Title", typeof(RectTransform));
            titleObj.transform.SetParent(header.transform, false);
            Text tTxt = titleObj.AddComponent<Text>();
            tTxt.text = "SELECT LEVEL";
            tTxt.font = font;
            tTxt.fontSize = 28;
            tTxt.fontStyle = FontStyle.Bold;
            tTxt.alignment = TextAnchor.MiddleCenter;
            tTxt.color = Color.white;
            Outline tOut = titleObj.AddComponent<Outline>();
            tOut.effectColor = new Color(0.05f, 0.2f, 0.45f, 1f);
            tOut.effectDistance = new Vector2(1.5f, -1.5f);
            RectTransform tRt = titleObj.GetComponent<RectTransform>();
            tRt.anchoredPosition = new Vector2(0, -16);
            tRt.sizeDelta = new Vector2(400, 36);

            // 2. Center World Heading & Page Indicator (Shifted down, clean name without bracketed numbers)
            GameObject worldHeader = new GameObject("WorldHeader", typeof(RectTransform));
            worldHeader.transform.SetParent(panel.transform, false);
            RectTransform whRt = worldHeader.GetComponent<RectTransform>();
            whRt.anchorMin = new Vector2(0.5f, 1);
            whRt.anchorMax = new Vector2(0.5f, 1);
            whRt.pivot = new Vector2(0.5f, 1);
            whRt.anchoredPosition = new Vector2(0, -92);
            whRt.sizeDelta = new Vector2(400, 46);

            Image whImg = worldHeader.AddComponent<Image>();
            whImg.sprite = Resources.Load<Sprite>("Sprites/ui_world_tab_active");
            whImg.color = new Color(0.12f, 0.42f, 0.78f, 0.95f);

            GameObject whTxtObj = new GameObject("TitleText", typeof(RectTransform));
            whTxtObj.transform.SetParent(worldHeader.transform, false);
            Text whTxt = whTxtObj.AddComponent<Text>();
            whTxt.text = "❄️ FROST PEAKS";
            whTxt.font = font;
            whTxt.fontSize = 22;
            whTxt.fontStyle = FontStyle.Bold;
            whTxt.alignment = TextAnchor.MiddleCenter;
            whTxt.color = Color.white;
            Outline whOut = whTxtObj.AddComponent<Outline>();
            whOut.effectColor = new Color(0.04f, 0.15f, 0.35f, 1f);
            whOut.effectDistance = new Vector2(1.5f, -1.5f);
            RectTransform whTxtRt = whTxtObj.GetComponent<RectTransform>();
            whTxtRt.anchorMin = Vector2.zero;
            whTxtRt.anchorMax = Vector2.one;
            whTxtRt.offsetMin = Vector2.zero;
            whTxtRt.offsetMax = Vector2.zero;
            manager.currentWorldTitleText = whTxt;

            // Page Dots under World Title (Shifted down)
            GameObject dotsObj = new GameObject("WorldDots", typeof(RectTransform));
            dotsObj.transform.SetParent(panel.transform, false);
            Text dotsTxt = dotsObj.AddComponent<Text>();
            dotsTxt.supportRichText = true;
            dotsTxt.text = "<color=#40e0ff>●</color>   <color=#507090>○</color>   <color=#507090>○</color>";
            dotsTxt.font = font;
            dotsTxt.fontSize = 15;
            dotsTxt.alignment = TextAnchor.MiddleCenter;
            RectTransform dotsRt = dotsObj.GetComponent<RectTransform>();
            dotsRt.anchorMin = new Vector2(0.5f, 1);
            dotsRt.anchorMax = new Vector2(0.5f, 1);
            dotsRt.pivot = new Vector2(0.5f, 1);
            dotsRt.anchoredPosition = new Vector2(0, -142);
            dotsRt.sizeDelta = new Vector2(200, 20);
            manager.worldPageIndicatorText = dotsTxt;

            // Left & Right World Navigation Arrow Buttons (Shifted down to align with new grid position)
            GameObject prevBtn = CreateMenuButton(panel.transform, "PrevWorldBtn", "◀", font,
                Resources.Load<Sprite>("Sprites/ui_btn_blue"), new Vector2(-590, -56), new Vector2(58, 84), 30,
                () => manager.PrevWorld());
            manager.prevWorldButton = prevBtn.GetComponent<Button>();

            GameObject nextBtn = CreateMenuButton(panel.transform, "NextWorldBtn", "▶", font,
                Resources.Load<Sprite>("Sprites/ui_btn_blue"), new Vector2(590, -56), new Vector2(58, 84), 30,
                () => manager.NextWorld());
            manager.nextWorldButton = nextBtn.GetComponent<Button>();

            // 3. 10 Levels Grid Container (5 columns x 2 rows, shifted down)
            GameObject grid = new GameObject("LevelGrid", typeof(RectTransform));
            grid.transform.SetParent(panel.transform, false);
            RectTransform gRt = grid.GetComponent<RectTransform>();
            gRt.anchoredPosition = new Vector2(0, -56);
            gRt.sizeDelta = new Vector2(1080, 420);

            GridLayoutGroup glg = grid.AddComponent<GridLayoutGroup>();
            glg.cellSize = new Vector2(195, 185);
            glg.spacing = new Vector2(20, 18);
            glg.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            glg.constraintCount = 5;
            glg.childAlignment = TextAnchor.MiddleCenter;

            manager.levelGridContainer = grid.transform;

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

            // Deep Midnight Aurora Frost Tint
            Image darkBg = panel.AddComponent<Image>();
            darkBg.color = new Color(0.01f, 0.04f, 0.10f, 0.94f);

            // 1. Top Header Bar
            GameObject header = new GameObject("HeaderBar", typeof(RectTransform));
            header.transform.SetParent(panel.transform, false);
            RectTransform hRt = header.GetComponent<RectTransform>();
            hRt.anchorMin = new Vector2(0, 1);
            hRt.anchorMax = new Vector2(1, 1);
            hRt.pivot = new Vector2(0.5f, 1);
            hRt.anchoredPosition = new Vector2(0, -10);
            hRt.sizeDelta = new Vector2(0, 70);

            // Back Button
            CreateMenuButton(header.transform, "BackBtn", "BACK", font, Resources.Load<Sprite>("Sprites/ui_btn_blue"),
                new Vector2(90, -30), new Vector2(140, 44), 18, () => manager.OnCharacterBackClicked(),
                new Vector2(0, 0.5f), new Vector2(0.5f, 0.5f));

            // Center Title
            GameObject titleObj = new GameObject("Title", typeof(RectTransform));
            titleObj.transform.SetParent(header.transform, false);
            Text tTxt = titleObj.AddComponent<Text>();
            tTxt.text = "SNOWBALL HERO ROSTER";
            tTxt.font = font;
            tTxt.fontSize = 28;
            tTxt.fontStyle = FontStyle.Bold;
            tTxt.alignment = TextAnchor.MiddleCenter;
            tTxt.color = Color.white;
            Outline tOut = titleObj.AddComponent<Outline>();
            tOut.effectColor = new Color(0.05f, 0.2f, 0.45f, 1f);
            tOut.effectDistance = new Vector2(1.5f, -1.5f);
            RectTransform tRt = titleObj.GetComponent<RectTransform>();
            tRt.anchoredPosition = new Vector2(0, -30);
            tRt.sizeDelta = new Vector2(500, 36);
            manager.characterPanelTitleText = tTxt;

            // 2. HERO SPOTLIGHT STAGE (Left Area: Center x = -260, y = 50)
            GameObject stageObj = new GameObject("HeroStage", typeof(RectTransform));
            stageObj.transform.SetParent(panel.transform, false);
            RectTransform stageRt = stageObj.GetComponent<RectTransform>();
            stageRt.anchoredPosition = new Vector2(-260, 40);
            stageRt.sizeDelta = new Vector2(340, 360);

            // Ambient Glow Ring
            GameObject ringObj = new GameObject("AuraRing", typeof(RectTransform));
            ringObj.transform.SetParent(stageObj.transform, false);
            Image ringImg = ringObj.AddComponent<Image>();
            ringImg.sprite = Resources.Load<Sprite>("Sprites/ui_aura_ring");
            RectTransform ringRt = ringObj.GetComponent<RectTransform>();
            ringRt.anchoredPosition = new Vector2(0, 45);
            ringRt.sizeDelta = new Vector2(270, 270);
            manager.heroAuraRingRt = ringRt;

            // Crystalline Pedestal
            GameObject pedObj = new GameObject("Pedestal", typeof(RectTransform));
            pedObj.transform.SetParent(stageObj.transform, false);
            Image pedImg = pedObj.AddComponent<Image>();
            pedImg.sprite = Resources.Load<Sprite>("Sprites/ui_pedestal");
            RectTransform pedRt = pedObj.GetComponent<RectTransform>();
            pedRt.anchoredPosition = new Vector2(0, -65);
            pedRt.sizeDelta = new Vector2(250, 94);

            // Active Ball Hero Avatar
            GameObject ballObj = new GameObject("HeroBall", typeof(RectTransform));
            ballObj.transform.SetParent(stageObj.transform, false);
            Image ballImg = ballObj.AddComponent<Image>();
            ballImg.sprite = GameDataManager.GetActiveCharacterSprite();
            RectTransform ballRt = ballObj.GetComponent<RectTransform>();
            ballRt.anchoredPosition = new Vector2(0, 45);
            ballRt.sizeDelta = new Vector2(165, 165);
            manager.activePreviewImage = ballImg;
            manager.heroStageBallRt = ballRt;

            // 3. HERO DOSSIER & ATTRIBUTES (Right Area: Center x = 200, y = 40)
            GameObject dossier = new GameObject("HeroDossier", typeof(RectTransform));
            dossier.transform.SetParent(panel.transform, false);
            RectTransform dRt = dossier.GetComponent<RectTransform>();
            dRt.anchoredPosition = new Vector2(200, 40);
            dRt.sizeDelta = new Vector2(560, 360);

            Image dImg = dossier.AddComponent<Image>();
            dImg.sprite = Resources.Load<Sprite>("Sprites/ui_card_glass_unlocked");
            dImg.color = new Color(0.08f, 0.22f, 0.44f, 0.95f);

            // Hero Name - Centered Horizontally
            GameObject nameObj = new GameObject("HeroName", typeof(RectTransform));
            nameObj.transform.SetParent(dossier.transform, false);
            Text hName = nameObj.AddComponent<Text>();
            hName.text = "COSMO";
            hName.font = font;
            hName.fontSize = 32;
            hName.fontStyle = FontStyle.Bold;
            hName.alignment = TextAnchor.MiddleCenter;
            hName.color = Color.white;
            Outline nOut = nameObj.AddComponent<Outline>();
            nOut.effectColor = new Color(0.05f, 0.15f, 0.35f, 1f);
            nOut.effectDistance = new Vector2(1.5f, -1.5f);
            RectTransform nRt = nameObj.GetComponent<RectTransform>();
            nRt.anchoredPosition = new Vector2(0, 134);
            nRt.sizeDelta = new Vector2(500, 38);
            manager.activeNameText = hName;

            // Hero Title Badge - Centered Horizontally
            GameObject tagObj = new GameObject("HeroTitleBadge", typeof(RectTransform));
            tagObj.transform.SetParent(dossier.transform, false);
            Text hTitle = tagObj.AddComponent<Text>();
            hTitle.text = "★ COSMIC NEBULA";
            hTitle.font = font;
            hTitle.fontSize = 17;
            hTitle.fontStyle = FontStyle.Bold;
            hTitle.alignment = TextAnchor.MiddleCenter;
            hTitle.color = new Color(1f, 0.85f, 0.3f, 1f);
            RectTransform tagRt = tagObj.GetComponent<RectTransform>();
            tagRt.anchoredPosition = new Vector2(0, 100);
            tagRt.sizeDelta = new Vector2(500, 26);
            manager.heroTitleText = hTitle;
            manager.activeDescText = null;

            // 4 STAT BARS HELPER
            Sprite barBgSpr = Resources.Load<Sprite>("Sprites/ui_stat_bar_bg");
            Sprite barFillSpr = Resources.Load<Sprite>("Sprites/ui_stat_bar_fill");

            // Stat 1: SPEED
            CreateStatRow(dossier.transform, font, barBgSpr, barFillSpr, "⚡ SPEED", 34, out manager.speedBarFill, out manager.speedValText);
            // Stat 2: TRACTION & CONTROL
            CreateStatRow(dossier.transform, font, barBgSpr, barFillSpr, "🛡️ CONTROL", 0, out manager.controlBarFill, out manager.controlValText);
            // Stat 3: BOUNCE
            CreateStatRow(dossier.transform, font, barBgSpr, barFillSpr, "🦘 BOUNCE", -34, out manager.bounceBarFill, out manager.bounceValText);

            // Special Perk Row
            GameObject perkObj = new GameObject("SpecialPerkRow", typeof(RectTransform));
            perkObj.transform.SetParent(dossier.transform, false);
            RectTransform pRt = perkObj.GetComponent<RectTransform>();
            pRt.anchoredPosition = new Vector2(0, -74);
            pRt.sizeDelta = new Vector2(480, 26);

            Text pTxt = perkObj.AddComponent<Text>();
            pTxt.text = "SPECIAL: 🪐 Graviton Singularity";
            pTxt.font = font;
            pTxt.fontSize = 14;
            pTxt.fontStyle = FontStyle.Bold;
            pTxt.alignment = TextAnchor.MiddleCenter;
            pTxt.color = new Color(0.4f, 0.95f, 0.85f, 1f);
            manager.heroSpecialPerkText = pTxt;

            // Equip / Play Action Button
            GameObject actionBtn = CreateMenuButton(dossier.transform, "ActionButton", "★ EQUIPPED", font,
                Resources.Load<Sprite>("Sprites/ui_btn_green"), new Vector2(0, -132), new Vector2(280, 46), 18,
                () => manager.OnHeroActionClicked());
            manager.heroActionButton = actionBtn.GetComponent<Button>();
            manager.heroActionBtnImg = actionBtn.GetComponent<Image>();
            manager.heroActionBtnText = actionBtn.GetComponentInChildren<Text>();
            manager.startButtonText = manager.heroActionBtnText;

            // 4. BOTTOM HORIZONTAL CHARACTER DOCK (Carousel Strip across the bottom)
            GameObject dock = new GameObject("CharacterDock", typeof(RectTransform));
            dock.transform.SetParent(panel.transform, false);
            RectTransform dockRt = dock.GetComponent<RectTransform>();
            dockRt.anchorMin = new Vector2(0.5f, 0);
            dockRt.anchorMax = new Vector2(0.5f, 0);
            dockRt.pivot = new Vector2(0.5f, 0);
            dockRt.anchoredPosition = new Vector2(0, 15);
            dockRt.sizeDelta = new Vector2(1160, 132);

            HorizontalLayoutGroup dhlg = dock.AddComponent<HorizontalLayoutGroup>();
            dhlg.childAlignment = TextAnchor.MiddleCenter;
            dhlg.spacing = 10;
            dhlg.childControlWidth = false;
            dhlg.childControlHeight = false;

            manager.characterListContainer = dock.transform;

            panel.SetActive(false);
            return panel;
        }

        private void CreateStatRow(Transform parent, Font font, Sprite bgSpr, Sprite fillSpr, string label, float yPos, out RectTransform fillRt, out Text valTxt)
        {
            GameObject row = new GameObject("Stat_" + label.Replace(" ", ""), typeof(RectTransform));
            row.transform.SetParent(parent, false);
            RectTransform rRt = row.GetComponent<RectTransform>();
            rRt.anchoredPosition = new Vector2(0, yPos);
            rRt.sizeDelta = new Vector2(480, 24);

            // Label Text (Left)
            GameObject lblObj = new GameObject("Label", typeof(RectTransform));
            lblObj.transform.SetParent(row.transform, false);
            Text lTxt = lblObj.AddComponent<Text>();
            lTxt.text = label;
            lTxt.font = font;
            lTxt.fontSize = 13;
            lTxt.fontStyle = FontStyle.Bold;
            lTxt.alignment = TextAnchor.MiddleLeft;
            lTxt.color = new Color(0.7f, 0.88f, 1f, 1f);
            RectTransform lRt = lblObj.GetComponent<RectTransform>();
            lRt.anchoredPosition = new Vector2(-155, 0);
            lRt.sizeDelta = new Vector2(130, 22);

            // Track Background
            GameObject track = new GameObject("Track", typeof(RectTransform));
            track.transform.SetParent(row.transform, false);
            Image tImg = track.AddComponent<Image>();
            tImg.sprite = bgSpr;
            RectTransform tRt = track.GetComponent<RectTransform>();
            tRt.anchoredPosition = new Vector2(25, 0);
            tRt.sizeDelta = new Vector2(220, 18);

            // Fill Bar
            GameObject fill = new GameObject("Fill", typeof(RectTransform));
            fill.transform.SetParent(track.transform, false);
            Image fImg = fill.AddComponent<Image>();
            fImg.sprite = fillSpr;
            fillRt = fill.GetComponent<RectTransform>();
            fillRt.anchorMin = new Vector2(0, 0.5f);
            fillRt.anchorMax = new Vector2(0, 0.5f);
            fillRt.pivot = new Vector2(0, 0.5f);
            fillRt.anchoredPosition = new Vector2(2, 0);
            fillRt.sizeDelta = new Vector2(180, 14);

            // Value Text (Right)
            GameObject valObj = new GameObject("Val", typeof(RectTransform));
            valObj.transform.SetParent(row.transform, false);
            valTxt = valObj.AddComponent<Text>();
            valTxt.text = "8.5";
            valTxt.font = font;
            valTxt.fontSize = 13;
            valTxt.fontStyle = FontStyle.Bold;
            valTxt.alignment = TextAnchor.MiddleRight;
            valTxt.color = Color.white;
            RectTransform vRt = valObj.GetComponent<RectTransform>();
            vRt.anchoredPosition = new Vector2(175, 0);
            vRt.sizeDelta = new Vector2(50, 22);
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

            // Subtitle on frosted body (dark blue text) - Enlarged
            GameObject subObj = new GameObject("Subtitle", typeof(RectTransform));
            subObj.transform.SetParent(dialog.transform, false);
            Text subTxt = subObj.AddComponent<Text>();
            subTxt.text = "Audio & Sound Preferences";
            subTxt.font = font;
            subTxt.fontSize = 24;
            subTxt.fontStyle = FontStyle.Bold;
            subTxt.alignment = TextAnchor.MiddleCenter;
            subTxt.color = new Color(0.08f, 0.18f, 0.38f, 1f);
            RectTransform sRt = subObj.GetComponent<RectTransform>();
            sRt.anchoredPosition = new Vector2(0, 65);
            sRt.sizeDelta = new Vector2(460, 36);

            // Sound Toggle Button (controls SFX only)
            bool sfxOn = GameDataManager.Data.isSfxEnabled;
            GameObject soundBtn = CreateMenuButton(dialog.transform, "SoundToggleBtn", sfxOn ? "SOUND: ON" : "SOUND: OFF", font,
                Resources.Load<Sprite>(sfxOn ? "Sprites/ui_btn_green" : "Sprites/ui_btn_red"),
                new Vector2(0, 8), new Vector2(280, 56), 22,
                () => manager.ToggleSoundFromSettings());
            manager.soundSettingBtnImg = soundBtn.GetComponent<Image>();
            manager.soundSettingText = soundBtn.GetComponentInChildren<Text>();

            // Music Toggle Button (controls Background Music only)
            bool musicOn = GameDataManager.Data.isMusicEnabled;
            GameObject musicBtn = CreateMenuButton(dialog.transform, "MusicToggleBtn", musicOn ? "MUSIC: ON" : "MUSIC: OFF", font,
                Resources.Load<Sprite>(musicOn ? "Sprites/ui_btn_green" : "Sprites/ui_btn_red"),
                new Vector2(0, -60), new Vector2(280, 56), 22,
                () => manager.ToggleMusicFromSettings());
            manager.musicSettingBtnImg = musicBtn.GetComponent<Image>();
            manager.musicSettingText = musicBtn.GetComponentInChildren<Text>();

            // BACK Button
            CreateMenuButton(dialog.transform, "BackBtn", "BACK", font, Resources.Load<Sprite>("Sprites/ui_btn_blue"),
                new Vector2(0, -135), new Vector2(190, 50), 20,
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

            // Content text on frosted body (bold, larger, dark text showing only Keyboard and Touch controls)
            GameObject contentObj = new GameObject("Content", typeof(RectTransform));
            contentObj.transform.SetParent(dialog.transform, false);
            Text cTxt = contentObj.AddComponent<Text>();
            cTxt.font = font;
            cTxt.fontSize = 19;
            cTxt.fontStyle = FontStyle.Bold;
            cTxt.lineSpacing = 1.35f;
            cTxt.alignment = TextAnchor.UpperLeft;
            cTxt.color = new Color(0.04f, 0.08f, 0.18f, 1f);
            cTxt.text =
                "KEYBOARD (PC):\n" +
                "  • Move Left / Right : A / D  or  Arrow Keys\n" +
                "  • Jump / Leap : W  or  Up Arrow  or  Spacebar\n" +
                "  • Pause Game : Escape  or  P\n\n" +
                "TOUCH (MOBILE):\n" +
                "  • Move Left / Right : Left & Right On-screen Arrows\n" +
                "  • Jump : Round Jump Button (Bottom Right)\n" +
                "  • Pause : Pause Button (Top Right)";

            RectTransform cRt = contentObj.GetComponent<RectTransform>();
            cRt.anchoredPosition = new Vector2(0, -10);
            cRt.sizeDelta = new Vector2(560, 290);

            // BACK Button
            CreateMenuButton(dialog.transform, "BackBtn", "BACK", font, Resources.Load<Sprite>("Sprites/ui_btn_blue"),
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
