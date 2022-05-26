using UnityEngine;
using System;

namespace SUHScripts.Functional.Composition
{
    public static class GenericExtensions
    {
        public static Action DoNothing = () => { };

        public static Func<T> ActionToFunction<T>(this Action @this, T value) =>
            () => value.Pass(@this);

        public static T ActionToExpression<T>(this Action @this, T value) =>
            value.Pass(@this);

        public static T DebugLogged<T>(this T expr, string prefix = null)
        {
            Debug.Log((prefix ?? "Value: ") + expr.ToString());
            return expr;
        }

        public static bool IfTrue(this bool @this, Action action) =>
            @this
            .PassThrough(value =>
            {
                if (value)
                    action();
            });


        public static bool IfFalse(this bool @this, Action action) =>
            @this
            .PassThrough(value =>
            {
                if (!value)
                    action();
            });

        public static T OnCondition<T>(this bool @this, Func<T> ifTrue, Func<T> ifFalse) =>
            @this ? ifTrue() : ifFalse();


        public static Tout MapInto<Tin, Tout>(this Tin @this, Func<Tin, Tout> functor) =>
            functor(@this);

        public static T Pass<T>(this T @this, Action action)
        {
            action();
            return @this;
        }

        public static T PassThrough<T>(this T @this, Action<T> action)
        {
            action(@this);
            return @this;
        }

        public static TOut MapNullable<TObject, TOut>
                (Func<TObject> valueFunction, Func<TObject, TOut> OnNotNull, Func<TObject, TOut> OnNull)
                where TObject : class
        {
            TObject t = valueFunction();
            if (t == null)
            {
                return OnNull(t);
            }
            else
            {
                return OnNotNull(t);
            }
        }
    }
}
