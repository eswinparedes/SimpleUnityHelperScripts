using SUHScripts.Functional;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SUHScripts.Functional.Functional;

namespace SUHScripts
{
    /// <summary>
    /// Currently in Testing
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class DisposableValue<T> : IDisposable
    {
        public Option<T> Value { get; private set; }

        Action<T> m_onDisposed;

        public DisposableValue(T value, Action<T> onDisposed)
        {
            Value = value.AsOption_SAFE();
            m_onDisposed = onDisposed;
        }

        public void Dispose()
        {
            if (Value.IsSome)
            {
                m_onDisposed(Value.Value);
                Value = None.Default;
            }
                
        }
    }

}
