using UnityEngine;

namespace SUHScripts
{
    public class DependencySourcePointer : ADependencySourceMono
    {
        [SerializeField] ADependencySourceMono m_sourceDependency = default;
        protected override IDependencySource GetSource()
        {
            return m_sourceDependency.GetDependencySource();
        }
    }

}
