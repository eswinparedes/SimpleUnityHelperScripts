using System;
using UnityEngine;
using UnityEngine.Events;

namespace SUHScripts
{
    [System.Serializable]
    [DefaultExecutionOrder(-100)]
    public class DependencySourceSubjectInjectorSingleton : MonoBehaviour
    {
        [SerializeField] GameObject[] m_subjectInjectorObjects = default;

        IDisposable m_instance;

        private void Awake()
        {
            var subject = new DependencySourceSubject();
            m_instance = new StaticInstance<IDependencySource>(subject);

            DependencySourceSubjectInjector.InjectDependenciesIntoSourceSubjectFromGameObjects(subject, m_subjectInjectorObjects);
        }

        private void OnDestroy()
        {
            m_instance.Dispose();
        }
    }
}
