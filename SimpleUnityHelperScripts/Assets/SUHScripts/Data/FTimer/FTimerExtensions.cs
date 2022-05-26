using System;
using UnityEngine;

namespace SUHScripts
{
    using Functional;
    using static Functional.Functional;
    public static class FTimerExtensions
    {
        public static FTimer AtAlpha(this FTimer @this, float alpha) =>
            @this.IsIncrementing ?
                @this.With(elapsed: @this.Length * alpha) :
                @this.With(elapsed: (@this.Length - @this.Length * alpha));

        public static FTimer With(this FTimer @this, float? length = null, float? elapsed = null, bool? isIncrementing = null) =>
            new FTimer(length ?? @this.Length, elapsed ?? @this.ElapsedTime, isIncrementing ?? @this.IsIncrementing);

        public static float TimeLeft(this FTimer @this) =>
            @this.IsIncrementing ?
                @this.Length - @this.ElapsedTime :
                @this.ElapsedTime;

        public static float ClampTick(this FTimer @this, float elapsed) =>
            @this.IsIncrementing ?
                Mathf.Clamp(@this.ElapsedTime + elapsed, 0, @this.Length) :
                Mathf.Clamp(@this.ElapsedTime - elapsed, 0, @this.Length);

        public static float TimeAlpha(this FTimer @this) =>
            @this.IsIncrementing ?
                @this.ElapsedTime / @this.Length :
                1 - (@this.Length - @this.ElapsedTime) / @this.Length;

        public static FTimer RestartedIncrementing(this FTimer @this, bool bounce = false) =>
            new FTimer(@this.Length, bounce ? @this.ElapsedTime : 0, true);

        public static FTimer RestartedDecrementing(this FTimer @this, bool bounce = false) =>
            new FTimer(@this.Length, bounce ? @this.ElapsedTime : @this.Length, false);

        public static FTimer Restarted(this FTimer @this, bool pingPong = false, bool bounce = false)
        {
            bool shouldIncrement = pingPong ? !@this.IsIncrementing : @this.IsIncrementing;
            return shouldIncrement ? @this.RestartedIncrementing(bounce) : @this.RestartedDecrementing(bounce);
        }

        public static bool HasCompleted(this FTimer @this) =>
            @this.IsIncrementing ?
                @this.Length <= @this.ElapsedTime : @this.ElapsedTime <= 0;

        public static FTimer Looped(this FTimer @this, bool pingPong) =>
            @this.HasCompleted() ? @this.Restarted(pingPong) : @this;

        public static FTimer Tick(this FTimer @this, float seconds, bool pingPong = false, bool loop = false)
        {
            FTimer t = @this.With(elapsed: @this.ClampTick(seconds));
            return loop ? t.Looped(pingPong) : t;
        }

        /* Call to "Where" for extensions causes garbage collection due to boxing
        public static FTimer Tick(this FTimer @this, float seconds, bool pingPong = false, bool loop = false, Action onComplete = null)
        {
            var result =
            @this
                .AsOption_UNSAFE()
                .Where(TimerWillCompleteNextTick(@this, seconds))
                .Match(None.Default, TickClamp(@this, seconds).AsOption_UNSAFE())
                .ForEachPass(onComplete ?? DoNothing)
                .Reduce(@this.With(elapsed: @this.ClampTick(seconds)));
            
           return loop ? result.Looped(pingPong) : result;
        }
        */
        static Func<FTimer, float, bool> TimerWillCompleteNextTick =
            (timer, tick) => tick >= timer.TimeLeft() && !timer.HasCompleted();

        static Func<FTimer, float, FTimer> TickClamp =
            (timer, tick) => timer.With(elapsed: timer.ClampTick(tick));

        static Action DoNothing = () => { };

    }
}
