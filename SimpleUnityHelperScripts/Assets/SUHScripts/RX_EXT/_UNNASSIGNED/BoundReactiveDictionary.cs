using UniRx;
using System;
using System.Collections;
using System.Collections.Generic;
using SUHScripts.Functional;

namespace SUHScripts
{
    public class BoundReactiveDictionary<TKey, TValue> : IReadOnlyReactiveDictionary<TKey, TValue>, IDisposable
    {
        public BoundReactiveDictionary(IReadOnlyReactiveDictionary<TKey, TValue> source)
        {
            Source = source;
        }

        public BoundReactiveDictionary<TKey, TNewValue> Map<TNewValue>(Func<TValue, TNewValue> mapFunction)
        {
            var d = new ReactiveDictionary<TKey, TNewValue>();

            foreach (var kvp in Source)
            {
                d.Add(kvp.Key, mapFunction(kvp.Value));
            }

            var sub0 = Source.ObserveAdd().Subscribe(evt =>
            {
                d.Add(evt.Key, mapFunction(evt.Value));
            }).AddTo(m_disposables);

            var sub1 = Source.ObserveReplace().Subscribe(evt =>
            {
                d[evt.Key] = mapFunction(evt.NewValue);
            }).AddTo(m_disposables);

            var sub2 = Source.ObserveRemove().Subscribe(evt =>
            {
                d.Remove(evt.Key);
            }).AddTo(m_disposables);

            var sub3 = Source.ObserveReset().Subscribe(evt =>
            {
                d.Clear();
            }).AddTo(m_disposables);

            return new BoundReactiveDictionary<TKey, TNewValue>(d);
        }

        public bool IsDisposed => m_disposables.IsDisposed;

        public void Dispose()
        {
            m_disposables.Dispose();
        }

        public bool ContainsKey(TKey key) => Source.ContainsKey(key);

        public bool TryGetValue(TKey key, out TValue value) => Source.TryGetValue(key, out value);

        public IObservable<DictionaryAddEvent<TKey, TValue>> ObserveAdd() => Source.ObserveAdd();

        public IObservable<int> ObserveCountChanged(bool notifyCurrentCount = false) => Source.ObserveCountChanged();

        public IObservable<DictionaryRemoveEvent<TKey, TValue>> ObserveRemove() => Source.ObserveRemove();

        public IObservable<DictionaryReplaceEvent<TKey, TValue>> ObserveReplace() => Source.ObserveReplace();

        public IObservable<Unit> ObserveReset() => Source.ObserveReset();

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => Source.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Source.GetEnumerator();

        public IReadOnlyReactiveDictionary<TKey, TValue> Source { get; private set; }

        public int Count => Source.Count;

        public TValue this[TKey index] => Source[index];

        CompositeDisposable m_disposables = new CompositeDisposable();
    }

    public static class BoundReactiveDictionaryExtensions
    {
        public static BoundReactiveDictionary<TKey, TValue> ToBoundReactiveDictionary<TKey, TValue>(this IReadOnlyList<InspectorKeyValuePair<TKey, TValue>> @this)
        {
            return new BoundReactiveDictionary<TKey, TValue>(@this.ToDictionary().ToReactiveDictionary());
        }
        public static BoundReactiveDictionary<TKey, TValue> ToBoundReactiveDictionary<TKey, TValue>(this Dictionary<TKey, TValue> @this)
        {
            return new BoundReactiveDictionary<TKey, TValue>(@this.ToReactiveDictionary());
        }

        /// <summary>
        /// Executes action if key is in dictionary, returns true if the action was executed, false if it was not.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static MayHappen<TKey> GetBoundAction<TKey, TValue>(IDictionary<TKey, TValue> dict, Action<TKey, TValue> action)
        {
            return (key) =>
            {
                if (dict.TryGetValue(key, out TValue value))
                {
                    action(key, value);
                    return true;
                }
                else
                {
                    return false;
                }
            };
        }

        /// <summary>
        /// Executes a map if key is in dictionary, returns 'Some' if the map was executed, 'NONE' if it was not.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static Func<TKey, Option<TNewValue>> GetBoundMap<TKey, TValue, TNewValue>(IDictionary<TKey, TValue> dict, Func<TKey, TValue, TNewValue> map)
        {
            return key =>
            {

                if (dict.TryGetValue(key, out TValue value))
                {
                    return map(key, value).AsOption_SAFE();
                }
                else
                {
                    return None.Default;
                }
            };
        }
    }

}

