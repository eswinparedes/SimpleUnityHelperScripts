using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SUHScripts
{
    public class HashSetObjectPool<T> : AObjectPool<T>
    {
        Func<T> m_instantiateFunction;

        Action<T> m_destroyActionOption;
        Action<T> m_onGetActionOption;
        Action<T> m_onReleaseActionOption;

        HashSet<T> m_availableToGet = new HashSet<T>();
        HashSet<T> m_waitingForRelease = new HashSet<T>();
        List<T> m_cacheList = new List<T>();

        public IEnumerable<T> WaitingForRelease => m_waitingForRelease;
        public IEnumerable<T> AvailableToGet => m_availableToGet;

        public HashSetObjectPool(Func<T> onInstantiateFunction, Action<T> onDestroyActionOption, Action<T> onGetActionOption, Action<T> onReleaseActionOption)
        {
            m_instantiateFunction = onInstantiateFunction;
            m_destroyActionOption = onDestroyActionOption;
            m_onGetActionOption = onGetActionOption;
            m_onReleaseActionOption = onReleaseActionOption;
        }

        public override T Get()
        {
            T t;
            
            if(m_availableToGet.Count == 0)
            {
                t = m_instantiateFunction();
            }
            else
            {
                t = m_availableToGet.First();
                m_availableToGet.Remove(t);
            };

            m_waitingForRelease.Add(t);
            m_onGetActionOption?.Invoke(t);
            return t;
        }

        public override bool Release(T obj)
        {
            if (!m_waitingForRelease.Remove(obj))
            {
                UnityEngine.Debug.LogWarning($"Object of type {typeof(T).Name} not managed by this Object Pool!");
                return false;
            }

            m_onReleaseActionOption?.Invoke(obj);
            m_availableToGet.Add(obj);

            return true;
        }

        public void ExtendPool(int count)
        {
            for (int i = 0; i < count; i++)
            {
                m_availableToGet.Add(m_instantiateFunction());
            }
        }

        public void Trim(int amount)
        {
            if (amount > m_availableToGet.Count)
            {
                UnityEngine.Debug.LogWarning($"Cannot trim {amount} elements from pool as there are only {m_availableToGet.Count} available! Clearing any available");
                amount = m_availableToGet.Count;
            }

            var removalCount = 1;

            foreach (var el in m_availableToGet)
            {
                if (removalCount > amount)
                {
                    break;
                }

                m_cacheList.Add(el);
                removalCount++;
            }

            if (m_destroyActionOption != null)
            {
                for (int i = 0; i < m_cacheList.Count; i++)
                {
                    m_availableToGet.Remove(m_cacheList[i]);
                    m_destroyActionOption(m_cacheList[i]);
                }
            }
            else
            {
                for (int i = 0; i < m_cacheList.Count; i++)
                {
                    m_availableToGet.Remove(m_cacheList[i]);
                }
            }

            m_cacheList.Clear();
        }

        public void Trim()
        {
            Trim(m_availableToGet.Count);
        }

        /// <summary>
        /// Will forcably despwn any objects that are spawned and then destroy them.
        /// </summary>
        /// <returns></returns>
        public bool Clear()
        {
            foreach (var el in m_waitingForRelease)
            {
                m_cacheList.Add(el);
            }

            for (int i = 0; i < m_cacheList.Count; i++)
            {
                Release(m_cacheList[i]);
            }

            var didForceRelease = m_cacheList.Count > 0;

            m_cacheList.Clear();

            Trim();

            return didForceRelease;
        }
    }

}