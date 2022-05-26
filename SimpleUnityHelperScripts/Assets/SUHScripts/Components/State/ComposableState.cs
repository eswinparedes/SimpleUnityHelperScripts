using System;

namespace SUHScripts
{
    public class ComposableState<T> : ComposableState, IState<T>
    {
        public ComposableState(T stateData, Action onEnter = null, Action onExit = null, Action<float> tick = null) : base(onEnter, onExit, tick)
        {
            StateData = stateData;
        }

        public T StateData { get;  }
    }

    public class ComposableStateDataFunc<T> : ComposableState, IState<T>
    {
        public ComposableStateDataFunc(Func<T> dataFunc, Action onEnter = null, Action onExit = null, Action<float> tick = null) : base(onEnter, onExit, tick)
        {
            m_dataFunc = dataFunc;
        }

        Func<T> m_dataFunc;
        public T StateData => m_dataFunc();
    }

    public class ComposableStateDataLink<T> : IState<T>
    {
        public ComposableStateDataLink(IState state, T stateData)
        {
            m_state = state;
            StateData = stateData;
        }

        IState m_state;

        public T StateData { get; set; }

        public void OnEnter()
        {
            m_state.OnEnter();
        }

        public void OnExit()
        {
            m_state.OnExit();
        }

        public void Tick(float deltaTime)
        {
            m_state.Tick(deltaTime);
        }
    }

    public class ComposableState : IState
    {
        Action m_onEnter;
        Action m_onExit;
        Action<float> m_tick;

        public ComposableState(Action onEnter = null, Action onExit = null, Action<float> tick = null)
        {
            m_onEnter = onEnter ?? State.EmptyAction;
            m_onExit = onExit ?? State.EmptyAction;
            m_tick = tick ?? State.EmptyFloatAction;
        }

        public void OnEnter() => m_onEnter();
        public void OnExit() => m_onExit();
        public void Tick(float deltaTime) => m_tick(deltaTime);
    }
}
