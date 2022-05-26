using System;
using UniRx;
using UnityEngine;

namespace SUHScripts
{
    using Functional;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public static class OBV_Ext 
    {
        //SUHS TODO: Take GetComponent and QueryComponent fucntions from the collider observables and
        // put in here then change the colliderobservable operations to simply select (other) and use these operations
        public static IObservable<T> DoLog<T>(this IObservable<T> @this, string message) =>
            @this.Do(value => Debug.Log($"{message} : {value}"));
        public static IObservable<T> SelectSome<T>(this IObservable<Option<T>> @this) =>
            Observable.Create<T>(observer =>
            {
                return
                @this.Subscribe(
                    opt =>
                    {
                        if (opt.IsSome) observer.OnNext(opt.Value);
                    }, observer.OnError, observer.OnCompleted);
            });

        /// <summary>
        /// Returns a stream that only emits when the Option is some and flattens the value out
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="this"></param>
        /// <param name="chooser"></param>
        /// <returns></returns>
        public static IObservable<R> Choose<T, R>(this IObservable<T> @this, Func<T, Option<R>> chooser) =>
            Observable.Create<R>(observer =>
            {
                return @this.Subscribe(t =>
                {
                    var choice = chooser(t);
                    if (choice.IsSome)
                    {
                        observer.OnNext(choice.Value);
                    }
                }, observer.OnError, observer.OnCompleted);
            });
            //@this
            //.Select(chooser)
            //.SelectSome();



        /// <summary>
        /// Given a stream of values and a predicate, returns a tuple with the predicate "state" (whether the predicate just failed, has been failing or stopped failing)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IObservable<(BoolState predicateState, T value)> TrackPredicate<T>(this IObservable<T> stream, Func<T, bool> predicate)
        {
            T tSeed = default;
            (BoolState codeState, T value) seed = (new BoolState(), tSeed);

            return stream.Scan(seed, (prev, newVal) => (prev.codeState.Update(predicate(newVal)), newVal));
        }

        public static IObservable<int> MergeTriggersIntoIndex<T>(params IObservable<T>[] others)
        {
            IObservable<int>[] selected = new IObservable<int>[others.Length];

            for (int i = 0; i < others.Length; i++)
            {
                var iClosure = i;
                selected[i] = others[i].Select(_ => iClosure);
            }

            return selected.Merge();
        }

        public static IObservable<R> SwitchTo<T, R>(this IObservable<T> @this, Func<T, IObservable<R>> selector) =>
            @this.Select(selector)
            .Switch();

        /// <summary>
        /// Terminates the Observable after the first value fails the predicate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IObservable<T> TakeWhile_IncludeLast<T>(this IObservable<T> source, Func<T, bool> predicate)
        {
            return Observable.Create<T>(observer =>
            {
                return source.Subscribe(
                    onNext: val =>
                    {
                        var shouldContinue = false;
                        try
                        {
                            shouldContinue = predicate(val);
                        }
                        catch (Exception e)
                        {
                            observer.OnError(e);
                            return;
                        }

                        observer.OnNext(val);
                        if (!shouldContinue)
                        {
                            observer.OnCompleted();
                        }
                    },
                    onError: err => observer.OnError(err),
                    onCompleted: () => observer.OnCompleted());
            });
        }

        /// <summary>
        /// Collects the latest value of each Observable from "source" when "selectionStream" emits.  
        /// Then, uses "selector" to select a new value based on the latest collected "source" observable values and the "selectionStream" value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="source"></param>
        /// <param name="selectionStream"></param>
        /// <param name="aggregate"></param>
        /// <returns></returns>
        public static IObservable<R> ReduceLatestBy<T, U, R>(this IObservable<IObservable<T>> source, IObservable<U> selectionStream, Func<T, T, T> aggregate, Func<int, U, T, R> resultCombiner)
        {
            return Observable.Create<R>(observer =>
            {
                var values = new Dictionary<IObservable<T>, T>();
                var subs = new List<IDisposable>();


                source.SelectMany(t =>
                {
                    return t.Select(v => (t, v)).Do(tv => values[tv.t] = tv.v).DoOnCompleted(() => values.Remove(t));
                }).Subscribe(onNext: _ => { }, onError: observer.OnError, onCompleted: observer.OnCompleted)
                .AddTo(subs);

                /*
                source
                .Subscribe(
                    onNext: valueProvider =>
                    {
                        var valueProviderClosure = valueProvider;
                        valueProvider
                        .Subscribe(
                            onNext: toScan => values[valueProviderClosure] = toScan,
                            onCompleted: () => values.Remove(valueProviderClosure),
                            onError: e => values.Remove(valueProviderClosure))
                        .AddTo(subs);
                    },
                    onError: observer.OnError,
                    onCompleted: observer.OnCompleted)
                .AddTo(subs);
                */
                selectionStream
                .Subscribe(
                    onNext: selectionTick =>
                    {
                        var valueCount = values.Count;

                        if (valueCount == 0) return;
                        
                        var enumerator = values.GetEnumerator();
                        T runningValue = default ;
                        int iterationCount = 0;
                        while(enumerator.MoveNext())
                        {
                            if (iterationCount == 0) runningValue = enumerator.Current.Value;
                            else runningValue = aggregate(runningValue, enumerator.Current.Value);
                            iterationCount++;
                        }
                        observer.OnNext(resultCombiner(iterationCount, selectionTick, runningValue));
                    },
                    onError: observer.OnError,
                    onCompleted: observer.OnCompleted)
                .AddTo(subs);

                return new CompositeDisposable(subs);
            });
        }

        /// <summary>
        /// Takes "source" observable emissions until "other" or "source" completes or errors
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="source"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static IObservable<T0> TakeDuring<T0, T1>(this IObservable<T0> source, IObservable<T1> other) =>
           Observable.Create<T0>(observer =>
           {
               bool terminated = false;
               Action<Exception> onError =
                   e =>
                   {
                       if (terminated) return;
                       observer.OnError(e);
                       terminated = true;
                   };

               Action<T0> onNext =
                   x =>
                   {
                       if (terminated) return;
                       observer.OnNext(x);
                   };

               Action onCompleted =
                   () =>
                   {
                       if (terminated) return;
                       observer.OnCompleted();
                       terminated = true;
                   };

               var sub0 =
                   other.Subscribe(x => { }, onError, onCompleted);

               var sub1 =
                   source.Subscribe(onNext, onError, onCompleted);

               return new CompositeDisposable(sub0, sub1);
           });


        /// <summary>
        /// In Testing
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="this"></param>
        /// <param name="result"></param>
        /// <param name="toggleSeed"></param>
        /// <returns></returns>
        public static IObservable<R> ToggleEach<T, R>(this IObservable<T> @this, Func<bool, T, R> result, bool toggleSeed)
        {
            (bool toggleState, T value) seed = (toggleSeed, default);
            return
                @this.Scan(seed, (prev, input) => (!prev.toggleState, input))
                .Select(inputs => result(inputs.toggleState, inputs.value));
        }


        /// <summary>
        /// Returns value onObvXFirst() for the first to complete
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="obv0"></param>
        /// <param name="obv1"></param>
        /// <param name="obv0LastSelector"></param>
        /// <param name="obv1LastSelector"></param>
        /// <returns></returns>
        public static IObservable<R> FirstCompleted<T0, T1, R>(this IObservable<T0> obv0, IObservable<T1> obv1, Func<R> onObv0First, Func<R> onObv1First) =>
            Observable.Create<R>(observer =>
            {
                List<IDisposable> subs = new List<IDisposable>();
                Exception exc = null;

                bool completed = false;

                obv0.Subscribe(
                    onNext: x => { },
                    onCompleted: () =>
                    {
                        if (!completed)
                        {
                            observer.OnNext(onObv0First());
                            observer.OnCompleted();
                            completed = true;
                        }
                    },
                    onError: e =>
                    {
                        if (exc != null)
                        {
                            var agg = new AggregateException(exc, e);
                            observer.OnError(e);
                        }
                        else
                        {
                            exc = e;
                        }
                    }).AddTo(subs);

                obv1.Subscribe(
                    onNext: x => { },
                    onCompleted: () =>
                    {
                        if (!completed)
                        {
                            observer.OnNext(onObv1First());
                            observer.OnCompleted();
                            completed = true;
                        }
                    },
                    onError: e =>
                    {
                        if (exc != null)
                        {
                            var agg = new AggregateException(exc, e);
                            observer.OnError(e);
                        }
                        else
                        {
                            exc = e;
                        }
                    }).AddTo(subs);

                return new CompositeDisposable(subs);
            });

        public static IObservable<Unit> FirstCompleted<T0, T1>(this IObservable<T0> obv0, IObservable<T1> obv1) =>
            obv0.FirstCompleted(obv1, () => Unit.Default, () => Unit.Default);

        /// <summary>
        /// Returns Some T if there was a last value, otherwise returns NONE if observable completes without a last value observed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static IObservable<Option<T>> TryLast<T>(this IObservable<T> @this) =>
            Observable.Create<Option<T>>(observer =>
            {
                Option<T> cache = None.Default;

                return
                @this.Subscribe(
                    onNext: t => cache = t.AsOption_SAFE(),
                    onCompleted: () =>
                    {
                        observer.OnNext(cache);
                        observer.OnCompleted();
                    },
                    onError: observer.OnError);
            });

        /// <summary>
        /// Returns Some R for the first "last" value that is 'Some' based on selector.  Returns NONE if both source and other complete without a last value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="source"></param>
        /// <param name="other"></param>
        /// <param name="sourceLastSelect"></param>
        /// <param name="otherLastSelect"></param>
        /// <returns></returns>
        public static IObservable<Option<R>> TryFirstLast<T, U, R>(this IObservable<T> source, IObservable<U> other, Func<T, R> sourceLastSelect, Func<U, R> otherLastSelect) =>
            Observable.Create<Option<R>>(observer =>
            {
                Option<R> cache = None.Default;
                List<IDisposable> subs = new List<IDisposable>();

                Exception exc = null;
                bool oneCompleted = false;
                bool firstLastFound = false;

                source.TryLast()
                .Where(_ => !cache.IsSome)
                .Choose(opt => opt)
                .Subscribe(
                    onNext: t =>
                    {
                        if (firstLastFound) return;
                        observer.OnNext(sourceLastSelect(t).AsOption_SAFE());
                        observer.OnCompleted();
                        firstLastFound = true;
                    },
                    onCompleted: () =>
                    {
                        if (!firstLastFound || (!firstLastFound && oneCompleted))
                            observer.OnCompleted();
                        else
                            oneCompleted = true;
                    },
                    onError: e =>
                    {
                        if (exc != null)
                        {
                            var agg = new AggregateException(exc, e);
                            observer.OnError(e);
                        }
                        else
                        {
                            exc = e;
                        }
                    }).AddTo(subs);

                other.TryLast()
                .Where(_ => !cache.IsSome)
                .Choose(opt => opt)
                .Subscribe(
                    onNext: u =>
                    {
                        if (firstLastFound) return;
                        observer.OnNext(otherLastSelect(u).AsOption_SAFE());
                        observer.OnCompleted();
                        firstLastFound = true;
                    },
                    onCompleted: () =>
                    {
                        if (!firstLastFound || (!firstLastFound && oneCompleted))
                            observer.OnCompleted();
                        else
                            oneCompleted = true;
                    },
                    onError: e =>
                    {
                        if (exc != null)
                        {
                            var agg = new AggregateException(exc, e);
                            observer.OnError(e);
                        }
                        else
                        {
                            exc = e;
                        }
                    }).AddTo(subs);


                return new CompositeDisposable(subs);
            });

        /// <summary>
        /// Binds the source Option Emitter to the TryFirstLast Operation.  If last value in source is "NONE", it will treat source observable as a stream with no final value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="source"></param>
        /// <param name="other"></param>
        /// <param name="sourceLastSelect"></param>
        /// <param name="otherLastSelect"></param>
        /// <returns></returns>
        public static IObservable<Option<R>> BindTryFirstLast<T, U, R>(this IObservable<Option<T>> source, IObservable<U> other, Func<T, R> sourceLastSelect, Func<U, R> otherLastSelect) =>
            source.TryLast()
            .Where(nested => nested.IsSome && nested.Value.IsSome)
            .Select(nested => nested.Value.Value)
            .TryFirstLast(other, sourceLastSelect, otherLastSelect);

        /// <summary>
        /// Binds the source Option emitter to the TryLast Operation.  If last value in source is "NONE", it wil treat source observable as a stream with no final value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IObservable<Option<T>> BindTryLast<T>(this IObservable<Option<T>> source) =>
            source.TryLast()
            .Select(nested => nested.IsSome ? nested.Value : None.Default);

        public static IObservable<T> AsNew<T>(this IObservable<T> @this) =>
            @this.AsObservable();

        public static IObservable<T> CompleteOnlyWith<T, U>(this IObservable<T> source, IObservable<U> other) =>
        Observable.Create<T>(observer =>
        {
            return
            source.Concat(Observable.Never<T>())
            .TakeDuring(other)
            .Subscribe(observer);
        });

        public static IObservable<Option<T>> ConcatNone<T>(this IObservable<T> @this) =>
    @this.Select(t => t.AsOption_SAFE()).Concat(ReturnNone<T>());

        public static IObservable<Option<T>> ReturnNone<T>() =>
            Observable.Return((Option<T>)None.Default);

        /// <summary>
        /// observer begins by observing 'source' stream
        /// When 'switchStream' emits 'Some' value, observer begins observing the switch stream. 
        /// When switchStream emits 'None" value, observer returns to 'source' stream
        /// Observer goes back and forth as determined by 'switchStream's' emissions of 'Some' and 'None' values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="switchStream"></param>
        /// <returns></returns>
        public static IObservable<T> SwitchValve<T>(this IObservable<T> source, IObservable<Option<T>> switchStream) =>
            Observable.Create<T>(observer =>
            {
                var switchValve =
                    switchStream.Select(opt => !opt.IsSome)
                    .DistinctUntilChanged()
                    .Concat(Observable.Return(true));

                var sourceSub = source.Valve(switchValve, true).Subscribe(observer);

                var switchSub =
                    switchStream.Valve(switchValve.Select(allowSource => !allowSource), false)
                    .Choose(opt => opt)
                    .Subscribe(t => observer.OnNext(t));

                return new CompositeDisposable(sourceSub, switchSub);
            });

        public static IObservable<T> SwitchValve<T>(this IObservable<T> source, params IObservable<Option<T>>[] switchStreams) =>
            source.SwitchValve(switchStreams.Merge());

        public static IObservable<(T scannedValue, TAccumulate accumulatedValue)> PassScan<T, TAccumulate>(this IObservable<T> @this, TAccumulate seed, Func<TAccumulate, T, TAccumulate> scanFunction)
        {
            T scanneDValue = default;
            var accumulatedValue = seed;

            return @this.Scan((scanneDValue, accumulatedValue), (accum, scan) =>
            {
                var a = scanFunction(accum.accumulatedValue, scan);
                return (scan, a);
            });
        }
    }
}