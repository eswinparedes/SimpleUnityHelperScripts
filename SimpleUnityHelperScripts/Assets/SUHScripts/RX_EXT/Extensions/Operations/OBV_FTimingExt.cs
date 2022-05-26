using SUHScripts.Functional;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace SUHScripts
{
    public static class OBV_FTimingExt
    {
        public static IObservable<(FTimer timer, T value)> TrackTimer<T>(this IObservable<T> @this, float time, Func<FTimer, T, FTimer> timerUpdater)
        {
            T tSeed = default;
            var timerSeed = new FTimer(time, 0);
            var initSeed = new FTimer(time, time);
            (FTimer timer, T value) seed = (initSeed, tSeed);

            return
                @this.Scan(
                    seed,
                    (prev, value) =>
                    {
                        var nextTimer =
                            prev.timer.HasCompleted()
                            ? timerSeed
                            : timerUpdater(prev.timer, value);

                        return (nextTimer, value);
                    });
        }

        public static IObservable<R> TrackTimer<T, R>(this IObservable<T> @this, float time, Func<FTimer, T, FTimer> timerUpdater, Func<FTimer, T, R> selector) =>
            @this.TrackTimer(time, timerUpdater)
            .Select(inputs => selector(inputs.timer, inputs.value));

        public static IObservable<(T value, FTimer timer)> TimerScan<T>(this IObservable<T> @this, Func<T, float> tickFunction, Func<FTimer> seed) =>
            Observable.Create<(T value, FTimer timer)>(observer =>
            {
                (T value, FTimer timer) seedInput = (default, seed());

                return
                @this.Scan(
                        seedInput,
                        (state, value) => (value, state.timer.Tick(tickFunction(value))))
                    .TakeWhile_IncludeLast(inputs => !inputs.timer.HasCompleted())
                    .Subscribe(observer.OnNext, observer.OnError, observer.OnCompleted);
            });

        public static IObservable<FTimer> TimerPingPongByEmission(this IObservable<bool> @this, IObservable<float> timerTickStream, float length) =>
            Observable.Create<FTimer>(observer =>
            {
                if (length <= 0)
                {
                    return @this.Select(shouldIncrement => shouldIncrement ? new FTimer(0, 0, true) : new FTimer(0, 0, false)).Subscribe(observer);
                }

                var activeTimer = (Option<FTimer>)None.Default;

                var sub0 = @this.Subscribe(shouldIncrement =>
                {
                    if (!activeTimer.IsSome)
                    {
                        var startTime = shouldIncrement ? 0 : length;
                        activeTimer = new FTimer(length, startTime, shouldIncrement).AsOption_UNSAFE();
                    }
                    else
                    {
                        var isDifferent = shouldIncrement != activeTimer.Value.IsIncrementing;
                        activeTimer = activeTimer.Value.Restarted(isDifferent, isDifferent).AsOption_UNSAFE();
                        observer.OnNext(activeTimer.Value);

                    }
                });

                var sub1 = timerTickStream.Subscribe(
                    tick =>
                    {
                        if (!activeTimer.Value.HasCompleted() && activeTimer.IsSome)
                        {
                            activeTimer = activeTimer.Value.Tick(tick).AsOption_UNSAFE();
                            observer.OnNext(activeTimer.Value);
                        }
                    },
                    observer.OnError,
                    observer.OnCompleted);

                return new CompositeDisposable(sub0, sub1);
            });



        public static IObservable<Unit> AutomaticFire(this IObservable<bool> triggerPullStream, IObservable<float> tick, float firePerSecond) =>
            Observable.Create<Unit>(observer =>
            {
                var cleanPulls = triggerPullStream.DistinctUntilChanged();
                var timer = new FTimer(firePerSecond, firePerSecond);
                bool isPulling = false;

                var tickSub = tick.Subscribe(delta =>
                {                  
                    if (timer.HasCompleted())
                    {
                        if (isPulling)
                        {
                            observer.OnNext(Unit.Default);
                            timer = timer.Restarted();
                        }
                    }
                    timer = timer.Tick(delta);
                });

                var pullSub = cleanPulls.Subscribe(isPull => isPulling = isPull);

                return new CompositeDisposable(tickSub, pullSub);
            });

        public static IObservable<Unit> SemiAutomaticFire(this IObservable<bool> triggerPullStream, IObservable<float> tick, float firePerSecond) =>
            Observable.Create<Unit>(observer =>
            {
                var cleanPulls = triggerPullStream.DistinctUntilChanged();
                var timer = new FTimer(firePerSecond, firePerSecond);
                var isPulling = false;
                var pullState = BoolState.FalseStay;

                var tickSub = tick.Subscribe(delta =>
                {
                    if (timer.HasCompleted())
                    {
                        if (pullState == BoolState.TrueThisFrame)
                        {
                            observer.OnNext(Unit.Default);
                            timer = timer.Restarted();
                        }
                    }

                    pullState = pullState.Update(isPulling);
                    timer = timer.Tick(delta);  
                });

                var pullSub = cleanPulls.Subscribe(isPull => isPulling = isPull);

                return new CompositeDisposable(tickSub, pullSub);
            });
    }
}
