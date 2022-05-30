using SUHScripts;
using System;

namespace SUHScripts
{
    public interface IStateTypeBinder
    {
        IDisposable BindState<T>(IState state);
    }
}