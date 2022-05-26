using System.Collections.Generic;
using UniRx;
using System;

namespace SUHScripts
{
    public static class DisposableExtensions 
    {
        public static Subject<T> AddTo<T>(this Subject<T> @this, List<IDisposable> addTo)
        {
            addTo.Add(@this);
            return @this;
        }

        public static IDisposable AddTo(this IDisposable @this, List<IDisposable> subList)
        {
            subList.Add(@this);
            return @this;
        }
    }
}

