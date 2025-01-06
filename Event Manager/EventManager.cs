using System;
using System.Collections.Generic;

namespace Toolbox.EventManager
{
    public class EventManager
    {
        private static EventManager _instance;

        public static EventManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EventManager();
                }

                return _instance;
            }
        }

        private readonly Dictionary<Type, List<object>> _eventListeners = new();

        // Register a listener for a specific event type
        public void Register<T>(Action<T> listener)
        {
            var eventType = typeof(T);
            if (!_eventListeners.ContainsKey(eventType))
            {
                _eventListeners[eventType] = new List<object>();
            }

            _eventListeners[eventType].Add(listener);
        }

        // Unregister a listener for a specific event type
        public void Unregister<T>(Action<T> listener)
        {
            var eventType = typeof(T);
            if (_eventListeners.ContainsKey(eventType))
            {
                _eventListeners[eventType].Remove(listener);

                // Clean up if no listeners remain
                if (_eventListeners[eventType].Count == 0)
                {
                    _eventListeners.Remove(eventType);
                }
            }
        }

        // Invoke an event and notify all listeners
        public void Invoke<T>(T evt)
        {
            var eventType = typeof(T);
            if (_eventListeners.ContainsKey(eventType))
            {
                foreach (var listener in _eventListeners[eventType])
                {
                    ((Action<T>)listener)?.Invoke(evt);
                }
            }
        }

        // Prevent direct instantiation of EventManager outside of the Singleton
        private EventManager()
        {
        }
    }
}