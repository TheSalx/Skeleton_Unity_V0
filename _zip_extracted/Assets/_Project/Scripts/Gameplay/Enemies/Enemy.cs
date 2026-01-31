using UnityEngine;
using RogueLite.Data;
using RogueLite.Gameplay.Combat;
using RogueLite.Core;

namespace RogueLite.Gameplay.Enemies
{
    /// <summary>
    /// Enemy: Inimigo com IA simples (seek player) e sistema de dano
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class Enemy : MonoBehaviour, IDamageable
    {
        [Header("References")]
        [SerializeField] private SpriteRenderer spriteRenderer;

        private EnemyDefinition definition;
        private float currentHP;
        private float currentSpeed;
        private Transform player;
        private Rigidbody2D rb;
        private Vector2 knockbackVelocity;
        private float knockbackEndTime;
        private IEventBus eventBus;

        // Stats modificados por dificuldade
        private float modifiedMaxHP;
        private float modifiedDamage;

        public EnemyDefinition Definition => definition;
        public float CurrentHP => currentHP;
        public float MaxHP => modifiedMaxHP;
        public bool IsDead => currentHP <= 0;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }
        }

        public void Initialize(EnemyDefinition def, float hpMultiplier, float damageMultiplier)
        {
            definition = def;
            modifiedMaxHP = def.maxHP * hpMultiplier;
            modifiedDamage = def.damage * damageMultiplier;
            currentHP = modifiedMaxHP;
            currentSpeed = def.moveSpeed;

            eventBus = ServiceLocator.Get<IEventBus>();

            // Escala
            transform.localScale = def.scale;

            // Encontra player
            var playerGO = GameObject.FindGameObjectWithTag(GameConstants.TAG_PLAYER);
            if (playerGO != null)
            {
                player = playerGO.transform;
            }
        }

        private void Update()
        {
            if (IsDead || player == null) return;

            // Movimento (seek player)
            if (Time.time >= knockbackEndTime)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                rb.velocity = direction * currentSpeed;
            }
            else
            {
                // Sob knockback
                rb.velocity = knockbackVelocity;
                knockbackVelocity = Vector2.Lerp(knockbackVelocity, Vector2.zero, Time.deltaTime * 5f);
            }
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            if (IsDead) return;

            currentHP -= damageInfo.Amount;
            currentHP = Mathf.Max(currentHP, 0);

            // Knockback
            if (definition.canKnockback && damageInfo.KnockbackForce > 0)
            {
                float resistance = definition.knockbackResistance;
                knockbackVelocity = damageInfo.HitDirection.normalized * (damageInfo.KnockbackForce / resistance);
                knockbackEndTime = Time.time + GameConstants.DEFAULT_KNOCKBACK_DURATION;
            }

            // Feedback visual (piscar)
            if (spriteRenderer != null)
            {
                StartCoroutine(FlashWhite());
            }

            // Checa morte
            if (IsDead)
            {
                Die();
            }
        }

        private void Die()
        {
            // Publica evento
            eventBus?.Publish(new OnEnemyKilledEvent
            {
                Enemy = gameObject,
                Position = transform.position,
                XPReward = definition.xpReward
            });

            // Spawna XP
            SpawnXP();

            // Efeito de morte
            if (definition.deathEffectPrefab != null)
            {
                Instantiate(definition.deathEffectPrefab, transform.position, Quaternion.identity);
            }

            // Retorna ao pool
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

        private void SpawnXP()
        {
            var xpManager = FindObjectOfType<XPManager>();
            if (xpManager != null)
            {
                xpManager.SpawnXP(transform.position, definition.xpReward);
            }
        }

        private System.Collections.IEnumerator FlashWhite()
        {
            if (spriteRenderer != null)
            {
                Color original = spriteRenderer.color;
                spriteRenderer.color = Color.white;
                yield return new WaitForSeconds(0.1f);
                spriteRenderer.color = original;
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            // Causa dano ao jogador por contato
            if (collision.gameObject.CompareTag(GameConstants.TAG_PLAYER))
            {
                var playerDamageable = collision.gameObject.GetComponent<IDamageable>();
                if (playerDamageable != null && !playerDamageable.IsDead)
                {
                    Vector3 direction = (collision.transform.position - transform.position).normalized;
                    var damageInfo = new DamageInfo(
                        modifiedDamage,
                        gameObject,
                        collision.contacts[0].point,
                        direction,
                        2f,
                        false
                    );
                    playerDamageable.TakeDamage(damageInfo);
                }
            }
        }
    }
}