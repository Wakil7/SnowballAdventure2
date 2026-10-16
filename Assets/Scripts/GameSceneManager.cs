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
                    // Revive by consuming a life (max 3 times)
                    currentLives--;
                    UpdateHeartsHUD();
                    player.RespawnAtCheckpoint();
                    isHandlingDeath = false;
                }
                else if (!hasUsedAdRevive)
                {
                    // 0 lives left: Offer video ad revive popup
                    ShowAdRevivePopup();
                }
                else
                {
                    // Ad revive was already used (last chance passed): Game Over!
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
            SoundManager.EnsureInstance().PlayClick();

            if (adRevivePanel != null)
            {
                adRevivePanel.SetActive(false);
            }

            // Revive player at checkpoint for their last chance!
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
            GameDataManager.CompleteLevel(level, currentCoins);

            StartCoroutine(ShowLevelCompleteSequence());
        }

        private IEnumerator ShowLevelCompleteSequence()
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
                if (starImages != null && starImages.Length >= 3)
                {
                    starImages[0].SetActive(true);
                    starImages[1].SetActive(currentCoins >= 20);
                    starImages[2].SetActive(currentCoins >= 30);
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
