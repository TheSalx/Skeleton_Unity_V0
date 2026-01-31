using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using RogueLite.Core;

namespace RogueLite.UI
{
    /// <summary>
    /// GameOverUI: Tela de Game Over / Victory
    /// </summary>
    public class GameOverUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject panel;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI statsText;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button mainMenuButton;

        private IEventBus eventBus;

        private void Start()
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }

            eventBus = ServiceLocator.Get<IEventBus>();
            if (eventBus != null)
            {
                eventBus.Subscribe<OnGameOverEvent>(OnGameOver);
                eventBus.Subscribe<OnPlayerDeathEvent>(OnPlayerDeath);
            }

            if (restartButton != null)
            {
                restartButton.onClick.AddListener(OnRestartClicked);
            }

            if (mainMenuButton != null)
            {
                mainMenuButton.onClick.AddListener(OnMainMenuClicked);
            }
        }

        private void OnDestroy()
        {
            if (eventBus != null)
            {
                eventBus.Unsubscribe<OnGameOverEvent>(OnGameOver);
                eventBus.Unsubscribe<OnPlayerDeathEvent>(OnPlayerDeath);
            }
        }

        private void OnGameOver(OnGameOverEvent evt)
        {
            Show(evt.Victory, evt.Time, evt.Level, evt.Kills);
        }

        private void OnPlayerDeath(OnPlayerDeathEvent evt)
        {
            Show(false, evt.SurvivalTime, 1, 0);
        }

        private void Show(bool victory, float time, int level, int kills)
        {
            if (panel != null)
            {
                panel.SetActive(true);
            }

            if (titleText != null)
            {
                titleText.text = victory ? "VICTORY!" : "GAME OVER";
                titleText.color = victory ? Color.yellow : Color.red;
            }

            if (statsText != null)
            {
                int minutes = Mathf.FloorToInt(time / 60f);
                int seconds = Mathf.FloorToInt(time % 60f);
                statsText.text = $"Time: {minutes:00}:{seconds:00}\nLevel: {level}\nKills: {kills}";
            }

            // Pausa o jogo
            Time.timeScale = 0f;
        }

        private void OnRestartClicked()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void OnMainMenuClicked()
        {
            Time.timeScale = 1f;
            // Se tiver cena de menu, carrega ela
            // SceneManager.LoadScene(GameConstants.SCENE_MENU);
            SceneManager.LoadScene(0);
        }
    }
}