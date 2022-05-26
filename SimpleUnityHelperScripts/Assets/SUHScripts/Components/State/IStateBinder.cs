using SUHScripts;
using System;

public interface IStateBinder<T>
{
    IDisposable BindState(T key, IState state);
}