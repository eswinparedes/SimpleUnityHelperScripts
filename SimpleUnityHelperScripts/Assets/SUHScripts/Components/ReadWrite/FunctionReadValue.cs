using System;

public class FunctionReadValue<T> : IReadValue<T>
{
    public FunctionReadValue(Func<T> run)
    {
        m_read = run;
    }

    Func<T> m_read;
    public T Read() => m_read();
}