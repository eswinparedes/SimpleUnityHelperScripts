using SUHScripts;
using System;

namespace SUHScripts
{
    
    public class EventWrapperSubject : IEventWrapper, IEventSubject
    {
        public event Action Event;

        public void Raise()
        {
            Event?.Invoke();
        }

        public void ClearEvent()
        {
            Event = null;
        }
    }
    public class EventWrapperSubject<T> : IEventWrapper<T>, IEventSubject<T>
    {
        public event Action<T> Event;

        public void Raise(T input)
        {
            Event?.Invoke(input);
        }

        public void ClearEvent()
        {
            Event = null;
        }
    }

}