using UnityEngine;
using System;

namespace RogueLite.Data
{
    [Serializable]
    public enum ProjectileBehavior
    {
        Straight,
        Homing,
        Boomerang,
        Orbit
    }

    [Serializable]
    public enum HitBehavior
    {
        SingleHit,
        Pierce,
        Bounce
    }

    /// <summary>
    /// ProjectileDefinition: Define comportamento de projéteis
    /// </summary>
    [CreateAssetMenu(fileName = "Projectile_", menuName = "RogueLite/Projectile Definition", order = 2)]
    public class ProjectileDefinition : ScriptableObject
    {
        [Header("Movement")]
        public ProjectileBehavior behavior = ProjectileBehavior.Straight;
        public float speed = 10f;
        public float lifetime = 5f;
        public float acceleration = 0f;

        [Header("Homing (if applicable)")]
        public float homingStrength = 5f;
        public float homingDelay = 0.1f;

        [Header("Hit Behavior")]
        public HitBehavior hitBehavior = HitBehavior.SingleHit;
        public int maxPierceCount = 0;
        public int maxBounces = 0;
        public float bounceAngleVariation = 30f;

        [Header("Visual")]
        public GameObject prefab;
        public Vector3 scale = Vector3.one;
        public bool rotateTowardsDirection = true;

        [Header("Effects")]
        public GameObject hitEffectPrefab;
        public GameObject trailEffectPrefab;
    }
}