using UnityEngine;

namespace SUHScripts
{
    public class DependencySourceSubjectInjectorMono : ADependencySourceMono
    {
        [SerializeField] GameObject[] m_gameObjectsWithDependencySourceSubjectInjectors = default;

        protected override IDependencySource GetSource()
        {
            return DependencySourceSubjectInjector
                .InjectDependenciesIntoSourceSubjectFromGameObjects(m_gameObjectsWithDependencySourceSubjectInjectors);
        }
    }
}
