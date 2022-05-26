using System;
using System.Collections.Generic;

namespace SUHScripts
{
    public class StateMachine
    {
        public IState CurrentState { get; private set; }

        private Dictionary<IState, List<Transition>> _transitions = new Dictionary<IState, List<Transition>>();
        private List<Transition> _currentTransitions = new List<Transition>();
        private List<Transition> _anyTransitions = new List<Transition>();

        private static List<Transition> EmptyTransitions = new List<Transition>(0);

        public void Tick(float deltaTime)
        {
            if (CurrentState == null) return;

            var transition = GetTransition();

            if (transition != null)
                SetState(transition.To);
            else
                CurrentState?.Tick(deltaTime);
        }

        public void SetState(IState state)
        {
            if (state == CurrentState)
                return;

            CurrentState?.OnExit();
            CurrentState = state;

            if (CurrentState == null) return;

            _transitions.TryGetValue(CurrentState, out _currentTransitions);

            if (_currentTransitions == null)
                _currentTransitions = EmptyTransitions;

            CurrentState.OnEnter();
        }

        public void AddTransition(Func<bool> predicate, IState to, IState from)
        {
            if (_transitions.TryGetValue(from, out var transitions) == false)
            {
                transitions = new List<Transition>();
                _transitions[from] = transitions;
            }

            transitions.Add(new Transition(to, predicate));
        }

        public void AddAnyTransition(Func<bool> predicate, IState to)
        {
            _anyTransitions.Add(new Transition(to, predicate));
        }

        private class Transition
        {
            public Func<bool> Condition { get; }
            public IState To { get; }

            public Transition(IState to, Func<bool> condition)
            {
                To = to;
                Condition = condition;
            }
        }

        private Transition GetTransition()
        {
            foreach (var transition in _anyTransitions)
                if (transition.Condition())
                    return transition;

            foreach (var transition in _currentTransitions)
                if (transition.Condition())
                    return transition;

            return null;
        }
    }
}
