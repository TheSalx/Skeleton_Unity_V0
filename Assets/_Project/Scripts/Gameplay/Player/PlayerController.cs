using UnityEngine;
using RogueLite.Gameplay.Combat;
using RogueLite.Core;

namespace RogueLite.Gameplay.Player
{
    /// <summary>
    /// PlayerController: Controla movimento do jogador
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerStats playerStats;

        private IInputProvider inputProvider;
        private Rigidbody2D rb;
        private Vector2 movementInput;
        private Vector2 knockbackVelocity;
        private float knockbackEndTime;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            if (playerStats == null)
            {
                playerStats = GetComponent<PlayerStats>();
            }

            // Usa input padrão, mas pode ser trocado por inversão de dependência
            inputProvider = new DefaultInputProvider();
        }

        private void Update()
        {
            movementInput = inputProvider.GetMovementInput();
        }

        private void FixedUpdate()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            Vector2 targetVelocity;

            // Se estiver sob efeito de knockback
            if (Time.time < knockbackEndTime)
            {
                targetVelocity = knockbackVelocity;
                knockbackVelocity = Vector2.Lerp(knockbackVelocity, Vector2.zero, Time.fixedDeltaTime * 5f);
            }
            else
            {
                // Movimento normal
                float moveSpeed = playerStats != null ? playerStats.StatBlock.GetMoveSpeed() : 5f;
                targetVelocity = movementInput.normalized * moveSpeed;
            }

            rb.linearVelocity = targetVelocity;
        }

        public void ApplyKnockback(Vector2 direction, float force)
        {
            knockbackVelocity = direction.normalized * force;
            knockbackEndTime = Time.time + GameConstants.DEFAULT_KNOCKBACK_DURATION;
        }

        public void SetInputProvider(IInputProvider provider)
        {
            inputProvider = provider;
        }
    }
}