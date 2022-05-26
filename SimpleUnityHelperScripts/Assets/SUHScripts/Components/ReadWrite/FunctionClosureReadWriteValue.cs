using System;

namespace SUHScripts
{

    public class FunctionClosureReadWriteValue<T> : IReadValue<T>, IWriteValue<T>
    {
        Action<T> m_writeAction;
        Func<T> m_readFunction;

        public FunctionClosureReadWriteValue(Func<T> readFunction, Action<T> writeAction)
        {
            m_writeAction = writeAction;
            m_readFunction = readFunction;
        }

        public T Read()
        {
            return m_readFunction();
        }

        public void Write(T value)
        {
            m_writeAction(value);
        }
    }
}
