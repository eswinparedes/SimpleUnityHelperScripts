using UnityEngine;
using System;
using System.Collections.Generic;
using SUHScripts.Functional;
using System.Linq;

namespace SUHScripts
{
    public class DependencySourceSubject : IDependencySource
    {
        Dictionary<Type, object> m_baseDependencies = new Dictionary<Type, object>();

        public bool InjectDependencies(params IDependencySource[] dependencies)
        {
            var containerType = typeof(DependencySourceTagContainer);

            var tags =
                dependencies
                .Choose(deps => deps.Get<DependencySourceTagContainer>())
                .SelectMany(container => container.Tags);

            if (m_baseDependencies.ContainsKey(containerType))
            {
                var container = (DependencySourceTagContainer)m_baseDependencies[containerType];
                tags = tags.Concat(container.Tags);
            }

            var finaltags = tags.Distinct().ToArray();

            m_baseDependencies[containerType] = new DependencySourceTagContainer(finaltags);

            var finalDependenciesToAdd =
                dependencies.SelectMany(deps => deps.DependencyMap)
                .Where(kvp => kvp.Key != containerType)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            foreach (var kvp in finalDependenciesToAdd)
            {
                if (m_baseDependencies.ContainsKey(kvp.Key))
                {
                    Debug.LogError($"Base Dependency of type {kvp.Key.Name} already registered");
                    return false;
                }
                else
                {
                    m_baseDependencies.Add(kvp.Key, kvp.Value);
                }
            }

            return true;
        }

        public IReadOnlyDictionary<Type, object> DependencyMap => m_baseDependencies;

        //public static int Count = 0;
        public Option<T> Get<T>()
        {
            var type = typeof(T);
            if (!m_baseDependencies.TryGetValue(typeof(T), out object obj))
            {
                if (DoLogWarnings)
                {
                    Debug.LogWarning($"Component of type '{type.Name}' not registered in this Depndency Source");
                }

                return None.Default;
            };

            var t = (T)obj;

            //Debug.Log($"Got dependency: {type.Name}");
            return t.AsOption_UNSAFE();
        }

        public bool Inject<T>(T dependency)
        {
            var type = typeof(T);
            if (m_baseDependencies.TryGetValue(type, out var obj))
            {
                Debug.LogError($"Base Dependency of type {type.Name} already registered");
                return false;
            }
            else
            {
                //Debug.Log($"Registering dependency of type {type.Name}");
                m_baseDependencies.Add(type, dependency);
                return true;
            }
        }

        public bool RemoveDependency<T>()
        {
            var type = typeof(T);

            if (m_baseDependencies.ContainsKey(type))
            {
                m_baseDependencies.Remove(type);
                return true;
            }
            else
            {
                if (DoLogWarnings)
                {
                    Debug.LogWarning($"Cannot remove dependency of type {type.Name} as it is not present!");
                }

                return false;
            }
        }

        public void InjectTag(DependencySourceTagSO tag)
        {
            var dstType = typeof(DependencySourceTagContainer);

            DependencySourceTagContainer container;

            if (m_baseDependencies.ContainsKey(dstType))
            {
                container = (DependencySourceTagContainer)m_baseDependencies[dstType];
            }
            else
            {
                container = new DependencySourceTagContainer();
                m_baseDependencies.Add(dstType, container);
            }

            container.AddTag(tag);
        }

        public void InjectTags(DependencySourceTagSO[] tags)
        {
            var dstType = typeof(DependencySourceTagContainer);

            DependencySourceTagContainer container;

            if (m_baseDependencies.ContainsKey(dstType))
            {
                container = (DependencySourceTagContainer)m_baseDependencies[dstType];
            }
            else
            {
                container = new DependencySourceTagContainer();
                m_baseDependencies.Add(dstType, container);
            }

            for (int i = 0; i < tags.Length; i++)
            {
                container.AddTag(tags[i]);
            }
        }

        public bool DoLogWarnings { get; set; } = false;
    }
}