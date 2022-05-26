using System;

public interface IRegistry<T>
{
    IDisposable Register(T register);
    public bool Contains(T register);
}
