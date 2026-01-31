using UnityEngine;
using System;
using System.Collections.Generic;

namespace RogueLite.Data
{
    [Serializable]
    public class WaveConfig
    {
        [Tooltip("Tempo em que essa wave começa (segundos)")]
        public float startTime;
        [Tooltip("Taxa de spawn (inimigos por segundo)")]
        public float spawnRate = 1f;
        [Tooltip("Tipos de inimigos liberados nesta wave")]
        public List<EnemyDefinition> availableEnemies = new List<EnemyDefinition>();
    }

    /// <summary>
    /// StageDefinition: Define um estágio completo com ondas e dificuldade
    /// </summary>
    [CreateAssetMenu(fileName = "Stage_", menuName = "RogueLite/Stage Definition", order = 5)]
    public class StageDefinition : ScriptableObject
    {
        [Header("Identity")]
        public string stageID;
        public string stageName;
        [TextArea(2, 4)]
        public string description;

        [Header("Duration")]
        [Tooltip("Duração total do stage em segundos (0 = infinito)")]
        public float duration = 600f; // 10 minutos

        [Header("Spawn Settings")]
        [Tooltip("Máximo de inimigos vivos simultaneamente")]
        public int maxActiveEnemies = 100;
        [Tooltip("Distância mínima do jogador para spawnar")]
        public float minSpawnDistance = 12f;
        [Tooltip("Distância máxima do jogador para spawnar")]
        public float maxSpawnDistance = 15f;

        [Header("Waves")]
        [Tooltip("Configurações de ondas ao longo do tempo")]
        public List<WaveConfig> waves = new List<WaveConfig>();

        [Header("Difficulty Curve")]
        [Tooltip("Multiplica HP dos inimigos ao longo do tempo")]
        public AnimationCurve hpMultiplierCurve = AnimationCurve.Linear(0, 1, 1, 2);
        [Tooltip("Multiplica dano dos inimigos ao longo do tempo")]
        public AnimationCurve damageMultiplierCurve = AnimationCurve.Linear(0, 1, 1, 1.5f);
        [Tooltip("Multiplica velocidade de spawn ao longo do tempo")]
        public AnimationCurve spawnRateMultiplierCurve = AnimationCurve.Linear(0, 1, 1, 3);

        [Header("Pooling")]
        [Tooltip("Quantidade de cada inimigo para prewarm no pool")]
        public int prewarmCount = 20;

        // ===== MÉTODOS HELPER =====

        public WaveConfig GetCurrentWave(float time)
        {
            WaveConfig current = waves.Count > 0 ? waves[0] : null;
            foreach (var wave in waves)
            {
                if (time >= wave.startTime)
                {
                    current = wave;
                }
                else
                {
                    break;
                }
            }
            return current;
        }

        public float GetHPMultiplier(float normalizedTime)
        {
            return hpMultiplierCurve.Evaluate(normalizedTime);
        }

        public float GetDamageMultiplier(float normalizedTime)
        {
            return damageMultiplierCurve.Evaluate(normalizedTime);
        }

        public float GetSpawnRateMultiplier(float normalizedTime)
        {
            return spawnRateMultiplierCurve.Evaluate(normalizedTime);
        }
    }
}