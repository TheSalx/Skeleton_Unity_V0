using System;
using System.Collections.Generic;
using UnityEngine;

namespace RogueLite.Gameplay.Combat
{
    /// <summary>
    /// StatBlock: Armazena e gerencia estatísticas do jogador com modificadores
    /// </summary>
    [Serializable]
    public class StatBlock
    {
        [Header("Base Stats")]
        public float maxHP = 100f;
        public float moveSpeed = 5f;
        public float pickupRadius = 1.5f;
        public float armor = 0f;
        public float hpRegen = 0f;
        public float damageMultiplier = 1f;
        public float cooldownReduction = 0f;
        public float critChance = 0f;
        public float critDamage = 1.5f;
        public float xpGainMultiplier = 1f;

        // Modificadores temporários
        private Dictionary<string, StatModifierValue> modifiers = new Dictionary<string, StatModifierValue>();

        private struct StatModifierValue
        {
            public float FlatBonus;
            public float PercentBonus;
        }

        public void AddModifier(string modifierID, Data.StatModifierType statType, float value, bool isPercentage)
        {
            string key = $"{modifierID}_{statType}";
            modifiers[key] = new StatModifierValue
            {
                FlatBonus = isPercentage ? 0 : value,
                PercentBonus = isPercentage ? value : 0
            };
        }

        public void RemoveModifier(string modifierID, Data.StatModifierType statType)
        {
            string key = $"{modifierID}_{statType}";
            modifiers.Remove(key);
        }

        public float GetMaxHP()
        {
            return ApplyModifiers(maxHP, Data.StatModifierType.MaxHP);
        }

        public float GetMoveSpeed()
        {
            return ApplyModifiers(moveSpeed, Data.StatModifierType.MoveSpeed);
        }

        public float GetPickupRadius()
        {
            return ApplyModifiers(pickupRadius, Data.StatModifierType.PickupRadius);
        }

        public float GetArmor()
        {
            return ApplyModifiers(armor, Data.StatModifierType.Armor);
        }

        public float GetHPRegen()
        {
            return ApplyModifiers(hpRegen, Data.StatModifierType.HPRegen);
        }

        public float GetDamageMultiplier()
        {
            return ApplyModifiers(damageMultiplier, Data.StatModifierType.DamageMultiplier);
        }

        public float GetCooldownReduction()
        {
            return ApplyModifiers(cooldownReduction, Data.StatModifierType.CooldownReduction);
        }

        public float GetCritChance()
        {
            return ApplyModifiers(critChance, Data.StatModifierType.CritChance);
        }

        public float GetCritDamage()
        {
            return ApplyModifiers(critDamage, Data.StatModifierType.CritDamage);
        }

        public float GetXPGainMultiplier()
        {
            return ApplyModifiers(xpGainMultiplier, Data.StatModifierType.XPGain);
        }

        private float ApplyModifiers(float baseValue, Data.StatModifierType statType)
        {
            float flatBonus = 0f;
            float percentBonus = 0f;

            foreach (var kvp in modifiers)
            {
                if (kvp.Key.EndsWith($"_{statType}"))
                {
                    flatBonus += kvp.Value.FlatBonus;
                    percentBonus += kvp.Value.PercentBonus;
                }
            }

            return (baseValue + flatBonus) * (1f + percentBonus);
        }
    }
}