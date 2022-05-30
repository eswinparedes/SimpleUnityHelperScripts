using SUHScripts;
using System;

namespace SUHScripts
{
    public interface IStateBinder<T>
    {
        IDisposable BindState(T key, IState state);
    }
}