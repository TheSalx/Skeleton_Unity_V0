using UnityEngine;
using System;

namespace RogueLite.Data
{
    [Serializable]
    public enum WeaponType
    {
        Projectile,
        Orbital,
        Area,
        Beam
    }

    [Serializable]
    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    /// <summary>
    /// WeaponDefinition: Define uma arma completa (data-driven)
    /// </summary>
    [CreateAssetMenu(fileName = "Weapon_", menuName = "RogueLite/Weapon Definition", order = 1)]
    public class WeaponDefinition : ScriptableObject
    {
        [Header("Identity")]
        public string weaponID;
        public string weaponName;
        [TextArea(2, 4)]
        public string description;
        public Sprite icon;
        public Rarity rarity = Rarity.Common;

        [Header("Type")]
        public WeaponType weaponType = WeaponType.Projectile;

        [Header("Base Stats")]
        public float baseDamage = 10f;
        public float baseCooldown = 1f;
        public int baseProjectileCount = 1;
        public float baseRange = 10f;
        
        [Header("Scaling per Level")]
        [Tooltip("Dano adicional por nível (ex: 0.1 = +10%)")]
        public float damagePerLevel = 0.1f;
        [Tooltip("Redução de cooldown por nível (ex: 0.05 = -5%)")]
        public float cooldownReductionPerLevel = 0.05f;
        public int projectilesPerLevel = 0;

        [Header("Combat Properties")]
        public float knockbackForce = 2f;
        public int pierceCount = 0;
        [Range(0f, 1f)]
        public float critChance = 0.05f;
        public float critMultiplier = 2f;

        [Header("Projectile (if applicable)")]
        public ProjectileDefinition projectileDefinition;

        [Header("Orbital (if applicable)")]
        public float orbitalRadius = 2f;
        public float orbitalSpeed = 180f;
        public int orbitalCount = 3;

        [Header("Visual")]
        public GameObject weaponPrefab;

        // ===== MÉTODOS DE CÁLCULO =====

        public float GetDamageAtLevel(int level)
        {
            return baseDamage * (1f + damagePerLevel * (level - 1));
        }

        public float GetCooldownAtLevel(int level)
        {
            float reduction = 1f - (cooldownReductionPerLevel * (level - 1));
            return Mathf.Max(baseCooldown * reduction, baseCooldown * 0.2f); // Min 20% do cooldown base
        }

        public int GetProjectileCountAtLevel(int level)
        {
            return baseProjectileCount + (projectilesPerLevel * (level - 1));
        }
    }
}