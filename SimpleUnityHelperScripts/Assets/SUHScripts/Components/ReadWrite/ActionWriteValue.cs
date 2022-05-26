using System;

public class ActionWriteValue<T> : IWriteValue<T>
{
    Action<T> m_writeAction;

    public ActionWriteValue(Action<T> writeAction)
    {
        m_writeAction = writeAction;
    }

    public void Write(T value)
    {
        m_writeAction(value);
    }
}
