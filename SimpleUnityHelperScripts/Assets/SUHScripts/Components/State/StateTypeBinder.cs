using SUHScripts;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SUHScripts
{

    public class StateTypeBinder : IStateTypeBinder
    {
        Func<Type, (IRegistry<IState> registry, IState aggregateState)> m_registryFunc;
        Dictionary<Type, (IRegistry<IState> registry, IState aggregateState)> m_stateMap = new Dictionary<Type, (IRegistry<IState> registry, IState aggregateState)>();
        HashSet<Type> m_closedRegistries = new HashSet<Type>();
        IDisposable empty = InterfaceUtils.Disposable(() => { });

        public void SetRegistryOpen<T>(bool isOpen)
        {
            var t = typeof(T);

            if (isOpen)
            {
                m_closedRegistries.Remove(t);
            }
            {
                m_closedRegistries.Add(t);
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

        public StateTypeBinder(Func<Type, (IRegistry<IState> registry, IState aggregateState)> registryFunc, params Type[] initialTypes)
        {
            m_registryFunc = registryFunc;

            for (int i = 0; i < initialTypes.Length; i++)
            {
                m_stateMap.Add(initialTypes[i], m_registryFunc(initialTypes[i]));
            }
        }

        public IDisposable BindState<T>(IState state)
        {
            var t = typeof(T);

            if (m_closedRegistries.Contains(t))
            {
                UnityEngine.Debug.LogError($"{t.Name} is locked, may not register this state");
                return empty;
            }

            if (!m_stateMap.ContainsKey(t))
            {
                m_stateMap.Add(t, m_registryFunc(t));
            }

            return m_stateMap[t].registry.Register(state);
        }

        public IState Get<T>()
        {
            var t = typeof(T);
            return m_stateMap[t].aggregateState;
        }
    }

}