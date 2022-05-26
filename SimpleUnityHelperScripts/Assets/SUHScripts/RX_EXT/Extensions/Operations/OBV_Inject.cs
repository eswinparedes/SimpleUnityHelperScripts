using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using SUHScripts.Functional;

namespace SUHScripts
{
    public static class OBV_Inject 
    {
        public static IObservable<R> Inject<T, U, R>(this IObservable<T> @this, IObservable<U> catalyst, Func<T, U, R> injection, U startWith) =>
            Observable.Create<R>(observer =>
            {
                var disp = new List<IDisposable>();

                var u = startWith;

                var sub0 =
                    catalyst
                    .Subscribe(uIn => u = uIn)
                    .AddTo(disp);

                var sub1 =
                @this.Subscribe(
                    onNext: t => observer.OnNext(injection(t, u)),
                    onCompleted: observer.OnCompleted,
                    onError: observer.OnError)
                .AddTo(disp) ;

                return new CompositeDisposable(disp) ;
            });

        public static IObservable<R> InjectionValve<T, U, R>(this IObservable<T> @this, IObservable<U> catalyst, Func<U, Option<U>> valveSelection, Func<T, U, R> selector, Option<U> startWith) =>
            Observable.Create<R>(observer =>
            {
                var disp = new List<IDisposable>();

                var optU = startWith;

                var sub0 =
                catalyst
                .Subscribe(u => optU = valveSelection(u))
                .AddTo(disp);

                var sub1 =
                @this.Where(_ => optU.IsSome)
                .Subscribe(
                    onNext: t => observer.OnNext(selector(t, optU.Value)),
                    onError: observer.OnError,
                    onCompleted:observer.OnCompleted)
                .AddTo(disp);

                return new CompositeDisposable(disp);

            });

        
    }

}
