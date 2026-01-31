using UnityEngine;
using RogueLite.Core;

namespace RogueLite.Gameplay.Progression
{
    /// <summary>
    /// XPOrb: Orbe de XP que o jogador coleta
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class XPOrb : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        private int xpValue;
        private Transform player;
        private bool isBeingCollected = false;

        public void Initialize(int value)
        {
            xpValue = value;
            isBeingCollected = false;

            var playerGO = GameObject.FindGameObjectWithTag(GameConstants.TAG_PLAYER);
            if (playerGO != null)
            {
                player = playerGO.transform;
            }

            var rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0;

            var collider = GetComponent<Collider2D>();
            collider.isTrigger = true;
        }

        private void Update()
        {
            if (player == null || isBeingCollected) return;

            float distance = Vector3.Distance(transform.position, player.position);
            
            // Magnet effect
            var xpManager = FindObjectOfType<XPManager>();
            float magnetRadius = xpManager != null ? xpManager.GetMagnetRadius() : GameConstants.XP_MAGNET_RADIUS;

            if (distance < magnetRadius)
            {
                // Move em direção ao player
                Vector3 direction = (player.position - transform.position).normalized;
                transform.position += direction * GameConstants.XP_MAGNET_SPEED * Time.deltaTime;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isBeingCollected) return;

            if (other.CompareTag(GameConstants.TAG_PLAYER))
            {
                Collect();
            }
        }

        private void Collect()
        {
            isBeingCollected = true;

            var xpManager = FindObjectOfType<XPManager>();
            if (xpManager != null)
            {
                xpManager.AddXP(xpValue);
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
    }
}