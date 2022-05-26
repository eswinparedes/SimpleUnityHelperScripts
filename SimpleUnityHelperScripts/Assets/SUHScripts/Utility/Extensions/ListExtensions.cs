using System;
using System.Collections.Generic;
using UnityEngine;

namespace SUHScripts
{

    public static class ListExtensions
    {
        /// <summary>
        /// Returns True if Item was added
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        public static bool AddItemNotPresent<T>(this List<T> list, T item)
        {
            if (!list.Contains(item))
            {
                list.Add(item);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns True if Item Was Removed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool RemoveItemPresent<T>(this List<T> list, T item)
        {
            if (list.Contains(item))
            {
                list.Remove(item);
                return true;
            }
            return false;
        }

        public static T RandomElement<T>(this IReadOnlyList<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        public static IReadOnlyList<R> Select<T, R>(this IReadOnlyList<T> list, Func<T, R> selector)
        {
            var rList = new List<R>();
            for(int i = 0; i < list.Count; i++)
            {
                rList.Add(selector(list[i]));
            }

            return rList;
        }

        /// <summary>
        /// Experimental
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="other"></param>
        /// <param name="comparator"></param>
        /// <returns></returns>
        public static bool AllItemsComparesToOneOther<T>(this IReadOnlyList<T> source, IReadOnlyList<T> other, Func<T, T, bool> comparator)
        {
            if (source.Count != other.Count) return false;

            var matches = new HashSet<int>();

            for(int i =0; i < source.Count; i++)
            {
                for(int j = 0; j < other.Count; j++)
                {
                    if(!matches.Contains(j) && comparator(source[i], other[j]))
                    {
                        matches.Add(j);
                        continue;
                    }
                }
            }

            return matches.Count == source.Count;
        }
    }
}