using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using RogueLite.Data;
using RogueLite.Gameplay.Progression;

namespace RogueLite.UI
{
    /// <summary>
    /// LevelUpUI: Tela de seleção de upgrade ao subir de nível
    /// </summary>
    public class LevelUpUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject panel;
        [SerializeField] private UpgradeCard[] upgradeCards = new UpgradeCard[3];

        private UpgradeSystem upgradeSystem;
        private List<UpgradeDefinition> currentOptions;

        private void Awake()
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }

            upgradeSystem = FindObjectOfType<UpgradeSystem>();
        }

        public void Show()
        {
            if (upgradeSystem == null)
            {
                Debug.LogError("[LevelUpUI] UpgradeSystem not found!");
                return;
            }

            currentOptions = upgradeSystem.GetRandomUpgrades(3);

            for (int i = 0; i < upgradeCards.Length; i++)
            {
                if (i < currentOptions.Count)
                {
                    upgradeCards[i].Setup(currentOptions[i], () => OnUpgradeSelected(currentOptions[i]));
                    upgradeCards[i].gameObject.SetActive(true);
                }
                else
                {
                    upgradeCards[i].gameObject.SetActive(false);
                }
            }

            if (panel != null)
            {
                panel.SetActive(true);
            }
        }

        private void OnUpgradeSelected(UpgradeDefinition upgrade)
        {
            upgradeSystem.ApplyUpgrade(upgrade);
            Hide();
        }

        private void Hide()
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }

            // Despausa o jogo
            Time.timeScale = 1f;
        }
    }

    /// <summary>
    /// UpgradeCard: Card individual de upgrade
    /// </summary>
    [System.Serializable]
    public class UpgradeCard : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Image rarityBorder;
        [SerializeField] private Button button;

        private System.Action onClickCallback;

        private void Awake()
        {
            if (button != null)
            {
                button.onClick.AddListener(OnClick);
            }
        }

        public void Setup(UpgradeDefinition upgrade, System.Action onClick)
        {
            if (upgrade == null) return;

            onClickCallback = onClick;

            if (iconImage != null)
            {
                iconImage.sprite = upgrade.icon;
                iconImage.enabled = upgrade.icon != null;
            }

            if (nameText != null)
            {
                nameText.text = upgrade.upgradeName;
            }

            if (descriptionText != null)
            {
                descriptionText.text = upgrade.description;
            }

            if (rarityBorder != null)
            {
                rarityBorder.color = GetRarityColor(upgrade.rarity);
            }
        }

        private void OnClick()
        {
            onClickCallback?.Invoke();
        }

        private Color GetRarityColor(Rarity rarity)
        {
            return rarity switch
            {
                Rarity.Common => Color.white,
                Rarity.Uncommon => Color.green,
                Rarity.Rare => Color.blue,
                Rarity.Epic => new Color(0.6f, 0f, 1f),
                Rarity.Legendary => new Color(1f, 0.5f, 0f),
                _ => Color.white
            };
        }
    }
}