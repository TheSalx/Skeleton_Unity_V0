using UnityEngine;
using RogueLite.Core;
using RogueLite.Gameplay.Player;

namespace RogueLite.Gameplay.Progression
{
    /// <summary>
    /// XPManager: Gerencia spawn de XP orbs e coleta
    /// </summary>
    public class XPManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private GameObject xpOrbPrefab;

        private PlayerStats playerStats;
        private LevelSystem levelSystem;
        private IObjectPoolManager poolManager;

        private void Start()
        {
            poolManager = ServiceLocator.Get<IObjectPoolManager>();
            levelSystem = FindObjectOfType<LevelSystem>();

            var playerGO = GameObject.FindGameObjectWithTag(GameConstants.TAG_PLAYER);
            if (playerGO != null)
            {
                playerStats = playerGO.GetComponent<PlayerStats>();
            }

            // Prewarm pool
            if (xpOrbPrefab != null && poolManager != null)
            {
                poolManager.Prewarm(xpOrbPrefab, 50);
            }
        }

        public void SpawnXP(Vector3 position, int amount)
        {
            if (xpOrbPrefab == null)
            {
                Debug.LogWarning("[XPManager] XP Orb prefab is null");
                return;
            }

            var orbGO = poolManager.Spawn(xpOrbPrefab, position, Quaternion.identity);
            var orb = orbGO.GetComponent<XPOrb>();
            if (orb == null)
            {
                orb = orbGO.AddComponent<XPOrb>();
            }

            orb.Initialize(amount);
        }

        public void AddXP(int amount)
        {
            if (levelSystem != null)
            {
                // Aplica multiplicador de XP gain
                float multiplier = playerStats != null ? playerStats.StatBlock.GetXPGainMultiplier() : 1f;
                int finalAmount = Mathf.RoundToInt(amount * multiplier);
                levelSystem.AddXP(finalAmount);
            }
        }

        public float GetMagnetRadius()
        {
            if (playerStats != null)
            {
                return playerStats.StatBlock.GetPickupRadius();
            }
            return GameConstants.XP_MAGNET_RADIUS;
        }
    }
}