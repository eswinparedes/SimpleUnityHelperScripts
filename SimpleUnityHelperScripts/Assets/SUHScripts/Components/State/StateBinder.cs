using SUHScripts;
using System;
using System.Collections.Generic;

namespace SUHScripts
{
    public class StateBinder<T> : IStateBinder<T>
    {
        Func<T, (IRegistry<IState> registry, IState aggregateState)> m_registryFunc;
        Dictionary<T, (IRegistry<IState> registry, IState aggregateState)> m_stateMap = new Dictionary<T, (IRegistry<IState> registry, IState aggregateState)>();
        HashSet<T> m_closedRegistries = new HashSet<T>();
        IDisposable empty = InterfaceUtils.Disposable(() => { });

        public StateBinder(Func<T, (IRegistry<IState> registry, IState aggregateState)> registryFunc, params T[] initialKeys)
        {
            m_registryFunc = registryFunc;

            for (int i = 0; i < initialKeys.Length; i++)
            {
                m_stateMap.Add(initialKeys[i], m_registryFunc(initialKeys[i]));
            }
        }


        public void SetRegistryOpen(T key, bool isOpen)
        {
            if (isOpen)
            {
                m_closedRegistries.Remove(key);
            }
            {
                m_closedRegistries.Add(key);
            }
        }

        public void OpenAllRegistries()
        {
            m_closedRegistries.Clear();
        }

        public void CloseAllRegistries()
        {
            foreach (var key in m_stateMap.Keys)
            {
                m_closedRegistries.Add(key);
            }
        }


        public IDisposable BindState(T key, IState state)
        {
            if (m_closedRegistries.Contains(key))
            {
                UnityEngine.Debug.LogError($"{key} is locked, may not register this state");
                return empty;
            }

            if (!m_stateMap.ContainsKey(key))
            {
                m_stateMap.Add(key, m_registryFunc(key));
            }

            return m_stateMap[key].registry.Register(state);
        }

        public IState Get(T key)
        {
            if (!m_stateMap.ContainsKey(key))
            {
                m_stateMap.Add(key, m_registryFunc(key));
            }

            return m_stateMap[key].aggregateState;
        }
    }
}