
using SUHScripts.Functional;

namespace SUHScripts
{

    public class SimpleReadWriteValue<T> : IReadValue<T>, IWriteValue<T>
    {
        public SimpleReadWriteValue(T value)
        {
            Value = value;
        }

        public T Value { get; private set; }

        public T Read()
        {
            return Value;
        }

        public void Write(T value)
        {
            Value = value;
        }
    }

}