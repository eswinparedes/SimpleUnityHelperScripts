using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SUHScripts.Functional;
using UniRx;

namespace SUHScripts
{
    public static partial class GenericHelpers 
    {
        public static Option<IObservable<U>> TryGetMergedObservables<T, U>(GameObject target, Func<T, IObservable<U>> targetFunction)
        {
            var comps = target.GetComponents<T>();

            if(comps != null && comps.Length > 0)
            {
                var obvss = new List<IObservable<U>>();
                for(int i =0; i < comps.Length; i++)
                {
                    var obv = targetFunction(comps[i]);
                    obvss.Add(obv);
                }

                return obvss.Merge().AsOption_SAFE();
            }
            else
            {
                return None.Default;
            }
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IReadOnlyList<InspectorKeyValuePair<TKey, TValue>> @this)
        {
            var d = new Dictionary<TKey, TValue>();

            for(int i =0; i < @this.Count; i++)
            {
                d.Add(@this[i].Key, @this[i].Value);
            }

            return d;
        }
        public static Dictionary<TKey, Action<TKey, TValue>> ActionLock<TKey, TValue>(this Dictionary<TKey, TValue> @this, Action<TKey, TValue> action)
        {
            var d = new Dictionary<TKey, Action<TKey, TValue>>();

            foreach(var kvp in @this)
            {
                Action<TKey, TValue> act =
                    (key, value) =>
                    {
                        if (@this.ContainsKey(key))
                            action(key, value);
                    };

                d.Add(kvp.Key, act);
            }

            return d;
        }


        public static Dictionary<TKey, TValue> Merge<TKey, TValue>(this IReadOnlyList<Dictionary<TKey, TValue>> @this)
        {
            var d = new Dictionary<TKey, TValue>();

            for(int i =0;i < @this.Count; i++)
            {
                foreach(var kvp in @this[i])
                {
                    d.Add(kvp.Key, kvp.Value);
                }
            }

            return d;
        }

        public static Action Append(this Action action, Action toAppend) =>
            () =>
            {
                action();
                toAppend();
            };
      
    }

    /// <summary>
    /// Represents an action that may or may not happen, true means it did, false means it did not
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public delegate bool MayHappen<T>(T value);
    public delegate bool MayHappen<T, U>(T tVal, U uVal);

    [System.Serializable]
    public abstract class InspectorKeyValuePair<TKey, TValue>
    {
        [SerializeField] TKey m_key = default;
        [SerializeField] TValue m_value = default;
        public TKey Key => m_key;
        public TValue Value => m_value;
    }
}

