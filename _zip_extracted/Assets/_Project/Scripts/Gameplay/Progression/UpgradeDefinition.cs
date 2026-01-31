using UnityEngine;
using System;
using System.Collections.Generic;

namespace RogueLite.Data
{
    [Serializable]
    public enum UpgradeType
    {
        NewWeapon,
        WeaponLevelUp,
        PassiveStat,
        Evolution
    }

    [Serializable]
    public enum StatModifierType
    {
        MaxHP,
        MoveSpeed,
        PickupRadius,
        Armor,
        HPRegen,
        DamageMultiplier,
        CooldownReduction,
        CritChance,
        CritDamage,
        XPGain
    }

    [Serializable]
    public class StatModifier
    {
        public StatModifierType statType;
        [Tooltip("Valor absoluto (ex: +10 HP) ou percentual (ex: 0.1 = +10%)")]
        public float value;
        public bool isPercentage = false;
    }

    /// <summary>
    /// UpgradeDefinition: Define um upgrade (arma nova, nível, passivo, evolução)
    /// </summary>
    [CreateAssetMenu(fileName = "Upgrade_", menuName = "RogueLite/Upgrade Definition", order = 3)]
    public class UpgradeDefinition : ScriptableObject
    {
        [Header("Identity")]
        public string upgradeID;
        public string upgradeName;
        [TextArea(2, 4)]
        public string description;
        public Sprite icon;
        public Rarity rarity = Rarity.Common;

        [Header("Type")]
        public UpgradeType upgradeType;

        [Header("Weapon (if type = NewWeapon or WeaponLevelUp)")]
        public WeaponDefinition weaponDefinition;

        [Header("Passive Stats (if type = PassiveStat)")]
        public List<StatModifier> statModifiers = new List<StatModifier>();

        [Header("Evolution (if type = Evolution)")]
        [Tooltip("Arma que será evoluída")]
        public WeaponDefinition baseWeapon;
        [Tooltip("Arma resultante da evolução")]
        public WeaponDefinition evolvedWeapon;
        [Tooltip("Nível mínimo da arma base")]
        public int requiredWeaponLevel = 5;
        [Tooltip("Outros upgrades necessários")]
        public List<UpgradeDefinition> requiredUpgrades = new List<UpgradeDefinition>();

        [Header("Availability")]
        [Tooltip("Peso para aparecer na seleção (maior = mais comum)")]
        [Range(1, 100)]
        public int weight = 50;
        [Tooltip("Nível mínimo do jogador")]
        public int minPlayerLevel = 1;
        [Tooltip("Nível máximo do jogador (0 = sem limite)")]
        public int maxPlayerLevel = 0;
    }
}