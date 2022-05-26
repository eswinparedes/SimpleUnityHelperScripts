using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace SUHScripts
{
    public static class CustomObservables
    {
        public static IObservable<T> ObserveNothing<T>()
        {
            return Observable.Create<T>(observer =>
            {
                return Disposable.Create(() => { });
            });
        }

        public static IObservable<bool> Latch(IObservable<Unit> tick, IObservable<Unit> latchTrue, bool initialValue)
        {
            return Observable.Create<bool>(observer =>
            {
                var value = initialValue;

                var latchSub = latchTrue.Subscribe(_ => value = true);

                var tickSub = tick.Subscribe(_ =>
                {
                    observer.OnNext(value);
                    value = false;
                },
                observer.OnError,
                observer.OnCompleted);

                return Disposable.Create(() =>
                {
                    latchSub.Dispose();
                    tickSub.Dispose();
                });
            });
        }

        public static IObservable<T> SelectRandom<T>(this IObservable<Unit> eventObs, T[] items)
        {
            var n = items.Length;
            if(n == 0)
            {
                return Observable.Empty<T>();
            }
            else if(n == 1)
            {
                return eventObs.Select(_ => items[0]);
            }

            var myItems = (T[])items.Clone();

            return Observable.Create<T>(observer =>
            {
                var sub = eventObs.Subscribe(_ =>
                {
                    var i = UnityEngine.Random.Range(1, n);
                    var value = myItems[i];

                    var temp = myItems[0];
                    myItems[0] = value;
                    myItems[i] = temp;

                    observer.OnNext(value);
                },
                observer.OnError,
                observer.OnCompleted);

                return Disposable.Create(() => sub.Dispose());
            });
        }
    }
}
