using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SnowballAdventure
{
    public class Level1Builder : MonoBehaviour
    {
        [Header("Sprites")]
        public Sprite groundBlockSprite;
        public Sprite iceBlockSprite;
        public Sprite woodPlatformSprite;
        public Sprite pineTreeSprite;
        public Sprite coinSprite;
        public Sprite spikeSprite;
        public Sprite springSprite;
        public Sprite crumblingSprite;
        public Sprite frostSawSprite;
        public Sprite icicleSprite;
        public Sprite iceFloeSprite;
        public Sprite goalArchSprite;
        public Sprite flagIdleSprite;
        public Sprite flagActiveSprite;
        public Sprite windDraftSprite;
        public Sprite blinkingIceSprite;
        public Sprite speedBoostSprite;
        public Sprite frostBarSprite;

        [Header("World 2 Sprites")]
        public Sprite deepFrostGroundSprite;
        public Sprite crystalPlatformSprite;
        public Sprite trampolineSprite;
        public Sprite movingPlatformSprite;
        public Sprite swingingMaceSprite;
        public Sprite frostChainSprite;
        public Sprite portalBlueSprite;
        public Sprite portalOrangeSprite;

        [Header("World 3 Sprites")]
        public Sprite cosmicGroundSprite;
        public Sprite cosmicPlatformSprite;
        public Sprite gravityOrbSprite;
        public Sprite laserBeamSprite;
        public Sprite laserEmitterSprite;
        public Sprite crushingPillarSprite;
        public Sprite conveyorSprite;
        public Sprite gravityFlipPadSprite;
        public Sprite snowflakeSprite;

        private GameObject levelRoot;

        private void Awake()
        {
            LoadResources();
            BuildLevel();
            BuildHUD();
        }

        private void LoadResources()
        {
            groundBlockSprite = Resources.Load<Sprite>("Sprites/ground_snow_block");
            iceBlockSprite = Resources.Load<Sprite>("Sprites/ice_platform_block");
            woodPlatformSprite = Resources.Load<Sprite>("Sprites/wooden_frost_platform");
            pineTreeSprite = Resources.Load<Sprite>("Sprites/winter_pine_tree");
            coinSprite = Resources.Load<Sprite>("Sprites/coin_gold");
            spikeSprite = Resources.Load<Sprite>("Sprites/ice_spikes");
            springSprite = Resources.Load<Sprite>("Sprites/bounce_spring");
            crumblingSprite = Resources.Load<Sprite>("Sprites/crumbling_ice");
            frostSawSprite = Resources.Load<Sprite>("Sprites/frost_saw");
            icicleSprite = Resources.Load<Sprite>("Sprites/falling_icicle");
            iceFloeSprite = Resources.Load<Sprite>("Sprites/ice_floe");
            goalArchSprite = Resources.Load<Sprite>("Sprites/level_goal_arch");
            flagIdleSprite = Resources.Load<Sprite>("Sprites/checkpoint_flag_idle");
            flagActiveSprite = Resources.Load<Sprite>("Sprites/checkpoint_flag_active");
            windDraftSprite = Resources.Load<Sprite>("Sprites/wind_draft");
            blinkingIceSprite = Resources.Load<Sprite>("Sprites/blinking_ice");
            speedBoostSprite = Resources.Load<Sprite>("Sprites/speed_boost");
            frostBarSprite = Resources.Load<Sprite>("Sprites/frost_bar");

            deepFrostGroundSprite = Resources.Load<Sprite>("Sprites/ground_frost_deep");
            crystalPlatformSprite = Resources.Load<Sprite>("Sprites/platform_crystal");
            trampolineSprite = Resources.Load<Sprite>("Sprites/frost_trampoline");
            movingPlatformSprite = Resources.Load<Sprite>("Sprites/moving_ice_platform");
            swingingMaceSprite = Resources.Load<Sprite>("Sprites/swinging_frost_mace");
            frostChainSprite = Resources.Load<Sprite>("Sprites/frost_chain_link");
            portalBlueSprite = Resources.Load<Sprite>("Sprites/frost_portal_blue");
            portalOrangeSprite = Resources.Load<Sprite>("Sprites/frost_portal_orange");

            cosmicGroundSprite = Resources.Load<Sprite>("Sprites/ground_cosmic_frost");
            cosmicPlatformSprite = Resources.Load<Sprite>("Sprites/platform_cosmic");
            gravityOrbSprite = Resources.Load<Sprite>("Sprites/gravity_orb");
            laserBeamSprite = Resources.Load<Sprite>("Sprites/frost_laser_beam");
            laserEmitterSprite = Resources.Load<Sprite>("Sprites/frost_laser_emitter");
            crushingPillarSprite = Resources.Load<Sprite>("Sprites/crushing_ice_pillar");
            conveyorSprite = Resources.Load<Sprite>("Sprites/frost_conveyor");
            gravityFlipPadSprite = Resources.Load<Sprite>("Sprites/gravity_flip_pad");
            snowflakeSprite = Resources.Load<Sprite>("Sprites/particle_snowflake");
        }

        public void BuildLevel()
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

            int currentLevel = Mathf.Clamp(GameDataManager.Data.selectedLevel, 1, 30);
            levelRoot = new GameObject($"Level_{currentLevel}_Geometry");

            switch (currentLevel)
            {
                case 1: BuildLevel1(); break;
                case 2: BuildLevel2(); break;
                case 3: BuildLevel3(); break;
                case 4: BuildLevel4(); break;
                case 5: BuildLevel5(); break;
                case 6: BuildLevel6(); break;
                case 7: BuildLevel7(); break;
                case 8: BuildLevel8(); break;
                case 9: BuildLevel9(); break;
                case 10: BuildLevel10(); break;
                case 11: BuildLevel11(); break;
                case 12: BuildLevel12(); break;
                case 13: BuildLevel13(); break;
                case 14: BuildLevel14(); break;
                case 15: BuildLevel15(); break;
                case 16: BuildLevel16(); break;
                case 17: BuildLevel17(); break;
                case 18: BuildLevel18(); break;
                case 19: BuildLevel19(); break;
                case 20: BuildLevel20(); break;
                case 21: BuildLevel21(); break;
                case 22: BuildLevel22(); break;
                case 23: BuildLevel23(); break;
                case 24: BuildLevel24(); break;
                case 25: BuildLevel25(); break;
                case 26: BuildLevel26(); break;
                case 27: BuildLevel27(); break;
                case 28: BuildLevel28(); break;
                case 29: BuildLevel29(); break;
                case 30: BuildLevel30(); break;
                default: BuildLevel1(); break;
            }
        }


        // =========================================================================
        // LEVEL 1: Frosty Foothills (Tutorial & Warmup) - Difficulty 1/10
        // =========================================================================
        private void BuildLevel1()
        {
            SetupCamera(114f, 8f);
            SetupParallax();
            CreatePlayer(new Vector3(0f, 0.8f, 0f));

            CreateGroundSegment(-4f, 14f, 0f);
            CreatePineTree(-2f, 0f);
            CreatePineTree(12f, 0f);
            CreateCoin(3f, 1.2f);
            CreateCoin(5.5f, 1.2f);
            CreateCoin(8f, 1.2f);
            CreateCoin(10.5f, 1.2f);

            CreateGroundSegment(15f, 18f, 0f);
            CreateBounceSpring(16.5f, 0f, 11.5f);
            CreatePlatform(19f, 26f, 4f);
            CreatePineTree(24f, 4f);
            CreateCoin(16.5f, 2.5f);
            CreateCoin(17.5f, 4.2f);
            CreateCoin(19f, 5.5f);
            CreateCoin(20.5f, 5.8f);
            CreateCoin(22.5f, 5.2f);
            CreateCoin(25f, 5.2f);

            CreateGroundSegment(27f, 39f, 0f);
            CreateSpikes(29.5f, 0f);
            CreatePlatform(28.5f, 31f, 2.2f);
            CreateSpikes(33.5f, 0f);
            CreatePlatform(32.5f, 35f, 2.2f);
            CreateCoin(27.5f, 1.2f);
            CreateCoin(29.8f, 3.4f);
            CreateCoin(33.8f, 3.4f);
            CreateCheckpoint(37.5f, 0f);
            CreatePineTree(39f, 0f);

            CreateCrumblingBlock(42f, 0.5f);
            CreateCrumblingBlock(45f, 0.5f);
            CreateCrumblingBlock(48f, 0.5f);
            CreateCoin(42f, 1.8f);
            CreateCoin(45f, 1.8f);
            CreateCoin(48f, 1.8f);

            CreateGroundSegment(52f, 68f, 0f);
            CreateMovingFrostHazard(60f, 0.7f, 5f, 3.2f);
            CreateCoin(55f, 1.2f);
            CreateCoin(58.5f, 2.6f);
            CreateCoin(61.5f, 2.6f);
            CreateCoin(65f, 1.2f);
            CreatePineTree(53f, 0f);
            CreatePineTree(67f, 0f);

            CreateGroundSegment(69f, 84f, 0f);
            CreatePlatform(70f, 82f, 5.5f);
            CreateFallingIcicle(73.5f, 5.0f);
            CreateFallingIcicle(77.5f, 5.0f);
            CreateCoin(71.5f, 1.2f);
            CreateCoin(75.5f, 1.2f);
            CreateCoin(79.5f, 1.2f);
            CreateCheckpoint(83f, 0f);

            CreateIceFloe(87f, 0f);
            CreateBounceSpring(87f, 0f, 13.5f);
            CreateIceFloe(92f, 0f);
            CreateIceFloe(97f, 0f);
            CreatePlatform(89f, 98f, 6.5f);
            CreateCoin(90f, 7.8f);
            CreateCoin(92.5f, 7.8f);
            CreateCoin(95f, 7.8f);
            CreateCoin(97.5f, 7.8f);
            CreateCoin(87f, 2.2f);
            CreateCoin(92f, 1.2f);
            CreateCoin(97f, 1.2f);

            CreateGroundSegment(101f, 116f, 0f);
            CreatePineTree(103f, 0f);
            CreatePineTree(114f, 0f);
            CreateGoalArch(108f, 0f);

            CreateChasmBoundary(-10f, 130f, -5f);
        }

        // =========================================================================
        // LEVEL 2: Pine Needle Peaks (Vertical Exploration) - Difficulty 2/10
        // =========================================================================
        private void BuildLevel2()
        {
            SetupCamera(124f, 10f);
            SetupParallax();
            CreatePlayer(new Vector3(0f, 0.8f, 0f));

            CreateGroundSegment(-4f, 12f, 0f);
            CreatePineTree(-1f, 0f);
            CreatePineTree(10f, 0f);
            CreateCoin(3f, 1.2f);
            CreateCoin(6f, 1.2f);
            CreateCoin(9f, 1.2f);

            CreateBounceSpring(13.5f, 0f, 12f);
            CreatePlatform(15f, 22f, 3.5f);
            CreateCoin(14f, 2.6f);
            CreateCoin(16f, 4.7f);
            CreateCoin(18.5f, 4.7f);
            CreateCoin(21f, 4.7f);

            CreateBounceSpring(23.5f, 3.5f, 12f);
            CreatePlatform(25f, 35f, 7.2f);
            CreatePineTree(33f, 7.2f);
            CreateCoin(24f, 6.2f);
            CreateCoin(26.5f, 8.4f);
            CreateCoin(29f, 8.4f);
            CreateCoin(31.5f, 8.4f);

            CreateGroundSegment(38f, 52f, 0f);
            CreateSpikes(41f, 0f);
            CreateSpikes(45f, 0f);
            CreatePlatform(40f, 46f, 2.4f);
            CreateCheckpoint(50f, 0f);
            CreateCoin(40.5f, 3.6f);
            CreateCoin(43f, 3.6f);
            CreateCoin(45.5f, 3.6f);

            CreateCrumblingBlock(54f, 0f);
            CreateCrumblingBlock(57f, 1.5f);
            CreateCrumblingBlock(60f, 3.0f);
            CreateCrumblingBlock(63f, 1.5f);
            CreateCoin(54f, 1.2f);
            CreateCoin(57f, 2.7f);
            CreateCoin(60f, 4.2f);
            CreateCoin(63f, 2.7f);

            CreateGroundSegment(66f, 84f, 0f);
            CreateMovingFrostHazard(74f, 0.7f, 6f, 3.6f);
            CreatePlatform(70f, 80f, 4.8f);
            CreateFallingIcicle(72f, 4.5f);
            CreateFallingIcicle(76f, 4.5f);
            CreateCoin(68f, 1.2f);
            CreateCoin(73f, 6.0f);
            CreateCoin(77f, 6.0f);
            CreateCoin(82f, 1.2f);
            CreateCheckpoint(84f, 0f);

            CreateIceFloe(88f, 0f);
            CreateBounceSpring(88f, 0f, 14f);
            CreateIceFloe(94f, 1.5f);
            CreateIceFloe(100f, 3.0f);
            CreateCoin(88.5f, 3.5f);
            CreateCoin(91f, 6.2f);
            CreateCoin(94f, 2.7f);
            CreateCoin(100f, 4.2f);

            CreateGroundSegment(104f, 126f, 0f);
            CreatePineTree(106f, 0f);
            CreatePineTree(122f, 0f);
            CreateCoin(106f, 1.2f);
            CreateCoin(109f, 1.2f);
            CreateCoin(112f, 1.2f);
            CreateCoin(115f, 1.2f);
            CreateGoalArch(118f, 0f);

            CreateChasmBoundary(-10f, 140f, -5f);
        }

        // =========================================================================
        // LEVEL 3: Slippery Ice Caverns - Difficulty 3/10 (Unlocks Rocky!)
        // =========================================================================
        private void BuildLevel3()
        {
            SetupCamera(134f, 9f);
            SetupParallax();
            CreatePlayer(new Vector3(0f, 0.8f, 0f));

            CreateIcePlatform(-4f, 14f, 0f);
            CreateCoin(3f, 1.2f);
            CreateCoin(6f, 1.2f);
            CreateCoin(9f, 1.2f);

            CreateSpikes(16f, 0f);
            CreatePlatform(15f, 20f, 2.5f);
            CreateFallingIcicle(17.5f, 5.5f);
            CreateCoin(16f, 3.7f);
            CreateCoin(19f, 3.7f);

            CreateIcePlatform(22f, 36f, 0f);
            CreateMovingFrostHazard(29f, 0.7f, 5f, 4.0f);
            CreateCoin(24f, 1.2f);
            CreateCoin(27.5f, 2.6f);
            CreateCoin(30.5f, 2.6f);
            CreateCoin(34f, 1.2f);
            CreateCheckpoint(36f, 0f);

            CreateCrumblingBlock(39f, 0f);
            CreateCrumblingBlock(42f, 0f);
            CreateCrumblingBlock(45f, 0f);
            CreateSpikes(42f, -2.5f);
            CreateCoin(39f, 1.2f);
            CreateCoin(42f, 1.2f);
            CreateCoin(45f, 1.2f);

            CreateIcePlatform(48f, 64f, 0f);
            CreatePlatform(50f, 62f, 5.0f);
            CreateFallingIcicle(53f, 4.6f);
            CreateFallingIcicle(56f, 4.6f);
            CreateFallingIcicle(59f, 4.6f);
            CreateCoin(50f, 1.2f);
            CreateCoin(53f, 6.2f);
            CreateCoin(56f, 6.2f);
            CreateCoin(59f, 6.2f);
            CreateCoin(62f, 1.2f);

            CreateIcePlatform(67f, 78f, 0f);
            CreateBounceSpring(68.5f, 0f, 13f);
            CreatePlatform(71f, 77f, 4.5f);
            CreateMovingFrostHazard(74f, 5.2f, 2f, 3f);
            CreateCheckpoint(78f, 0f);
            CreateCoin(69f, 3.2f);
            CreateCoin(72f, 5.7f);
            CreateCoin(75f, 5.7f);

            CreateCrumblingBlock(81f, 0.5f);
            CreateCrumblingBlock(84f, 1.2f);
            CreateCrumblingBlock(87f, 1.9f);
            CreateCrumblingBlock(90f, 1.2f);
            CreateCoin(81f, 1.7f);
            CreateCoin(84f, 2.4f);
            CreateCoin(87f, 3.1f);
            CreateCoin(90f, 2.4f);

            CreateIceFloe(94f, 0f);
            CreateIceFloe(99f, 0f);
            CreateIceFloe(104f, 0f);
            CreateCoin(94f, 1.2f);
            CreateCoin(99f, 1.2f);
            CreateCoin(104f, 1.2f);

            CreateGroundSegment(108f, 136f, 0f);
            CreatePineTree(110f, 0f);
            CreatePineTree(132f, 0f);
            CreateCoin(112f, 1.2f);
            CreateCoin(116f, 1.2f);
            CreateCoin(120f, 1.2f);
            CreateGoalArch(124f, 0f);

            CreateChasmBoundary(-10f, 150f, -5f);
        }

        // =========================================================================
        // LEVEL 4: Blizzard Chasm - Difficulty 4/10 (Wind Geysers & Ice Floes)
        // =========================================================================
        private void BuildLevel4()
        {
            SetupCamera(144f, 12f);
            SetupParallax();
            CreatePlayer(new Vector3(0f, 0.8f, 0f));

            CreateGroundSegment(-4f, 12f, 0f);
            CreatePineTree(2f, 0f);
            CreateCoin(4f, 1.2f);
            CreateCoin(7f, 1.2f);
            CreateCoin(10f, 1.2f);

            CreateIceFloe(15f, 0f);
            CreateWindDraft(18f, 0f, 14f, 2f);
            CreatePlatform(19f, 28f, 6.5f);
            CreateCoin(18f, 3.5f);
            CreateCoin(18.5f, 6.0f);
            CreateCoin(21f, 7.8f);
            CreateCoin(24f, 7.8f);
            CreateCoin(27f, 7.8f);

            CreateGroundSegment(31f, 44f, 0f);
            CreateSpikes(35f, 0f);
            CreateMovingFrostHazard(39f, 0.7f, 3f, 3.5f);
            CreateCheckpoint(44f, 0f);
            CreateCoin(33f, 1.2f);
            CreateCoin(37f, 1.2f);
            CreateCoin(41f, 1.2f);

            CreateCrumblingBlock(47f, 0f);
            CreateCrumblingBlock(50f, 0f);
            CreateWindDraft(53f, -1f, 15f, 3f);
            CreatePlatform(54f, 64f, 7.5f);
            CreateCoin(47f, 1.2f);
            CreateCoin(50f, 1.2f);
            CreateCoin(53f, 4.0f);
            CreateCoin(56f, 8.8f);
            CreateCoin(60f, 8.8f);

            CreateIceFloe(67f, 2.0f);
            CreateIceFloe(72f, 1.0f);
            CreateIceFloe(77f, 0f);
            CreateCheckpoint(78f, 0f);
            CreateCoin(67f, 3.2f);
            CreateCoin(72f, 2.2f);
            CreateCoin(77f, 1.2f);

            CreateGroundSegment(81f, 96f, 0f);
            CreateMovingFrostHazard(87f, 0.7f, 6f, 4.2f);
            CreateFallingIcicle(89f, 4.5f);
            CreateFallingIcicle(93f, 4.5f);
            CreatePlatform(86f, 94f, 4.8f);
            CreateCoin(83f, 1.2f);
            CreateCoin(88f, 6.0f);
            CreateCoin(92f, 6.0f);

            CreateWindDraft(99f, 0f, 15f, 2.5f);
            CreatePlatform(101f, 112f, 7.0f);
            CreateCheckpoint(112f, 7.0f);
            CreateCoin(99f, 4.0f);
            CreateCoin(103f, 8.2f);
            CreateCoin(107f, 8.2f);

            CreateGroundSegment(116f, 146f, 0f);
            CreatePineTree(118f, 0f);
            CreatePineTree(142f, 0f);
            CreateCoin(120f, 1.2f);
            CreateCoin(125f, 1.2f);
            CreateCoin(130f, 1.2f);
            CreateCoin(133f, 1.2f);
            CreateCoin(135f, 1.2f);
            CreateGoalArch(138f, 0f);

            CreateChasmBoundary(-10f, 160f, -5f);
        }

        // =========================================================================
        // LEVEL 5: Clockwork Frostworks - Difficulty 5/10 (Rotating Saws & Speed Booster)
        // =========================================================================
        private void BuildLevel5()
        {
            SetupCamera(154f, 10f);
            SetupParallax();
            CreatePlayer(new Vector3(0f, 0.8f, 0f));

            CreateGroundSegment(-4f, 14f, 0f);
            CreateCoin(3f, 1.2f);
            CreateCoin(6f, 1.2f);
            CreateCoin(9f, 1.2f);

            CreateSpeedBoost(12f, 0f, 16f);
            CreateIceFloe(20f, 0f);
            CreateCoin(15f, 2.0f);
            CreateCoin(17.5f, 2.5f);
            CreateCoin(20f, 1.2f);

            CreateGroundSegment(24f, 38f, 0f);
            CreateRotatingFrostBar(31f, 1.8f, 70f, true);
            CreateCheckpoint(38f, 0f);
            CreateCoin(26f, 1.2f);
            CreateCoin(31f, 3.6f);
            CreateCoin(36f, 1.2f);

            CreatePlatform(42f, 52f, 3.0f);
            CreateBounceSpring(43f, 3.0f, 12f);
            CreateRotatingFrostBar(48f, 5.5f, 60f, false);
            CreatePlatform(52f, 62f, 6.0f);
            CreateCoin(44f, 5.5f);
            CreateCoin(48f, 7.8f);
            CreateCoin(55f, 7.2f);
            CreateCoin(59f, 7.2f);

            CreateGroundSegment(66f, 82f, 0f);
            CreateMovingFrostHazard(72f, 0.7f, 4f, 4.5f);
            CreateMovingFrostHazard(78f, 0.7f, 3f, 4.5f);
            CreateCheckpoint(82f, 0f);
            CreateCoin(68f, 1.2f);
            CreateCoin(73f, 2.5f);
            CreateCoin(77f, 2.5f);

            CreateSpeedBoost(84f, 0f, 16f);
            CreateIceFloe(93f, 0f);
            CreateRotatingFrostBar(97f, 2.0f, 80f, true);
            CreateIceFloe(101f, 0f);
            CreateCoin(87f, 2.0f);
            CreateCoin(90f, 2.5f);
            CreateCoin(95f, 3.8f);
            CreateCoin(101f, 1.2f);

            CreateGroundSegment(106f, 124f, 0f);
            CreateSpikes(112f, 0f);
            CreateSpikes(116f, 0f);
            CreatePlatform(110f, 118f, 2.5f);
            CreateCheckpoint(124f, 0f);
            CreateCoin(108f, 1.2f);
            CreateCoin(114f, 3.8f);
            CreateCoin(120f, 1.2f);

            CreateGroundSegment(128f, 156f, 0f);
            CreatePineTree(130f, 0f);
            CreatePineTree(152f, 0f);
            CreateCoin(132f, 1.2f);
            CreateCoin(136f, 1.2f);
            CreateCoin(140f, 1.2f);
            CreateCoin(143f, 1.2f);
            CreateCoin(146f, 1.2f);
            CreateCoin(149f, 1.2f);
            CreateCoin(151f, 1.2f);
            CreateGoalArch(154f, 0f);

            CreateChasmBoundary(-10f, 170f, -5f);
        }

        // =========================================================================
        // LEVEL 6: Crystal Glade - Difficulty 6/10 (Unlocks Minty!)
        // =========================================================================
        private void BuildLevel6()
        {
            SetupCamera(158f, 11f);
            SetupParallax();
            CreatePlayer(new Vector3(0f, 0.8f, 0f));

            CreateGroundSegment(-4f, 12f, 0f);
            CreateCoin(3f, 1.2f);
            CreateCoin(6f, 1.2f);
            CreateCoin(9f, 1.2f);

            CreateBlinkingPlatform(15f, 1.5f, 0f);
            CreateBlinkingPlatform(19f, 3.0f, 0.5f);
            CreateBlinkingPlatform(23f, 4.5f, 1.0f);
            CreatePlatform(26f, 34f, 6.0f);
            CreateCoin(15f, 2.7f);
            CreateCoin(19f, 4.2f);
            CreateCoin(23f, 5.7f);
            CreateCoin(29f, 7.2f);

            CreateGroundSegment(38f, 52f, 0f);
            CreateRotatingFrostBar(44f, 2.0f, 75f, true);
            CreateCheckpoint(52f, 0f);
            CreateCoin(40f, 1.2f);
            CreateCoin(44f, 4.2f);
            CreateCoin(48f, 1.2f);

            CreateCrumblingBlock(55f, 0.5f);
            CreateCrumblingBlock(58f, 1.5f);
            CreateBlinkingPlatform(62f, 3.0f, 0f);
            CreateBlinkingPlatform(66f, 3.0f, 0.8f);
            CreateCoin(55f, 1.7f);
            CreateCoin(58f, 2.7f);
            CreateCoin(62f, 4.2f);
            CreateCoin(66f, 4.2f);

            CreateGroundSegment(70f, 86f, 0f);
            CreateMovingFrostHazard(77f, 0.7f, 5f, 5.0f);
            CreateSpikes(74f, 0f);
            CreateSpikes(80f, 0f);
            CreateCheckpoint(86f, 0f);
            CreateCoin(72f, 1.2f);
            CreateCoin(77f, 2.8f);
            CreateCoin(83f, 1.2f);

            CreateBounceSpring(89f, 0f, 14f);
            CreateBlinkingPlatform(93f, 5.5f, 0f);
            CreateBlinkingPlatform(98f, 6.5f, 1.0f);
            CreateRotatingFrostBar(104f, 7.0f, 80f, false);
            CreatePlatform(107f, 116f, 6.0f);
            CreateCoin(90f, 3.5f);
            CreateCoin(93f, 6.7f);
            CreateCoin(98f, 7.7f);
            CreateCoin(109f, 7.2f);

            CreateGroundSegment(120f, 134f, 0f);
            CreateSpikes(125f, 0f);
            CreateSpikes(128f, 0f);
            CreateCheckpoint(134f, 0f);
            CreateCoin(122f, 1.2f);
            CreateCoin(126.5f, 2.6f);
            CreateCoin(131f, 1.2f);

            CreateGroundSegment(138f, 160f, 0f);
            CreatePineTree(140f, 0f);
            CreatePineTree(156f, 0f);
            CreateCoin(142f, 1.2f);
            CreateCoin(146f, 1.2f);
            CreateCoin(149f, 1.2f);
            CreateCoin(152f, 1.2f);
            CreateCoin(154f, 1.2f);
            CreateCoin(156f, 1.2f);
            CreateGoalArch(158f, 0f);

            CreateChasmBoundary(-10f, 175f, -5f);
        }

        // =========================================================================
        // LEVEL 7: Glacier Crags - Difficulty 7/10 (Hazard Gauntlet)
        // =========================================================================
        private void BuildLevel7()
        {
            SetupCamera(168f, 12f);
            SetupParallax();
            CreatePlayer(new Vector3(0f, 0.8f, 0f));

            CreateIcePlatform(-4f, 12f, 0f);
            CreateCoin(3f, 1.2f);
            CreateCoin(6f, 1.2f);
            CreateCoin(9f, 1.2f);

            CreateMovingFrostHazard(16f, 0.7f, 4f, 5.2f);
            CreateIcePlatform(13f, 24f, 0f);
            CreateSpikes(20f, 0f);
            CreateCoin(15f, 2.2f);
            CreateCoin(18f, 2.2f);
            CreateCoin(22f, 1.2f);

            CreateBounceSpring(26f, 0f, 12.5f);
            CreateBounceSpring(32f, 2f, 12.5f);
            CreateBounceSpring(38f, 4f, 12.5f);
            CreatePlatform(42f, 52f, 8.0f);
            CreateCheckpoint(52f, 8.0f);
            CreateCoin(27f, 3.5f);
            CreateCoin(33f, 5.5f);
            CreateCoin(39f, 7.5f);
            CreateCoin(45f, 9.2f);

            CreateCrumblingBlock(56f, 5.5f);
            CreateCrumblingBlock(59f, 4.0f);
            CreateCrumblingBlock(62f, 2.5f);
            CreateWindDraft(66f, -1f, 16f, 2f);
            CreatePlatform(68f, 78f, 6.0f);
            CreateCoin(56f, 6.7f);
            CreateCoin(59f, 5.2f);
            CreateCoin(62f, 3.7f);
            CreateCoin(66f, 3.5f);
            CreateCoin(72f, 7.2f);

            CreateIcePlatform(82f, 96f, 0f);
            CreateRotatingFrostBar(88f, 2.0f, 90f, true);
            CreateRotatingFrostBar(92f, 2.0f, 90f, false);
            CreateCheckpoint(96f, 0f);
            CreateCoin(84f, 1.2f);
            CreateCoin(88f, 4.2f);
            CreateCoin(92f, 4.2f);
            CreateCoin(95f, 1.2f);

            CreateBlinkingPlatform(100f, 1.5f, 0f);
            CreateBlinkingPlatform(105f, 2.5f, 0.7f);
            CreateBlinkingPlatform(110f, 3.5f, 1.4f);
            CreateCoin(100f, 2.7f);
            CreateCoin(105f, 3.7f);
            CreateCoin(110f, 4.7f);

            CreateIcePlatform(114f, 134f, 0f);
            CreateFallingIcicle(118f, 4.0f);
            CreateFallingIcicle(122f, 4.0f);
            CreateFallingIcicle(126f, 4.0f);
            CreateMovingFrostHazard(124f, 0.7f, 5f, 5.5f);
            CreateCheckpoint(134f, 0f);
            CreateCoin(118f, 1.2f);
            CreateCoin(122f, 1.2f);
            CreateCoin(126f, 1.2f);
            CreateCoin(130f, 1.2f);

            CreateGroundSegment(138f, 170f, 0f);
            CreatePineTree(142f, 0f);
            CreatePineTree(166f, 0f);
            CreateCoin(144f, 1.2f);
            CreateCoin(149f, 1.2f);
            CreateCoin(152f, 1.2f);
            CreateCoin(155f, 1.2f);
            CreateGoalArch(158f, 0f);

            CreateChasmBoundary(-10f, 185f, -5f);
        }

        // =========================================================================
        // LEVEL 8: Northern Lights Ridge - Difficulty 8/10 (High Altitude Route)
        // =========================================================================
        private void BuildLevel8()
        {
            SetupCamera(172f, 14f);
            SetupParallax();
            CreatePlayer(new Vector3(0f, 0.8f, 0f));

            CreateGroundSegment(-4f, 12f, 0f);
            CreateCoin(3f, 1.2f);
            CreateCoin(6f, 1.2f);
            CreateCoin(9f, 1.2f);

            CreateWindDraft(14f, 0f, 16f, 2.5f);
            CreatePlatform(16f, 28f, 8.5f);
            CreateCoin(14f, 4.0f);
            CreateCoin(17f, 9.7f);
            CreateCoin(21f, 9.7f);
            CreateCoin(25f, 9.7f);

            CreateRotatingFrostBar(32f, 8.5f, 85f, true);
            CreatePlatform(35f, 46f, 7.5f);
            CreateBounceSpring(37f, 7.5f, 12f);
            CreateCoin(32f, 10.7f);
            CreateCoin(38f, 10.2f);
            CreateCoin(42f, 8.7f);

            CreateGroundSegment(50f, 66f, 0f);
            CreateMovingFrostHazard(57f, 0.7f, 5f, 5.0f);
            CreateCheckpoint(66f, 0f);
            CreateCoin(52f, 1.2f);
            CreateCoin(57f, 2.5f);
            CreateCoin(62f, 1.2f);

            CreateSpeedBoost(68f, 0f, 17f);
            CreateIceFloe(78f, 0f);
            CreateRotatingFrostBar(82f, 2.5f, 90f, false);
            CreateIceFloe(86f, 0f);
            CreateCoin(72f, 2.5f);
            CreateCoin(78f, 1.2f);
            CreateCoin(82f, 4.7f);
            CreateCoin(86f, 1.2f);

            CreateCrumblingBlock(90f, 0.5f);
            CreateCrumblingBlock(93f, 1.2f);
            CreateCrumblingBlock(96f, 1.9f);
            CreateCrumblingBlock(99f, 2.6f);
            CreateCrumblingBlock(102f, 3.3f);
            CreateCoin(90f, 1.7f);
            CreateCoin(96f, 3.1f);
            CreateCoin(102f, 4.5f);

            CreatePlatform(106f, 122f, 5.0f);
            CreateBlinkingPlatform(112f, 7.5f, 0f);
            CreateBlinkingPlatform(117f, 8.5f, 0.8f);
            CreateCheckpoint(122f, 5.0f);
            CreateCoin(108f, 6.2f);
            CreateCoin(112f, 8.7f);
            CreateCoin(117f, 9.7f);

            CreateWindDraft(126f, 0f, 17f, 3f);
            CreatePlatform(128f, 140f, 9.0f);
            CreateRotatingFrostBar(134f, 11.5f, 90f, true);
            CreateCoin(126f, 5.0f);
            CreateCoin(130f, 10.2f);
            CreateCoin(137f, 10.2f);

            CreateGroundSegment(144f, 175f, 0f);
            CreatePineTree(146f, 0f);
            CreatePineTree(170f, 0f);
            CreateCoin(148f, 1.2f);
            CreateCoin(154f, 1.2f);
            CreateCoin(157f, 1.2f);
            CreateCoin(160f, 1.2f);
            CreateGoalArch(164f, 0f);

            CreateChasmBoundary(-10f, 190f, -5f);
        }

        // =========================================================================
        // LEVEL 9: The Frost Spire - Difficulty 9/10 (Unlocks Glacier!)
        // =========================================================================
        private void BuildLevel9()
        {
            SetupCamera(178f, 15f);
            SetupParallax();
            CreatePlayer(new Vector3(0f, 0.8f, 0f));

            CreateIcePlatform(-4f, 12f, 0f);
            CreateCoin(3f, 1.2f);
            CreateCoin(6f, 1.2f);
            CreateCoin(9f, 1.2f);

            CreateBounceSpring(13.5f, 0f, 13f);
            CreatePlatform(15f, 22f, 4.5f);
            CreateBounceSpring(20f, 4.5f, 13f);
            CreatePlatform(23f, 32f, 9.0f);
            CreateCoin(14f, 3.2f);
            CreateCoin(17f, 5.7f);
            CreateCoin(21f, 7.7f);
            CreateCoin(25f, 10.2f);

            CreateRotatingFrostBar(36f, 9.0f, 95f, true);
            CreateBlinkingPlatform(40f, 8.0f, 0f);
            CreateBlinkingPlatform(44f, 6.5f, 0.7f);
            CreateCheckpoint(48f, 5.0f);
            CreatePlatform(46f, 54f, 5.0f);
            CreateCoin(36f, 11.2f);
            CreateCoin(40f, 9.2f);
            CreateCoin(44f, 7.7f);

            CreateCrumblingBlock(57f, 4.0f);
            CreateCrumblingBlock(60f, 3.0f);
            CreateCrumblingBlock(63f, 2.0f);
            CreateCrumblingBlock(66f, 1.0f);
            CreateCoin(57f, 5.2f);
            CreateCoin(60f, 4.2f);
            CreateCoin(63f, 3.2f);
            CreateCoin(66f, 2.2f);

            CreateGroundSegment(70f, 90f, 0f);
            CreateMovingFrostHazard(78f, 0.7f, 6f, 6.0f);
            CreateSpikes(74f, 0f);
            CreateSpikes(84f, 0f);
            CreateCheckpoint(90f, 0f);
            CreateCoin(72f, 1.2f);
            CreateCoin(78f, 2.8f);
            CreateCoin(87f, 1.2f);

            CreateWindDraft(93f, 0f, 18f, 3.5f);
            CreatePlatform(96f, 112f, 10.5f);
            CreateRotatingFrostBar(104f, 12.5f, 100f, false);
            CreateCoin(93f, 5.5f);
            CreateCoin(98f, 11.7f);
            CreateCoin(104f, 14.7f);
            CreateCoin(109f, 11.7f);

            CreateBlinkingPlatform(116f, 9.0f, 0f);
            CreateBlinkingPlatform(121f, 6.5f, 0.8f);
            CreateBlinkingPlatform(126f, 4.0f, 1.6f);
            CreateCoin(116f, 10.2f);
            CreateCoin(121f, 7.7f);
            CreateCoin(126f, 5.2f);

            CreateIcePlatform(130f, 148f, 0f);
            CreateFallingIcicle(134f, 4.5f);
            CreateFallingIcicle(138f, 4.5f);
            CreateFallingIcicle(142f, 4.5f);
            CreateCheckpoint(148f, 0f);
            CreateCoin(134f, 1.2f);
            CreateCoin(138f, 1.2f);
            CreateCoin(142f, 1.2f);

            CreateGroundSegment(152f, 180f, 0f);
            CreatePineTree(154f, 0f);
            CreatePineTree(176f, 0f);
            CreateCoin(156f, 1.2f);
            CreateCoin(160f, 1.2f);
            CreateCoin(164f, 1.2f);
            CreateGoalArch(168f, 0f);

            CreateChasmBoundary(-10f, 195f, -5f);
        }

        // =========================================================================
        // LEVEL 10: Avalanche Citadel (Grand Boss Challenge!) - Difficulty 10/10
        // =========================================================================
        private void BuildLevel10()
        {
            SetupCamera(192f, 16f);
            SetupParallax();
            CreatePlayer(new Vector3(0f, 0.8f, 0f));

            CreateGroundSegment(-4f, 14f, 0f);
            CreatePineTree(-1f, 0f);
            CreateCoin(3f, 1.2f);
            CreateCoin(6f, 1.2f);
            CreateCoin(9f, 1.2f);

            // Grand speed booster launch across abyss
            CreateSpeedBoost(12f, 0f, 18f);
            CreateIceFloe(22f, 0f);
            CreateRotatingFrostBar(27f, 2.5f, 100f, true);
            CreateIceFloe(32f, 0f);
            CreateCoin(16f, 2.5f);
            CreateCoin(19f, 3.2f);
            CreateCoin(22f, 1.2f);
            CreateCoin(32f, 1.2f);

            CreateGroundSegment(36f, 52f, 0f);
            CreateMovingFrostHazard(42f, 0.7f, 5f, 6.0f);
            CreateSpikes(47f, 0f);
            CreateCheckpoint(52f, 0f);
            CreateCoin(38f, 1.2f);
            CreateCoin(43f, 2.8f);
            CreateCoin(49f, 1.2f);

            // Blinking ice staircase
            CreateBlinkingPlatform(56f, 2.0f, 0f);
            CreateBlinkingPlatform(60f, 4.0f, 0.6f);
            CreateBlinkingPlatform(64f, 6.0f, 1.2f);
            CreateBlinkingPlatform(68f, 8.0f, 1.8f);
            CreatePlatform(71f, 82f, 9.5f);
            CreateCoin(56f, 3.2f);
            CreateCoin(60f, 5.2f);
            CreateCoin(64f, 7.2f);
            CreateCoin(68f, 9.2f);
            CreateCoin(76f, 10.7f);

            // High Wind Tunnel
            CreateWindDraft(85f, 7f, 18f, 4f);
            CreatePlatform(88f, 102f, 13.0f);
            CreateRotatingFrostBar(95f, 15.0f, 110f, false);
            CreateCheckpoint(102f, 13.0f);
            CreateCoin(86f, 11.0f);
            CreateCoin(90f, 14.2f);
            CreateCoin(95f, 17.2f);
            CreateCoin(100f, 14.2f);

            // Collapse Drop & Rapid Springs
            CreateCrumblingBlock(106f, 10.0f);
            CreateCrumblingBlock(109f, 7.0f);
            CreateCrumblingBlock(112f, 4.0f);
            CreateBounceSpring(116f, 0f, 14f);
            CreateBounceSpring(122f, 2f, 14f);
            CreateIcePlatform(126f, 140f, 4f);
            CreateCheckpoint(140f, 4f);
            CreateCoin(106f, 11.2f);
            CreateCoin(109f, 8.2f);
            CreateCoin(112f, 5.2f);
            CreateCoin(117f, 3.5f);
            CreateCoin(123f, 5.5f);

            // Citadel Defense Gauntlet (Dual Saws + Icicles)
            CreateMovingFrostHazard(130f, 4.7f, 4f, 6.2f);
            CreateFallingIcicle(132f, 8.5f);
            CreateFallingIcicle(136f, 8.5f);
            CreateCoin(128f, 5.2f);
            CreateCoin(135f, 5.2f);

            // Final Citadel Approach
            CreateSpeedBoost(142f, 4f, 18f);
            CreateIceFloe(152f, 2f);
            CreateRotatingFrostBar(156f, 4.5f, 110f, true);
            CreateIceFloe(160f, 1f);
            CreateCoin(146f, 6.0f);
            CreateCoin(152f, 3.2f);
            CreateCoin(160f, 2.2f);

            // Citadel Courtyard & Grand Victory Arch
            CreateGroundSegment(164f, 196f, 0f);
            CreatePineTree(166f, 0f);
            CreatePineTree(172f, 0f);
            CreatePineTree(192f, 0f);
            CreateCheckpoint(168f, 0f);
            CreateCoin(180f, 1.2f);
            CreateGoalArch(184f, 0f);

            CreateChasmBoundary(-10f, 210f, -5f);
        }

        // =========================================================================
        // LEVEL 11: Aurora Highlands (Night Sky, Mega Trampolines, Moving Platforms)
        // =========================================================================
        private void BuildLevel11()
        {
            SetupCamera(120f, 10f);
            SetupParallax(11);
            CreatePlayer(new Vector3(0f, 0.8f, 0f));

            CreateGroundSegment(-4f, 12f, 0f);
            CreatePineTree(-2f, 0f);
            CreatePineTree(10f, 0f);
            CreateCoin(1.5f, 1.2f);
            CreateCoin(3.5f, 1.2f);
            CreateCoin(5.5f, 1.2f);
            CreateCoin(7.5f, 1.2f);
            CreateCoin(9.5f, 1.2f);

            // Intro to Mega Frost Trampoline
            CreateFrostTrampoline(11.2f, 0f, 17f);
            CreateCoin(11.5f, 3.2f);
            CreateCoin(13f, 5.5f);
            CreateCoin(14.5f, 7.2f);
            CreateCoin(16f, 7.2f);

            // High Crystal Platform
            CreatePlatform(17f, 25f, 6.5f);
            CreateCoin(18f, 7.7f);
            CreateCoin(20f, 7.7f);
            CreateCoin(22f, 7.7f);
            CreateCoin(24f, 7.7f);

            // Moving platform over deep chasm
            CreateMovingIcePlatform(27f, 6.5f, new Vector2(6f, 0f), 2.4f, 0.3f);
            CreateCoin(27f, 8f);
            CreateCoin(29.5f, 8f);
            CreateCoin(32f, 8f);

            // Landing & Mid Checkpoint
            CreateGroundSegment(35f, 50f, 4.5f);
            CreateCheckpoint(37f, 4.5f);
            CreateCoin(36f, 5.7f);
            CreateCoin(38.5f, 5.7f);
            CreateCoin(41f, 5.7f);
            CreateCoin(43f, 5.7f);
            CreatePineTree(48f, 4.5f);

            // Hazard section: Spikes with Trampoline leap
            CreateSpikes(44.5f, 4.5f);
            CreateFrostTrampoline(46.5f, 4.5f, 16.5f);
            CreateCoin(46.5f, 7.2f);
            CreateCoin(48f, 9.2f);
            CreateCoin(50f, 9.2f);

            // High Floating Island
            CreatePlatform(52f, 60f, 8f);
            CreateCoin(53.5f, 9.2f);
            CreateCoin(56f, 9.2f);
            CreateCoin(58.5f, 9.2f);

            // Downward descent onto moving ice platform
            CreateMovingIcePlatform(62f, 5.5f, new Vector2(0f, -3f), 2f, 0.4f);
            CreateCoin(63f, 6.8f);
            CreateCoin(63f, 4f);

            // Final Ground Run
            CreateGroundSegment(66f, 85f, 1f);
            CreateCoin(70f, 2.2f);
            CreateCoin(75f, 2.2f);
            CreatePineTree(77f, 1f);

            CreateGoalArch(83f, 1f);
            CreateIcyChasm(-15f, 95f, -6f);
        }

        // =========================================================================
        // LEVEL 12: Crystal Chasm (Moving Rafts Over Spike Beds) -> Unlocks Berry!
        // =========================================================================
        private void BuildLevel12()
        {
            SetupCamera(125f, 9f);
            SetupParallax(12);
            CreatePlayer(new Vector3(0f, 0.8f, 0f));

            CreateGroundSegment(-4f, 10f, 0f);
            CreateCoin(1.5f, 1.2f);
            CreateCoin(3.5f, 1.2f);
            CreateCoin(5.5f, 1.2f);
            CreateCoin(7.5f, 1.2f);

            // Wide Spike Pit with Dual Oscillating Platforms
            CreateGroundSegment(10f, 32f, -3f);
            CreateSpikes(12f, -3f);
            CreateSpikes(16f, -3f);
            CreateSpikes(20f, -3f);
            CreateSpikes(24f, -3f);
            CreateSpikes(28f, -3f);

            CreateMovingIcePlatform(12f, 0.5f, new Vector2(5f, 0f), 2.2f, 0.3f);
            CreateCoin(12.5f, 2f);
            CreateCoin(14.5f, 2.2f);
            CreateCoin(16.5f, 2f);

            CreateCrumblingBlock(20f, 1f);
            CreateCoin(19f, 2.2f);
            CreateCoin(21f, 2.2f);

            CreateMovingIcePlatform(23f, 0.5f, new Vector2(5f, 0f), 2.2f, 0.3f);
            CreateCoin(23.5f, 2f);
            CreateCoin(25.5f, 2.2f);
            CreateCoin(27.5f, 2f);

            // Mid Island & Checkpoint
            CreateGroundSegment(33f, 48f, 0f);
            CreateCheckpoint(35f, 0f);
            CreateCoin(34f, 1.2f);
            CreateCoin(37f, 1.2f);
            CreateCoin(40f, 1.2f);
            CreateCoin(43f, 1.2f);
            CreatePineTree(45f, 0f);

            // Vertical Moving Lift Section
            CreateMovingIcePlatform(50f, 0.5f, new Vector2(0f, 5.5f), 2.5f, 0.4f);
            CreateCoin(51f, 2.5f);
            CreateCoin(51f, 4.5f);
            CreateCoin(51f, 6.5f);

            // High Crystal Arch
            CreatePlatform(54f, 66f, 6.5f);
            CreateSpikes(58f, 6.5f);
            CreateCoin(55f, 7.7f);
            CreateCoin(58f, 8.2f);
            CreateCoin(61f, 8.2f);
            CreateCoin(64f, 7.7f);

            // Crumbling stair drop
            CreateCrumblingBlock(68f, 4.5f);
            CreateCoin(68f, 5.8f);
            CreateCrumblingBlock(71f, 3f);
            CreateCoin(71f, 4.3f);
            CreateCrumblingBlock(74f, 1.5f);
            CreateCoin(74f, 2.8f);

            // Final Dash
            CreateGroundSegment(77f, 96f, 0f);
            CreatePineTree(79f, 0f);
            CreateCoin(79f, 1.2f);
            CreateCoin(82f, 1.2f);
            CreateCoin(85f, 1.2f);
            CreateCoin(88f, 1.2f);

            CreateGoalArch(94f, 0f);
            CreateIcyChasm(-15f, 105f, -8f);
        }

        // =========================================================================
        // LEVEL 13: Pendulum Hollow (Swinging Frost Maces & Timing Gauntlets)
        // =========================================================================
        private void BuildLevel13()
        {
            SetupCamera(130f, 10f);
            SetupParallax(13);
            CreatePlayer(new Vector3(0f, 0.8f, 0f));

            CreateGroundSegment(-4f, 14f, 0f);
            CreateCoin(2f, 1.2f);
            CreateCoin(4.5f, 1.2f);
            CreateCoin(7f, 1.2f);
            CreateCoin(9.5f, 1.2f);

            // First Swinging Mace
            CreateSwingingMace(18f, 6f, 3.6f, 55f, 2.2f, 0f);
            CreateGroundSegment(14f, 26f, 0f);
            CreateCoin(15f, 1.2f);
            CreateCoin(17.5f, 1.2f);
            CreateCoin(20f, 1.2f);
            CreateCoin(22.5f, 1.2f);

            // Boost Ramp through second Swinging Mace
            CreateGroundSegment(28f, 46f, 0f);
            CreateSpeedBoost(30f, 0f, 15f);
            CreateSwingingMace(37f, 6.5f, 3.8f, 65f, 2.8f, 1.5f);
            CreateCoin(31f, 1.8f);
            CreateCoin(34f, 2.2f);
            CreateCoin(37f, 2.2f);
            CreateCoin(40f, 1.8f);

            // Mid Checkpoint
            CreateCheckpoint(45f, 0f);
            CreatePineTree(46f, 0f);
            CreateCoin(43.5f, 1.2f);
            CreateCoin(45.5f, 1.2f);
            CreateCoin(47f, 1.2f);

            // Double Alternating Maces over Floating Platforms
            CreatePlatform(49f, 55f, 2.5f);
            CreateSwingingMace(52f, 7.5f, 3.5f, 50f, 2.5f, 0f);
            CreateCoin(50f, 3.8f);
            CreateCoin(53f, 3.8f);

            CreatePlatform(57f, 63f, 2.5f);
            CreateSwingingMace(60f, 7.5f, 3.5f, 50f, 2.5f, 3.14f);
            CreateCoin(58f, 3.8f);
            CreateCoin(61f, 3.8f);

            // Trampoline over double spike pit
            CreateGroundSegment(65f, 88f, 0f);
            CreateSpikes(68f, 0f);
            CreateFrostTrampoline(71f, 0f, 16.5f);
            CreateSpikes(75f, 0f);
            CreateCoin(68f, 3f);
            CreateCoin(70f, 5f);
            CreateCoin(72f, 6.8f);
            CreateCoin(74f, 6.8f);
            CreateCoin(76f, 4.5f);

            // Final Mace Timing corridor
            CreateSwingingMace(81f, 6f, 3.4f, 60f, 2.4f, 0.5f);
            CreateCoin(79f, 1.2f);
            CreateCoin(81f, 1.2f);
            CreateCoin(83f, 1.2f);
            CreateCoin(85f, 1.2f);
            CreateCoin(87f, 1.2f);
            CreateCoin(89f, 1.2f);

            CreateGoalArch(92f, 0f);
            CreateIcyChasm(-15f, 98f, -7f);
        }

        // =========================================================================
        // LEVEL 14: Warp Caverns (Introduction to Frost Portals & Momentum Loops)
        // =========================================================================
        private void BuildLevel14()
        {
            SetupCamera(135f, 12f);
            SetupParallax(14);
            CreatePlayer(new Vector3(0f, 0.8f, 0f));

            CreateGroundSegment(-4f, 12f, 0f);
            CreateCoin(2f, 1.2f);
            CreateCoin(4f, 1.2f);
            CreateCoin(6f, 1.2f);
            CreateCoin(8f, 1.2f);

            // Portal 1: Teleports across a bottomless chasm to high platform
            CreatePortalPair(new Vector2(10f, 0f), new Vector2(16f, 5.5f), new Vector2(7f, 4f));
            CreatePlatform(15f, 25f, 5.5f);
            CreateCoin(13f, 4f);
            CreateCoin(17f, 7f);
            CreateCoin(20f, 7f);
            CreateCoin(23f, 7f);

            // Jump down onto bounce spring into Portal 2
            CreateGroundSegment(27f, 38f, 0f);
            CreateBounceSpring(29f, 0f, 13f);
            CreateCoin(28f, 2f);
            CreateCoin(30f, 3.8f);
            CreateCoin(32f, 2.5f);
            CreateCoin(34f, 1.2f);

            // Portal 2: Upward launch through coin ring
            CreatePortalPair(new Vector2(35f, 0f), new Vector2(40f, 2f), new Vector2(8f, 7f));
            CreateCoin(41f, 4.5f);
            CreateCoin(43f, 6.8f);
            CreateCoin(45f, 7.5f);
            CreateCoin(47f, 5.5f);

            // Mid Island & Checkpoint
            CreateGroundSegment(48f, 62f, 2f);
            CreateCheckpoint(50f, 2f);
            CreatePineTree(52f, 2f);
            CreateCoin(50f, 3.2f);
            CreateCoin(53f, 3.2f);
            CreateCoin(56f, 3.2f);
            CreateCoin(59f, 3.2f);

            // Portal 3: Secret coin alcove & return
            CreatePortalPair(new Vector2(61.5f, 2f), new Vector2(66f, 7f), new Vector2(6f, 2f));
            CreatePlatform(65f, 75f, 7f);
            CreateCoin(66.5f, 8.2f);
            CreateCoin(68.5f, 8.5f);
            CreateCoin(70.5f, 8.5f);
            CreateCoin(72.5f, 8.2f);
            CreateCoin(74.5f, 7.5f);

            // Portal 4: Launch into finish stretch
            CreatePortalPair(new Vector2(74f, 7f), new Vector2(79f, 1f), new Vector2(8f, 4f));
            CreateGroundSegment(78f, 96f, 0f);
            CreateCoin(81f, 3.5f);
            CreateCoin(84f, 2f);
            CreateCoin(87f, 1.2f);
            CreateCoin(90f, 1.2f);
            CreateCoin(93f, 1.2f);

            CreateGoalArch(95f, 0f);
            CreateIcyChasm(-15f, 105f, -8f);
        }

        // =========================================================================
        // LEVEL 15: Neon Glacier (Speed Boosts & Vertical Moving Lifts) -> Unlocks Midas!
        // =========================================================================
        private void BuildLevel15()
        {
            SetupCamera(135f, 12f);
            SetupParallax(15);
            CreatePlayer(new Vector3(0f, 0.8f, 0f));

            CreateGroundSegment(-4f, 10f, 0f);
            CreateCoin(2.5f, 1.2f);
            CreateCoin(5f, 1.2f);
            CreateCoin(7.5f, 1.2f);

            // Speed booster into long launch
            CreateSpeedBoost(8.5f, 0f, 16f);
            CreateCoin(10f, 2.5f);
            CreateCoin(12.5f, 3.5f);
            CreateCoin(15f, 3.5f);
            CreateCoin(17.5f, 2.5f);

            // Landing onto moving elevator platform
            CreateMovingIcePlatform(20f, 1f, new Vector2(0f, 6f), 2.6f, 0.4f);
            CreateCoin(21f, 3f);
            CreateCoin(21f, 5.5f);
            CreateCoin(21f, 8f);

            // High Neon Ridge
            CreatePlatform(23f, 36f, 7.5f);
            CreateSwingingMace(28f, 12f, 3.6f, 50f, 2.4f, 0f);
            CreateCoin(25f, 8.8f);
            CreateCoin(28f, 8.8f);
            CreateCoin(31f, 8.8f);
            CreateCoin(34f, 8.8f);

            // Mega Trampoline launch across mega gap
            CreateFrostTrampoline(35f, 7.5f, 18f);
            CreateCoin(37f, 10.5f);
            CreateCoin(39.5f, 11.8f);
            CreateCoin(42f, 11.8f);
            CreateCoin(44.5f, 9.5f);

            // Mid Island & Checkpoint
            CreateGroundSegment(46f, 62f, 3f);
            CreateCheckpoint(48f, 3f);
            CreateCoin(48f, 4.2f);
            CreateCoin(51f, 4.2f);
            CreateCoin(54f, 4.2f);
            CreatePineTree(56f, 3f);
            CreateCoin(57f, 4.2f);

            // Speed Boost through Frost Saw Gauntlet
            CreateSpeedBoost(60f, 3f, 15f);
            CreateMovingFrostHazard(66f, 3.8f, 4f, 3.5f);
            CreateCoin(62f, 4.2f);
            CreateCoin(65f, 4.5f);
            CreateCoin(68f, 4.5f);
            CreateCoin(70.5f, 4.2f);

            // Final Moving Platform ride
            CreateMovingIcePlatform(72f, 2.5f, new Vector2(6f, 0f), 2.5f, 0.3f);
            CreateCoin(74f, 4f);
            CreateCoin(77f, 4f);

            // Final Run
            CreateGroundSegment(80f, 96f, 1f);
            CreateCoin(83f, 2.2f);
            CreateCoin(87f, 2.2f);

            CreateGoalArch(94f, 1f);
            CreateIcyChasm(-15f, 105f, -8f);
        }

        // =========================================================================
        // LEVEL 16: The Celestial Lift (Towering Vertical Climbs with Wind & Elevators)
        // =========================================================================
        private void BuildLevel16()
        {
            SetupCamera(125f, 15f);
            SetupParallax(16);
            CreatePlayer(new Vector3(0f, 0.8f, 0f));

            CreateGroundSegment(-4f, 10f, 0f);
            CreateCoin(2.5f, 1.2f);
            CreateCoin(5f, 1.2f);
            CreateCoin(7.5f, 1.2f);

            // Vertical Lift 1
            CreateMovingIcePlatform(12f, 0.5f, new Vector2(0f, 5f), 2.5f, 0.35f);
            CreateCoin(13f, 2f);
            CreateCoin(13f, 3.8f);
            CreateCoin(13f, 5.2f);

            // Platform Tier 1
            CreatePlatform(15f, 24f, 5.5f);
            CreateRotatingFrostBar(19.5f, 6.5f, 60f, true);
            CreateCoin(17f, 6.7f);
            CreateCoin(20f, 7.2f);
            CreateCoin(23f, 6.7f);

            // Wind Draft Geyser Lift to Tier 2
            CreateWindDraft(25.5f, 5.5f, 13f, 1.5f);
            CreateCoin(25.5f, 7.5f);
            CreateCoin(26.5f, 9.5f);
            CreateCoin(27.5f, 11f);
            CreateCoin(29f, 11.5f);

            // High Tier 2 Island & Checkpoint
            CreatePlatform(30f, 44f, 10.5f);
            CreateCheckpoint(32f, 10.5f);
            CreateCoin(33f, 11.7f);
            CreateCoin(36f, 11.7f);
            CreateCoin(39f, 11.7f);
            CreateCoin(42f, 11.7f);
            CreatePineTree(42f, 10.5f);

            // Mega Trampoline into descent
            CreateFrostTrampoline(43.5f, 10.5f, 17f);
            CreateCoin(44.5f, 13.5f);
            CreateCoin(46.5f, 14.5f);
            CreateCoin(48.5f, 13.5f);
            CreateCoin(50f, 11.5f);

            // Tier 3 Landing
            CreatePlatform(51f, 60f, 8.5f);
            CreateSwingingMace(55.5f, 13.5f, 3.5f, 55f, 2.5f, 0f);
            CreateCoin(53f, 9.7f);
            CreateCoin(55.5f, 9.7f);
            CreateCoin(58f, 9.7f);

            // Vertical Descent via Moving Platform
            CreateMovingIcePlatform(62f, 8.5f, new Vector2(0f, -5.5f), 2.4f, 0.4f);
            CreateCoin(63f, 7f);
            CreateCoin(63f, 4.8f);

            // Mid-low platform
            CreatePlatform(65f, 75f, 3f);
            CreateSpikes(70f, 3f);
            CreateCoin(67f, 4.2f);
            CreateCoin(72f, 4.2f);

            // Final Sprint
            CreateGroundSegment(77f, 95f, 0f);
            CreateCoin(80f, 1.2f);
            CreateCoin(84f, 1.2f);

            CreateGoalArch(93f, 0f);
            CreateIcyChasm(-15f, 105f, -8f);
        }

        // =========================================================================
        // LEVEL 17: Frostfire Gauntlet (Blinking Phase Platforms & Moving Obstacles)
        // =========================================================================
        private void BuildLevel17()
        {
            SetupCamera(130f, 12f);
            SetupParallax(17);
            CreatePlayer(new Vector3(0f, 0.8f, 0f));

            CreateGroundSegment(-4f, 10f, 0f);
            CreateCoin(2.5f, 1.2f);
            CreateCoin(5f, 1.2f);
            CreateCoin(7.5f, 1.2f);

            // Alternating Phase Platforms over Chasm
            CreateBlinkingPlatform(12f, 1.5f, 0f);
            CreateCoin(12f, 2.8f);
            CreateCoin(14f, 3.8f);
            CreateCoin(16f, 4.8f);

            CreateBlinkingPlatform(18f, 4.5f, 0f);
            CreateCoin(18f, 5.8f);

            // High Platform with Moving Hazard
            CreatePlatform(21f, 32f, 5.5f);
            CreateMovingFrostHazard(26f, 6.2f, 3.5f, 3f);
            CreateCoin(23f, 6.8f);
            CreateCoin(26f, 7.5f);
            CreateCoin(29f, 6.8f);

            // Moving Platform through Rotating Frost Bar
            CreateMovingIcePlatform(34f, 5f, new Vector2(5f, 0f), 2.2f, 0.3f);
            CreateRotatingFrostBar(36.5f, 7f, 60f, false);
            CreateCoin(34f, 6.5f);
            CreateCoin(36.5f, 6.5f);
            CreateCoin(39f, 6.5f);

            // Mid Island & Checkpoint
            CreateGroundSegment(42f, 56f, 3f);
            CreateCheckpoint(44f, 3f);
            CreateCoin(44f, 4.2f);
            CreateCoin(47f, 4.2f);
            CreateCoin(50f, 4.2f);
            CreateCoin(53f, 4.2f);
            CreatePineTree(55f, 3f);

            // Phase Platform staircase
            CreateBlinkingPlatform(58f, 4.5f, 0f);
            CreateCoin(57f, 5.5f);
            CreateCoin(60f, 6.8f);
            CreateBlinkingPlatform(61f, 6f, 1.5f);
            CreateCoin(63f, 8f);
            CreateBlinkingPlatform(64f, 7.5f, 0f);
            CreateCoin(65.5f, 8.8f);

            // Falling Icicle trap on Crystal Bridge
            CreatePlatform(67f, 78f, 7.5f);
            CreateFallingIcicle(70f, 12f);
            CreateFallingIcicle(74f, 12f);
            CreateCoin(68.5f, 8.8f);
            CreateCoin(71.5f, 8.8f);
            CreateCoin(74.5f, 8.8f);

            // Mega Trampoline into Victory Valley
            CreateFrostTrampoline(77f, 7.5f, 17f);
            CreateCoin(78.5f, 10.5f);
            CreateCoin(80.5f, 11.2f);
            CreateCoin(82.5f, 9.5f);

            CreateGroundSegment(85f, 98f, 1f);
            CreateCoin(86f, 2.2f);
            CreateCoin(89f, 2.2f);
            CreateCoin(92f, 2.2f);

            CreateGoalArch(96f, 1f);
            CreateIcyChasm(-15f, 108f, -8f);
        }

        // =========================================================================
        // LEVEL 18: Portal Maze Ridge (Momentum Portals & High Altitude Leaps) -> Unlocks Aurora!
        // =========================================================================
        private void BuildLevel18()
        {
            SetupCamera(135f, 14f);
            SetupParallax(18);
            CreatePlayer(new Vector3(0f, 0.8f, 0f));

            CreateGroundSegment(-4f, 10f, 0f);
            CreateCoin(2.5f, 1.2f);
            CreateCoin(5f, 1.2f);
            CreateCoin(7.5f, 1.2f);

            // Portal Sequence 1: Fired upward into airborne coins
            CreatePortalPair(new Vector2(9.5f, 0f), new Vector2(15f, 3f), new Vector2(9f, 8f));
            CreateCoin(16f, 6f);
            CreateCoin(18f, 8f);
            CreateCoin(20f, 8f);
            CreateCoin(22f, 6.5f);
            CreateCoin(25f, 7.2f);

            // Landing on High Platform
            CreatePlatform(22f, 32f, 6f);
            CreateCoin(27f, 7.2f);
            CreateCoin(29.5f, 7.2f);
            CreateCoin(32f, 7.2f);

            // Drop down into Trampoline to Portal 2
            CreateGroundSegment(34f, 44f, 1f);
            CreateFrostTrampoline(36f, 1f, 16.5f);
            CreateCoin(35.5f, 3f);
            CreateCoin(37f, 5f);
            CreateCoin(39f, 4f);

            // Portal 2: Warps across chasm to mid checkpoint
            CreatePortalPair(new Vector2(41f, 1f), new Vector2(46f, 4f), new Vector2(7f, 3f));
            CreateGroundSegment(46f, 60f, 3f);
            CreateCheckpoint(48f, 3f);
            CreateCoin(48f, 4.2f);
            CreateCoin(51f, 4.2f);
            CreateCoin(54f, 4.2f);
            CreateCoin(57f, 4.2f);
            CreatePineTree(55f, 3f);

            // Tricky Portal 3: Velocity leap over Spikes and Mace
            CreatePortalPair(new Vector2(59f, 3f), new Vector2(64f, 7f), new Vector2(10f, 5f));
            CreateSpikes(68f, 1f);
            CreateSwingingMace(70f, 12f, 4f, 50f, 2.5f, 0f);
            CreateCoin(65f, 8.5f);
            CreateCoin(67.5f, 10f);
            CreateCoin(70f, 10.5f);
            CreateCoin(72.5f, 9.5f);
            CreateCoin(74.5f, 8f);

            // High Landing Platform
            CreatePlatform(74f, 84f, 6.5f);
            CreateCoin(77f, 7.7f);
            CreateCoin(80f, 7.7f);
            CreateCoin(83f, 7.7f);

            // Portal 4: Drop down into final goal run
            CreatePortalPair(new Vector2(83f, 6.5f), new Vector2(88f, 1f), new Vector2(6f, 2f));
            CreateGroundSegment(87f, 102f, 0f);
            CreateCoin(89f, 2.5f);
            CreateCoin(91.5f, 1.2f);
            CreateCoin(94f, 1.2f);
            CreateCoin(97f, 1.2f);

            CreateGoalArch(100f, 0f);
            CreateIcyChasm(-15f, 112f, -8f);
        }

        // =========================================================================
        // LEVEL 19: Blizzard Abyss (Wind Geysers, Moving Hazards & Spiked Chains)
        // =========================================================================
        private void BuildLevel19()
        {
            SetupCamera(140f, 14f);
            SetupParallax(19);
            CreatePlayer(new Vector3(0f, 0.8f, 0f));

            CreateGroundSegment(-4f, 10f, 0f);
            CreateCoin(2.5f, 1.2f);
            CreateCoin(5f, 1.2f);
            CreateCoin(7.5f, 1.2f);

            // Huge chasm bridged by Wind Draft Geyser
            CreateWindDraft(12f, -2f, 14f, 3f);
            CreateCoin(11f, 1.5f);
            CreateCoin(12.5f, 3.5f);
            CreateCoin(14f, 5f);
            CreateCoin(15.5f, 5.5f);

            // Middle floating platform with Swinging Mace
            CreatePlatform(17f, 26f, 4.5f);
            CreateSwingingMace(21.5f, 9.5f, 3.5f, 60f, 2.6f, 0f);
            CreateCoin(18.5f, 5.7f);
            CreateCoin(21.5f, 5.7f);
            CreateCoin(24.5f, 5.7f);

            // Second Wind Draft pushing to Tier 2
            CreateWindDraft(28f, 2f, 14f, 3f);
            CreateCoin(28f, 6.5f);
            CreateCoin(30f, 8.5f);
            CreateCoin(32f, 9.5f);

            // High Ridge & Mid Checkpoint
            CreatePlatform(33f, 48f, 8f);
            CreateCheckpoint(35f, 8f);
            CreateCoin(35f, 9.2f);
            CreateCoin(38f, 9.2f);
            CreateCoin(41f, 9.2f);
            CreateRotatingFrostBar(43f, 9.5f, 65f, true);
            CreateCoin(44f, 9.2f);

            // Moving Ice Platform over void with Falling Icicles
            CreateMovingIcePlatform(50f, 7.5f, new Vector2(6f, 0f), 2.2f, 0.3f);
            CreateFallingIcicle(53f, 13f);
            CreateCoin(49f, 9f);
            CreateCoin(52f, 9f);
            CreateCoin(55f, 9f);

            // Crumbling Step Sequence
            CreateCrumblingBlock(59f, 6.5f);
            CreateCoin(59f, 7.8f);
            CreateCrumblingBlock(62f, 5f);
            CreateCoin(62f, 6.3f);
            CreateCrumblingBlock(65f, 3.5f);
            CreateCoin(65f, 4.8f);

            // Speed Boost through Dual Buzz Saws
            CreateGroundSegment(68f, 86f, 1f);
            CreateSpeedBoost(70f, 1f, 16f);
            CreateMovingFrostHazard(75f, 1.8f, 3f, 3.5f);
            CreateMovingFrostHazard(81f, 1.8f, 3f, 3.5f);
            CreateCoin(71f, 2.2f);
            CreateCoin(74f, 2.2f);
            CreateCoin(77f, 2.2f);
            CreateCoin(80f, 2.2f);

            // Final Run
            CreateGroundSegment(88f, 102f, 0f);
            CreateCoin(86f, 1.2f);
            CreateCoin(90f, 1.2f);
            CreateCoin(94f, 1.2f);

            CreateGoalArch(100f, 0f);
            CreateIcyChasm(-15f, 112f, -8f);
        }

        // =========================================================================
        // LEVEL 20: Crown of the Glacier (Epic World 2 Grand Finale!)
        // =========================================================================
        private void BuildLevel20()
        {
            SetupCamera(150f, 16f);
            SetupParallax(20);
            CreatePlayer(new Vector3(0f, 0.8f, 0f));

            // Phase 1: High speed boost into Mega Trampoline
            CreateGroundSegment(-4f, 12f, 0f);
            CreateCoin(2.5f, 1.2f);
            CreateCoin(5f, 1.2f);
            CreateSpeedBoost(7f, 0f, 16f);
            CreateFrostTrampoline(11f, 0f, 18f);
            CreateCoin(8f, 2.5f);
            CreateCoin(11f, 4.5f);

            // Phase 2: High Sky Crystal Platform with Swinging Mace
            CreatePlatform(14f, 28f, 8f);
            CreateSwingingMace(22f, 13.5f, 4f, 60f, 2.8f, 0f);
            CreateCoin(15f, 8f);
            CreateCoin(19f, 9.2f);
            CreateCoin(23f, 9.2f);
            CreateCoin(27f, 9.2f);

            // Phase 3: Moving Lift down into Portal 1
            CreateMovingIcePlatform(30f, 7.5f, new Vector2(0f, -4.5f), 2.5f, 0.35f);
            CreateCoin(31f, 6.5f);
            CreateCoin(31f, 4.5f);
            CreateCoin(34f, 3.2f);

            CreatePortalPair(new Vector2(33f, 2.5f), new Vector2(38f, 9f), new Vector2(8f, 5f));
            CreateCoin(39f, 10.5f);
            CreateCoin(41.5f, 11.5f);
            CreateCoin(44f, 10.5f);

            // Phase 4: Mid Checkpoint Island
            CreatePlatform(44f, 58f, 9.5f);
            CreateCheckpoint(46f, 9.5f);
            CreateCoin(48f, 10.7f);
            CreateCoin(52f, 10.7f);
            CreateRotatingFrostBar(54f, 11f, 65f, false);
            CreateCoin(56f, 10.7f);

            // Phase 5: Alternating Blinking Ice Platforms over bottomless abyss
            CreateBlinkingPlatform(61f, 9f, 0f);
            CreateCoin(61f, 10.3f);
            CreateBlinkingPlatform(64f, 8f, 1.5f);
            CreateCoin(64f, 9.3f);
            CreateBlinkingPlatform(67f, 7f, 0f);
            CreateCoin(67f, 8.3f);

            // Phase 6: Wind Geyser launch into Portal 2
            CreateWindDraft(70f, 4f, 14f, 2f);
            CreateCoin(70f, 7f);
            CreateCoin(71.5f, 9.5f);
            CreateCoin(74f, 8.5f);

            CreatePortalPair(new Vector2(73f, 8f), new Vector2(78f, 3f), new Vector2(9f, 4f));
            CreateCoin(79f, 5.5f);
            CreateCoin(82f, 5.5f);
            CreateCoin(85f, 5.2f);

            // Phase 7: Moving platform between dual opposing Frost Bars
            CreatePlatform(85f, 98f, 4f);
            CreateRotatingFrostBar(88f, 5.5f, 60f, true);
            CreateRotatingFrostBar(94f, 5.5f, 60f, false);

            // Phase 8: Final Trampoline Leap into the Victory Igloo
            CreateFrostTrampoline(97f, 4f, 17f);
            CreateCoin(89f, 7.5f);
            CreateCoin(92f, 8.5f);

            CreateGroundSegment(103f, 120f, 2f);
            CreateCoin(96f, 3.2f);
            CreateCoin(100f, 3.2f);

            CreateGoalArch(117f, 2f);
            CreateIcyChasm(-15f, 130f, -8f);
        }


        // =========================================================================
        // LEVEL 21: Starlight Steppes (Cosmic Twilight, Bouncy Gravity Orbs) -> Unlocks Blaze!
        // =========================================================================
        private void BuildLevel21()
        {
            SetupCamera(130f, 12f);
            SetupParallax(21);
            CreatePlayer(new Vector3(0f, 0.8f, 0f));

            CreateGroundSegment(-4f, 12f, 0f);
            CreateCoin(2f, 1.2f);
            CreateCoin(4.5f, 1.2f);
            CreateCoin(7f, 1.2f);
            CreateCoin(9.5f, 1.2f);

            // Intro to Mid-Air Gravity Orb over bottomless chasm
            CreateGravityOrb(15f, 2.5f, 14f, 4.5f);
            CreateCoin(13f, 1.8f);
            CreateCoin(15f, 4.5f);
            CreateCoin(17f, 3f);

            // Landing Platform 1
            CreatePlatform(18f, 28f, 2f);
            CreateCoin(20f, 3.2f);
            CreateCoin(23f, 3.2f);
            CreateCoin(26f, 3.2f);

            // Double Gravity Orb Aerial Chain
            CreateGravityOrb(31f, 3.5f, 14f, 4.5f);
            CreateGravityOrb(36f, 6.5f, 14f, 4.5f);
            CreateCoin(31f, 5.5f);
            CreateCoin(33.5f, 7.5f);
            CreateCoin(36f, 8.5f);
            CreateCoin(38.5f, 7.5f);

            // Mid Island & Checkpoint
            CreateGroundSegment(40f, 56f, 4.5f);
            CreateCheckpoint(42f, 4.5f);
            CreateCoin(44.5f, 5.7f);
            CreateCoin(47f, 5.7f);
            CreatePineTree(49f, 4.5f);
            CreateCoin(51.5f, 5.7f);
            CreateCoin(54f, 5.7f);

            // Speed Boost into Gravity Orb launch
            CreateSpeedBoost(58f, 4.5f, 16f);
            CreateGravityOrb(64f, 6f, 15f, 5f);
            CreateCoin(60f, 5.5f);
            CreateCoin(64f, 8.5f);
            CreateCoin(67f, 7f);

            // High Platform
            CreatePlatform(69f, 80f, 5f);
            CreateSpikes(74f, 5f);
            CreateCoin(71f, 6.2f);
            CreateCoin(77f, 6.2f);

            // Final Run
            CreateGroundSegment(82f, 98f, 1f);
            CreateCoin(83f, 2.2f);
            CreateCoin(85f, 2.2f);
            CreateCoin(87.5f, 2.2f);
            CreateCoin(90f, 2.2f);
            CreateCoin(92.5f, 2.2f);
            CreateCoin(94.5f, 2.2f);
            CreateCoin(96.5f, 2.2f);

            CreateGoalArch(95f, 1f);
            CreateIcyChasm(-15f, 108f, -8f);
        }

        // =========================================================================
        // LEVEL 22: Laser Caverns (Pulsing Cryo-Lasers & Precision Timing)
        // =========================================================================
        private void BuildLevel22()
        {
            SetupCamera(135f, 12f);
            SetupParallax(22);
            CreatePlayer(new Vector3(0f, 0.8f, 0f));

            CreateGroundSegment(-4f, 12f, 0f);
            CreateCoin(2f, 1.2f);
            CreateCoin(4.5f, 1.2f);
            CreateCoin(7f, 1.2f);
            CreateCoin(9.5f, 1.2f);
            CreateCoin(11f, 1.2f);

            // First Cryo Laser Barrier
            CreateFrostLaser(15f, 0f, 4.5f, 0f, 2.0f, 1.4f);
            CreateGroundSegment(12f, 25f, 0f);
            CreateCoin(13f, 1.2f);
            CreateCoin(17f, 1.2f);
            CreateCoin(20f, 1.2f);
            CreateCoin(23f, 1.2f);

            // Moving Ice Platform between Dual Alternating Lasers
            CreateMovingIcePlatform(27f, 1f, new Vector2(8f, 0f), 2.2f, 0.35f);
            CreateFrostLaser(30f, 1f, 5f, 0f, 1.8f, 1.3f);
            CreateFrostLaser(34f, 1f, 5f, 1.0f, 1.8f, 1.3f);
            CreateCoin(28.5f, 2.5f);
            CreateCoin(30f, 2.5f);
            CreateCoin(32f, 2.8f);
            CreateCoin(35.5f, 2.5f);

            // Mid Island & Checkpoint
            CreateGroundSegment(38f, 54f, 1f);
            CreateCheckpoint(40f, 1f);
            CreateCoin(42.5f, 2.2f);
            CreateCoin(45f, 2.2f);
            CreatePineTree(47f, 1f);
            CreateCoin(49.5f, 2.2f);
            CreateCoin(52f, 2.2f);

            // Laser Corridor on High Platform
            CreatePlatform(56f, 72f, 4.5f);
            CreateFrostLaser(60f, 4.5f, 5f, 0f, 1.8f, 1.3f);
            CreateFrostLaser(66f, 4.5f, 5f, 1.2f, 1.8f, 1.3f);
            CreateCoin(58f, 5.7f);
            CreateCoin(61f, 5.7f);
            CreateCoin(63f, 5.7f);
            CreateCoin(69f, 5.7f);

            // Trampoline leap over wide laser field
            CreateFrostTrampoline(74f, 4.5f, 17f);
            CreateFrostLaser(79f, 0f, 7f, 0f, 2.5f, 1.2f);
            CreateCoin(75.5f, 7f);
            CreateCoin(78f, 9.5f);
            CreateCoin(81f, 8f);

            // Final Run
            CreateGroundSegment(83f, 100f, 1f);
            CreateCoin(85f, 2.2f);
            CreateCoin(88f, 2.2f);
            CreateCoin(91f, 2.2f);
            CreateCoin(92.5f, 2.2f);
            CreateCoin(94f, 2.2f);
            CreateCoin(96.5f, 2.2f);

            CreateGoalArch(98f, 1f);
            CreateIcyChasm(-15f, 110f, -8f);
        }

        // =========================================================================
        // LEVEL 23: Crushing Ice Citadel (Ceiling Smasher Gauntlet)
        // =========================================================================
        private void BuildLevel23()
        {
            SetupCamera(135f, 12f);
            SetupParallax(23);
            CreatePlayer(new Vector3(0f, 0.8f, 0f));

            CreateGroundSegment(-4f, 10f, 0f);
            CreateCoin(2f, 1.2f);
            CreateCoin(4.5f, 1.2f);
            CreateCoin(7f, 1.2f);
            CreateCoin(9f, 1.2f);

            // First Crushing Pillar
            CreateGroundSegment(10f, 24f, 0f);
            CreateCrushingPillar(16f, 6.5f, 4.5f);
            CreateCoin(13f, 1.2f);
            CreateCoin(16f, 1.2f);
            CreateCoin(19f, 1.2f);
            CreateCoin(22f, 1.2f);

            // Dual Crushing Pillars with Crumbling Steps
            CreateCrumblingBlock(27f, 0.5f);
            CreateCrushingPillar(27f, 7f, 4.5f);
            CreateCoin(27f, 1.8f);

            CreateCrumblingBlock(31f, 1.5f);
            CreateCrushingPillar(31f, 8f, 4.5f);
            CreateCoin(31f, 2.8f);

            // Mid Island & Checkpoint
            CreateGroundSegment(34f, 50f, 2f);
            CreateCheckpoint(36f, 2f);
            CreateCoin(36f, 3.2f);
            CreateCoin(38.5f, 3.2f);
            CreateCoin(41f, 3.2f);
            CreatePineTree(43f, 2f);
            CreateCoin(45.5f, 3.2f);
            CreateCoin(48f, 3.2f);

            // Speed Boost beneath Rapid Crushing Pillars
            CreateGroundSegment(52f, 74f, 2f);
            CreateSpeedBoost(54f, 2f, 15f);
            CreateCrushingPillar(60f, 8.5f, 4.5f);
            CreateCrushingPillar(66f, 8.5f, 4.5f);
            CreateCoin(57f, 3.2f);
            CreateCoin(60f, 3.2f);
            CreateCoin(63f, 3.2f);
            CreateCoin(66f, 3.2f);
            CreateCoin(69f, 3.2f);
            CreateCoin(72f, 3.2f);
            CreateCoin(74f, 3.2f);

            // Moving Ice Platform through final Crusher
            CreateMovingIcePlatform(76f, 2f, new Vector2(6f, 0f), 2.2f, 0.35f);
            CreateCrushingPillar(79f, 8.5f, 4.5f);
            CreateCoin(77.5f, 3.5f);
            CreateCoin(80.5f, 3.5f);

            // Final Run
            CreateGroundSegment(84f, 102f, 1f);
            CreateCoin(85.5f, 2.2f);
            CreateCoin(87f, 2.2f);
            CreateCoin(90f, 2.2f);
            CreateCoin(93f, 2.2f);
            CreateCoin(96f, 2.2f);
            CreateCoin(98.5f, 2.2f);

            CreateGoalArch(100f, 1f);
            CreateIcyChasm(-15f, 112f, -8f);
        }

        // =========================================================================
        // LEVEL 24: Cosmic Conveyor Way (High-Speed Conveyors) -> Unlocks Cyber!
        // =========================================================================
        private void BuildLevel24()
        {
            SetupCamera(140f, 12f);
            SetupParallax(24);
            CreatePlayer(new Vector3(0f, 0.8f, 0f));

            CreateGroundSegment(-4f, 10f, 0f);
            CreateCoin(2f, 1.2f);
            CreateCoin(4.5f, 1.2f);
            CreateCoin(7f, 1.2f);

            // First Forward High-Speed Conveyor Belt
            CreateConveyorBelt(10f, 24f, 0f, 6.5f);
            CreateCoin(12f, 1.2f);
            CreateCoin(15f, 1.2f);
            CreateCoin(18f, 1.2f);
            CreateCoin(21f, 1.2f);

            // Launch from Conveyor into Gravity Orb
            CreateGravityOrb(28f, 2.5f, 14f, 5f);
            CreateCoin(25f, 2.5f);
            CreateCoin(28f, 4.5f);
            CreateCoin(31f, 3.5f);

            // High Conveyor with Frost Saw
            CreateConveyorBelt(33f, 48f, 3f, 6.5f);
            CreateMovingFrostHazard(40f, 3.8f, 3f, 3.5f);
            CreateCoin(35f, 4.2f);
            CreateCoin(38f, 4.2f);
            CreateCoin(42f, 4.2f);
            CreateCoin(45f, 4.2f);

            // Mid Island & Checkpoint
            CreateGroundSegment(50f, 64f, 2f);
            CreateCheckpoint(52f, 2f);
            CreateCoin(54.5f, 3.2f);
            CreateCoin(57f, 3.2f);
            CreatePineTree(59f, 2f);
            CreateCoin(61.5f, 3.2f);

            // Reverse Conveyor Challenge (Pushing against player under Mace)
            CreateConveyorBelt(66f, 80f, 2f, -3.5f);
            CreateSwingingMace(73f, 7.5f, 3.5f, 55f, 2.5f, 0f);
            CreateCoin(68f, 3.2f);
            CreateCoin(71f, 3.2f);
            CreateCoin(74f, 3.2f);
            CreateCoin(77f, 3.2f);

            // Super Conveyor into Trampoline Launch
            CreateConveyorBelt(82f, 92f, 2f, 8f);
            CreateFrostTrampoline(93f, 2f, 17f);
            CreateCoin(84f, 3.2f);
            CreateCoin(87f, 3.2f);
            CreateCoin(90f, 3.2f);
            CreateCoin(94.5f, 6f);
            CreateCoin(97f, 8f);

            // Goal Island
            CreateGroundSegment(99f, 114f, 1f);
            CreateCoin(101.5f, 2.2f);
            CreateCoin(104f, 2.2f);
            CreateCoin(107f, 2.2f);
            CreateCoin(109f, 2.2f);

            CreateGoalArch(110f, 1f);
            CreateIcyChasm(-15f, 122f, -8f);
        }

        // =========================================================================
        // LEVEL 25: Antigravity Vaults (Antigravity Wells & Ceiling Platforming)
        // =========================================================================
        private void BuildLevel25()
        {
            SetupCamera(135f, 14f);
            SetupParallax(25);
            CreatePlayer(new Vector3(0f, 0.8f, 0f));

            CreateGroundSegment(-4f, 10f, 0f);
            CreateCoin(2f, 1.2f);
            CreateCoin(4.5f, 1.2f);
            CreateCoin(7f, 1.2f);

            // Antigravity Lift Well 1
            CreateGravityFlipPad(11f, 0f, 13.5f, 4f);
            CreateCoin(11f, 2.5f);
            CreateCoin(12.5f, 5f);
            CreateCoin(14f, 7f);

            // Ceiling High Platform
            CreatePlatform(15f, 28f, 7.5f);
            CreateCoin(17f, 8.7f);
            CreateCoin(18.5f, 8.7f);
            CreateCoin(20f, 8.7f);
            CreateCoin(23f, 8.7f);
            CreateCoin(26f, 8.7f);

            // Drop down into Gravity Orb to cross chasm
            CreateGravityOrb(32f, 3.5f, 14f, 4.5f);
            CreateCoin(29.5f, 6f);
            CreateCoin(32f, 5.5f);
            CreateCoin(34.5f, 4.5f);

            // Mid Island & Checkpoint
            CreateGroundSegment(37f, 52f, 2f);
            CreateCheckpoint(39f, 2f);
            CreateCoin(41.5f, 3.2f);
            CreateCoin(44f, 3.2f);
            CreatePineTree(46f, 2f);
            CreateCoin(48.5f, 3.2f);

            // Antigravity Well 2: Lifting over Frost Bar
            CreateGravityFlipPad(53f, 2f, 14f, 4f);
            CreateRotatingFrostBar(58f, 4f, 65f, true);
            CreateCoin(54f, 4.5f);
            CreateCoin(56.5f, 7f);
            CreateCoin(59f, 7.5f);
            CreateCoin(61.5f, 5f);

            // Floating High Platform with Frost Laser
            CreatePlatform(64f, 78f, 4f);
            CreateFrostLaser(71f, 4f, 5f, 0f, 1.8f, 1.3f);
            CreateCoin(66f, 5.2f);
            CreateCoin(69f, 5.2f);
            CreateCoin(73f, 5.2f);
            CreateCoin(76f, 5.2f);

            // Final Descent Run
            CreateGroundSegment(80f, 98f, 1f);
            CreateCoin(83f, 2.2f);
            CreateCoin(86f, 2.2f);
            CreateCoin(89f, 2.2f);
            CreateCoin(92f, 2.2f);
            CreateCoin(94f, 2.2f);

            CreateGoalArch(95f, 1f);
            CreateIcyChasm(-15f, 108f, -8f);
        }

        // =========================================================================
        // LEVEL 26: The Starlit Abyss (Multi-Orb Mid-Air Jump Chains)
        // =========================================================================
        private void BuildLevel26()
        {
            SetupCamera(140f, 15f);
            SetupParallax(26);
            CreatePlayer(new Vector3(0f, 0.8f, 0f));

            CreateGroundSegment(-4f, 10f, 0f);
            CreateCoin(2.5f, 1.2f);
            CreateCoin(5f, 1.2f);
            CreateCoin(7.5f, 1.2f);

            // Triple Gravity Orb Chain across vast canyon
            CreateGravityOrb(13f, 1.5f, 13f, 4f);
            CreateGravityOrb(18f, 4f, 13f, 4f);
            CreateGravityOrb(23f, 6.5f, 13f, 4f);
            CreateCoin(13f, 3.5f);
            CreateCoin(15.5f, 5f);
            CreateCoin(18f, 6f);
            CreateCoin(20.5f, 7.5f);
            CreateCoin(23f, 8.5f);

            // High Platform 1
            CreatePlatform(26f, 38f, 8f);
            CreateSpikes(32f, 8f);
            CreateCoin(28f, 9.2f);
            CreateCoin(30f, 9.2f);
            CreateCoin(33f, 9.2f);
            CreateCoin(35f, 9.2f);

            // Trampoline into Orb 4
            CreateFrostTrampoline(39f, 8f, 16.5f);
            CreateGravityOrb(44f, 11f, 14f, 4.5f);
            CreateCoin(40.5f, 11f);
            CreateCoin(44f, 13f);
            CreateCoin(47f, 11f);

            // Mid Island & Checkpoint
            CreateGroundSegment(49f, 64f, 7f);
            CreateCheckpoint(51f, 7f);
            CreateCoin(53.5f, 8.2f);
            CreateCoin(56f, 8.2f);
            CreatePineTree(58f, 7f);
            CreateCoin(60.5f, 8.2f);

            // Descending Gravity Orb flight
            CreateGravityOrb(68f, 6f, 12f, 4f);
            CreateGravityOrb(73f, 4f, 12f, 4f);
            CreateCoin(66f, 7f);
            CreateCoin(68f, 8f);
            CreateCoin(71f, 6.5f);
            CreateCoin(73f, 5.5f);

            // Low Platform with Frost Saw
            CreatePlatform(76f, 88f, 2.5f);
            CreateMovingFrostHazard(82f, 3.2f, 3f, 3.5f);
            CreateCoin(78f, 3.7f);
            CreateCoin(80f, 3.7f);
            CreateCoin(83f, 3.7f);
            CreateCoin(85f, 3.7f);

            // Final Run
            CreateGroundSegment(90f, 106f, 0f);
            CreateCoin(93f, 1.2f);
            CreateCoin(96f, 1.2f);
            CreateCoin(99f, 1.2f);
            CreateCoin(102f, 1.2f);

            CreateGoalArch(104f, 0f);
            CreateIcyChasm(-15f, 115f, -8f);
        }

        // =========================================================================
        // LEVEL 27: Pillar of Frostfire (Vertical Crusher & Laser Gauntlet) -> Unlocks Cosmo!
        // =========================================================================
        private void BuildLevel27()
        {
            SetupCamera(140f, 14f);
            SetupParallax(27);
            CreatePlayer(new Vector3(0f, 0.8f, 0f));

            CreateGroundSegment(-4f, 10f, 0f);
            CreateCoin(2.5f, 1.2f);
            CreateCoin(5f, 1.2f);
            CreateCoin(7.5f, 1.2f);
            CreateCoin(9.5f, 1.2f);

            // Laser + Crusher Combination
            CreateGroundSegment(10f, 28f, 0f);
            CreateFrostLaser(15f, 0f, 5f, 0f, 1.8f, 1.3f);
            CreateCrushingPillar(21f, 7f, 4.5f);
            CreateCoin(12.5f, 1.2f);
            CreateCoin(18f, 1.2f);
            CreateCoin(24f, 1.2f);

            // Vertical Lift Platform past Laser
            CreateMovingIcePlatform(30f, 0.5f, new Vector2(0f, 5.5f), 2.5f, 0.4f);
            CreateFrostLaser(34f, 3f, 4f, 0.5f, 1.8f, 1.2f);
            CreateCoin(31f, 2.5f);
            CreateCoin(31f, 4.5f);
            CreateCoin(31f, 6.5f);

            // High Platform & Mid Checkpoint
            CreatePlatform(35f, 50f, 6.5f);
            CreateCheckpoint(37f, 6.5f);
            CreateCrushingPillar(43f, 13.5f, 4.5f);
            CreateCoin(39.5f, 7.7f);
            CreateCoin(41f, 7.7f);
            CreateCoin(43f, 7.7f);
            CreateCoin(46.5f, 7.7f);

            // Conveyor Belt under Swinging Mace
            CreateConveyorBelt(52f, 68f, 6.5f, 5.5f);
            CreateSwingingMace(60f, 12.5f, 4f, 60f, 2.8f, 0f);
            CreateCoin(54f, 7.7f);
            CreateCoin(58f, 7.7f);
            CreateCoin(62f, 7.7f);
            CreateCoin(66f, 7.7f);

            // Trampoline into Gravity Orb
            CreateFrostTrampoline(70f, 6.5f, 16f);
            CreateGravityOrb(76f, 9.5f, 14f, 4.5f);
            CreateCoin(71.5f, 9f);
            CreateCoin(74f, 11f);
            CreateCoin(76f, 12f);
            CreateCoin(78.5f, 10f);

            // Landing Platform with final Crusher
            CreatePlatform(80f, 92f, 4f);
            CreateCrushingPillar(86f, 11f, 4.5f);
            CreateCoin(82f, 5.2f);
            CreateCoin(86f, 5.2f);
            CreateCoin(89f, 5.2f);

            // Final Run
            CreateGroundSegment(94f, 110f, 1f);
            CreateCoin(95f, 2.2f);
            CreateCoin(97f, 2.2f);
            CreateCoin(100f, 2.2f);
            CreateCoin(103f, 2.2f);
            CreateCoin(106f, 2.2f);

            CreateGoalArch(108f, 1f);
            CreateIcyChasm(-15f, 120f, -8f);
        }

        // =========================================================================
        // LEVEL 28: Orbital Relay (Multi-Portal & Gravity Orb Momentum Networks)
        // =========================================================================
        private void BuildLevel28()
        {
            SetupCamera(140f, 14f);
            SetupParallax(28);
            CreatePlayer(new Vector3(0f, 0.8f, 0f));

            CreateGroundSegment(-4f, 10f, 0f);
            CreateCoin(2.5f, 1.2f);
            CreateCoin(5f, 1.2f);
            CreateCoin(7.5f, 1.2f);

            // Portal 1: Launch into Gravity Orb
            CreatePortalPair(new Vector2(9.5f, 0f), new Vector2(15f, 3f), new Vector2(9f, 6f));
            CreateGravityOrb(20f, 6.5f, 14f, 4.5f);
            CreateCoin(17f, 6f);
            CreateCoin(20f, 8.5f);
            CreateCoin(23f, 7f);

            // Landing High Platform
            CreatePlatform(24f, 36f, 5.5f);
            CreateCoin(26f, 6.7f);
            CreateCoin(29f, 6.7f);
            CreateCoin(32f, 6.7f);
            CreateCoin(34f, 6.7f);

            // Conveyor into Portal 2
            CreateConveyorBelt(37f, 47f, 5.5f, 6f);
            CreateCoin(39f, 6.7f);
            CreateCoin(42f, 6.7f);
            CreateCoin(45f, 6.7f);

            // Portal 2: Warp to Mid Checkpoint Island
            CreatePortalPair(new Vector2(47.5f, 5.5f), new Vector2(52f, 2f), new Vector2(7f, 2f));
            CreateGroundSegment(51f, 66f, 1f);
            CreateCheckpoint(53f, 1f);
            CreateCoin(55.5f, 2.2f);
            CreateCoin(58f, 2.2f);
            CreatePineTree(60f, 1f);
            CreateCoin(62.5f, 2.2f);
            CreateCoin(64.5f, 2.2f);

            // Antigravity Well into Portal 3
            CreateGravityFlipPad(67f, 1f, 14f, 3.5f);
            CreateCoin(67f, 3.5f);
            CreateCoin(69f, 6.5f);

            CreatePortalPair(new Vector2(70f, 7f), new Vector2(75f, 10f), new Vector2(9f, 4f));
            CreateCoin(77f, 12f);
            CreateCoin(80f, 12f);

            // High Celestial Ledge
            CreatePlatform(81f, 92f, 9f);
            CreateCoin(83f, 10.2f);
            CreateCoin(84.5f, 10.2f);
            CreateCoin(86f, 10.2f);
            CreateCoin(87.5f, 10.2f);
            CreateCoin(89f, 10.2f);

            // Portal 4: Launch into finish sprint
            CreatePortalPair(new Vector2(92f, 9f), new Vector2(97f, 1f), new Vector2(7f, 2f));
            CreateGroundSegment(96f, 112f, 0f);
            CreateCoin(99f, 1.2f);
            CreateCoin(102f, 1.2f);
            CreateCoin(105f, 1.2f);
            CreateCoin(108f, 1.2f);

            CreateGoalArch(110f, 0f);
            CreateIcyChasm(-15f, 122f, -8f);
        }

        // =========================================================================
        // LEVEL 29: Supernova Ridge (Intense Pre-Finale Mastery Gauntlet)
        // =========================================================================
        private void BuildLevel29()
        {
            SetupCamera(145f, 14f);
            SetupParallax(29);
            CreatePlayer(new Vector3(0f, 0.8f, 0f));

            CreateGroundSegment(-4f, 10f, 0f);
            CreateCoin(2.5f, 1.2f);
            CreateCoin(5f, 1.2f);
            CreateCoin(7.5f, 1.2f);

            // Conveyor through Frost Laser & Crusher
            CreateConveyorBelt(10f, 26f, 0f, 6.5f);
            CreateFrostLaser(15f, 0f, 4.5f, 0f, 1.7f, 1.2f);
            CreateCrushingPillar(21f, 6.5f, 4.5f);
            CreateCoin(12f, 1.2f);
            CreateCoin(15f, 2.5f);
            CreateCoin(18f, 1.2f);
            CreateCoin(21f, 2.5f);
            CreateCoin(24f, 1.2f);

            // Launch into Gravity Orb
            CreateGravityOrb(30f, 2.5f, 14f, 5f);
            CreateCoin(27.5f, 2.5f);
            CreateCoin(30f, 4.5f);
            CreateCoin(33f, 3.5f);

            // High Platform with Moving Buzz Saw
            CreatePlatform(35f, 48f, 3.5f);
            CreateMovingFrostHazard(41f, 4.2f, 3.5f, 4f);
            CreateCoin(37f, 4.7f);
            CreateCoin(41f, 5.5f);
            CreateCoin(45f, 4.7f);

            // Mid Island & Checkpoint
            CreateGroundSegment(50f, 65f, 2f);
            CreateCheckpoint(52f, 2f);
            CreateCoin(54.5f, 3.2f);
            CreateCoin(57f, 3.2f);
            CreatePineTree(59f, 2f);
            CreateCoin(61.5f, 3.2f);

            // Dual Blinking Platforms under Crushing Pillar
            CreateBlinkingPlatform(68f, 3.5f, 0f);
            CreateBlinkingPlatform(73f, 5f, 1.5f);
            CreateCrushingPillar(70.5f, 11f, 4.5f);
            CreateCoin(68f, 4.8f);
            CreateCoin(70.5f, 6f);
            CreateCoin(73f, 6.3f);

            // Speed Boost into Mega Trampoline
            CreateConveyorBelt(76f, 88f, 5f, 7.5f);
            CreateFrostTrampoline(89f, 5f, 17f);
            CreateCoin(78f, 6.2f);
            CreateCoin(82f, 6.2f);
            CreateCoin(86f, 6.2f);
            CreateCoin(90f, 8f);
            CreateCoin(93f, 10f);

            // Final Run
            CreateGroundSegment(95f, 112f, 1f);
            CreateCoin(98f, 2.2f);
            CreateCoin(101f, 2.2f);
            CreateCoin(104f, 2.2f);
            CreateCoin(107f, 2.2f);
            CreateCoin(109f, 2.2f);

            CreateGoalArch(111f, 1f);
            CreateIcyChasm(-15f, 122f, -8f);
        }

        // =========================================================================
        // LEVEL 30: The Cosmic Crown (Grand Finale of Snowball Adventure 2!)
        // =========================================================================
        private void BuildLevel30()
        {
            SetupCamera(160f, 16f);
            SetupParallax(30);
            CreatePlayer(new Vector3(0f, 0.8f, 0f));

            // Phase 1: High speed conveyor launch into Gravity Orb
            CreateGroundSegment(-4f, 10f, 0f);
            CreateCoin(2.5f, 1.2f);
            CreateCoin(5f, 1.2f);
            CreateCoin(8f, 1.2f);
            CreateConveyorBelt(10f, 20f, 0f, 7.5f);
            CreateCoin(12.5f, 1.2f);
            CreateCoin(16f, 1.2f);

            CreateGravityOrb(24f, 2.5f, 14f, 5f);
            CreateCoin(21f, 2.5f);
            CreateCoin(24f, 4.5f);

            // Phase 2: High Sky Crystal Platform with Swinging Mace & Laser
            CreatePlatform(27f, 42f, 5.5f);
            CreateSwingingMace(34f, 11.5f, 4f, 60f, 2.8f, 0f);
            CreateFrostLaser(39f, 5.5f, 4.5f, 0.5f, 1.8f, 1.3f);
            CreateCoin(29f, 6.7f);
            CreateCoin(34f, 7.2f);
            CreateCoin(38f, 6.7f);

            // Phase 3: Portal 1 into Checkpoint Island
            CreatePortalPair(new Vector2(42f, 5.5f), new Vector2(47f, 3f), new Vector2(8f, 4f));
            CreateGroundSegment(46f, 62f, 2f);
            CreateCheckpoint(48f, 2f);
            CreateCoin(50.5f, 3.2f);
            CreateCoin(53f, 3.2f);
            CreatePineTree(55f, 2f);
            CreateCoin(57.5f, 3.2f);
            CreateCoin(60f, 3.2f);

            // Phase 4: Crushing Pillar over moving platform
            CreateMovingIcePlatform(64f, 2f, new Vector2(7f, 0f), 2.2f, 0.35f);
            CreateCrushingPillar(67.5f, 8.5f, 4.5f);
            CreateCoin(65f, 3.5f);
            CreateCoin(68f, 4f);
            CreateCoin(71f, 3.5f);

            // Phase 5: Antigravity Well into Sky Trampoline
            CreateGravityFlipPad(73f, 2f, 14f, 3.5f);
            CreateFrostTrampoline(78f, 7f, 18f);
            CreateCoin(74f, 4.5f);
            CreateCoin(76f, 7f);
            CreateCoin(79.5f, 10f);
            CreateCoin(82f, 12f);

            // Phase 6: Portal 2 Warp across the Cosmic Vortex
            CreatePortalPair(new Vector2(83f, 11f), new Vector2(88f, 5f), new Vector2(8f, 4f));
            CreatePlatform(87f, 102f, 4f);
            CreateRotatingFrostBar(94f, 5.5f, 70f, true);
            CreateCoin(90f, 5.2f);
            CreateCoin(94f, 6.5f);
            CreateCoin(98f, 5.2f);

            // Phase 7: Final Trampoline Leap into the Grand Snow King Victory Arch!
            CreateFrostTrampoline(101f, 4f, 17f);
            CreateCoin(103f, 7f);
            CreateCoin(106f, 8.5f);

            CreateGroundSegment(108f, 126f, 2f);
            CreateCoin(110f, 3.2f);
            CreateCoin(113f, 3.2f);
            CreateCoin(116f, 3.2f);
            CreateCoin(119f, 3.2f);

            CreateGoalArch(122f, 2f);
            CreateIcyChasm(-15f, 136f, -8f);
        }



        // =========================================================================
        // HELPER BUILDER METHODS
        // =========================================================================
        private void SetupCamera(float maxX, float maxY)
        {
            SmoothFollowCamera sfc = Camera.main != null ? Camera.main.GetComponent<SmoothFollowCamera>() : null;
            if (sfc == null && Camera.main != null)
            {
                sfc = Camera.main.gameObject.AddComponent<SmoothFollowCamera>();
            }
            if (sfc == null) return;

            sfc.minX = 0f;
            sfc.maxX = maxX;
            sfc.minY = 1.0f;
            sfc.maxY = maxY;
        }

        private void SetupParallax(int level = 1)
        {
            Camera cam = Camera.main;
            if (cam == null) return;

            string skyPath, mtnPath, pinePath;
            if (level >= 21)
            {
                skyPath = "Sprites/bg_cosmic_sky";
                mtnPath = "Sprites/bg_cosmic_mountains";
                pinePath = "Sprites/bg_cosmic_pines";
            }
            else if (level >= 11)
            {
                skyPath = "Sprites/bg_aurora_sky";
                mtnPath = "Sprites/bg_aurora_mountains";
                pinePath = "Sprites/bg_aurora_pines";
            }
            else
            {
                skyPath = "Sprites/bg_sky";
                mtnPath = "Sprites/bg_mountains";
                pinePath = "Sprites/bg_pines";
            }

            Sprite skySpr = Resources.Load<Sprite>(skyPath);
            Sprite mtnSpr = Resources.Load<Sprite>(mtnPath);
            Sprite pineSpr = Resources.Load<Sprite>(pinePath);

            if (skySpr != null)
            {
                GameObject skyObj = new GameObject("SkyBackdrop");
                skyObj.transform.SetParent(cam.transform, false);
                skyObj.transform.localPosition = new Vector3(0f, 0f, 20f);
                skyObj.transform.localScale = new Vector3(28f, 16f, 1f);

                SpriteRenderer skyRend = skyObj.AddComponent<SpriteRenderer>();
                skyRend.sprite = skySpr;
                skyRend.sortingOrder = -50;
            }

            GameObject bgRoot = new GameObject("Parallax_Backgrounds");

            if (mtnSpr != null)
            {
                GameObject mtnLayerObj = new GameObject("MountainLayer");
                mtnLayerObj.transform.SetParent(bgRoot.transform, false);
                SmoothParallaxLayer spl = mtnLayerObj.AddComponent<SmoothParallaxLayer>();
                spl.parallaxFactor = 0.12f;
                spl.Initialize(cam.transform, mtnSpr, -30, 24f, 2.6f, 2.2f);
            }

            if (pineSpr != null)
            {
                GameObject pineLayerObj = new GameObject("PineForestLayer");
                pineLayerObj.transform.SetParent(bgRoot.transform, false);
                SmoothParallaxLayer spl = pineLayerObj.AddComponent<SmoothParallaxLayer>();
                spl.parallaxFactor = 0.38f;
                spl.Initialize(cam.transform, pineSpr, -20, 24f, 2.6f, -0.6f);
            }

            // Universal Falling Snow Weather Effect across all levels
            SetupFallingSnowEffect(cam);
        }

        private void SetupFallingSnowEffect(Camera cam)
        {
            if (cam == null) return;
            if (cam.transform.Find("FallingSnowSystem") != null) return;

            GameObject snowObj = new GameObject("FallingSnowSystem");
            snowObj.transform.SetParent(cam.transform, false);
            snowObj.transform.localPosition = new Vector3(0f, 6.5f, 10f);

            ParticleSystem ps = snowObj.AddComponent<ParticleSystem>();
            var main = ps.main;
            main.startLifetime = 5.5f;
            main.startSpeed = 0f;
            main.startSize = new ParticleSystem.MinMaxCurve(0.12f, 0.32f);
            main.startColor = new Color(1f, 1f, 1f, 0.8f);
            main.maxParticles = 250;
            main.simulationSpace = ParticleSystemSimulationSpace.World;

            var emission = ps.emission;
            emission.rateOverTime = 42f;

            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Box;
            shape.scale = new Vector3(28f, 1f, 1f);

            var vel = ps.velocityOverLifetime;
            vel.enabled = true;
            vel.space = ParticleSystemSimulationSpace.World;
            vel.x = new ParticleSystem.MinMaxCurve(-0.6f, 0.6f);
            vel.y = new ParticleSystem.MinMaxCurve(-3.0f, -1.6f);
            vel.z = new ParticleSystem.MinMaxCurve(0f, 0f);

            ParticleSystemRenderer psr = snowObj.GetComponent<ParticleSystemRenderer>();
            psr.sortingOrder = -15; // In front of far parallax mountains, behind gameplay objects

            Sprite spr = snowflakeSprite ?? Resources.Load<Sprite>("Sprites/particle_snowflake");
            Shader sprShader = Shader.Find("Universal Render Pipeline/2D/Sprite-Unlit") ?? Shader.Find("Sprites/Default");
            if (sprShader != null)
            {
                Material mat = new Material(sprShader);
                if (spr != null) mat.mainTexture = spr.texture;
                psr.material = mat;
            }
        }

        private void CreateGroundSegment(float startX, float endX, float y, bool deepFrost = false)
        {
            float width = endX - startX;
            float midX = startX + width * 0.5f;

            GameObject ground = new GameObject($"Ground_{startX}_{endX}");
            ground.transform.SetParent(levelRoot.transform, false);
            float height = 1.28f;
            ground.transform.position = new Vector3(midX, y - height * 0.5f, 0f);

            SpriteRenderer rend = ground.AddComponent<SpriteRenderer>();
            int lvl = GameDataManager.Data.selectedLevel;
            Sprite chosen;
            if (lvl >= 21)
                chosen = cosmicGroundSprite ?? deepFrostGroundSprite ?? groundBlockSprite;
            else if (lvl >= 11 || deepFrost)
                chosen = deepFrostGroundSprite ?? groundBlockSprite;
            else
                chosen = groundBlockSprite;

            rend.sprite = chosen;
            rend.drawMode = SpriteDrawMode.Tiled;
            rend.size = new Vector2(width, height);
            rend.sortingOrder = 2;

            BoxCollider2D col = ground.AddComponent<BoxCollider2D>();
            col.size = new Vector2(width, height);
        }

        private void CreatePlatform(float startX, float endX, float y, bool isCrystal = false)
        {
            float width = endX - startX;
            float midX = startX + width * 0.5f;

            GameObject plat = new GameObject($"Platform_{startX}_{endX}");
            plat.transform.SetParent(levelRoot.transform, false);
            float height = 0.32f;
            plat.transform.position = new Vector3(midX, y - height * 0.5f, 0f);

            SpriteRenderer rend = plat.AddComponent<SpriteRenderer>();
            int lvl = GameDataManager.Data.selectedLevel;
            Sprite chosen;
            if (lvl >= 21)
                chosen = cosmicPlatformSprite ?? crystalPlatformSprite ?? woodPlatformSprite;
            else if (lvl >= 11 || isCrystal)
                chosen = crystalPlatformSprite ?? woodPlatformSprite;
            else
                chosen = woodPlatformSprite;

            rend.sprite = chosen;
            rend.drawMode = SpriteDrawMode.Tiled;
            rend.size = new Vector2(width, height);
            rend.sortingOrder = 3;

            BoxCollider2D col = plat.AddComponent<BoxCollider2D>();
            col.size = new Vector2(width, height);
        }

        private void CreateIcePlatform(float startX, float endX, float y)
        {
            float width = endX - startX;
            float midX = startX + width * 0.5f;

            GameObject plat = new GameObject($"IcePlat_{startX}_{endX}");
            plat.transform.SetParent(levelRoot.transform, false);
            float height = 1.28f;
            plat.transform.position = new Vector3(midX, y - height * 0.5f, 0f);

            SpriteRenderer rend = plat.AddComponent<SpriteRenderer>();
            rend.sprite = iceBlockSprite;
            rend.drawMode = SpriteDrawMode.Tiled;
            rend.size = new Vector2(width, height);
            rend.sortingOrder = 3;

            BoxCollider2D col = plat.AddComponent<BoxCollider2D>();
            col.size = new Vector2(width, height);
        }

        private void CreatePineTree(float x, float y)
        {
            GameObject tree = new GameObject("PineTree");
            tree.transform.SetParent(levelRoot.transform, false);
            tree.transform.position = new Vector3(x, y + 0.96f, 0f);

            SpriteRenderer rend = tree.AddComponent<SpriteRenderer>();
            rend.sprite = pineTreeSprite;
            rend.sortingOrder = 1;
        }

        private void CreateCoin(float x, float y)
        {
            GameObject coin = new GameObject("Coin");
            coin.transform.SetParent(levelRoot.transform, false);
            coin.transform.position = new Vector3(x, y, 0f);
            coin.transform.localScale = new Vector3(0.65f, 0.65f, 1f);

            SpriteRenderer rend = coin.AddComponent<SpriteRenderer>();
            rend.sprite = coinSprite;
            rend.sortingOrder = 8;

            CircleCollider2D col = coin.AddComponent<CircleCollider2D>();
            col.isTrigger = true;
            col.radius = 0.32f;

            coin.AddComponent<CoinItem>();
        }

        private void CreateSpikes(float x, float y)
        {
            GameObject spike = new GameObject("IceSpikes");
            spike.transform.SetParent(levelRoot.transform, false);
            spike.transform.position = new Vector3(x, y + 0.28f, 0f);
            spike.transform.localScale = new Vector3(0.85f, 0.85f, 1f);

            SpriteRenderer rend = spike.AddComponent<SpriteRenderer>();
            rend.sprite = spikeSprite;
            rend.sortingOrder = 4;

            PolygonCollider2D col = spike.AddComponent<PolygonCollider2D>();
            col.isTrigger = true;

            spike.AddComponent<HazardSpike>();
        }

        private void CreateBounceSpring(float x, float y, float force)
        {
            GameObject spring = new GameObject("BounceSpring");
            spring.transform.SetParent(levelRoot.transform, false);
            spring.transform.position = new Vector3(x, y + 0.28f, 0f);
            spring.transform.localScale = new Vector3(0.9f, 0.9f, 1f);

            SpriteRenderer rend = spring.AddComponent<SpriteRenderer>();
            rend.sprite = springSprite;
            rend.sortingOrder = 4;

            BoxCollider2D col = spring.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            col.size = new Vector2(0.8f, 0.65f);

            BounceSpring bs = spring.AddComponent<BounceSpring>();
            bs.bounceForce = force;
            bs.springVisual = spring.transform;
        }

        private void CreateCrumblingBlock(float x, float y)
        {
            GameObject crumb = new GameObject("CrumblingIce");
            crumb.transform.SetParent(levelRoot.transform, false);
            crumb.transform.position = new Vector3(x, y, 0f);

            SpriteRenderer rend = crumb.AddComponent<SpriteRenderer>();
            rend.sprite = crumblingSprite;
            rend.sortingOrder = 3;

            BoxCollider2D col = crumb.AddComponent<BoxCollider2D>();
            col.size = new Vector2(1.25f, 0.4f);

            crumb.AddComponent<CrumblingPlatform>();
        }

        private void CreateMovingFrostHazard(float x, float y, float dist, float speed)
        {
            GameObject saw = new GameObject("FrostSawHazard");
            saw.transform.SetParent(levelRoot.transform, false);
            saw.transform.position = new Vector3(x, y, 0f);
            saw.transform.localScale = new Vector3(0.8f, 0.8f, 1f);

            SpriteRenderer rend = saw.AddComponent<SpriteRenderer>();
            rend.sprite = frostSawSprite;
            rend.sortingOrder = 5;

            CircleCollider2D col = saw.AddComponent<CircleCollider2D>();
            col.isTrigger = true;
            col.radius = 0.4f;

            MovingFrostHazard mfh = saw.AddComponent<MovingFrostHazard>();
            mfh.moveDistance = dist;
            mfh.moveSpeed = speed;
        }

        private void CreateFallingIcicle(float x, float y)
        {
            GameObject icicle = new GameObject("FallingIcicle");
            icicle.transform.SetParent(levelRoot.transform, false);
            icicle.transform.position = new Vector3(x, y, 0f);
            icicle.transform.localScale = new Vector3(0.8f, 0.8f, 1f);

            SpriteRenderer rend = icicle.AddComponent<SpriteRenderer>();
            rend.sprite = icicleSprite;
            rend.sortingOrder = 4;

            PolygonCollider2D col = icicle.AddComponent<PolygonCollider2D>();
            col.isTrigger = true;

            Rigidbody2D rb = icicle.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;

            icicle.AddComponent<FallingIcicle>();
        }

        private void CreateIceFloe(float x, float y)
        {
            GameObject floe = new GameObject("IceFloe");
            floe.transform.SetParent(levelRoot.transform, false);
            floe.transform.position = new Vector3(x, y - 0.22f, 0f);

            SpriteRenderer rend = floe.AddComponent<SpriteRenderer>();
            rend.sprite = iceFloeSprite;
            rend.sortingOrder = 3;

            BoxCollider2D col = floe.AddComponent<BoxCollider2D>();
            col.size = new Vector2(1.28f, 0.45f);
        }

        private void CreateWindDraft(float x, float y, float liftSpeed = 12f, float forwardPush = 0f)
        {
            GameObject wind = new GameObject("WindDraft");
            wind.transform.SetParent(levelRoot.transform, false);
            wind.transform.position = new Vector3(x, y + 1.28f, 0f);
            wind.transform.localScale = new Vector3(1f, 2f, 1f);

            SpriteRenderer rend = wind.AddComponent<SpriteRenderer>();
            rend.sprite = windDraftSprite;
            rend.sortingOrder = 6;
            rend.color = new Color(1f, 1f, 1f, 0.75f);

            BoxCollider2D col = wind.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            col.size = new Vector2(0.64f, 1.28f);

            WindDraftZone wdz = wind.AddComponent<WindDraftZone>();
            wdz.upwardLiftSpeed = liftSpeed;
            wdz.forwardPushSpeed = forwardPush;
        }

        private void CreateBlinkingPlatform(float x, float y, float initialOffset = 0f)
        {
            GameObject plat = new GameObject("BlinkingIcePlatform");
            plat.transform.SetParent(levelRoot.transform, false);
            float height = 0.36f;
            plat.transform.position = new Vector3(x, y - height * 0.5f, 0f);

            SpriteRenderer rend = plat.AddComponent<SpriteRenderer>();
            rend.sprite = blinkingIceSprite;
            rend.sortingOrder = 3;

            BoxCollider2D col = plat.AddComponent<BoxCollider2D>();
            col.size = new Vector2(1.28f, height);

            BlinkingIcePlatform bip = plat.AddComponent<BlinkingIcePlatform>();
            bip.initialOffset = initialOffset;
        }

        private void CreateSpeedBoost(float x, float y, float boostSpeed = 15f)
        {
            GameObject ramp = new GameObject("SpeedBoostRamp");
            ramp.transform.SetParent(levelRoot.transform, false);
            ramp.transform.position = new Vector3(x, y + 0.16f, 0f);

            SpriteRenderer rend = ramp.AddComponent<SpriteRenderer>();
            rend.sprite = speedBoostSprite;
            rend.sortingOrder = 3;

            BoxCollider2D col = ramp.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            col.size = new Vector2(1.28f, 0.32f);

            SpeedBoostRamp sbr = ramp.AddComponent<SpeedBoostRamp>();
            sbr.boostSpeed = boostSpeed;
        }

        private void CreateRotatingFrostBar(float x, float y, float speed = 65f, bool clockwise = true)
        {
            GameObject bar = new GameObject("RotatingFrostBar");
            bar.transform.SetParent(levelRoot.transform, false);
            bar.transform.position = new Vector3(x, y, 0f);

            SpriteRenderer rend = bar.AddComponent<SpriteRenderer>();
            rend.sprite = frostBarSprite;
            rend.sortingOrder = 4;

            BoxCollider2D col = bar.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            col.size = new Vector2(1.6f, 0.32f);

            RotatingFrostBar rfb = bar.AddComponent<RotatingFrostBar>();
            rfb.rotationSpeed = speed;
            rfb.clockwise = clockwise;
        }

        private void CreateFrostTrampoline(float x, float y, float force = 17.5f)
        {
            GameObject tr = new GameObject("FrostTrampoline");
            tr.transform.SetParent(levelRoot.transform, false);
            tr.transform.position = new Vector3(x, y + 0.32f, 0f);

            SpriteRenderer rend = tr.AddComponent<SpriteRenderer>();
            rend.sprite = trampolineSprite ?? springSprite;
            rend.sortingOrder = 4;

            BoxCollider2D col = tr.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            col.size = new Vector2(1.28f, 0.64f);

            FrostTrampoline ft = tr.AddComponent<FrostTrampoline>();
            ft.bounceForce = force;
            ft.trampolineVisual = tr.transform;
        }

        private void CreateMovingIcePlatform(float x, float y, Vector2 moveDelta, float speed = 2.2f, float waitTime = 0.35f)
        {
            GameObject plat = new GameObject("MovingIcePlatform");
            plat.transform.SetParent(levelRoot.transform, false);
            plat.transform.position = new Vector3(x, y, 0f);

            SpriteRenderer rend = plat.AddComponent<SpriteRenderer>();
            rend.sprite = movingPlatformSprite ?? woodPlatformSprite;
            rend.sortingOrder = 3;

            BoxCollider2D col = plat.AddComponent<BoxCollider2D>();
            col.size = new Vector2(1.28f, 0.32f);

            MovingIcePlatform mip = plat.AddComponent<MovingIcePlatform>();
            mip.moveDelta = moveDelta;
            mip.speed = speed;
            mip.waitTime = waitTime;
        }

        private void CreateSwingingMace(float anchorX, float anchorY, float length = 3.5f, float maxAngle = 60f, float speed = 2.4f, float phaseOffset = 0f)
        {
            GameObject anchor = new GameObject("SwingingMaceAnchor");
            anchor.transform.SetParent(levelRoot.transform, false);
            anchor.transform.position = new Vector3(anchorX, anchorY, 0f);

            SwingingFrostMace mace = anchor.AddComponent<SwingingFrostMace>();
            mace.maxAngle = maxAngle;
            mace.swingSpeed = speed;
            mace.phaseOffset = phaseOffset;

            int links = Mathf.Max(1, Mathf.RoundToInt(length / 0.32f));
            for (int i = 0; i < links; i++)
            {
                GameObject link = new GameObject($"ChainLink_{i}");
                link.transform.SetParent(anchor.transform, false);
                link.transform.localPosition = new Vector3(0f, -i * 0.32f, 0f);
                SpriteRenderer linkRend = link.AddComponent<SpriteRenderer>();
                linkRend.sprite = frostChainSprite;
                linkRend.sortingOrder = 4;
            }

            GameObject head = new GameObject("MaceHead");
            head.transform.SetParent(anchor.transform, false);
            head.transform.localPosition = new Vector3(0f, -length, 0f);

            SpriteRenderer headRend = head.AddComponent<SpriteRenderer>();
            headRend.sprite = swingingMaceSprite;
            headRend.sortingOrder = 5;

            CircleCollider2D col = head.AddComponent<CircleCollider2D>();
            col.isTrigger = true;
            col.radius = 0.45f;

            head.AddComponent<HazardSpike>();
        }

        private void CreatePortalPair(Vector2 entrancePos, Vector2 exitPos, Vector2 exitVel)
        {
            // Entrance (Blue)
            GameObject pIn = new GameObject("Portal_Entrance");
            pIn.transform.SetParent(levelRoot.transform, false);
            pIn.transform.position = new Vector3(entrancePos.x, entrancePos.y + 0.64f, 0f);

            SpriteRenderer rIn = pIn.AddComponent<SpriteRenderer>();
            rIn.sprite = portalBlueSprite;
            rIn.sortingOrder = 4;

            BoxCollider2D cIn = pIn.AddComponent<BoxCollider2D>();
            cIn.isTrigger = true;
            cIn.size = new Vector2(0.8f, 1.28f);

            FrostPortal fpIn = pIn.AddComponent<FrostPortal>();
            fpIn.exitVelocity = exitVel;

            // Exit (Orange)
            GameObject pOut = new GameObject("Portal_Exit");
            pOut.transform.SetParent(levelRoot.transform, false);
            pOut.transform.position = new Vector3(exitPos.x, exitPos.y + 0.64f, 0f);

            SpriteRenderer rOut = pOut.AddComponent<SpriteRenderer>();
            rOut.sprite = portalOrangeSprite;
            rOut.sortingOrder = 4;

            BoxCollider2D cOut = pOut.AddComponent<BoxCollider2D>();
            cOut.isTrigger = true;
            cOut.size = new Vector2(0.8f, 1.28f);

            FrostPortal fpOut = pOut.AddComponent<FrostPortal>();

            fpIn.targetPortal = fpOut;
        }

        private void CreateGravityOrb(float x, float y, float upward = 13.5f, float forward = 4.5f)
        {
            GameObject orb = new GameObject("BounceGravityOrb");
            orb.transform.SetParent(levelRoot.transform, false);
            orb.transform.position = new Vector3(x, y, 0f);

            SpriteRenderer rend = orb.AddComponent<SpriteRenderer>();
            rend.sprite = gravityOrbSprite;
            rend.sortingOrder = 5;

            CircleCollider2D col = orb.AddComponent<CircleCollider2D>();
            col.isTrigger = true;
            col.radius = 0.32f;

            BounceGravityOrb bgo = orb.AddComponent<BounceGravityOrb>();
            bgo.upwardBounce = upward;
            bgo.forwardBoost = forward;
        }

        private void CreateFrostLaser(float x, float y, float height = 4.5f, float delay = 0f, float inactive = 2f, float active = 1.4f)
        {
            GameObject laserRoot = new GameObject("FrostLaserRoot");
            laserRoot.transform.SetParent(levelRoot.transform, false);
            laserRoot.transform.position = new Vector3(x, y, 0f);

            // Top Emitter
            GameObject topEmitter = new GameObject("Emitter_Top");
            topEmitter.transform.SetParent(laserRoot.transform, false);
            topEmitter.transform.localPosition = new Vector3(0f, height * 0.5f, 0f);
            SpriteRenderer topRend = topEmitter.AddComponent<SpriteRenderer>();
            topRend.sprite = laserEmitterSprite;
            topRend.sortingOrder = 6;

            // Bottom Emitter
            GameObject botEmitter = new GameObject("Emitter_Bottom");
            botEmitter.transform.SetParent(laserRoot.transform, false);
            botEmitter.transform.localPosition = new Vector3(0f, -height * 0.5f, 0f);
            SpriteRenderer botRend = botEmitter.AddComponent<SpriteRenderer>();
            botRend.sprite = laserEmitterSprite;
            botRend.sortingOrder = 6;

            // Beam
            GameObject beamObj = new GameObject("LaserBeam");
            beamObj.transform.SetParent(laserRoot.transform, false);
            beamObj.transform.localPosition = Vector3.zero;

            SpriteRenderer beamRend = beamObj.AddComponent<SpriteRenderer>();
            beamRend.sprite = laserBeamSprite;
            beamRend.sortingOrder = 5;
            beamRend.drawMode = SpriteDrawMode.Tiled;
            beamRend.size = new Vector2(0.32f, height);

            BoxCollider2D beamCol = beamObj.AddComponent<BoxCollider2D>();
            beamCol.isTrigger = true;
            beamCol.size = new Vector2(0.32f, height);

            FrostLaserBeam flb = laserRoot.AddComponent<FrostLaserBeam>();
            flb.beamRenderer = beamRend;
            flb.beamCollider = beamCol;
            flb.initialDelay = delay;
            flb.inactiveDuration = inactive;
            flb.activeDuration = active;
        }

        private void CreateCrushingPillar(float x, float ceilingY, float dropDist = 4.5f)
        {
            GameObject pillar = new GameObject("CrushingIcePillar");
            pillar.transform.SetParent(levelRoot.transform, false);
            pillar.transform.position = new Vector3(x, ceilingY, 0f);

            SpriteRenderer rend = pillar.AddComponent<SpriteRenderer>();
            rend.sprite = crushingPillarSprite;
            rend.sortingOrder = 5;

            BoxCollider2D col = pillar.AddComponent<BoxCollider2D>();
            col.size = new Vector2(0.8f, 1.2f);

            CrushingIcePillar cip = pillar.AddComponent<CrushingIcePillar>();
            cip.dropDistance = dropDist;
        }

        private void CreateConveyorBelt(float startX, float endX, float y, float speed = 6f)
        {
            float width = endX - startX;
            float midX = startX + width * 0.5f;

            GameObject conv = new GameObject($"Conveyor_{startX}_{endX}");
            conv.transform.SetParent(levelRoot.transform, false);
            float height = 0.32f;
            conv.transform.position = new Vector3(midX, y - height * 0.5f, 0f);

            SpriteRenderer rend = conv.AddComponent<SpriteRenderer>();
            rend.sprite = conveyorSprite;
            rend.drawMode = SpriteDrawMode.Tiled;
            rend.size = new Vector2(width, height);
            rend.sortingOrder = 3;

            BoxCollider2D col = conv.AddComponent<BoxCollider2D>();
            col.size = new Vector2(width, height);

            FrostConveyor fc = conv.AddComponent<FrostConveyor>();
            fc.conveyorSpeed = speed;
        }

        private void CreateGravityFlipPad(float x, float y, float lift = 13.5f, float forward = 3.5f)
        {
            GameObject pad = new GameObject("GravityFlipPad");
            pad.transform.SetParent(levelRoot.transform, false);
            pad.transform.position = new Vector3(x, y + 0.16f, 0f);

            SpriteRenderer rend = pad.AddComponent<SpriteRenderer>();
            rend.sprite = gravityFlipPadSprite;
            rend.sortingOrder = 3;

            BoxCollider2D col = pad.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            col.size = new Vector2(0.96f, 0.32f);

            GravityFlipPad gfp = pad.AddComponent<GravityFlipPad>();
            gfp.antigravityLift = lift;
            gfp.pushForward = forward;
        }

        private void CreateCheckpoint(float x, float y)
        {
            GameObject cp = new GameObject("Checkpoint");
            cp.transform.SetParent(levelRoot.transform, false);
            cp.transform.position = new Vector3(x, y + 0.5f, 0f);

            SpriteRenderer rend = cp.AddComponent<SpriteRenderer>();
            rend.sprite = flagIdleSprite;
            rend.sortingOrder = 3;

            BoxCollider2D col = cp.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            col.size = new Vector2(0.64f, 1f);

            Checkpoint cpComp = cp.AddComponent<Checkpoint>();
            cpComp.idleSprite = flagIdleSprite;
            cpComp.activeSprite = flagActiveSprite;
            cpComp.flagRenderer = rend;
            cpComp.flagActiveSprite = flagActiveSprite;
        }

        private void CreateGoalArch(float x, float y)
        {
            GameObject goal = new GameObject("GoalArch");
            goal.transform.SetParent(levelRoot.transform, false);
            goal.transform.position = new Vector3(x, y + 0.6f, 0f);

            SpriteRenderer rend = goal.AddComponent<SpriteRenderer>();
            rend.sprite = goalArchSprite;
            rend.sortingOrder = 3;

            BoxCollider2D col = goal.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            col.size = new Vector2(1.2f, 1.2f);

            goal.AddComponent<GoalArch>();
        }

        private void CreatePlayer(Vector3 startPos)
        {
            GameObject ballObj = GameObject.FindWithTag("Player");
            if (ballObj == null)
            {
                ballObj = new GameObject("Snowball");
                ballObj.tag = "Player";
            }

            ballObj.transform.position = startPos;
            ballObj.transform.localScale = Vector3.one;

            // Ensure Squash child exists (handles world-aligned squash/stretch)
            Transform squash = ballObj.transform.Find("Squash");
            if (squash == null)
            {
                GameObject sqObj = new GameObject("Squash");
                sqObj.transform.SetParent(ballObj.transform, false);
                sqObj.transform.localPosition = Vector3.zero;
                sqObj.transform.localRotation = Quaternion.identity;
                sqObj.transform.localScale = Vector3.one;
                squash = sqObj.transform;
            }

            // Ensure Visual child exists under Squash (handles 360° rolling rotation)
            Transform visual = squash.Find("Visual") ?? ballObj.transform.Find("Visual");
            if (visual == null)
            {
                GameObject vObj = new GameObject("Visual");
                vObj.transform.SetParent(squash, false);
                vObj.transform.localPosition = Vector3.zero;
                vObj.transform.localRotation = Quaternion.identity;
                vObj.transform.localScale = Vector3.one;
                visual = vObj.transform;
            }
            else if (visual.parent != squash)
            {
                visual.SetParent(squash, false);
            }

            SpriteRenderer sr = visual.GetComponent<SpriteRenderer>();
            if (sr == null)
            {
                sr = visual.gameObject.AddComponent<SpriteRenderer>();
            }
            sr.sortingOrder = 10;

            SnowballController sc = ballObj.GetComponent<SnowballController>();
            if (sc == null) sc = ballObj.AddComponent<SnowballController>();
            sc.squashTransform = squash;
            sc.visualTransform = visual;
            sc.visualRenderer = sr;
            sc.respawnPosition = startPos;
            sc.ApplyCharacterSkin();

            // Link camera
            Camera cam = Camera.main;
            if (cam != null)
            {
                SmoothFollowCamera sfc = cam.GetComponent<SmoothFollowCamera>();
                if (sfc == null) sfc = cam.gameObject.AddComponent<SmoothFollowCamera>();
                sfc.target = ballObj.transform;
            }
        }

        private void CreateChasmBoundary(float startX, float endX, float y)
        {
            CreateIcyChasm(startX, endX, y);
        }

        private void CreateIcyChasm(float startX, float endX, float y)
        {
            GameObject chasm = new GameObject("IcyChasm");
            chasm.transform.SetParent(levelRoot.transform, false);
            chasm.transform.position = new Vector3((startX + endX) * 0.5f, y, 0f);

            BoxCollider2D col = chasm.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            col.size = new Vector2(endX - startX, 2f);

            chasm.AddComponent<IcyChasmTrigger>();
        }

        public static string GetLevelTitle(int lvl)
        {
            switch (lvl)
            {
                case 1: return "Frosty Foothills";
                case 2: return "Pine Needle Peaks";
                case 3: return "Slippery Ice Caverns";
                case 4: return "Blizzard Chasm";
                case 5: return "Clockwork Frostworks";
                case 6: return "Crystal Glade";
                case 7: return "Glacier Crags";
                case 8: return "Northern Lights Ridge";
                case 9: return "The Frost Spire";
                case 10: return "Avalanche Citadel";
                case 11: return "Aurora Highlands";
                case 12: return "Crystal Chasm";
                case 13: return "Pendulum Hollow";
                case 14: return "Warp Caverns";
                case 15: return "Neon Glacier";
                case 16: return "The Celestial Lift";
                case 17: return "Frostfire Gauntlet";
                case 18: return "Portal Maze Ridge";
                case 19: return "Blizzard Abyss";
                case 20: return "Crown of the Glacier";
                case 21: return "Starlight Steppes";
                case 22: return "Laser Caverns";
                case 23: return "Crushing Ice Citadel";
                case 24: return "Cosmic Conveyor Way";
                case 25: return "Antigravity Vaults";
                case 26: return "The Starlit Abyss";
                case 27: return "Pillar of Frostfire";
                case 28: return "Orbital Relay";
                case 29: return "Supernova Ridge";
                case 30: return "The Cosmic Crown";
                default: return $"Level {lvl}";
            }
        }

        // =========================================================================
        // HUD CREATION
        // =========================================================================
        private void BuildHUD()
        {
            GameObject hudCanvas = new GameObject("HUD_Canvas");
            Canvas canvas = hudCanvas.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = hudCanvas.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280, 720);
            scaler.matchWidthOrHeight = 0.5f;
            hudCanvas.AddComponent<GraphicRaycaster>();

            Font defaultFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf") ?? Resources.GetBuiltinResource<Font>("Arial.ttf");

            // Top Bar Container
            GameObject topBar = new GameObject("TopBar");
            topBar.transform.SetParent(hudCanvas.transform, false);
            RectTransform topBarRt = topBar.AddComponent<RectTransform>();
            topBarRt.anchorMin = new Vector2(0, 1);
            topBarRt.anchorMax = new Vector2(1, 1);
            topBarRt.pivot = new Vector2(0.5f, 1);
            topBarRt.anchoredPosition = Vector2.zero;
            topBarRt.sizeDelta = new Vector2(0, 70);

            // Hearts Container (Top Left Corner - 3 Lives)
            Sprite heartFullSpr = Resources.Load<Sprite>("Sprites/ui_heart_full");
            Sprite heartEmptySpr = Resources.Load<Sprite>("Sprites/ui_heart_empty");

            GameObject heartsContainer = new GameObject("HeartsContainer");
            heartsContainer.transform.SetParent(topBar.transform, false);
            RectTransform hcRt = heartsContainer.AddComponent<RectTransform>();
            hcRt.anchorMin = new Vector2(0, 0.5f);
            hcRt.anchorMax = new Vector2(0, 0.5f);
            hcRt.pivot = new Vector2(0, 0.5f);
            hcRt.anchoredPosition = new Vector2(25, 0);
            hcRt.sizeDelta = new Vector2(130, 45);

            Image[] hearts = new Image[3];
            for (int i = 0; i < 3; i++)
            {
                GameObject hObj = new GameObject($"Heart_{i}");
                hObj.transform.SetParent(heartsContainer.transform, false);
                RectTransform hRt = hObj.AddComponent<RectTransform>();
                hRt.anchorMin = new Vector2(0, 0.5f);
                hRt.anchorMax = new Vector2(0, 0.5f);
                hRt.pivot = new Vector2(0, 0.5f);
                hRt.anchoredPosition = new Vector2(i * 38f, 0);
                hRt.sizeDelta = new Vector2(34, 34);
                Image hImg = hObj.AddComponent<Image>();
                hImg.sprite = heartFullSpr;
                hearts[i] = hImg;
            }

            // Level Text (Top Left, to the right of the hearts)
            GameObject lvlObj = new GameObject("LevelText");
            lvlObj.transform.SetParent(topBar.transform, false);
            RectTransform lvlRt = lvlObj.AddComponent<RectTransform>();
            lvlRt.anchorMin = new Vector2(0, 0.5f);
            lvlRt.anchorMax = new Vector2(0, 0.5f);
            lvlRt.pivot = new Vector2(0, 0.5f);
            lvlRt.anchoredPosition = new Vector2(160, 0);
            lvlRt.sizeDelta = new Vector2(360, 45);

            Text lvlTxt = lvlObj.AddComponent<Text>();
            lvlTxt.font = defaultFont;
            lvlTxt.fontSize = 20;
            lvlTxt.fontStyle = FontStyle.Bold;
            lvlTxt.color = new Color(0.9f, 0.96f, 1f, 1f);
            lvlTxt.alignment = TextAnchor.MiddleLeft;
            lvlTxt.text = $"LVL {GameDataManager.Data.selectedLevel}: {GetLevelTitle(GameDataManager.Data.selectedLevel)}";

            // Coin Display (Top Center-Right)
            GameObject coinObj = new GameObject("CoinCounter");
            coinObj.transform.SetParent(topBar.transform, false);
            RectTransform coinRt = coinObj.AddComponent<RectTransform>();
            coinRt.anchorMin = new Vector2(0.5f, 0.5f);
            coinRt.anchorMax = new Vector2(0.5f, 0.5f);
            coinRt.pivot = new Vector2(0.5f, 0.5f);
            coinRt.anchoredPosition = new Vector2(0, 0);
            coinRt.sizeDelta = new Vector2(200, 45);

            GameObject coinIcon = new GameObject("CoinIcon");
            coinIcon.transform.SetParent(coinObj.transform, false);
            RectTransform coinIconRt = coinIcon.AddComponent<RectTransform>();
            coinIconRt.anchorMin = new Vector2(0, 0.5f);
            coinIconRt.anchorMax = new Vector2(0, 0.5f);
            coinIconRt.pivot = new Vector2(0, 0.5f);
            coinIconRt.anchoredPosition = new Vector2(0, 0);
            coinIconRt.sizeDelta = new Vector2(36, 36);
            Image coinImg = coinIcon.AddComponent<Image>();
            coinImg.sprite = coinSprite;

            GameObject coinTxtObj = new GameObject("CoinText");
            coinTxtObj.transform.SetParent(coinObj.transform, false);
            RectTransform coinTxtRt = coinTxtObj.AddComponent<RectTransform>();
            coinTxtRt.anchorMin = new Vector2(0, 0);
            coinTxtRt.anchorMax = new Vector2(1, 1);
            coinTxtRt.offsetMin = new Vector2(45, 0);
            coinTxtRt.offsetMax = Vector2.zero;
            Text coinTxt = coinTxtObj.AddComponent<Text>();
            coinTxt.font = defaultFont;
            coinTxt.fontSize = 26;
            coinTxt.fontStyle = FontStyle.Bold;
            coinTxt.color = new Color(1f, 0.88f, 0.2f, 1f);
            coinTxt.alignment = TextAnchor.MiddleLeft;
            coinTxt.text = $"0 / 30";

            // Add GameSceneManager component
            GameSceneManager gsm = hudCanvas.AddComponent<GameSceneManager>();
            gsm.coinCounterText = coinTxt;
            gsm.levelTitleText = lvlTxt;
            gsm.heartFullSprite = heartFullSpr;
            gsm.heartEmptySprite = heartEmptySpr;
            gsm.heartImages = hearts;

            // Pause Button (Top Right of TopBar)
            GameObject pauseBtn = CreateUIButton(topBar.transform, "PauseBtn", "PAUSE ||", defaultFont, Resources.Load<Sprite>("Sprites/ui_btn_blue"), new Vector2(-25, 0), new Vector2(110, 42), 16, () => gsm.TogglePause(), new Vector2(1, 0.5f), new Vector2(1, 0.5f));

            // Pause Panel
            GameObject pausePanel = BuildPausePanel(hudCanvas.transform, defaultFont, gsm);
            gsm.pausePanel = pausePanel;

            // Level Complete Panel
            GameObject completePanel = BuildLevelCompletePanel(hudCanvas.transform, defaultFont, gsm, out Text compCoinsText, out GameObject[] stars);
            gsm.levelCompletePanel = completePanel;
            gsm.completeCoinsText = compCoinsText;
            gsm.starImages = stars;

            // Ad Revive Panel
            GameObject adRevivePanel = BuildAdRevivePanel(hudCanvas.transform, defaultFont, gsm);
            gsm.adRevivePanel = adRevivePanel;

            // Game Over Panel
            GameObject gameOverPanel = BuildGameOverPanel(hudCanvas.transform, defaultFont, gsm, out Text gameOverCoinsText);
            gsm.gameOverPanel = gameOverPanel;
            gsm.gameOverCoinsText = gameOverCoinsText;

            BuildMobileControls(hudCanvas.transform, defaultFont);
        }

        private GameObject BuildPausePanel(Transform canvas, Font font, GameSceneManager gsm)
        {
            GameObject panel = new GameObject("PausePanel", typeof(RectTransform));
            panel.transform.SetParent(canvas, false);
            RectTransform rt = panel.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            Image darkBg = panel.AddComponent<Image>();
            darkBg.color = new Color(0, 0.08f, 0.2f, 0.88f);

            GameObject dialog = new GameObject("Dialog", typeof(RectTransform));
            dialog.transform.SetParent(panel.transform, false);
            RectTransform dRt = dialog.GetComponent<RectTransform>();
            dRt.sizeDelta = new Vector2(480, 360);
            Image dImg = dialog.AddComponent<Image>();
            dImg.sprite = Resources.Load<Sprite>("Sprites/ui_dialog_panel");
            dImg.color = Color.white;

            // Title in header ribbon
            GameObject title = new GameObject("Title", typeof(RectTransform));
            title.transform.SetParent(dialog.transform, false);
            RectTransform tRt = title.GetComponent<RectTransform>();
            tRt.anchoredPosition = new Vector2(0, 115);
            tRt.sizeDelta = new Vector2(400, 40);
            Text tTxt = title.AddComponent<Text>();
            tTxt.text = "PAUSED";
            tTxt.font = font;
            tTxt.fontSize = 28;
            tTxt.fontStyle = FontStyle.Bold;
            tTxt.alignment = TextAnchor.MiddleCenter;
            tTxt.color = Color.white;

            Sprite btnGreen = Resources.Load<Sprite>("Sprites/ui_btn_green");
            Sprite btnBlue = Resources.Load<Sprite>("Sprites/ui_btn_blue");

            CreateUIButton(dialog.transform, "ResumeBtn", "RESUME ▶", font, btnGreen, new Vector2(0, 40), new Vector2(240, 50), 20, () => gsm.OnResumeClicked());
            CreateUIButton(dialog.transform, "RestartBtn", "RESTART ↻", font, btnBlue, new Vector2(0, -25), new Vector2(240, 48), 18, () => gsm.OnRestartClicked());
            CreateUIButton(dialog.transform, "MenuBtn", "MAIN MENU ⌂", font, btnBlue, new Vector2(0, -85), new Vector2(240, 48), 18, () => gsm.OnMainMenuClicked());

            panel.SetActive(false);
            return panel;
        }

        private GameObject BuildLevelCompletePanel(Transform canvas, Font font, GameSceneManager gsm, out Text coinsText, out GameObject[] starObjs)
        {
            GameObject panel = new GameObject("LevelCompletePanel", typeof(RectTransform));
            panel.transform.SetParent(canvas, false);
            RectTransform rt = panel.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            Image darkBg = panel.AddComponent<Image>();
            darkBg.color = new Color(0, 0.08f, 0.2f, 0.92f);

            GameObject dialog = new GameObject("Dialog", typeof(RectTransform));
            dialog.transform.SetParent(panel.transform, false);
            RectTransform dRt = dialog.GetComponent<RectTransform>();
            dRt.sizeDelta = new Vector2(580, 480);
            Image dImg = dialog.AddComponent<Image>();
            dImg.sprite = Resources.Load<Sprite>("Sprites/ui_dialog_panel");
            dImg.color = Color.white;

            // Title in header ribbon
            GameObject title = new GameObject("Title", typeof(RectTransform));
            title.transform.SetParent(dialog.transform, false);
            RectTransform tRt = title.GetComponent<RectTransform>();
            tRt.anchoredPosition = new Vector2(0, 175);
            tRt.sizeDelta = new Vector2(500, 45);
            Text tTxt = title.AddComponent<Text>();
            tTxt.text = "LEVEL COMPLETE!";
            tTxt.font = font;
            tTxt.fontSize = 32;
            tTxt.fontStyle = FontStyle.Bold;
            tTxt.alignment = TextAnchor.MiddleCenter;
            tTxt.color = new Color(1f, 0.9f, 0.2f, 1f);

            // Stars over frosted white panel
            Sprite starSpr = Resources.Load<Sprite>("Sprites/ui_star");
            starObjs = new GameObject[3];
            float[] starX = new float[] { -80f, 0f, 80f };
            for (int i = 0; i < 3; i++)
            {
                GameObject star = new GameObject($"Star_{i}", typeof(RectTransform));
                star.transform.SetParent(dialog.transform, false);
                RectTransform sRt = star.GetComponent<RectTransform>();
                sRt.anchoredPosition = new Vector2(starX[i], 95);
                sRt.sizeDelta = new Vector2(64, 64);
                Image sImg = star.AddComponent<Image>();
                sImg.sprite = starSpr;
                starObjs[i] = star;
            }

            // Coins text in dark blue over frosted white panel
            GameObject cTxtObj = new GameObject("CoinsText", typeof(RectTransform));
            cTxtObj.transform.SetParent(dialog.transform, false);
            RectTransform cRt = cTxtObj.GetComponent<RectTransform>();
            cRt.anchoredPosition = new Vector2(0, 30);
            cRt.sizeDelta = new Vector2(500, 35);
            coinsText = cTxtObj.AddComponent<Text>();
            coinsText.text = "Coins Collected: 0 / 30";
            coinsText.font = font;
            coinsText.fontSize = 22;
            coinsText.fontStyle = FontStyle.Bold;
            coinsText.alignment = TextAnchor.MiddleCenter;
            coinsText.color = new Color(0.08f, 0.18f, 0.38f, 1f);

            Sprite btnGreen = Resources.Load<Sprite>("Sprites/ui_btn_green");
            Sprite btnBlue = Resources.Load<Sprite>("Sprites/ui_btn_blue");

            CreateUIButton(dialog.transform, "NextBtn", "NEXT LEVEL ▶", font, btnGreen, new Vector2(0, -45), new Vector2(260, 52), 22, () => gsm.OnNextLevelClicked());
            CreateUIButton(dialog.transform, "RestartBtn", "RESTART ↻", font, btnBlue, new Vector2(-120, -115), new Vector2(200, 48), 18, () => gsm.OnRestartClicked());
            CreateUIButton(dialog.transform, "MenuBtn", "MAIN MENU ⌂", font, btnBlue, new Vector2(120, -115), new Vector2(200, 48), 18, () => gsm.OnMainMenuClicked());

            panel.SetActive(false);
            return panel;
        }

        private GameObject BuildAdRevivePanel(Transform canvas, Font font, GameSceneManager gsm)
        {
            GameObject panel = new GameObject("AdRevivePanel", typeof(RectTransform));
            panel.transform.SetParent(canvas, false);
            RectTransform rt = panel.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            Image darkBg = panel.AddComponent<Image>();
            darkBg.color = new Color(0, 0.08f, 0.2f, 0.92f);

            GameObject dialog = new GameObject("Dialog", typeof(RectTransform));
            dialog.transform.SetParent(panel.transform, false);
            RectTransform dRt = dialog.GetComponent<RectTransform>();
            dRt.sizeDelta = new Vector2(520, 430);
            Image dImg = dialog.AddComponent<Image>();
            dImg.sprite = Resources.Load<Sprite>("Sprites/ui_dialog_panel");
            dImg.color = Color.white;

            // Title in header ribbon
            GameObject title = new GameObject("Title", typeof(RectTransform));
            title.transform.SetParent(dialog.transform, false);
            RectTransform tRt = title.GetComponent<RectTransform>();
            tRt.anchoredPosition = new Vector2(0, 150);
            tRt.sizeDelta = new Vector2(460, 40);
            Text tTxt = title.AddComponent<Text>();
            tTxt.text = "OUT OF LIVES!";
            tTxt.font = font;
            tTxt.fontSize = 30;
            tTxt.fontStyle = FontStyle.Bold;
            tTxt.alignment = TextAnchor.MiddleCenter;
            tTxt.color = Color.white;

            // Description in dark blue over frosted white panel
            GameObject desc = new GameObject("Description", typeof(RectTransform));
            desc.transform.SetParent(dialog.transform, false);
            RectTransform descRt = desc.GetComponent<RectTransform>();
            descRt.anchoredPosition = new Vector2(0, 85);
            descRt.sizeDelta = new Vector2(460, 60);
            Text dTxt = desc.AddComponent<Text>();
            dTxt.text = "Watch a video ad to revive\nand continue from checkpoint!";
            dTxt.font = font;
            dTxt.fontSize = 20;
            dTxt.fontStyle = FontStyle.Bold;
            dTxt.alignment = TextAnchor.MiddleCenter;
            dTxt.color = new Color(0.08f, 0.18f, 0.38f, 1f);

            // Heart Icon graphic with +1 badge
            GameObject iconObj = new GameObject("ReviveHeart", typeof(RectTransform));
            iconObj.transform.SetParent(dialog.transform, false);
            RectTransform iRt = iconObj.GetComponent<RectTransform>();
            iRt.anchoredPosition = new Vector2(0, 5);
            iRt.sizeDelta = new Vector2(64, 64);
            Image iImg = iconObj.AddComponent<Image>();
            iImg.sprite = Resources.Load<Sprite>("Sprites/ui_heart_full");

            Sprite btnGreen = Resources.Load<Sprite>("Sprites/ui_btn_green");
            Sprite btnBlue = Resources.Load<Sprite>("Sprites/ui_btn_blue");

            // Button: Watch Ad To Revive (Bypasses video since not implemented yet)
            CreateUIButton(dialog.transform, "WatchAdBtn", "WATCH AD TO REVIVE 🎬", font, btnGreen, new Vector2(0, -75), new Vector2(300, 52), 18, () => gsm.OnWatchAdReviveClicked());

            // Button: Give Up / No Thanks
            CreateUIButton(dialog.transform, "GiveUpBtn", "NO THANKS", font, btnBlue, new Vector2(0, -145), new Vector2(200, 42), 16, () => gsm.OnGiveUpAdReviveClicked());

            panel.SetActive(false);
            return panel;
        }

        private GameObject BuildGameOverPanel(Transform canvas, Font font, GameSceneManager gsm, out Text coinsText)
        {
            GameObject panel = new GameObject("GameOverPanel", typeof(RectTransform));
            panel.transform.SetParent(canvas, false);
            RectTransform rt = panel.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            Image darkBg = panel.AddComponent<Image>();
            darkBg.color = new Color(0, 0.08f, 0.2f, 0.94f);

            GameObject dialog = new GameObject("Dialog", typeof(RectTransform));
            dialog.transform.SetParent(panel.transform, false);
            RectTransform dRt = dialog.GetComponent<RectTransform>();
            dRt.sizeDelta = new Vector2(500, 390);
            Image dImg = dialog.AddComponent<Image>();
            dImg.sprite = Resources.Load<Sprite>("Sprites/ui_dialog_panel");
            dImg.color = Color.white;

            // Title in header ribbon
            GameObject title = new GameObject("Title", typeof(RectTransform));
            title.transform.SetParent(dialog.transform, false);
            RectTransform tRt = title.GetComponent<RectTransform>();
            tRt.anchoredPosition = new Vector2(0, 130);
            tRt.sizeDelta = new Vector2(420, 45);
            Text tTxt = title.AddComponent<Text>();
            tTxt.text = "GAME OVER";
            tTxt.font = font;
            tTxt.fontSize = 32;
            tTxt.fontStyle = FontStyle.Bold;
            tTxt.alignment = TextAnchor.MiddleCenter;
            tTxt.color = Color.white;

            // Subtitle in dark blue over frosted white panel
            GameObject sub = new GameObject("Subtitle", typeof(RectTransform));
            sub.transform.SetParent(dialog.transform, false);
            RectTransform sRt = sub.GetComponent<RectTransform>();
            sRt.anchoredPosition = new Vector2(0, 75);
            sRt.sizeDelta = new Vector2(400, 30);
            Text sTxt = sub.AddComponent<Text>();
            sTxt.text = "The snowball melted away!";
            sTxt.font = font;
            sTxt.fontSize = 19;
            sTxt.fontStyle = FontStyle.Bold;
            sTxt.alignment = TextAnchor.MiddleCenter;
            sTxt.color = new Color(0.12f, 0.22f, 0.42f, 1f);

            // Coins collected in dark blue over frosted white panel
            GameObject cObj = new GameObject("CoinsCollected", typeof(RectTransform));
            cObj.transform.SetParent(dialog.transform, false);
            RectTransform cRt = cObj.GetComponent<RectTransform>();
            cRt.anchoredPosition = new Vector2(0, 25);
            cRt.sizeDelta = new Vector2(400, 35);
            coinsText = cObj.AddComponent<Text>();
            coinsText.text = "Coins Collected: 0 / 30";
            coinsText.font = font;
            coinsText.fontSize = 20;
            coinsText.fontStyle = FontStyle.Bold;
            coinsText.alignment = TextAnchor.MiddleCenter;
            coinsText.color = new Color(0.08f, 0.18f, 0.38f, 1f);

            Sprite btnGreen = Resources.Load<Sprite>("Sprites/ui_btn_green");
            Sprite btnBlue = Resources.Load<Sprite>("Sprites/ui_btn_blue");

            // Restart Level
            CreateUIButton(dialog.transform, "RestartBtn", "RESTART LEVEL ↻", font, btnGreen, new Vector2(0, -45), new Vector2(240, 50), 19, () => gsm.OnRestartClicked());

            // Main Menu
            CreateUIButton(dialog.transform, "MenuBtn", "MAIN MENU ⌂", font, btnBlue, new Vector2(0, -110), new Vector2(240, 48), 18, () => gsm.OnMainMenuClicked());

            panel.SetActive(false);
            return panel;
        }

        private GameObject CreateUIButton(Transform parent, string name, string text, Font font, Sprite spr, Vector2 pos, Vector2 size, int fontSize, UnityEngine.Events.UnityAction onClick, Vector2? anchor = null, Vector2? pivot = null)
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
            RectTransform txtRt = txtObj.GetComponent<RectTransform>();
            txtRt.anchorMin = Vector2.zero;
            txtRt.anchorMax = Vector2.one;
            txtRt.offsetMin = Vector2.zero;
            txtRt.offsetMax = Vector2.zero;

            Text txt = txtObj.AddComponent<Text>();
            txt.text = text;
            txt.font = font;
            txt.fontSize = fontSize;
            txt.fontStyle = FontStyle.Bold;
            txt.alignment = TextAnchor.MiddleCenter;
            txt.color = Color.white;

            return btnObj;
        }

        private void BuildMobileControls(Transform parent, Font font)
        {
            Sprite touchLeft = Resources.Load<Sprite>("Sprites/ui_touch_left");
            Sprite touchRight = Resources.Load<Sprite>("Sprites/ui_touch_right");
            Sprite touchJump = Resources.Load<Sprite>("Sprites/ui_touch_jump");

            CreateTouchButton(parent, "TouchBtn_Left", new Vector2(90, 90), new Vector2(100, 100), touchLeft, () =>
            {
                SnowballController.SetMoveLeft(true);
            }, () =>
            {
                SnowballController.SetMoveLeft(false);
            });

            CreateTouchButton(parent, "TouchBtn_Right", new Vector2(210, 90), new Vector2(100, 100), touchRight, () =>
            {
                SnowballController.SetMoveRight(true);
            }, () =>
            {
                SnowballController.SetMoveRight(false);
            });

            CreateTouchButton(parent, "TouchBtn_Jump", new Vector2(-110, 90), new Vector2(115, 115), touchJump, () =>
            {
                SnowballController.RequestJump();
            }, null, true);
        }

        private void CreateTouchButton(Transform parent, string name, Vector2 pos, Vector2 size, Sprite spr, System.Action onDown, System.Action onUp, bool isRightAnchor = false)
        {
            GameObject btnObj = new GameObject(name);
            btnObj.transform.SetParent(parent, false);

            RectTransform rt = btnObj.AddComponent<RectTransform>();
            if (isRightAnchor)
            {
                rt.anchorMin = new Vector2(1, 0);
                rt.anchorMax = new Vector2(1, 0);
                rt.pivot = new Vector2(1, 0);
            }
            else
            {
                rt.anchorMin = new Vector2(0, 0);
                rt.anchorMax = new Vector2(0, 0);
                rt.pivot = new Vector2(0, 0);
            }
            rt.anchoredPosition = pos;
            rt.sizeDelta = size;

            Image img = btnObj.AddComponent<Image>();
            img.sprite = spr;
            img.color = new Color(1f, 1f, 1f, 0.7f);

            TouchTrigger trigger = btnObj.AddComponent<TouchTrigger>();
            trigger.onPointerDownAction = onDown;
            trigger.onPointerUpAction = onUp;
        }
    }

    public class TouchTrigger : MonoBehaviour, UnityEngine.EventSystems.IPointerDownHandler, UnityEngine.EventSystems.IPointerUpHandler
    {
        public System.Action onPointerDownAction;
        public System.Action onPointerUpAction;

        public void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
        {
            onPointerDownAction?.Invoke();
        }

        public void OnPointerUp(UnityEngine.EventSystems.PointerEventData eventData)
        {
            onPointerUpAction?.Invoke();
        }
    }
}
