using UnityEngine;
using System;

namespace RogueLite.Data
{
    [Serializable]
    public enum EnemyType
    {
        Basic,
        Fast,
        Tank,
        Ranged,
        Elite,
        Boss
    }

    /// <summary>
    /// EnemyDefinition: Define um tipo de inimigo (data-driven)
    /// </summary>
    [CreateAssetMenu(fileName = "Enemy_", menuName = "RogueLite/Enemy Definition", order = 4)]
    public class EnemyDefinition : ScriptableObject
    {
        [Header("Identity")]
        public string enemyID;
        public string enemyName;
        public EnemyType enemyType = EnemyType.Basic;

        [Header("Stats")]
        public float maxHP = 20f;
        public float moveSpeed = 2f;
        public float damage = 5f;
        public float attackCooldown = 1f;

        [Header("Behavior")]
        [Tooltip("Distância para começar a atacar")]
        public float attackRange = 1f;
        [Tooltip("Distância de detecção do jogador")]
        public float detectionRange = 15f;
        public bool canKnockback = true;
        public float knockbackResistance = 1f;

        [Header("Rewards")]
        public int xpReward = 1;
        [Range(0f, 1f)]
        public float dropChance = 0.1f;

        [Header("Spawning")]
        [Tooltip("Peso no pool de spawn (maior = mais comum)")]
        [Range(1, 100)]
        public int spawnWeight = 50;

        [Header("Visual")]
        public GameObject prefab;
        public Vector3 scale = Vector3.one;

        [Header("Effects")]
        public GameObject deathEffectPrefab;
    }
}