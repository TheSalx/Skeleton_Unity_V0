using UnityEngine;
using RogueLite.Core;

namespace RogueLite.Gameplay.Progression
{
    /// <summary>
    /// LevelSystem: Gerencia XP e níveis do jogador
    /// </summary>
    public class LevelSystem : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int baseXPRequired = 10;
        [SerializeField] private float xpScalingFactor = 1.5f;

        private int currentLevel = 1;
        private int currentXP = 0;
        private int xpRequiredForNextLevel;
        private IEventBus eventBus;

        public int CurrentLevel => currentLevel;
        public int CurrentXP => currentXP;
        public int XPRequiredForNextLevel => xpRequiredForNextLevel;

        private void Start()
        {
            eventBus = ServiceLocator.Get<IEventBus>();
            xpRequiredForNextLevel = CalculateXPRequired(currentLevel);
        }

        public void AddXP(int amount)
        {
            currentXP += amount;

            eventBus?.Publish(new OnXPGainedEvent
            {
                Amount = amount,
                CurrentXP = currentXP,
                RequiredXP = xpRequiredForNextLevel
            });

            // Checa level up
            while (currentXP >= xpRequiredForNextLevel)
            {
                LevelUp();
            }
        }

        private void LevelUp()
        {
            currentXP -= xpRequiredForNextLevel;
            currentLevel++;
            xpRequiredForNextLevel = CalculateXPRequired(currentLevel);

            eventBus?.Publish(new OnLevelUpEvent
            {
                NewLevel = currentLevel,
                CurrentXP = currentXP
            });

            Debug.Log($"[LevelSystem] Level Up! Now level {currentLevel}");

            // Pausa o jogo e mostra UI de upgrade
            Time.timeScale = 0f;
            var upgradeUI = FindObjectOfType<UI.LevelUpUI>();
            if (upgradeUI != null)
            {
                upgradeUI.Show();
            }
        }

        private int CalculateXPRequired(int level)
        {
            return Mathf.RoundToInt(baseXPRequired * Mathf.Pow(xpScalingFactor, level - 1));
        }
    }
}