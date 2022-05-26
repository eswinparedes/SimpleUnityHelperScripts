using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SUHScripts.Functional;
using System.Threading;

namespace SUHScripts
{

    public static class State 
    {
        public static void AddTransitionsFor(this SUHScripts.StateMachine @this, Func<bool> predicate, IState to, params IState[] from)
        {
            for (int i = 0; i < from.Length; i++)
            {
                @this.AddTransition(predicate, to, from[i]);
            }
        }
        public enum EnterExitComposeOption
        {
            Prepend,
            Append,
            Infix,
            Outfix
        }

        public static Action EmptyAction { get; } = () => { };
        public static Action<float> EmptyFloatAction { get; } = d => { };

        public static IState EmptyState { get; } = new ComposableState(() => { }, () => { }, d => { });

        public static IState Append(this IState target, IState appendState) =>
        new ComposableState(
            () => { target.OnEnter(); appendState.OnEnter(); },
            () => { target.OnExit(); appendState.OnExit(); },
            dt => { target.Tick(dt); appendState.Tick(dt); });

        public static IState Prepend(this IState target, IState prependState) =>
            new ComposableState(
                () => { prependState.OnEnter(); target.OnEnter(); },
                () => { prependState.OnExit(); target.OnExit(); },
                dt => { prependState.Tick(dt); target.Tick(dt); });

        public static IState PrependAll(this IState @this, params IState[] prependStates)
        {
            var prepends = new ComposableState(
                () =>
                {
                    for (int i = 0; i < prependStates.Length; i++)
                    {
                        prependStates[i].OnEnter();
                    }

                    @this.OnEnter();
                },
                () =>
                {
                    for (int i = 0; i < prependStates.Length; i++)
                    {
                        prependStates[i].OnExit();
                    }

                    @this.OnExit();
                },
                dt =>
                {
                    for (int i = 0; i < prependStates.Length; i++)
                    {
                        prependStates[i].Tick(dt);
                    }

                    @this.Tick(dt);
                });

            return prepends;
        }

        public static IState Append(this IState @this, Action onEnter = null, Action onExit = null, Action<float> tick = null) =>
            new ComposableState(
                onEnter == null ? (Action)@this.OnEnter : () => { @this.OnEnter(); onEnter(); },
                onExit == null ? (Action)@this.OnExit : () => { @this.OnExit(); onExit(); },
                tick == null ? (Action<float>)@this.Tick : dt => { @this.Tick(dt); tick(dt); });

        public static IState Logged(this IState @this, string prepend, bool tick = false) =>
            @this.Append(
                () => Debug.Log($"{prepend}: On Enter"),
                () => Debug.Log($"{prepend}: On Exit"),
                tick ? dt => Debug.Log($"{prepend}: Tick") : EmptyFloatAction);

        public static IState AppendAll(this IState @this, params IState[] appendStates)
        {

            var appends = new ComposableState(
                () =>
                {
                    for (int i = 0; i < appendStates.Length; i++)
                    {
                        appendStates[i].OnEnter();
                    }
                },
                () =>
                {
                    for (int i = 0; i < appendStates.Length; i++)
                    {
                        appendStates[i].OnExit();
                    }
                },
                dt =>
                {
                    for (int i = 0; i < appendStates.Length; i++)
                    {
                        appendStates[i].Tick(dt);
                    }
                });

            return @this.Append(appends);
        }

        public static IState AppendAll(params IState[][] appendStates)
        {
            return new ComposableState(
                () =>
                {
                    for (int i = 0; i < appendStates.Length; i++)
                    {
                        
                    }
                },
                () =>
                {
                    for (int i = 0; i < appendStates.Length; i++)
                    {
                        for (int j = 0; j < appendStates[i].Length; j++)
                        {
                            appendStates[i][j].OnExit();
                        }
                    }
                },
                dt =>
                {
                    for (int i = 0; i < appendStates.Length; i++)
                    {
                        for (int j = 0; j < appendStates[i].Length; j++)
                        {
                            appendStates[i][j].Tick(dt);
                        }
                    }
                });
        }

        public static IState AppendAll(params IState[] appendStates)
        {
            return new ComposableState(
                () =>
                {
                    for (int i = 0; i < appendStates.Length; i++)
                    {
                        appendStates[i].OnEnter();
                    }
                },
                () =>
                {
                    for (int i = 0; i < appendStates.Length; i++)
                    {
                        appendStates[i].OnExit();
                    }
                },
                dt =>
                {
                    for (int i = 0; i < appendStates.Length; i++)
                    {
                        appendStates[i].Tick(dt);
                    }
                });
        }

        public static IState AppendAll(IReadOnlyList<IState> appendStates)
        {
            return new ComposableState(
                () =>
                {
                    for (int i = 0; i < appendStates.Count; i++)
                    {
                        appendStates[i].OnEnter();
                    }
                },
                () =>
                {
                    for (int i = 0; i < appendStates.Count; i++)
                    {
                        appendStates[i].OnExit();
                    }
                },
                dt =>
                {
                    for (int i = 0; i < appendStates.Count; i++)
                    {
                        appendStates[i].Tick(dt);
                    }
                });
        }

        public static IState EnterExitCompose(this IState @this, EnterExitComposeOption composition, Action onEnter = null, Action onExit = null)
        {
            switch (composition)
            {
                case EnterExitComposeOption.Prepend: return EnterExitPrepend(@this, onEnter, onExit);
                case EnterExitComposeOption.Append: return EnterExitAppend(@this, onEnter, onExit);
                case EnterExitComposeOption.Infix: return EnterExitInfix(@this, onEnter, onExit);
                case EnterExitComposeOption.Outfix: return EnterExitOutfix(@this, onEnter, onExit);
                default: return EnterExitCompose(@this, EnterExitComposeOption.Prepend, onEnter, onExit);
            }
        }

        public static IState EnterExitPrepend(this IState @this, Action onEnter = null, Action onExit = null)
        {
            Action enter = onEnter == null ? (Action)@this.OnEnter :
                () =>
                {
                    onEnter();
                    @this.OnEnter();
                };

            Action exit = onExit == null ? (Action)@this.OnExit :
                () =>
                {
                    onExit();
                    @this.OnExit();
                };

            return new ComposableState(enter, exit, @this.Tick);
        }

        public static IState EnterExitAppend(this IState @this, Action onEnter, Action onExit)
        {
            Action enter = onEnter == null ? (Action)@this.OnEnter :
                () =>
                {
                    @this.OnEnter();
                    onEnter();
                };

            Action exit = onExit == null ? (Action)@this.OnExit :
                () =>
                {
                    @this.OnExit();
                    onExit();
                };

            return new ComposableState(enter, exit, @this.Tick);
        }

        public static IState EnterExitOutfix(this IState @this, Action onEnter, Action onExit)
        {
            Action enter = onEnter == null ? (Action)@this.OnEnter :
                () =>
                {
                    onEnter();
                    @this.OnEnter();
                };

            Action exit = onExit == null ? (Action)@this.OnExit :
                () =>
                {
                    @this.OnExit();
                    onExit();
                };

            return new ComposableState(enter, exit, @this.Tick);
        }

        public static IState EnterExitInfix(this IState @this, Action onEnter, Action onExit)
        {
            Action enter = onEnter == null ? (Action)@this.OnEnter :
                () =>
                {
                    @this.OnEnter();
                    onEnter();
                };

            Action exit = onExit == null ? (Action)@this.OnExit :
                () =>
                {
                    onExit();
                    @this.OnExit();
                };

            return new ComposableState(enter, exit, @this.Tick);
        }

        public static IState ToState(this StateMachine @this, IState defaultState)
        {
            Action enter = () => @this.SetState(defaultState);
            Action exit = () => @this.SetState(null);
            Action<float> tick = t => @this.Tick(t);
            return new ComposableState(enter, exit, tick);
        }
    }

}
