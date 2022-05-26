using SUHScripts.Functional;
using System.Collections.Generic;
using System;

namespace SUHScripts
{
    public interface IDependencySource
    {
        Option<T> Get<T>();
        IReadOnlyDictionary<Type, object> DependencyMap { get; }
    }

}