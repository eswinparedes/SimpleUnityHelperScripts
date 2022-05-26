using System;
using static SUHScripts.Functional.Functional;
using System.Collections.Generic;

namespace SUHScripts.Functional
{
    public static class Option
    {
        /// <summary>
        /// Create an option from this value, return None if null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Option<T> SAFE<T>(T value) => Option<T>.ToOption(value);

        /// <summary>
        /// Create an option from this value, return Some null if value is null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Option<T> UNSAFE<T>(T value) => Option<T>.ToOptionUnsafe(value);
    }

    public struct None
    {
        public static None Default = new None();
    }

    public struct Option<T>
    {
        readonly bool isSome;
        readonly T value;

        public static readonly Option<T> _NONE = new Option<T>();

        private Option(T value)
        {
            this.isSome = value.NotNull();
            this.value = value;
        }

        private Option(T value, bool isSome)
        {
            this.isSome = isSome;
            this.value = value;
        }

        public static Option<TOut> ToOptionUnsafe<TOut>(TOut value)
        {
            return new Option<TOut>(value, true);
        }

        public static Option<TOut> ToOption<TOut>(TOut value)
        {
            return new Option<TOut>(value);
        }

        public static implicit operator Option<T>(None _) =>
            _NONE;

        public static explicit operator Option<T>(T value) =>
            new Option<T>(value, true);

        public R Match<R>(Func<R> onNone, Func<T, R> onSome) =>
            isSome ?  onSome(value) : onNone();

        public R Match<R>(R onNone, R onSome) =>
            isSome ? onSome : onNone;

        public T Reduce(T onNone) =>
            isSome ? value : onNone;

        public IEnumerable<T> AsEnumerable()
        {
            if (isSome) yield return value;
        }

        public bool IsSome => isSome;
        public T Value => value;

        public override string ToString() =>
            isSome ? value.ToString() : "NONE";
    }
}