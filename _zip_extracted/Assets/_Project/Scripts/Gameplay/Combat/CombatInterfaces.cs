using UnityEngine;

namespace RogueLite.Gameplay.Combat
{
    /// <summary>
    /// DamageInfo: Informações sobre dano causado
    /// </summary>
    public struct DamageInfo
    {
        public float Amount;
        public GameObject Source;
        public Vector3 HitPoint;
        public Vector3 HitDirection;
        public float KnockbackForce;
        public bool IsCritical;

        public DamageInfo(float amount, GameObject source, Vector3 hitPoint, Vector3 hitDirection, float knockbackForce = 0f, bool isCritical = false)
        {
            Amount = amount;
            Source = source;
            HitPoint = hitPoint;
            HitDirection = hitDirection;
            KnockbackForce = knockbackForce;
            IsCritical = isCritical;
        }
    }

    /// <summary>
    /// IDamageable: Interface para entidades que podem receber dano
    /// </summary>
    public interface IDamageable
    {
        void TakeDamage(DamageInfo damageInfo);
        bool IsDead { get; }
        float CurrentHP { get; }
        float MaxHP { get; }
    }

    /// <summary>
    /// ITargetProvider: Interface para selecionar alvos
    /// </summary>
    public interface ITargetProvider
    {
        Transform GetNearestTarget(Vector3 position, float maxRange);
        Transform[] GetTargetsInRange(Vector3 position, float range);
    }
}