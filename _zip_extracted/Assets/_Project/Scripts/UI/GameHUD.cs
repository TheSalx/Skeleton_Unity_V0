using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RogueLite.Gameplay.Player;
using RogueLite.Gameplay.Progression;
using RogueLite.Core;

namespace RogueLite.UI
{
    /// <summary>
    /// GameHUD: Interface principal do jogo (HP, XP, Timer, Level)
    /// </summary>
    public class GameHUD : MonoBehaviour
    {
        [Header("HP")]
        [SerializeField] private Slider hpBar;
        [SerializeField] private TextMeshProUGUI hpText;

        [Header("XP")]
        [SerializeField] private Slider xpBar;
        [SerializeField] private TextMeshProUGUI levelText;

        [Header("Timer")]
        [SerializeField] private TextMeshProUGUI timerText;

        private PlayerStats playerStats;
        private LevelSystem levelSystem;
        private IEventBus eventBus;
        private float gameStartTime;

        private void Start()
        {
            eventBus = ServiceLocator.Get<IEventBus>();
            gameStartTime = Time.time;

            var playerGO = GameObject.FindGameObjectWithTag(GameConstants.TAG_PLAYER);
            if (playerGO != null)
            {
                playerStats = playerGO.GetComponent<PlayerStats>();
            }

            levelSystem = FindObjectOfType<LevelSystem>();

            // Subscribe to events
            if (eventBus != null)
            {
                eventBus.Subscribe<OnPlayerDamagedEvent>(OnPlayerDamaged);
                eventBus.Subscribe<OnXPGainedEvent>(OnXPGained);
                eventBus.Subscribe<OnLevelUpEvent>(OnLevelUp);
            }

            UpdateHPBar();
            UpdateXPBar();
            UpdateLevelText();
        }

        private void Update()
        {
            UpdateTimer();
        }

        private void OnDestroy()
        {
            if (eventBus != null)
            {
                eventBus.Unsubscribe<OnPlayerDamagedEvent>(OnPlayerDamaged);
                eventBus.Unsubscribe<OnXPGainedEvent>(OnXPGained);
                eventBus.Unsubscribe<OnLevelUpEvent>(OnLevelUp);
            }
        }

        private void OnPlayerDamaged(OnPlayerDamagedEvent evt)
        {
            UpdateHPBar();
        }

        private void OnXPGained(OnXPGainedEvent evt)
        {
            UpdateXPBar();
        }

        private void OnLevelUp(OnLevelUpEvent evt)
        {
            UpdateXPBar();
            UpdateLevelText();
        }

        private void UpdateHPBar()
        {
            if (playerStats == null) return;

            if (hpBar != null)
            {
                hpBar.value = playerStats.CurrentHP / playerStats.MaxHP;
            }

            if (hpText != null)
            {
                hpText.text = $"{Mathf.CeilToInt(playerStats.CurrentHP)} / {Mathf.CeilToInt(playerStats.MaxHP)}";
            }
        }

        private void UpdateXPBar()
        {
            if (levelSystem == null) return;

            if (xpBar != null)
            {
                float progress = (float)levelSystem.CurrentXP / levelSystem.XPRequiredForNextLevel;
                xpBar.value = progress;
            }
        }

        private void UpdateLevelText()
        {
            if (levelSystem == null) return;

            if (levelText != null)
            {
                levelText.text = $"Lv {levelSystem.CurrentLevel}";
            }
        }

        private void UpdateTimer()
        {
            if (timerText == null) return;

            float elapsed = Time.time - gameStartTime;
            int minutes = Mathf.FloorToInt(elapsed / 60f);
            int seconds = Mathf.FloorToInt(elapsed % 60f);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }
}