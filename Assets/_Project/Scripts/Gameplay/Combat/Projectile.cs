using UnityEngine;
using RogueLite.Data;
using RogueLite.Gameplay.Combat;
using RogueLite.Core;
using System.Collections.Generic;

namespace RogueLite.Gameplay.Player
{
    /// <summary>
    /// Projectile: Projétil que se move e causa dano
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class Projectile : MonoBehaviour
    {
        private ProjectileDefinition definition;
        private Vector3 direction;
        private float damage;
        private float knockbackForce;
        private int pierceRemaining;
        private bool isCritical;
        private GameObject source;

        private Rigidbody2D rb;
        private float currentSpeed;
        private float spawnTime;
        private HashSet<GameObject> hitTargets = new HashSet<GameObject>();
        private Transform homingTarget;
        private float homingStartTime;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0;
        }

        public void Initialize(ProjectileDefinition def, Vector3 dir, float dmg, float knockback, int pierce, bool crit, GameObject src)
        {
            definition = def;
            direction = dir.normalized;
            damage = dmg;
            knockbackForce = knockback;
            pierceRemaining = pierce;
            isCritical = crit;
            source = src;

            currentSpeed = definition.speed;
            spawnTime = Time.time;
            hitTargets.Clear();

            // Rotaciona para a direção
            if (definition.rotateTowardsDirection)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }

            // Homing
            if (definition.behavior == ProjectileBehavior.Homing)
            {
                homingStartTime = Time.time + definition.homingDelay;
                var targetProvider = new EnemyTargetProvider();
                homingTarget = targetProvider.GetNearestTarget(transform.position, 20f);
            }

            // Escala
            transform.localScale = definition.scale;
        }

        private void Update()
        {
            // Checa lifetime
            if (Time.time - spawnTime >= definition.lifetime)
            {
                Despawn();
                return;
            }

            // Aceleração
            if (definition.acceleration != 0)
            {
                currentSpeed += definition.acceleration * Time.deltaTime;
            }

            // Comportamento de homing
            if (definition.behavior == ProjectileBehavior.Homing && Time.time >= homingStartTime)
            {
                if (homingTarget != null && homingTarget.gameObject.activeInHierarchy)
                {
                    Vector3 toTarget = (homingTarget.position - transform.position).normalized;
                    direction = Vector3.Lerp(direction, toTarget, definition.homingStrength * Time.deltaTime).normalized;

                    if (definition.rotateTowardsDirection)
                    {
                        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                        transform.rotation = Quaternion.Euler(0, 0, angle);
                    }
                }
            }

            // Movimento
            rb.linearVelocity = direction * currentSpeed;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Ignora source e projéteis
            if (other.gameObject == source || other.CompareTag(GameConstants.TAG_PROJECTILE))
            {
                return;
            }

            // Checa se é damageable
            var damageable = other.GetComponent<IDamageable>();
            if (damageable == null || damageable.IsDead)
            {
                return;
            }

            // Evita hit duplo no mesmo alvo (pierce)
            if (hitTargets.Contains(other.gameObject))
            {
                return;
            }

            // Aplica dano
            Vector3 hitDir = (other.transform.position - transform.position).normalized;
            var damageInfo = new DamageInfo(
                damage,
                source,
                transform.position,
                hitDir,
                knockbackForce,
                isCritical
            );

            damageable.TakeDamage(damageInfo);
            hitTargets.Add(other.gameObject);

            // Efeito de hit
            if (definition.hitEffectPrefab != null)
            {
                Instantiate(definition.hitEffectPrefab, transform.position, Quaternion.identity);
            }

            // Checa pierce
            if (definition.hitBehavior == HitBehavior.Pierce && pierceRemaining > 0)
            {
                pierceRemaining--;
            }
            else if (definition.hitBehavior == HitBehavior.SingleHit)
            {
                Despawn();
            }
        }

        private void Despawn()
        {
            var poolManager = ServiceLocator.Get<IObjectPoolManager>();
            if (poolManager != null)
            {
                poolManager.Despawn(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}