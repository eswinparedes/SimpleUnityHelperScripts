using System;
using SUHScripts.Functional;

namespace SUHScripts
{
    public class FunctionOptionReadWriteValue<T> : IReadValue<T>, IWriteValue<T>
    {
        Func<T, Option<T>> m_func;

        public T Value { get; set; }
        public T Read() => Value;
        public void Write(T value)
        {
            var opt = m_func(value);
            if (opt.IsSome)
            {
                Value = opt.Value;
            }
        }
    }
}
