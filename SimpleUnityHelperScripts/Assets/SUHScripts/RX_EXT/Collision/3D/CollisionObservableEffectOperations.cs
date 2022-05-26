using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using SUHScripts.Pending;
namespace SUHScripts
{
    using Functional;
    public static class CollisionObservableEffectOperations 
    {
        public static IObservable<CollisionObserved> OnEnterAny(
            this IEnumerable<AColliderObservable> @this) =>
            @this
            .Select(obs => obs.OnEnter)
            .Merge();

        public static IObservable<CollisionObserved> OnExitAny(
            this IEnumerable<AColliderObservable> @this) =>
            @this
            .Select(obs => obs.OnExit)
            .Merge();
        public static IObservable<Option<T>> OnEnterComponentOption<T>(this AColliderObservable @this) =>
            @this
            .OnEnter
            .Select(entered => entered.CollidingOther.GetComponentOption<T>());

        public static IObservable<T> OnEnterGetComponentSome<T>(this AColliderObservable @this) =>
            @this
            .OnEnterComponentOption<T>()
            .SelectSome();

        public static (IObservable<CollisionObserved> onEnter, IObservable<CollisionObserved> onExit)
            ObserveCollisions(Collider colliderToObserve)
        {
            var comp = colliderToObserve.gameObject.GetOrAddComponent<ColliderObservable>();
            return (comp.OnEnter, comp.OnExit);
        }

        public static IObservable<EnterExitable<CollisionObserved>> ObserveCollisionsGrouped(IReadOnlyList<Collider> collidersToObserve) =>
            Observable.Create<EnterExitable<CollisionObserved>>(observer =>
            {
                List<ColliderObservable> attachedObservables = new List<ColliderObservable>();

                for (int i = 0; i < collidersToObserve.Count(); i++)
                {
                    var colObservable = collidersToObserve[i].gameObject.GetOrAddComponent<ColliderObservable>();

                    attachedObservables.Add(colObservable);
                }

                var entered = attachedObservables.OnEnterAny();
                var exited = attachedObservables.OnExitAny();

                return OBV_EnterExit.EnterExitByCount(entered, exited, col => col.CollidingOther).Subscribe(observer);
            });

        public static IObservable<Collider> EnterDuringConditionQueue(IObservable<CollisionObserved> enterStream, IObservable<CollisionObserved> exitStream, IObservable<Unit> tick, Func<bool> condition) =>
            Observable.Create<Collider>(observer =>
            {
                var entered = new HashSet<Collider>();

                var sub0 =
                    enterStream.Subscribe(cols =>
                    {
                        if (!entered.Contains(cols.CollidingOther))
                            entered.Add(cols.CollidingOther);
                    },
                    observer.OnError,
                    observer.OnCompleted);

                var sub1 =
                    exitStream.Subscribe(cols =>
                    {
                        if (entered.Contains(cols.CollidingOther))
                            entered.Remove(cols.CollidingOther);
                    },
                    observer.OnError,
                    observer.OnCompleted);

                var sub2 =
                    tick.Subscribe(_ =>
                    {
                        if (condition())
                        {
                            foreach(var el in entered)
                            {
                                observer.OnNext(el);
                            }

                            entered.Clear();
                        }
                    },
                    observer.OnError,
                    observer.OnCompleted);

                return new CompositeDisposable(sub0, sub1, sub2);
            });
    }
}

