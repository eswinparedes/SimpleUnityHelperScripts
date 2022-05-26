using System;
using System.Collections.Generic;

public class RegistryHashset<T> : IRegistry<T>
{
    public HashSet<T> HashSet { get; }  = new HashSet<T>();

    public bool Contains(T register)
    {
        return HashSet.Contains(register);
    }

    public IDisposable Register(T register)
    {
        HashSet.Add(register);
        return InterfaceUtils.Disposable(() => HashSet.Remove(register));
    }
}
