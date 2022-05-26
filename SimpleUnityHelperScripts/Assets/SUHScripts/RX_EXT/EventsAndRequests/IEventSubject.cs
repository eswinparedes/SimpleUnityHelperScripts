namespace SUHScripts
{
    public interface IEventSubject
    {
        void Raise();
    }

    public interface IEventSubject<TEvent>
    {
        void Raise(TEvent payLoad);
    }

    public interface IEventSubject<TEvent0, TEvent1>
    {
        void Raise(TEvent0 event0, TEvent1 event1);
    }
}