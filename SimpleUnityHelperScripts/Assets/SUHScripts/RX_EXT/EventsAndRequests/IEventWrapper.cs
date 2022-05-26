using System;

namespace SUHScripts
{
    public interface IEventWrapper
    {
        public event Action Event;
    }
    public interface IEventWrapper<T>
    {
        public event Action<T> Event;
    }
}