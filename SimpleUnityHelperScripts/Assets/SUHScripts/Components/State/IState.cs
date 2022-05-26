using SUHScripts.Functional;

namespace SUHScripts
{
    public interface IState<T> : IState
    {
        public T StateData { get; }
    }

    public interface IState
    {
        void OnEnter();
        void OnExit();
        void Tick(float deltaTime);
    }
}
