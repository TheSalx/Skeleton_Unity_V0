using System.Collections.Generic;
using UnityEngine;

namespace RogueLite.Core
{
    public interface IObjectPoolManager
    {
        GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation);
        void Despawn(GameObject obj);
        void Prewarm(GameObject prefab, int count);
        void ClearPool(GameObject prefab);
        void ClearAll();
    }

    /// <summary>
    /// ObjectPoolManager: Gerencia pools de objetos reutilizáveis
    /// </summary>
    public class ObjectPoolManager : MonoBehaviour, IObjectPoolManager
    {
        private class Pool
        {
            public GameObject Prefab;
            public Queue<GameObject> Available = new Queue<GameObject>();
            public HashSet<GameObject> Active = new HashSet<GameObject>();
            public Transform Parent;
        }

        private readonly Dictionary<GameObject, Pool> pools = new Dictionary<GameObject, Pool>();
        private readonly Dictionary<GameObject, GameObject> instanceToPrefab = new Dictionary<GameObject, GameObject>();

        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            if (prefab == null)
            {
                Debug.LogError("[ObjectPool] Cannot spawn null prefab");
                return null;
            }

            // Cria pool se não existir
            if (!pools.TryGetValue(prefab, out var pool))
            {
                pool = CreatePool(prefab);
            }

            GameObject instance;
            if (pool.Available.Count > 0)
            {
                instance = pool.Available.Dequeue();
            }
            else
            {
                instance = Instantiate(prefab, pool.Parent);
                instanceToPrefab[instance] = prefab;
            }

            instance.transform.position = position;
            instance.transform.rotation = rotation;
            instance.SetActive(true);
            pool.Active.Add(instance);

            return instance;
        }

        public void Despawn(GameObject obj)
        {
            if (obj == null) return;

            if (!instanceToPrefab.TryGetValue(obj, out var prefab))
            {
                Debug.LogWarning($"[ObjectPool] Object {obj.name} not from pool. Destroying.");
                Destroy(obj);
                return;
            }

            if (!pools.TryGetValue(prefab, out var pool))
            {
                Debug.LogWarning($"[ObjectPool] Pool not found for {prefab.name}. Destroying.");
                Destroy(obj);
                return;
            }

            pool.Active.Remove(obj);
            pool.Available.Enqueue(obj);
            obj.SetActive(false);
        }

        public void Prewarm(GameObject prefab, int count)
        {
            if (prefab == null || count <= 0) return;

            if (!pools.TryGetValue(prefab, out var pool))
            {
                pool = CreatePool(prefab);
            }

            for (int i = 0; i < count; i++)
            {
                var instance = Instantiate(prefab, pool.Parent);
                instance.SetActive(false);
                pool.Available.Enqueue(instance);
                instanceToPrefab[instance] = prefab;
            }

            Debug.Log($"[ObjectPool] Prewarmed {count} instances of {prefab.name}");
        }

        public void ClearPool(GameObject prefab)
        {
            if (!pools.TryGetValue(prefab, out var pool)) return;

            foreach (var obj in pool.Active)
            {
                if (obj != null)
                {
                    instanceToPrefab.Remove(obj);
                    Destroy(obj);
                }
            }

            while (pool.Available.Count > 0)
            {
                var obj = pool.Available.Dequeue();
                if (obj != null)
                {
                    instanceToPrefab.Remove(obj);
                    Destroy(obj);
                }
            }

            if (pool.Parent != null)
            {
                Destroy(pool.Parent.gameObject);
            }

            pools.Remove(prefab);
        }

        public void ClearAll()
        {
            var prefabList = new List<GameObject>(pools.Keys);
            foreach (var prefab in prefabList)
            {
                ClearPool(prefab);
            }
        }

        private Pool CreatePool(GameObject prefab)
        {
            var poolParent = new GameObject($"Pool_{prefab.name}");
            poolParent.transform.SetParent(transform);

            var pool = new Pool
            {
                Prefab = prefab,
                Parent = poolParent.transform
            };

            pools[prefab] = pool;
            return pool;
        }

        private void OnDestroy()
        {
            ClearAll();
        }
    }
}