using UnityEngine;
using RogueLite.Data;
using RogueLite.Core;
using System.Collections.Generic;
using System.Linq;

namespace RogueLite.Gameplay.Enemies
{
    /// <summary>
    /// EnemySpawner: Spawna inimigos baseado em StageDefinition e curvas de dificuldade
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private StageDefinition stageDefinition;
        [SerializeField] private Transform player;

        private IObjectPoolManager poolManager;
        private float stageStartTime;
        private float lastSpawnTime;
        private int activeEnemyCount;
        private List<GameObject> activeEnemies = new List<GameObject>();

        private void Start()
        {
            poolManager = ServiceLocator.Get<IObjectPoolManager>();
            stageStartTime = Time.time;

            if (player == null)
            {
                var playerGO = GameObject.FindGameObjectWithTag(GameConstants.TAG_PLAYER);
                if (playerGO != null)
                {
                    player = playerGO.transform;
                }
            }

            if (stageDefinition == null)
            {
                Debug.LogError("[EnemySpawner] StageDefinition is null!");
                return;
            }

            PrewarmPools();
        }

        private void PrewarmPools()
        {
            if (poolManager == null || stageDefinition.waves == null) return;

            var allEnemies = new HashSet<EnemyDefinition>();
            foreach (var wave in stageDefinition.waves)
            {
                foreach (var enemy in wave.availableEnemies)
                {
                    if (enemy != null && enemy.prefab != null)
                    {
                        allEnemies.Add(enemy);
                    }
                }
            }

            foreach (var enemyDef in allEnemies)
            {
                poolManager.Prewarm(enemyDef.prefab, stageDefinition.prewarmCount);
            }

            Debug.Log($"[EnemySpawner] Prewarmed {allEnemies.Count} enemy types");
        }

        private void Update()
        {
            if (stageDefinition == null || player == null) return;

            float elapsed = Time.time - stageStartTime;

            // Checa fim do stage
            if (stageDefinition.duration > 0 && elapsed >= stageDefinition.duration)
            {
                // Victory
                var eventBus = ServiceLocator.Get<IEventBus>();
                eventBus?.Publish(new OnGameOverEvent
                {
                    Victory = true,
                    Time = elapsed,
                    Level = FindObjectOfType<Gameplay.Progression.LevelSystem>()?.CurrentLevel ?? 1,
                    Kills = 0 // TODO: implementar contador de kills
                });
                enabled = false;
                return;
            }

            // Limpa referências de inimigos inativos
            activeEnemies.RemoveAll(e => e == null || !e.activeInHierarchy);
            activeEnemyCount = activeEnemies.Count;

            // Spawna novos inimigos
            TrySpawn(elapsed);
        }

        private void TrySpawn(float elapsed)
        {
            // Checa limite de inimigos
            if (activeEnemyCount >= stageDefinition.maxActiveEnemies)
            {
                return;
            }

            // Obtém wave atual
            var currentWave = stageDefinition.GetCurrentWave(elapsed);
            if (currentWave == null || currentWave.availableEnemies.Count == 0)
            {
                return;
            }

            // Calcula spawn rate com multiplicador
            float normalizedTime = stageDefinition.duration > 0 ? elapsed / stageDefinition.duration : 0;
            float spawnRateMult = stageDefinition.GetSpawnRateMultiplier(normalizedTime);
            float effectiveSpawnRate = currentWave.spawnRate * spawnRateMult;
            float spawnInterval = 1f / effectiveSpawnRate;

            // Checa se deve spawnar
            if (Time.time - lastSpawnTime < spawnInterval)
            {
                return;
            }

            lastSpawnTime = Time.time;

            // Seleciona inimigo baseado em peso
            var enemyDef = SelectEnemy(currentWave.availableEnemies);
            if (enemyDef == null || enemyDef.prefab == null)
            {
                return;
            }

            // Posição de spawn (ao redor do player)
            Vector3 spawnPos = GetRandomSpawnPosition();

            // Spawna
            var enemyGO = poolManager.Spawn(enemyDef.prefab, spawnPos, Quaternion.identity);
            var enemy = enemyGO.GetComponent<Enemy>();
            if (enemy == null)
            {
                enemy = enemyGO.AddComponent<Enemy>();
            }

            // Aplica multiplicadores de dificuldade
            float hpMult = stageDefinition.GetHPMultiplier(normalizedTime);
            float dmgMult = stageDefinition.GetDamageMultiplier(normalizedTime);
            enemy.Initialize(enemyDef, hpMult, dmgMult);

            activeEnemies.Add(enemyGO);
        }

        private EnemyDefinition SelectEnemy(List<EnemyDefinition> enemies)
        {
            if (enemies.Count == 0) return null;
            if (enemies.Count == 1) return enemies[0];

            // Weighted random
            int totalWeight = enemies.Sum(e => e.spawnWeight);
            int randomValue = Random.Range(0, totalWeight);
            int cumulative = 0;

            foreach (var enemy in enemies)
            {
                cumulative += enemy.spawnWeight;
                if (randomValue < cumulative)
                {
                    return enemy;
                }
            }

            return enemies[enemies.Count - 1];
        }

        private Vector3 GetRandomSpawnPosition()
        {
            float distance = Random.Range(stageDefinition.minSpawnDistance, stageDefinition.maxSpawnDistance);
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * distance;
            return player.position + offset;
        }
    }
}