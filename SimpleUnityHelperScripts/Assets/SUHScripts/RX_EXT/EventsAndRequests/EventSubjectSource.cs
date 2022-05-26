using System;
using SUHScripts;

public class EventSubjectSource<TEvent> : IEventSubject<TEvent>
{
    Action<TEvent> m_onRaise;

    public EventSubjectSource(Action<TEvent> onRaise)
    {
        m_onRaise = onRaise;
    }

    public void Raise(TEvent tEvent) => m_onRaise(tEvent);
}

public class EventSubjectSource<TEvent0, TEvent1> : IEventSubject<TEvent0, TEvent1>
{
    Action<TEvent0, TEvent1> m_onRaise;

    public EventSubjectSource(Action<TEvent0, TEvent1> onRaise)
    {
        m_onRaise = onRaise;
    }

    public void Raise(TEvent0 tEvent0, TEvent1 tEvent1) => m_onRaise(tEvent0, tEvent1);
}