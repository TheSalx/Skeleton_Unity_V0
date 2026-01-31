using UnityEngine;
using RogueLite.Gameplay.Combat;
using System.Collections.Generic;

namespace RogueLite.Gameplay.Player
{
    /// <summary>
    /// OrbitalWeapon: Arma que orbita o jogador causando dano por contato
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class OrbitalWeapon : MonoBehaviour
    {
        private float damage;
        private float knockbackForce;
        private GameObject source;
        private HashSet<GameObject> hitTargets = new HashSet<GameObject>();
        private float lastHitTime;
        private const float HIT_COOLDOWN = 0.5f;

        public void Initialize(float dmg, float knockback, GameObject src)
        {
            damage = dmg;
            knockbackForce = knockback;
            source = src;

            var collider = GetComponent<Collider2D>();
            collider.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (Time.time - lastHitTime < HIT_COOLDOWN) return;

            if (other.gameObject == source) return;

            var damageable = other.GetComponent<IDamageable>();
            if (damageable == null || damageable.IsDead) return;

            Vector3 hitDir = (other.transform.position - transform.position).normalized;
            var damageInfo = new DamageInfo(
                damage,
                source,
                transform.position,
                hitDir,
                knockbackForce,
                false
            );

            damageable.TakeDamage(damageInfo);
            lastHitTime = Time.time;
        }
    }
}