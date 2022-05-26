using SUHScripts.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SUHScripts
{
    public class CompositeDependencySourceMono : ADependencySourceMono
    {
        [Header("Dependency Targets")]
        [SerializeField] GameObject[] m_targets = default;

        protected override IDependencySource GetSource()
        {
            var dependencies =
                m_targets
                .SelectMany(target => target.GetComponents<ADependencySourceMono>())
                .Where(source => source != this && source != null)
                .Select(source => source.GetDependencySource())
                .ToArray();


            var depSubject = new DependencySourceSubject();
            depSubject.InjectDependencies(dependencies);
            return depSubject;
        }
    }

}
