using System;
using SUHScripts.Functional;
using System.Collections.Generic;
using System.Linq;

namespace SUHScripts
{
    /// <summary>
    /// Given an array of DependencySources, any request for a dependency will start from the first element to the last, returning the first requested object
    /// </summary>
    public class FirstInFirstQueryDependencySource : IDependencySource
    {
        IDependencySource[] m_dependencySources;

        public IReadOnlyDictionary<Type, object> DependencyMap { get; }
        public FirstInFirstQueryDependencySource(params IDependencySource[] dependencySources)
        {
            m_dependencySources = dependencySources;

            DependencyMap =
                m_dependencySources
                .SelectMany(source => source.DependencyMap)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public Option<T> Get<T>()
        {
            for (int i = 0; i < m_dependencySources.Length; i++)
            {
                var depOpt = m_dependencySources[i].Get<T>();
                if (depOpt.IsSome) return depOpt;
            }

            return None.Default;
        }
    }
}