using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using RogueLite.Data;
using RogueLite.Gameplay.Player;
using RogueLite.Core;

namespace RogueLite.Gameplay.Progression
{
    /// <summary>
    /// UpgradeSystem: Gerencia aplicação de upgrades e seleção aleatória
    /// </summary>
    public class UpgradeSystem : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private List<UpgradeDefinition> allUpgrades = new List<UpgradeDefinition>();

        private PlayerStats playerStats;
        private PlayerCombat playerCombat;
        private LevelSystem levelSystem;
        private List<UpgradeDefinition> selectedUpgrades = new List<UpgradeDefinition>();
        private IEventBus eventBus;

        private void Start()
        {
            eventBus = ServiceLocator.Get<IEventBus>();
            levelSystem = FindObjectOfType<LevelSystem>();

            var playerGO = GameObject.FindGameObjectWithTag(GameConstants.TAG_PLAYER);
            if (playerGO != null)
            {
                playerStats = playerGO.GetComponent<PlayerStats>();
                playerCombat = playerGO.GetComponent<PlayerCombat>();
            }
        }

        public List<UpgradeDefinition> GetRandomUpgrades(int count)
        {
            var available = GetAvailableUpgrades();
            if (available.Count == 0)
            {
                Debug.LogWarning("[UpgradeSystem] No available upgrades!");
                return new List<UpgradeDefinition>();
            }

            // Seleciona 'count' upgrades sem repetição, considerando raridade
            var selected = new List<UpgradeDefinition>();
            var pool = new List<UpgradeDefinition>(available);

            for (int i = 0; i < count && pool.Count > 0; i++)
            {
                var upgrade = SelectWeightedRandom(pool);
                selected.Add(upgrade);
                pool.Remove(upgrade);
            }

            return selected;
        }

        private List<UpgradeDefinition> GetAvailableUpgrades()
        {
            int playerLevel = levelSystem != null ? levelSystem.CurrentLevel : 1;
            var available = new List<UpgradeDefinition>();

            foreach (var upgrade in allUpgrades)
            {
                if (upgrade == null) continue;

                // Checa nível
                if (upgrade.minPlayerLevel > playerLevel) continue;
                if (upgrade.maxPlayerLevel > 0 && upgrade.maxPlayerLevel < playerLevel) continue;

                // Checa tipo
                switch (upgrade.upgradeType)
                {
                    case UpgradeType.NewWeapon:
                        // Só oferece se não tiver a arma
                        if (playerCombat != null && !playerCombat.HasWeapon(upgrade.weaponDefinition))
                        {
                            available.Add(upgrade);
                        }
                        break;

                    case UpgradeType.WeaponLevelUp:
                        // Só oferece se tiver a arma
                        if (playerCombat != null && playerCombat.HasWeapon(upgrade.weaponDefinition))
                        {
                            available.Add(upgrade);
                        }
                        break;

                    case UpgradeType.PassiveStat:
                        // Sempre disponível
                        available.Add(upgrade);
                        break;

                    case UpgradeType.Evolution:
                        // Checa requisitos
                        if (CanEvolve(upgrade))
                        {
                            available.Add(upgrade);
                        }
                        break;
                }
            }

            return available;
        }

        private bool CanEvolve(UpgradeDefinition evolution)
        {
            if (playerCombat == null) return false;

            // Checa se tem a arma base no nível requerido
            if (evolution.baseWeapon != null)
            {
                int weaponLevel = playerCombat.GetWeaponLevel(evolution.baseWeapon);
                if (weaponLevel < evolution.requiredWeaponLevel)
                {
                    return false;
                }
            }

            // Checa upgrades necessários
            foreach (var required in evolution.requiredUpgrades)
            {
                if (!selectedUpgrades.Contains(required))
                {
                    return false;
                }
            }

            return true;
        }

        private UpgradeDefinition SelectWeightedRandom(List<UpgradeDefinition> pool)
        {
            if (pool.Count == 0) return null;
            if (pool.Count == 1) return pool[0];

            int totalWeight = pool.Sum(u => u.weight);
            int randomValue = Random.Range(0, totalWeight);
            int cumulative = 0;

            foreach (var upgrade in pool)
            {
                cumulative += upgrade.weight;
                if (randomValue < cumulative)
                {
                    return upgrade;
                }
            }

            return pool[pool.Count - 1];
        }

        public void ApplyUpgrade(UpgradeDefinition upgrade)
        {
            if (upgrade == null) return;

            selectedUpgrades.Add(upgrade);

            switch (upgrade.upgradeType)
            {
                case UpgradeType.NewWeapon:
                    if (playerCombat != null && upgrade.weaponDefinition != null)
                    {
                        playerCombat.AddWeapon(upgrade.weaponDefinition);
                    }
                    break;

                case UpgradeType.WeaponLevelUp:
                    if (playerCombat != null && upgrade.weaponDefinition != null)
                    {
                        playerCombat.LevelUpWeapon(upgrade.weaponDefinition);
                    }
                    break;

                case UpgradeType.PassiveStat:
                    ApplyStatModifiers(upgrade);
                    break;

                case UpgradeType.Evolution:
                    if (playerCombat != null && upgrade.evolvedWeapon != null)
                    {
                        // Remove arma base e adiciona evoluída
                        // (simplificado: apenas adiciona a evoluída)
                        playerCombat.AddWeapon(upgrade.evolvedWeapon);
                    }
                    break;
            }

            eventBus?.Publish(new OnUpgradeSelectedEvent
            {
                UpgradeID = upgrade.upgradeID,
                UpgradeName = upgrade.upgradeName
            });

            Debug.Log($"[UpgradeSystem] Applied upgrade: {upgrade.upgradeName}");
        }

        private void ApplyStatModifiers(UpgradeDefinition upgrade)
        {
            if (playerStats == null) return;

            foreach (var modifier in upgrade.statModifiers)
            {
                playerStats.StatBlock.AddModifier(
                    upgrade.upgradeID,
                    modifier.statType,
                    modifier.value,
                    modifier.isPercentage
                );

                Debug.Log($"[UpgradeSystem] Applied stat modifier: {modifier.statType} {(modifier.isPercentage ? "+" + (modifier.value * 100) + "%" : "+" + modifier.value)}");
            }
        }
    }
}