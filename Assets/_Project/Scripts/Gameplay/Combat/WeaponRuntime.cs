using UnityEngine;
using RogueLite.Data;
using RogueLite.Gameplay.Player;
using RogueLite.Gameplay.Combat;
using RogueLite.Core;
using System.Collections.Generic;

namespace RogueLite.Gameplay.Player
{
    /// <summary>
    /// WeaponRuntime: Lógica runtime de uma arma equipada
    /// </summary>
    public class WeaponRuntime : MonoBehaviour
    {
        private WeaponDefinition definition;
        private PlayerStats playerStats;
        private Transform owner;
        private int level = 1;
        private float nextFireTime;
        private ITargetProvider targetProvider;
        private IObjectPoolManager poolManager;

        // Para armas orbitais
        private List<GameObject> orbitalInstances = new List<GameObject>();
        private float orbitalAngle = 0f;

        public WeaponDefinition Definition => definition;
        public int Level => level;

        public void Initialize(WeaponDefinition weaponDef, PlayerStats stats, Transform ownerTransform)
        {
            definition = weaponDef;
            playerStats = stats;
            owner = ownerTransform;
            level = 1;

            targetProvider = new EnemyTargetProvider();
            poolManager = ServiceLocator.Get<IObjectPoolManager>();

            if (definition.weaponType == WeaponType.Orbital)
            {
                SetupOrbital();
            }
        }

        public void LevelUp()
        {
            level++;
            if (definition.weaponType == WeaponType.Orbital)
            {
                SetupOrbital(); // Recria orbitais com novo level
            }
        }

        private void Update()
        {
            if (definition == null) return;

            switch (definition.weaponType)
            {
                case WeaponType.Projectile:
                    UpdateProjectile();
                    break;
                case WeaponType.Orbital:
                    UpdateOrbital();
                    break;
            }
        }

        private void UpdateProjectile()
        {
            if (Time.time < nextFireTime) return;

            float cooldown = definition.GetCooldownAtLevel(level);
            if (playerStats != null)
            {
                float cdReduction = playerStats.StatBlock.GetCooldownReduction();
                cooldown *= (1f - cdReduction);
            }

            nextFireTime = Time.time + cooldown;

            FireProjectiles();
        }

        private void FireProjectiles()
        {
            if (definition.projectileDefinition == null || definition.projectileDefinition.prefab == null)
            {
                Debug.LogWarning($"[WeaponRuntime] {definition.weaponName} missing projectile prefab");
                return;
            }

            int projectileCount = definition.GetProjectileCountAtLevel(level);
            Transform target = targetProvider.GetNearestTarget(owner.position, definition.baseRange);

            for (int i = 0; i < projectileCount; i++)
            {
                Vector3 direction;
                if (target != null)
                {
                    direction = (target.position - owner.position).normalized;
                }
                else
                {
                    // Dispara em direção aleatória se não houver alvo
                    float angle = Random.Range(0f, 360f);
                    direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
                }

                // Spread entre projéteis
                if (projectileCount > 1)
                {
                    float spreadAngle = ((float)i / (projectileCount - 1) - 0.5f) * 30f;
                    direction = Quaternion.Euler(0, 0, spreadAngle) * direction;
                }

                SpawnProjectile(direction);
            }
        }

        private void SpawnProjectile(Vector3 direction)
        {
            var projectileGO = poolManager.Spawn(
                definition.projectileDefinition.prefab,
                owner.position,
                Quaternion.identity
            );

            var projectile = projectileGO.GetComponent<Projectile>();
            if (projectile == null)
            {
                projectile = projectileGO.AddComponent<Projectile>();
            }

            float damage = definition.GetDamageAtLevel(level);
            if (playerStats != null)
            {
                damage *= playerStats.StatBlock.GetDamageMultiplier();
            }

            // Checa crit
            bool isCrit = false;
            float critChance = definition.critChance;
            if (playerStats != null)
            {
                critChance += playerStats.StatBlock.GetCritChance();
            }
            if (Random.value < critChance)
            {
                isCrit = true;
                float critMult = definition.critMultiplier;
                if (playerStats != null)
                {
                    critMult += playerStats.StatBlock.GetCritDamage() - 1f;
                }
                damage *= critMult;
            }

            projectile.Initialize(definition.projectileDefinition, direction, damage, definition.knockbackForce, definition.pierceCount, isCrit, owner.gameObject);
        }

        private void SetupOrbital()
        {
            // Remove orbitais existentes
            foreach (var orbital in orbitalInstances)
            {
                if (orbital != null)
                {
                    Destroy(orbital);
                }
            }
            orbitalInstances.Clear();

            int count = definition.orbitalCount + (level - 1);
            for (int i = 0; i < count; i++)
            {
                GameObject orbital = Instantiate(definition.weaponPrefab, owner);
                orbital.transform.localPosition = Vector3.zero;
                
                var orbitalBehavior = orbital.GetComponent<OrbitalWeapon>();
                if (orbitalBehavior == null)
                {
                    orbitalBehavior = orbital.AddComponent<OrbitalWeapon>();
                }

                float damage = definition.GetDamageAtLevel(level);
                if (playerStats != null)
                {
                    damage *= playerStats.StatBlock.GetDamageMultiplier();
                }

                orbitalBehavior.Initialize(damage, definition.knockbackForce, owner.gameObject);
                orbitalInstances.Add(orbital);
            }
        }

        private void UpdateOrbital()
        {
            orbitalAngle += definition.orbitalSpeed * Time.deltaTime;
            if (orbitalAngle >= 360f) orbitalAngle -= 360f;

            for (int i = 0; i < orbitalInstances.Count; i++)
            {
                if (orbitalInstances[i] == null) continue;

                float angle = orbitalAngle + (360f / orbitalInstances.Count) * i;
                float rad = angle * Mathf.Deg2Rad;
                Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * definition.orbitalRadius;
                orbitalInstances[i].transform.localPosition = offset;
            }
        }

        private void OnDestroy()
        {
            foreach (var orbital in orbitalInstances)
            {
                if (orbital != null)
                {
                    Destroy(orbital);
                }
            }
        }
    }

    /// <summary>
    /// EnemyTargetProvider: Encontra inimigos como alvos
    /// </summary>
    public class EnemyTargetProvider : ITargetProvider
    {
        public Transform GetNearestTarget(Vector3 position, float maxRange)
        {
            var enemies = GameObject.FindGameObjectsWithTag(GameConstants.TAG_ENEMY);
            Transform nearest = null;
            float minDist = maxRange;

            foreach (var enemy in enemies)
            {
                if (!enemy.activeInHierarchy) continue;
                float dist = Vector3.Distance(position, enemy.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = enemy.transform;
                }
            }

            return nearest;
        }

        public Transform[] GetTargetsInRange(Vector3 position, float range)
        {
            var enemies = GameObject.FindGameObjectsWithTag(GameConstants.TAG_ENEMY);
            var targets = new System.Collections.Generic.List<Transform>();

            foreach (var enemy in enemies)
            {
                if (!enemy.activeInHierarchy) continue;
                if (Vector3.Distance(position, enemy.transform.position) <= range)
                {
                    targets.Add(enemy.transform);
                }
            }

            return targets.ToArray();
        }
    }
}