using UnityEngine;
using UnityEngine.SceneManagement;

namespace RogueLite.Core
{
    /// <summary>
    /// Bootstrap: Inicializa todos os sistemas do jogo na ordem correta
    /// </summary>
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private bool dontDestroyOnLoad = true;

        private void Awake()
        {
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }

            InitializeSystems();
        }

        private void InitializeSystems()
        {
            // Inicializa ServiceLocator
            ServiceLocator.Initialize();

            // Registra EventBus
            var eventBus = new EventBus();
            ServiceLocator.Register<IEventBus>(eventBus);

            // Registra ObjectPool
            var poolManager = gameObject.AddComponent<ObjectPoolManager>();
            ServiceLocator.Register<IObjectPoolManager>(poolManager);

            Debug.Log("[Bootstrap] Systems initialized successfully");
        }
    }
}