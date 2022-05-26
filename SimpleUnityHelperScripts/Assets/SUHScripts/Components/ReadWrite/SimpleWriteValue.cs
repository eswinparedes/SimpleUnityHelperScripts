public class SimpleWriteValue<T> : IWriteValue<T>
{
    public SimpleWriteValue(T seed)
    {
        m_value = seed;
    }

    public void Write(T value)
    {
        m_value = value;
    }

    T m_value;
    public T Value => m_value;
}

