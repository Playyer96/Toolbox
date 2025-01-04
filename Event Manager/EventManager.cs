using System;
using System.Collections.Generic;

namespace Toolbox.EventManager
{
    public class EventManager : IEventManager
    {
        private static EventManager _instance;
        public static EventManager Instance => _instance ??= new EventManager();

        private readonly Dictionary<Type, object> _eventHandlers = new();

        // Register an observer for a specific event type
        public void Register<T>(Action<T> observer) where T : class
        {
            if (!_eventHandlers.TryGetValue(typeof(T), out var handlers))
            {
                handlers = new List<Action<T>>();
                _eventHandlers[typeof(T)] = handlers;
            }

            var typedHandlers = (List<Action<T>>)handlers;
            if (!typedHandlers.Contains(observer))
            {
                typedHandlers.Add(observer);
            }
        }

        // Unregister an observer for a specific event type
        public void Unregister<T>(Action<T> observer) where T : class
        {
            if (_eventHandlers.TryGetValue(typeof(T), out var handlers))
            {
                var typedHandlers = (List<Action<T>>)handlers;
                typedHandlers.Remove(observer);

                if (typedHandlers.Count == 0)
                {
                    _eventHandlers.Remove(typeof(T));
                }
            }
        }

        // Invoke all observers for a specific event type
        public void Invoke<T>(T eventArgs) where T : class
        {
            if (_eventHandlers.TryGetValue(typeof(T), out var handlers))
            {
                var typedHandlers = (List<Action<T>>)handlers;
                foreach (var handler in typedHandlers)
                {
                    handler.Invoke(eventArgs);
                }
            }
        }
    }
}