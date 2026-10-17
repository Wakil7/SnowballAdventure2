using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SnowballAdventure
{
    public class GameSceneManager : MonoBehaviour
    {
        public static GameSceneManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<GameSceneManager>();
                }
                return _instance;
            }
            private set => _instance = value;
        }
        private static GameSceneManager _instance;

        public const int MAX_LIVES = 3;

        [Header("HUD Elements")]
        public Text coinCounterText;
        public Text levelTitleText;
        public Image[] heartImages = new Image[MAX_LIVES];
        public Sprite heartFullSprite;
        public Sprite heartEmptySprite;

        [Header("Panels")]
        public GameObject pausePanel;
        public GameObject levelCompletePanel;
        public Text completeCoinsText;
        public GameObject[] starImages;
        public GameObject adRevivePanel;
        public GameObject gameOverPanel;
        public Text gameOverCoinsText;

        [Header("Character Unlock Notification")]
        public GameObject characterUnlockContainer;
        public Image characterUnlockImage;
        public Text characterUnlockTitleText;
        public Text characterUnlockDescText;
        public RectTransform levelCompleteDialogRt;
        public RectTransform levelCompleteTitleRt;
        public RectTransform levelCompleteStarsContainerRt;
        public RectTransform levelCompleteCoinsRt;
        public RectTransform levelCompleteNextBtnRt;
        public RectTransform levelCompleteRestartBtnRt;
        public RectTransform levelCompleteMenuBtnRt;

        [Header("Runtime")]
        public int currentCoins = 0;
        public int currentLives = MAX_LIVES;
        public bool hasUsedAdRevive = false;
        public bool isGamePaused = false;
        public bool isLevelFinished = false;
        public bool isHandlingDeath = false;

        private SnowballController activePlayer;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Time.timeScale = 1f;
            currentCoins = 0;
            currentLives = MAX_LIVES;
            hasUsedAdRevive = false;
            isLevelFinished = false;
            isGamePaused = false;
            isHandlingDeath = false;

            if (pausePanel != null) pausePanel.SetActive(false);
            if (levelCompletePanel != null) levelCompletePanel.SetActive(false);
            if (adRevivePanel != null) adRevivePanel.SetActive(false);
            if (gameOverPanel != null) gameOverPanel.SetActive(false);

            UpdateHUD();
            UpdateHeartsHUD();

            SoundManager.EnsureInstance().PlayGameplayMusic();
        }

        public void AddCoin()
        {
            currentCoins++;
            UpdateHUD();
        }

        private void UpdateHUD()
        {
            if (coinCounterText != null)
            {
                coinCounterText.text = $"{currentCoins} / {GameDataManager.COINS_PER_LEVEL}";
            }

            if (levelTitleText != null)
            {
                levelTitleText.text = $"LEVEL {GameDataManager.Data.selectedLevel}";
            }
        }

        public void UpdateHeartsHUD()
        {
            if (heartImages == null) return;

            for (int i = 0; i < heartImages.Length; i++)
            {
                if (heartImages[i] == null) continue;

                if (i < currentLives)
                {
                    if (heartFullSprite != null) heartImages[i].sprite = heartFullSprite;
                    heartImages[i].color = Color.white;
                }
                else
                {
                    if (heartEmptySprite != null) heartImages[i].sprite = heartEmptySprite;
                    heartImages[i].color = new Color(0.7f, 0.72f, 0.8f, 0.85f);
                }
            }
        }

        public void OnPlayerDied(SnowballController player)
        {
            if (isLevelFinished || isHandlingDeath) return;
            isHandlingDeath = true;
            activePlayer = player;

            player.TriggerDeathShrink(() =>
            {
                if (currentLives > 0)
                {
                    // Consumes a heart (3 -> 2, 2 -> 1, 1 -> 0)
                    // When the last heart is gone (0 hearts), player still gets a chance just like before!
                    currentLives--;
                    UpdateHeartsHUD();
                    player.RespawnAtCheckpoint();
                    isHandlingDeath = false;
                }
                else if (!hasUsedAdRevive)
                {
                    // Player died while already at 0 hearts: Offer ad revive popup
                    ShowAdRevivePopup();
                }
                else
                {
                    // Ad revive was already used: Game Over!
                    ShowGameOver();
                }
            });
        }

        private void ShowAdRevivePopup()
        {
            if (adRevivePanel != null)
            {
                adRevivePanel.SetActive(true);
            }
            else
            {
                // Fallback to game over if panel missing
                ShowGameOver();
            }
        }

        public void OnWatchAdReviveClicked()
        {
            // Bypasses video playback as requested
            hasUsedAdRevive = true;
            currentLives = MAX_LIVES; // Refills all 3 hearts!
            UpdateHeartsHUD();
            SoundManager.EnsureInstance().PlayClick();

            if (adRevivePanel != null)
            {
                adRevivePanel.SetActive(false);
            }

            // Revive player at checkpoint with all 3 hearts refilled!
            if (activePlayer != null)
            {
                activePlayer.RespawnAtCheckpoint();
            }
            isHandlingDeath = false;
        }

        public void OnGiveUpAdReviveClicked()
        {
            SoundManager.EnsureInstance().PlayClick();
            if (adRevivePanel != null)
            {
                adRevivePanel.SetActive(false);
            }
            ShowGameOver();
        }

        public void ShowGameOver()
        {
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);

                if (gameOverCoinsText != null)
                {
                    gameOverCoinsText.text = $"Coins Collected: {currentCoins} / {GameDataManager.COINS_PER_LEVEL}";
                }
            }
            SoundManager.EnsureInstance().PlayPop();
        }

        public void TriggerLevelComplete()
        {
            if (isLevelFinished) return;
            isLevelFinished = true;

            int level = GameDataManager.Data.selectedLevel;

            // Check if completing this level unlocks a new snowball character
            CharacterDef unlockedChar = GameDataManager.GetCharacterUnlockedByLevel(level);

            GameDataManager.CompleteLevel(level, currentCoins);

            if (unlockedChar != null)
            {
                // When ball is unlocked it must be default selected
                GameDataManager.Data.selectedCharacter = unlockedChar.id;
                GameDataManager.Save();
            }

            StartCoroutine(ShowLevelCompleteSequence(unlockedChar));
        }

        private IEnumerator ShowLevelCompleteSequence(CharacterDef unlockedChar)
        {
            yield return new WaitForSeconds(0.4f);

            if (levelCompletePanel != null)
            {
                levelCompletePanel.SetActive(true);

                if (completeCoinsText != null)
                {
                    completeCoinsText.text = $"Coins Collected: {currentCoins} / {GameDataManager.COINS_PER_LEVEL}";
                }

                // Show stars (1 star for completing, 2 stars for >= 20 coins, 3 stars for all 30 coins!)
                // Uncollected stars appear as shadow silhouette stars
                int earnedStars = 1;
                if (currentCoins >= 30) earnedStars = 3;
                else if (currentCoins >= 20) earnedStars = 2;

                if (starImages != null && starImages.Length >= 3)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (starImages[i] != null)
                        {
                            starImages[i].SetActive(true);
                            Image img = starImages[i].GetComponent<Image>();
                            if (img != null)
                            {
                                img.color = (i < earnedStars) 
                                    ? Color.white 
                                    : new Color(0.12f, 0.22f, 0.38f, 0.35f);
                            }
                        }
                    }
                }

                if (unlockedChar != null && characterUnlockContainer != null)
                {
                    characterUnlockContainer.SetActive(true);

                    if (characterUnlockImage != null)
                    {
                        Sprite s = Resources.Load<Sprite>(unlockedChar.spriteResource);
                        if (s != null) characterUnlockImage.sprite = s;
                    }

                    if (characterUnlockTitleText != null)
                    {
                        characterUnlockTitleText.text = $"{unlockedChar.name.ToUpper()} UNLOCKED!";
                    }

                    if (characterUnlockDescText != null)
                    {
                        characterUnlockDescText.text = $"{unlockedChar.title} • Default Selected!";
                    }

                    // Dynamically expand dialog and reposition elements for the unlock banner
                    if (levelCompleteDialogRt != null) levelCompleteDialogRt.sizeDelta = new Vector2(590, 560);
                    if (levelCompleteTitleRt != null) levelCompleteTitleRt.anchoredPosition = new Vector2(0, 215);
                    if (levelCompleteStarsContainerRt != null) levelCompleteStarsContainerRt.anchoredPosition = new Vector2(0, 135);
                    if (levelCompleteCoinsRt != null) levelCompleteCoinsRt.anchoredPosition = new Vector2(0, 75);
                    if (levelCompleteNextBtnRt != null) levelCompleteNextBtnRt.anchoredPosition = new Vector2(0, -115);
                    if (levelCompleteRestartBtnRt != null)
                    {
                        levelCompleteRestartBtnRt.anchoredPosition = new Vector2(-135, -185);
                        levelCompleteRestartBtnRt.sizeDelta = new Vector2(230, 56);
                    }
                    if (levelCompleteMenuBtnRt != null)
                    {
                        levelCompleteMenuBtnRt.anchoredPosition = new Vector2(135, -185);
                        levelCompleteMenuBtnRt.sizeDelta = new Vector2(230, 56);
                    }

                    // Update scene active player sprite if present
                    if (activePlayer != null)
                    {
                        Sprite s = Resources.Load<Sprite>(unlockedChar.spriteResource);
                        SpriteRenderer sr = activePlayer.GetComponent<SpriteRenderer>();
                        if (sr != null && s != null) sr.sprite = s;
                    }
                }
                else
                {
                    if (characterUnlockContainer != null) characterUnlockContainer.SetActive(false);

                    if (levelCompleteDialogRt != null) levelCompleteDialogRt.sizeDelta = new Vector2(580, 470);
                    if (levelCompleteTitleRt != null) levelCompleteTitleRt.anchoredPosition = new Vector2(0, 175);
                    if (levelCompleteStarsContainerRt != null) levelCompleteStarsContainerRt.anchoredPosition = new Vector2(0, 95);
                    if (levelCompleteCoinsRt != null) levelCompleteCoinsRt.anchoredPosition = new Vector2(0, 30);
                    if (levelCompleteNextBtnRt != null) levelCompleteNextBtnRt.anchoredPosition = new Vector2(0, -45);
                    if (levelCompleteRestartBtnRt != null)
                    {
                        levelCompleteRestartBtnRt.anchoredPosition = new Vector2(-135, -118);
                        levelCompleteRestartBtnRt.sizeDelta = new Vector2(230, 56);
                    }
                    if (levelCompleteMenuBtnRt != null)
                    {
                        levelCompleteMenuBtnRt.anchoredPosition = new Vector2(135, -118);
                        levelCompleteMenuBtnRt.sizeDelta = new Vector2(230, 56);
                    }
                }
            }
        }

        public void TogglePause()
        {
            isGamePaused = !isGamePaused;
            Time.timeScale = isGamePaused ? 0f : 1f;

            if (pausePanel != null)
            {
                pausePanel.SetActive(isGamePaused);
            }

            SoundManager.EnsureInstance().PlayClick();
        }

        public void OnResumeClicked()
        {
            isGamePaused = false;
            Time.timeScale = 1f;
            if (pausePanel != null) pausePanel.SetActive(false);
            SoundManager.EnsureInstance().PlayClick();
        }

        public void OnRestartClicked()
        {
            Time.timeScale = 1f;
            SoundManager.EnsureInstance().PlayClick();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void OnNextLevelClicked()
        {
            Time.timeScale = 1f;
            SoundManager.EnsureInstance().PlayClick();

            int nextLevel = GameDataManager.Data.selectedLevel + 1;
            if (nextLevel <= GameDataManager.TOTAL_LEVELS)
            {
                GameDataManager.Data.selectedLevel = nextLevel;
                GameDataManager.Save();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                SceneManager.LoadScene("MainMenuScene");
            }
        }

        public void OnMainMenuClicked()
        {
            Time.timeScale = 1f;
            SoundManager.EnsureInstance().PlayClick();
            SceneManager.LoadScene("MainMenuScene");
        }

        // On-screen touch control callbacks
        public void OnTouchLeftDown() => SnowballController.SetMoveLeft(true);
        public void OnTouchLeftUp() => SnowballController.SetMoveLeft(false);
        public void OnTouchRightDown() => SnowballController.SetMoveRight(true);
        public void OnTouchRightUp() => SnowballController.SetMoveRight(false);
        public void OnTouchJumpDown() => SnowballController.RequestJump();
    }
}
