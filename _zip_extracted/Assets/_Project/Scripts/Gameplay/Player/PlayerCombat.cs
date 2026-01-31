using UnityEngine;
using System.Collections.Generic;
using RogueLite.Data;
using RogueLite.Core;

namespace RogueLite.Gameplay.Player
{
    /// <summary>
    /// PlayerCombat: Gerencia armas equipadas e auto-ataque
    /// </summary>
    public class PlayerCombat : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Transform weaponContainer;

        private List<WeaponRuntime> activeWeapons = new List<WeaponRuntime>();
        private PlayerStats playerStats;

        private void Awake()
        {
            if (weaponContainer == null)
            {
                weaponContainer = transform;
            }

            playerStats = GetComponent<PlayerStats>();
        }

        public void AddWeapon(WeaponDefinition weaponDef)
        {
            if (weaponDef == null)
            {
                Debug.LogError("[PlayerCombat] Cannot add null weapon");
                return;
            }

            // Checa se já tem essa arma
            var existing = activeWeapons.Find(w => w.Definition == weaponDef);
            if (existing != null)
            {
                existing.LevelUp();
                Debug.Log($"[PlayerCombat] Leveled up {weaponDef.weaponName} to level {existing.Level}");
                return;
            }

            // Cria nova arma
            var weaponGO = new GameObject($"Weapon_{weaponDef.weaponName}");
            weaponGO.transform.SetParent(weaponContainer);
            weaponGO.transform.localPosition = Vector3.zero;

            var weaponRuntime = weaponGO.AddComponent<WeaponRuntime>();
            weaponRuntime.Initialize(weaponDef, playerStats, transform);
            activeWeapons.Add(weaponRuntime);

            Debug.Log($"[PlayerCombat] Added weapon: {weaponDef.weaponName}");
        }

        public void LevelUpWeapon(WeaponDefinition weaponDef)
        {
            var weapon = activeWeapons.Find(w => w.Definition == weaponDef);
            if (weapon != null)
            {
                weapon.LevelUp();
            }
        }

        public bool HasWeapon(WeaponDefinition weaponDef)
        {
            return activeWeapons.Exists(w => w.Definition == weaponDef);
        }

        public int GetWeaponLevel(WeaponDefinition weaponDef)
        {
            var weapon = activeWeapons.Find(w => w.Definition == weaponDef);
            return weapon != null ? weapon.Level : 0;
        }

        public List<WeaponRuntime> GetActiveWeapons()
        {
            return new List<WeaponRuntime>(activeWeapons);
        }
    }
}