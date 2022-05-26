using System;
using System.Collections.Generic;
using UnityEngine;
using static SUHScripts.Functional.Functional;
using SUHScripts.Functional.UnitType;
using SUHScripts.Functional.Composition;

namespace SUHScripts.Functional
{
    public static class OptionExtensions
    {
        //SUHS TODO: Performance updates!!
        #region CORE
        public static Option<R> Map<T, R>(this Option<T> optT, Func<T, R> f) =>
            optT.IsSome ? f(optT.Value).AsOption_SAFE() : None.Default;

        public static Option<R> Bind<T, R>(this Option<T> optT, Func<T, Option<R>> f) =>
            optT.IsSome ? f(optT.Value) : None.Default;

        public static Option<T> Lookup<K, T>(this IDictionary<K, T> dict, K key)
        {
            return dict.TryGetValue(key, out T value) ?
                (Option<T>)Option.SAFE(value) : None.Default;

        }

        public static Option<T> Lookup<T>(this List<T> lst, T val)
        {
            bool contains = lst.Contains(val);
            return contains? Option.SAFE(lst[lst.IndexOf(val)]) : None.Default;
        }

        public static Option<T> WhereLift<T>(this T @this, bool pred) =>
            pred ? @this.AsOption_SAFE() : None.Default;

        public static Option<T> WhereLift<T>(this T @this, Func<T, bool> pred) =>
            @this.WhereLift(pred(@this));

        public static Option<T> Where<T>(this Option<T> optT, bool pred) =>
            optT.IsSome ? pred ? optT.Value.AsOption_SAFE() : None.Default : None.Default;

        public static Option<T> Where<T>(this Option<T> optT, Func<T, bool> pred) =>
            optT.IsSome ? pred(optT.Value) ? optT.Value.AsOption_SAFE() : None.Default : None.Default;

        public static T Reduce<T>(this Option<T> optT, Func<T> onNone) =>
            optT.IsSome ? optT.Value : onNone();

        public static Option<T> AsOption_SAFE<T>(this T @this)
        {
            return
                @this.IsNull() ? None.Default : Option.SAFE(@this);
        }

        public static Option<T> AsOption_UNSAFE<T>(this T @this)
        {
            return Option.UNSAFE(@this);
        }
        #endregion

        #region Action Help
        public static Option<Unit> ForEach<T>(this Option<T> opt, Action<T> action) =>
            Map(opt, action.ToFunc());

        public static Option<T> ForEachPass<T>(this Option<T> opt, Action<T> action) =>
            opt.Map(t => { action(t); return t; });

        public static Option<T> ForEachPass<T>(this Option<T> opt, Action action)
        {
            if (opt.IsSome)
                action();

            return opt;
        }


        public static Option<T> ForNonePass<T>(this Option<T> opt, Action onNone)
        {
            if (!opt.IsSome) onNone();
            return opt;
        }


        public static Option<T> DebugLogged<T>(this Option<T> @this, string message = "")
        {
            if (message == null)
                Debug.Log(@this);
            else
                Debug.Log(message + @this);

            return @this;
        }
            

        public static Option<T> SomeLogged<T>(this Option<T> @this, string message = "") =>
            @this.ForEachPass((x) => @this.DebugLogged(message));

        public static Option<T> NoneLogged<T>(this Option<T> @this, string message = "") =>
            @this.ForNonePass(() => @this.DebugLogged(message));

        public static Unit Match<T>(this Option<T> @this, Action onNone, Action<T> onSome) =>
            @this.Match(onNone.ToFunc(), onSome.ToFunc());
        #endregion


        #region Option Comparators
        public static Option<T> WhenNone<T>(this Option<T> @this, Option<T> whenNone) =>
            @this
            .Match(
                () => whenNone,
                opt => opt.AsOption_SAFE());

        public static Option<T> WhenNone<T>(this Option<T> @this, Func<Option<T>> whenNone) =>
            @this.WhenNone(whenNone());

        public static Option<T> WhenNone<T>(this Option<T> @this, T whenNone) =>
            @this
            .Match(
                () => whenNone.AsOption_SAFE(),
                opt => opt.AsOption_SAFE());

        public static Option<T> WhenNone<T>(this Option<T> @this, Func<T> whenNone) =>
            @this.WhenNone(whenNone());
        
        public static Option<R> MergedWith<T0, T1, R>(this Option<T0> firstOption, Option<T1> secondOption,
            Func<T0, T1, Option<R>> forBoth = null, Func<T0, Option<R>> forFirst = null, Func<T1, Option<R>> forSecond = null, Func<Option<R>> forNone = null) =>
            firstOption.Match(
                () => secondOption.Match(
                    () => forNone == null ? (Option<R>)None.Default : forNone(),
                    other => forSecond == null ? (Option<R>)None.Default : forSecond(other)),
                self => secondOption.Match(
                    () => forFirst == null ? (Option<R>)None.Default : forFirst(self),
                    other => forBoth == null ? (Option<R>)None.Default : forBoth(self, other)));

        public static R MergedWith<T0, T1, R>(this Option<T0> firstOption, Option<T1> secondOption,
            Func<T0, T1, R> forBoth, Func<T0, R> forFirst, Func<T1, R> forSecond, Func<R> forNone) =>
            firstOption.Match(
                () => secondOption.Match(
                    () =>  forNone(),
                    other =>  forSecond(other)),
                self => secondOption.Match(
                    () => forFirst(self),
                    other => forBoth(self, other)));


        public static Option<T> OtherIfUniqueOrNone<T>(this Option<T> @this, T otherValue) =>
            @this.Match(
                () => otherValue.AsOption_SAFE(),
                t => DoCompareEqual(@this, otherValue.AsOption_SAFE()) ? None.Default : otherValue.AsOption_SAFE());

        public static Option<T> OtherIfUniqueOrNone<T>(this Option<T> @this, Func<T> otherValue) =>
            @this.OtherIfUniqueOrNone(otherValue());

        public static Option<T> OtherIfUniqueOrNone<T>(this Option<T> selfOption, Option<T> otherOption) =>
            selfOption.MergedWith(otherOption,
                (self, other) => selfOption.OtherIfUniqueOrNone(other),
                self => None.Default,
                other => other.AsOption_SAFE(),
                () => None.Default);

        public static Option<T> OtherIfTheSameOrNone<T>(this Option<T> @this, T otherValue) =>
            @this.Match(
                () => None.Default,
                opt => DoCompareEqual(opt, otherValue) ? otherValue.AsOption_SAFE() : None.Default);

        public static Option<T> OtherIfTheSameOrNone<T>(this Option<T> @this, Func<T> otherValue) =>
            @this.OtherIfTheSameOrNone(otherValue());

        public static Option<T> OtherIfTheSameOrNone<T>(this Option<T> selfOption, Option<T> otherOption) =>
            selfOption.MergedWith<T, T, T>(otherOption, (self, other) => DoCompareEqual(self, other) ? other.AsOption_SAFE() : None.Default);


        public static Option<T> Invert<T>(this Option<T> @this, T value) =>
            @this.Match(
                () => (Option<T>) value,
                v => None.Default);

        public static Option<T> Invert<T>(this Option<T> @this, Func<T> f) =>
            @this.Invert(f());

        public static bool ComparesTo<T>(this Option<T> @this, Option<T> otherOption, bool considerNONE = false)
        {
            if (@this.IsSome)
            {
                if (otherOption.IsSome)
                {
                    return @this.Value.Equals(otherOption.Value);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (otherOption.IsSome)
                {
                    return false;
                }
                else
                {
                    return considerNONE;
                }
            }
        }

        public static bool ComparesTo<T>(this Option<T> @this, T other, bool considerNull = false) =>
            @this
            .ComparesTo(other.AsOption_SAFE(), considerNull);
        #endregion

        public static bool DoCompareEqual<T>(T a, T b) =>
            object.Equals(a, b);

        //TODO: Refactor to make it use Funcs? Though this is not necesssary because it returns only options
        /// <summary>
        /// Returns other option whenever "toggle on" is true and executes onToggleOff on self if it is some and onToggleOn on other if some
        /// returns NONE if toggle off is true and is some
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="other"></param>
        /// <param name="toggleOn"></param>
        /// <param name="toggleOff"></param>
        /// <param name="onToggleOn"></param>
        /// <param name="onToggleOff"></param>
        /// <returns></returns>
        public static Option<T> ForToggleUpdate<T>(this Option<T> @this, Option<T> other, bool toggleOn, bool toggleOff, Action<T> onToggleOn = null, Action<T> onToggleOff = null)
        {
            if(toggleOn)
            {
                other.ForEach(onToggleOn);
                @this.ForEach(onToggleOff);
                return other;
            }
            else if (toggleOff)
            {
                @this.ForEach(onToggleOff);
                return None.Default;
            }

            return @this;
        }

        //Coulod possibly reuse other compare functions like GetOtherIfTheSameOrNone
        /// <summary>
        /// Returns { @this } if { valueFunction } returns an option that compares as equal or the new option if it compares as different.
        /// Executes onValue Enter the frame in which the { valueFunction } returns an option that is different to the input option and
        /// on exit on the input option the frame it is no longer returned
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="other"></param>
        /// <param name="onValueEnter"></param>
        /// <param name="onValueExit"></param>
        /// <returns></returns>
        public static Option<T> ForNewEntryUpdate<T>(this Option<T> @this, Option<T> other, Action<T> onValueEnter = null, Action<T> onValueExit = null)
        {
            var current = @this;

            if (!other.IsSome && current.IsSome)
            {
                current.ForEach(onValueExit);
                current = None.Default;
            }

            var newOption =
                other
                .Where(found => !current.ComparesTo(found))
                .ForEachPass(newVal =>
                {
                    current.ForEach(onValueExit);
                    current = newVal.AsOption_SAFE();
                    current.ForEach(onValueEnter);
                });

            return current;
        }
    }
}

