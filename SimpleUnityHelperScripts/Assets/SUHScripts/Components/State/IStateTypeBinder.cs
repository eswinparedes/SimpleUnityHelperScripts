using SUHScripts;
using System;

public interface IStateTypeBinder
{
    IDisposable BindState<T>(IState state);
}