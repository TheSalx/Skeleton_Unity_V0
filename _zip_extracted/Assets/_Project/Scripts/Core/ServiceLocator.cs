using System;
using System.Collections.Generic;
using UnityEngine;

namespace RogueLite.Core
{
    /// <summary>
    /// ServiceLocator: Gerencia dependências globais de forma simples e explícita
    /// </summary>
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> services = new Dictionary<Type, object>();
        private static bool isInitialized = false;

        public static void Initialize()
        {
            if (isInitialized)
            {
                Debug.LogWarning("[ServiceLocator] Already initialized");
                return;
            }

            services.Clear();
            isInitialized = true;
            Debug.Log("[ServiceLocator] Initialized");
        }

        public static void Register<T>(T service) where T : class
        {
            var type = typeof(T);
            if (services.ContainsKey(type))
            {
                Debug.LogWarning($"[ServiceLocator] Service {type.Name} already registered. Overwriting.");
                services[type] = service;
            }
            else
            {
                services.Add(type, service);
                Debug.Log($"[ServiceLocator] Registered {type.Name}");
            }
        }

        public static T Get<T>() where T : class
        {
            var type = typeof(T);
            if (services.TryGetValue(type, out var service))
            {
                return service as T;
            }

            Debug.LogError($"[ServiceLocator] Service {type.Name} not found!");
            return null;
        }

        public static bool TryGet<T>(out T service) where T : class
        {
            var type = typeof(T);
            if (services.TryGetValue(type, out var obj))
            {
                service = obj as T;
                return service != null;
            }

            service = null;
            return false;
        }

        public static void Unregister<T>() where T : class
        {
            var type = typeof(T);
            if (services.Remove(type))
            {
                Debug.Log($"[ServiceLocator] Unregistered {type.Name}");
            }
        }

        public static void Clear()
        {
            services.Clear();
            isInitialized = false;
            Debug.Log("[ServiceLocator] Cleared");
        }
    }
}