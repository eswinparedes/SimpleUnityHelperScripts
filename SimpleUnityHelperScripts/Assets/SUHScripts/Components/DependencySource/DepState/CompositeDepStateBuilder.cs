using SUHScripts;
using SUHScripts.Functional;
using System.Linq;
using UnityEngine;

namespace SUHScripts
{
    public class CompositeDepStateBuilder : ADepStateBuilderMono
    {
        [Header("State Builder")]
        [SerializeField] GameObject[] m_depStateBuilderTargets = default;

        protected override Option<IState> OnBuild(IDependencySource deps)
        {
            var states =
                m_depStateBuilderTargets
                .SelectMany(t => t.GetComponents<IDepStateBuilder>())
#pragma warning disable CS0252 // Possible unintended reference comparison; left hand side needs cast
                .Where(t => t != this)
#pragma warning restore CS0252 // Possible unintended reference comparison; left hand side needs cast
                .Choose(t => t.Build(deps))
                .ToArray();


            var finalState = State.AppendAll(states);

            return Option.UNSAFE(finalState);
        }
    }
}
