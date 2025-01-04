using System;

namespace Toolbox.EventManager
{
    public interface IEventManager
    {
        void Register<T>(Action<T> observer) where T : class;
        void Unregister<T>(Action<T> observer) where T : class;
        void Invoke<T>(T eventArgs) where T : class;
    }
}