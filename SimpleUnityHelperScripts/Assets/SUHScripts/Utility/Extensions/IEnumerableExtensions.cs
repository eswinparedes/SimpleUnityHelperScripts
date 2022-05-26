using System;
using System.Collections.Generic;
using System.Linq;
using SUHScripts.Functional.UnitType;

namespace SUHScripts.Functional
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Executes an Action on each item returning Unit for that item- execution is deffered
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IEnumerable<Unit> ForEach<T>(this IEnumerable<T> ts, Action<T> action) =>
            ts.Select(t =>
            {
                action(t);
                return Unit.Default;
            });

        /// <summary>
        /// Executes an Action on each item, return that item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IEnumerable<T> ForEachPass<T>(this IEnumerable<T> ts, Action<T> action)
        {
            foreach (T item in ts)
            {
                action(item);
                yield return item;
            }
        }

        /// <summary>
        /// Quick method to evaluate Ienumerable items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts"></param>
        public static void Consume<T>(this IEnumerable<T> ts) =>
            ts.ToList();

        public static IEnumerable<R> Bind<T, R>(this IEnumerable<T> ts, Func<T, IEnumerable<R>> f)
        {
            foreach (T t in ts)
                foreach (R r in f(t))
                    yield return r;
        }

        public static IEnumerable<R> Bind<T, R>(this IEnumerable<T> list, Func<T, Option<R>> func) =>
            list.Bind(t => func(t).AsEnumerable());

        public static IEnumerable<R> Map<T, R>(this IEnumerable<T> ts, Func<T, R> f)
        {
            foreach (var t in ts)
                yield return f(t);
        }

        public static bool ContainsAllItemsIn<T>(this IEnumerable<T> a, IEnumerable<T> b) =>
        !b.Except(a).Any();

        public static bool ContainsAtLeastOneIn<T>(this IEnumerable<T> a, IEnumerable<T> b) =>
            a.Intersect(b).Any();

        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int N) =>
            source.Skip(Math.Max(0, source.Count() - N));

        public static IEnumerable<R> Choose<T,R>(this IEnumerable<T> @this, Func<T, Option<R>> func)
        {
            var enumerator = @this.GetEnumerator();

            while (enumerator.MoveNext())
            {
                var opt = func(enumerator.Current);
                if (opt.IsSome) yield return opt.Value;
            }
        }

        public static T RandomElementIterated<T>(this IEnumerable<T> @this)
        {
            var count = @this.Count();

            if (count == 0) 
                throw new Exception("No elements in enumerable to select!");
            
            var rand = UnityEngine.Random.Range(0, count);
            var idx = 0;

            var selected = default(T);

            foreach (var el in @this)
            {
                if (idx == rand)
                {
                    selected= el;
                }

                idx += 1;
            }

            return selected;
        }

    }
}