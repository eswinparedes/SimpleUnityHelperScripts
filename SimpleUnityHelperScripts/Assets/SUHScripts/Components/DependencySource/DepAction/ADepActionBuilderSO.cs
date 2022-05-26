using SUHScripts.Functional;
using System;
using UnityEngine;

namespace SUHScripts
{
    public abstract class ADepActionBuilderSO : ScriptableObject, IDependencySourceActionBuilder
    {
        public abstract Option<Action> Build(IDependencySource deps);
    }

}
