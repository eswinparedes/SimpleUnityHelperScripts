using System;
using static SUHScripts.Functional.Functional;
using SUHScripts.Functional.UnitType;

namespace SUHScripts.Functional.Composition
{
    public static class DelegateExtensions
    {
        public static Func<Unit> ToFunc(this Action action)
          => () => { action(); return UNIT; };

        public static Func<T, Unit> ToFunc<T>(this Action<T> action)
            => t => { action(t); return UNIT; };

        public static Func<T1, T2, Unit> ToFunc<T1, T2>(this Action<T1, T2> action)
            => (T1 t1, T2 t2) => { action(t1, t2); return UNIT; };


    }
}