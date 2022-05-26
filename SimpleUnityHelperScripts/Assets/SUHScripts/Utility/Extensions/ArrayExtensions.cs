using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace SUHScripts
{
    public static class ArrayExtensions
    {
        public static T[] Shuffled<T>(this T[] arr)
        {
            var newArr = new T[arr.Length];

            for (int i = 0; i < arr.Length; i++)
            {
                var temp = arr[i];
                var randomIndex = UnityEngine.Random.Range(i, arr.Length);
                newArr[i] = arr[randomIndex];
                newArr[randomIndex] = temp;
            }

            return newArr;
        }
        public static T RandomElement<T>(this T[] arr)
        {
            return arr[UnityEngine.Random.Range(0, arr.Length)];
        }

        public static R[] Select<T,R>(this T[] arr, Func<T, R> selector)
        {
            var rArr = new R[arr.Length];
            for (int i = 0; i < arr.Length; i++){
                rArr[i] = selector(arr[i]);
            }

            return rArr;
        }
    }

}
