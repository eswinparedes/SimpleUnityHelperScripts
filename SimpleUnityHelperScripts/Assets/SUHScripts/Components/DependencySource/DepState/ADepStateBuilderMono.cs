using SUHScripts;
using SUHScripts.Functional;
using UnityEngine;

namespace SUHScripts
{
    public abstract class ADepStateBuilderMono : MonoBehaviour, IDepStateBuilder
    {
        protected bool m_disallowMultipleBuilds = true;

        protected abstract Option<IState> OnBuild(IDependencySource deps);

        IDependencySource m_source = default;

        public Option<IState> Build(IDependencySource deps)
        {
            if (m_source != null && m_disallowMultipleBuilds)
            {
                throw new System.Exception($"Mono Dep State Builder on {gameObject.name} cannot build more than once");
            }

            return OnBuild(deps);
        }
    }
}

