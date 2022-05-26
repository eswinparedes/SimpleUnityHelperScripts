using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using SUHScripts;

public static class InterfaceUtils
{
    public static IDisposable Disposable(Action disposal) =>
        new _Disposable(disposal);

    public class _Disposable : IDisposable
    {
        public _Disposable(Action disposal)
        {
            if(disposal == null)
            {
                throw new NullReferenceException();
            }

            m_disposalAction = disposal;
        }
        Action m_disposalAction;

        public void Dispose()
        {
            if(m_disposalAction != null)
            {
                m_disposalAction();
                m_disposalAction = null;
            }
        }
    }

    public static IComparer<T> Comparer<T>(Func<T, T, int> comparisonFunction) =>
        new _Comparer<T>(comparisonFunction);
    
    class _Comparer<T> : IComparer<T>
    {
        Func<T, T, int> m_compare;

        public _Comparer(Func<T, T, int> compare)
        {
            m_compare = compare;
        }

        public int Compare(T x, T y)
        {
            return m_compare(x, y);
        }
    }

}
