using SUHScripts.Functional;
using System;

namespace SUHScripts
{
    public interface IDependencySourceActionBuilder
    {
        public Option<Action> Build(IDependencySource deps);
    }

}
