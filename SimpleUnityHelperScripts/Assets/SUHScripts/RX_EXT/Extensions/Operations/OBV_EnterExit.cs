using System;
using System.Collections.Generic;
using UniRx;

namespace SUHScripts
{
    public static class OBV_EnterExit 
    {
        public static IObservable<T> OnEnter<T>(this IObservable<EnterExitable<T>> @this) =>
            @this.Where(t => t.IsEntered).Select(t => t.Value);

        public static IObservable<T> OnExit<T>(this IObservable<EnterExitable<T>> @this) =>
            @this.Where(t => !t.IsEntered).Select(t => t.Value);

        /// <summary>
        /// Given "KeySelector", tracks how many have "Entered" and how many have "Exited";
        /// Emits An Entered Item for the first 'T' emitted by enter as selected for via 'KeySelector'
        /// as enter emits after the first selection has been emitted, will track this value until "exit" has emitted as many times as have entered
        /// If a value has not entered but this function recieves an exit value for it, it will ignore the value
        /// if a value exits and tracked count is above 0 then exit will not emit
        /// if a value enters that has not exited, it will ignore the value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="enter"></param>
        /// <param name="exit"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IObservable<EnterExitable<T>> EnterExitByCount<T, R>(IObservable<T> enter, IObservable<T> exit, Func<T, R> keySelector) =>
            Observable.Create<EnterExitable<T>>(observer =>
            {
                var trackedCount = new Dictionary<R, int>();
                var subs = new List<IDisposable>();

                var ks = keySelector;

                enter.FirstCompleted<T, T, Unit>(exit, () => Unit.Default, () => Unit.Default)
                    .Subscribe(onNext: x => { }, onError: observer.OnError, onCompleted: observer.OnCompleted)
                    .AddTo(subs);

                enter.Subscribe(
                    onNext: t =>
                    {
                        var r = ks(t);

                        if (!trackedCount.AnyCounted(r))
                            observer.OnNext(new EnterExitable<T>(t, true));

                        trackedCount.CountAdd(r);
                    }).AddTo(subs);

                exit.Subscribe(t =>
                {
                    var r = ks(t);

                    if (!trackedCount.AnyCounted(r)) return;

                    trackedCount.CountSubtract(r);

                    if (!trackedCount.AnyCounted(r))
                        observer.OnNext(new EnterExitable<T>(t, false));
                }).AddTo(subs);

                enter.FirstCompleted(exit).Subscribe(_ => { }, observer.OnError, observer.OnCompleted).AddTo(subs);

                return new CompositeDisposable(subs);
            });

        /// <summary>
        /// emits a value when an "entered value" the first 'enter' and does not emit another enter value until 'exit' emits
        /// if a value is 'entered' and has not 'exited', this will not emit
        /// if a value is 'exited' and has not 'entered', this will not emit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enter"></param>
        /// <param name="exit"></param>
        /// <returns></returns>
        public static IObservable<EnterExitable<T>> EnterExit<T, R>(IObservable<T> enter, IObservable<T> exit, Func<T, R> hashableSelector) =>
            Observable.Create<EnterExitable<T>>(observer =>
            {
                var collidersEntered = new HashSet<R>();

                var subs = new List<IDisposable>();

                enter.Subscribe(value =>
                {
                    var r = hashableSelector(value);

                    if (!collidersEntered.Contains(r))
                    {
                        collidersEntered.Add(r);
                        observer.OnNext(new EnterExitable<T>(value, true));
                    }
                }).AddTo(subs);

                exit.Subscribe(value =>
                {
                    var r = hashableSelector(value);
                    if (collidersEntered.Contains(r))
                    {
                        collidersEntered.Remove(r);
                        observer.OnNext(new EnterExitable<T>(value, false));
                    }
                }).AddTo(subs);

                enter.FirstCompleted(exit).Subscribe(_ => { }, observer.OnError, observer.OnCompleted).AddTo(subs);

                return new CompositeDisposable(subs);
            });

        public static bool AnyCounted<T>(this Dictionary<T, int> @this, T checkFor)
        {
            if (@this.ContainsKey(checkFor))
                return @this[checkFor] > 0;
            else
                return false;
        }

        public static int CountAdd<T>(this Dictionary<T, int> @this, T countUpFor)
        {
            if (@this.ContainsKey(countUpFor))
            {
                @this[countUpFor] = @this[countUpFor] + 1;
            }
            else
            {
                @this.Add(countUpFor, 1);
            }
            return @this[countUpFor];
        }

        public static int CountSubtract<T>(this Dictionary<T, int> @this, T countDownFor)
        {
            if (@this.ContainsKey(countDownFor))
            {
                @this[countDownFor] = @this[countDownFor] - 1;

                if (@this[countDownFor] <= 0)
                {
                    @this.Remove(countDownFor);
                    return 0;
                }

                return @this[countDownFor];
            }
            else
            {
                return 0;
            }
        }
    }

    public class EnterExitable<T>
    {
        public EnterExitable(T value, bool isEntered)
        {
            Value = value;
            IsEntered = isEntered;
        }

        public T Value { get; }
        public bool IsEntered { get; }
    }
}

