using UnityEngine;
using RogueLite.Gameplay.Combat;
using RogueLite.Core;

namespace RogueLite.Gameplay.Player
{
    /// <summary>
    /// PlayerStats: Gerencia HP, stats e dano recebido pelo jogador
    /// </summary>
    public class PlayerStats : MonoBehaviour, IDamageable
    {
        [Header("Stats")]
        [SerializeField] private StatBlock statBlock = new StatBlock();

        private float currentHP;
        private float invulnerabilityEndTime;
        private IEventBus eventBus;

        public StatBlock StatBlock => statBlock;
        public float CurrentHP => currentHP;
        public float MaxHP => statBlock.GetMaxHP();
        public bool IsDead => currentHP <= 0;

        private void Awake()
        {
            currentHP = statBlock.GetMaxHP();
        }

        private void Start()
        {
            eventBus = ServiceLocator.Get<IEventBus>();
        }

        private void Update()
        {
            // Regeneração de HP
            if (currentHP < MaxHP)
            {
                float regen = statBlock.GetHPRegen() * Time.deltaTime;
                if (regen > 0)
                {
                    currentHP = Mathf.Min(currentHP + regen, MaxHP);
                }
            }
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            // Checa invulnerabilidade
            if (Time.time < invulnerabilityEndTime)
            {
                return;
            }

            // Aplica armor (reduz dano)
            float armor = statBlock.GetArmor();
            float damageReduction = armor / (armor + 100f); // Fórmula comum de armor
            float finalDamage = damageInfo.Amount * (1f - damageReduction);

            currentHP -= finalDamage;
            currentHP = Mathf.Max(currentHP, 0f);

            // Ativa invulnerabilidade temporária
            invulnerabilityEndTime = Time.time + GameConstants.DEFAULT_INVULNERABILITY_DURATION;

            // Aplica knockback
            if (damageInfo.KnockbackForce > 0)
            {
                var controller = GetComponent<PlayerController>();
                if (controller != null)
                {
                    controller.ApplyKnockback(damageInfo.HitDirection, damageInfo.KnockbackForce);
                }
            }

            // Publica evento
            eventBus?.Publish(new OnPlayerDamagedEvent
            {
                Damage = finalDamage,
                CurrentHP = currentHP,
                MaxHP = MaxHP,
                Source = damageInfo.Source
            });

            // Checa morte
            if (IsDead)
            {
                HandleDeath();
            }
        }

        private void HandleDeath()
        {
            eventBus?.Publish(new OnPlayerDeathEvent
            {
                SurvivalTime = Time.timeSinceLevelLoad
            });

            Debug.Log("[Player] Death");
            // Game Over será tratado por GameManager
        }

        public void Heal(float amount)
        {
            currentHP = Mathf.Min(currentHP + amount, MaxHP);
        }

        public void IncreaseMaxHP(float amount)
        {
            float oldMax = MaxHP;
            statBlock.maxHP += amount;
            float newMax = MaxHP;
            // Aumenta HP atual proporcionalmente
            currentHP = currentHP * (newMax / oldMax);
        }
    }
}