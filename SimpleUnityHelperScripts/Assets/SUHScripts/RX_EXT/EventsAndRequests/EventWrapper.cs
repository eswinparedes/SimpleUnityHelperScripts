using System;

namespace SUHScripts
{
    public static class EventWrapper
    {        
        public static IDisposable Subscribe<T>(this IEventWrapper<T> @this, Action<T> action)
        {
            @this.Event += action;

            return new _EventWrapperDisposable(
                () =>
                {
                    @this.Event -= action;
                });
        }

        public static IDisposable Subscribe(this IEventWrapper @this, Action action)
        {
            @this.Event += action;

            return new _EventWrapperDisposable(
                () =>
                {
                    @this.Event -= action;
                });
        }

        class _EventWrapperDisposable : IDisposable
        {
            Action m_disposal;

            bool m_isDisposed;

            public _EventWrapperDisposable(Action disposal)
            {
                m_disposal = disposal;
            }

            public void Dispose()
            {
                if (m_isDisposed)
                {
                    return;
                }

                m_disposal();

                m_isDisposed = true;
            }
        }
    }
}