using System;
using System.Collections.Generic;

namespace SUHScripts
{
    public class RegistryList<T> : IRegistry<T>
    {
        public Action<T> OnAdd;
        public Action<T> OnRemove;
        public List<T> List { get; } = new List<T>();
        public IDisposable Register(T register)
        {
            List.Add(register);
            OnAdd?.Invoke(register);

            return InterfaceUtils.Disposable(
                () =>
                {
                    if (List.Contains(register))
                    {
                        List.Remove(register);
                        OnRemove?.Invoke(register);
                    }
                });
        }

        public bool Contains(T register)
        {
            return List.Contains(register);
        }
    }

}