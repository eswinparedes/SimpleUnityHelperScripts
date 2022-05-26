using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using SUHScripts.Functional;
using static SUHScripts.Functional.Functional;

namespace SUHScripts
{
    public static class OBV_CommandChain 
    {
        public static IObservable<Option<R>> CommandChain<T, R>(this IObservable<T> source, Func<T, Option<R>> onFail, params Func<T, Option<R>>[] commands) =>
            Observable.Create<Option<R>>(observer =>
            {
                var sub =
                source.Subscribe(
                    onNext: t =>
                    {
                        for (int i = 0; i < commands.Length; i++)
                        {
                            var r = commands[i](t);

                            if (r.IsSome)
                            {
                                observer.OnNext(r.Value.AsOption_SAFE());
                                return;
                            }
                        }

                        observer.OnNext(onFail(t));
                    },
                    onCompleted: () => observer.OnCompleted(),
                    onError: e => observer.OnError(e));

                return Disposable.Create(() => sub.Dispose());
            });

        public static IObservable<R> CommandChain<T, R>(this IObservable<T> source, params Func<T, Option<R>>[] commands) =>
            Observable.Create<R>(observer =>
            {
                var sub =
                source.CommandChain(t => None.Default, commands)
                .SelectSome()
                .Subscribe(observer.OnNext, observer.OnError, observer.OnCompleted);

                return Disposable.Create(() => sub.Dispose());
            });

        public static IObservable<R> CommandChain<T, R>(this IObservable<T> source, Func<T, R> onFail, params Func<T, Option<R>>[] commands) =>
            Observable.Create<R>(observer =>
            {
                var sub =
                source.CommandChain(t => onFail(t).AsOption_SAFE(), commands)
                .SelectSome()
                .Subscribe(observer.OnNext, observer.OnError, observer.OnCompleted);

                return Disposable.Create(() => sub.Dispose());
            });

    }
}
