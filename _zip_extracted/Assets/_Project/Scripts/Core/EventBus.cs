using System;
using System.Collections.Generic;
using UnityEngine;

namespace RogueLite.Core
{
    public interface IEventBus
    {
        void Subscribe<T>(Action<T> handler) where T : struct;
        void Unsubscribe<T>(Action<T> handler) where T : struct;
        void Publish<T>(T eventData) where T : struct;
    }

    /// <summary>
    /// EventBus: Sistema de eventos desacoplado usando structs
    /// </summary>
    public class EventBus : IEventBus
    {
        private readonly Dictionary<Type, Delegate> eventHandlers = new Dictionary<Type, Delegate>();

        public void Subscribe<T>(Action<T> handler) where T : struct
        {
            var eventType = typeof(T);
            if (eventHandlers.TryGetValue(eventType, out var existingHandler))
            {
                eventHandlers[eventType] = Delegate.Combine(existingHandler, handler);
            }
            else
            {
                eventHandlers[eventType] = handler;
            }
        }

        public void Unsubscribe<T>(Action<T> handler) where T : struct
        {
            var eventType = typeof(T);
            if (eventHandlers.TryGetValue(eventType, out var existingHandler))
            {
                var newHandler = Delegate.Remove(existingHandler, handler);
                if (newHandler == null)
                {
                    eventHandlers.Remove(eventType);
                }
                else
                {
                    eventHandlers[eventType] = newHandler;
                }
            }
        }

        public void Publish<T>(T eventData) where T : struct
        {
            var eventType = typeof(T);
            if (eventHandlers.TryGetValue(eventType, out var handler))
            {
                (handler as Action<T>)?.Invoke(eventData);
            }
        }
    }

    // ===== EVENT DEFINITIONS =====
    // Todos os eventos do jogo definidos como structs

    public struct OnEnemyKilledEvent
    {
        public GameObject Enemy;
        public Vector3 Position;
        public int XPReward;
    }

    public struct OnXPGainedEvent
    {
        public int Amount;
        public int CurrentXP;
        public int RequiredXP;
    }

    public struct OnLevelUpEvent
    {
        public int NewLevel;
        public int CurrentXP;
    }

    public struct OnPlayerDamagedEvent
    {
        public float Damage;
        public float CurrentHP;
        public float MaxHP;
        public GameObject Source;
    }

    public struct OnPlayerDeathEvent
    {
        public float SurvivalTime;
    }

    public struct OnUpgradeSelectedEvent
    {
        public string UpgradeID;
        public string UpgradeName;
    }

    public struct OnGameOverEvent
    {
        public bool Victory;
        public float Time;
        public int Level;
        public int Kills;
    }
}