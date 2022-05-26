using UnityEngine;
using System;
using SUHScripts.Functional;

namespace SUHScripts
{   
    public class StaticInstance<TInstance> : IDisposable
    {
        static Option<TInstance> m_instance;
        public static TInstance Instance
        {
            get
            {
                if (!m_instance.IsSome)
                {
                    Debug.LogWarning($"Instance of type {typeof(TInstance).Name} not set!");
                    return default;
                }

                return m_instance.Value;
            }
        }

        bool m_isDisposed = false;

        public bool IsActive { get; private set; } = false;

        public StaticInstance(TInstance context)
        {
            TrySetContext(context);
        }

        protected StaticInstance() { }

        protected bool TrySetContext(TInstance context)
        {
            if (!IsContextAlreadySet())
            {
                Debug.Log($"Setting Static Instance of type: {typeof(TInstance).Name}");
                m_instance = context.AsOption_UNSAFE();
                IsActive = true;
                return true;
            }

            return false;
        }
        protected bool IsContextAlreadySet()
        {
            if (m_instance.IsSome)
            {
                Debug.LogError($"Instanced Context already set for {typeof(TInstance).Name}");
                return true;
            }

            return false;
        }

        public virtual void Dispose()
        {
            if (!IsActive) return;

            if (m_isDisposed)
            {
                Debug.LogError($"MessageBoxAsyncContext already disposed.");
                return;
            }

            m_instance = None.Default;
            IsActive = false;
            m_isDisposed = true;            
        }
    }

}
