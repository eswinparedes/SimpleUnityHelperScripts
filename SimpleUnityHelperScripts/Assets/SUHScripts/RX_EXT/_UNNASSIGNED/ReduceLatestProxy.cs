using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SUHScripts.Functional;

namespace SUHScripts
{
    public class ReduceLatestProxy<T>
    {
        Dictionary<object, T> m_latestValues = new Dictionary<object, T>();

        public ReduceLatestProxy(Func<T, T, T> aggregator, Func<int, T, T> reducer, Func<T, T, T> interpolator, T defaulValue)
        {
            m_defaultValue = defaulValue;
            m_aggregator = aggregator;
            m_reducer = reducer;
            m_interpolator = interpolator;
        }

        T m_defaultValue;
        Func<T, T, T> m_aggregator;
        Func<int, T, T> m_reducer;
        Func<T, T, T> m_interpolator;
        T m_lastValue;

        public T Update()
        {
            var valueCount = m_latestValues.Count;

            if (valueCount == 0)
            {
                m_lastValue = m_defaultValue;
                return m_defaultValue;
            }

            var enumerator = m_latestValues.GetEnumerator();
            T runningValue = default;
            int iterationCount = 0;
            while (enumerator.MoveNext())
            {
                if (iterationCount == 0) runningValue = enumerator.Current.Value;
                else runningValue = m_aggregator(runningValue, enumerator.Current.Value);
                iterationCount++;
            }

            var newValue = m_reducer(iterationCount, runningValue);
            var finalValue = m_interpolator(m_lastValue, newValue);
            m_lastValue = finalValue;
            return finalValue;
        }

        public void SendLatest(object key, T value)
        {
            m_latestValues[key] = value;
        }

        public void Clear(object key)
        {
            if (m_latestValues.ContainsKey(key))
                m_latestValues.Remove(key);
            else
                Debug.LogError($"this object key does not have a latest value");
        }

        public void ClearAll()
        {
            m_latestValues.Clear();
        }
    }
}

