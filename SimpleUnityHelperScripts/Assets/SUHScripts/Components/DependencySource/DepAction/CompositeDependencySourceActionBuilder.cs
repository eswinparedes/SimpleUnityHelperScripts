using SUHScripts;
using SUHScripts.Functional;
using System;
using System.Linq;
using UnityEngine;  

namespace SUHScripts
{
    public class CompositeDependencySourceActionBuilder : MonoBehaviour
    {
        [Header("Dep Action Targets")]
        [SerializeField] GameObject[] m_actionTargets = default;
        [SerializeField] ADepActionBuilderSO[] m_actions = default;
        [SerializeField] UnityEvent_DependencySource m_depEvent = default;

        Action m_action;

        public Option<Action> Build(IDependencySource deps)
        {
            if(m_action != null)
            {
                return Option.UNSAFE(m_action);
            }

            var actions =
                m_actions
                .Choose(a => a.Build(deps))
                .Concat(
                    m_actionTargets
                    .SelectMany(t => t.GetComponents<IDependencySourceActionBuilder>())
                    .Choose(t => t.Build(deps)))
                .Concat(
                    m_actionTargets
                    .SelectMany(t => t.GetComponents<IAction>())
                    .Select(a => (Action) a.Run))
                .ToArray();


            m_action = () =>
            {
                for (int i = 0; i < actions.Length; i++)
                {
                    actions[i]();
                }

                m_depEvent.Invoke(deps);
            };

            return Option.UNSAFE(m_action);
        }

        private void OnValidate()
        {
            if (m_actionTargets != null)
            {
                for (int i = 0; i < m_actionTargets.Length; i++)
                {
                    var comp = m_actionTargets[i].GetComponents<IDependencySourceActionBuilder>();
                    if (comp == null)
                    {
                        Debug.LogWarning($"Composite Dep State Builder on {this.gameObject.name} unable to find any Dep Action Builder on {m_actionTargets[i].name}");
                    }
                }
            }
        }
    }

}
