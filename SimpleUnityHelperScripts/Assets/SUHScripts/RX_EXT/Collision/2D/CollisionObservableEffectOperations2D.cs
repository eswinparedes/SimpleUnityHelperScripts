using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using SUHScripts.Functional;
using SUHScripts;

namespace SUHScripts
{
    public static class CollisionObservableEffectOperations2D
    {
        public static IObservable<CollisionObserved2D> OnEnterAny(
            this IEnumerable<A_ColliderObservable2D> @this) =>
            @this
            .Select(obs => obs.OnEnter)
            .Merge();

        public static IObservable<CollisionObserved2D> OnExitAny(
            this IEnumerable<A_ColliderObservable2D> @this) =>
            @this
            .Select(obs => obs.OnExit)
            .Merge();
        public static IObservable<Option<T>> OnEnterComponentOption<T>(this A_ColliderObservable2D @this) =>
            @this
            .OnEnter
            .Select(entered => entered.CollidingOther.GetComponentOption<T>());

        public static IObservable<T> OnEnterGetComponentSome<T>(this A_ColliderObservable2D @this) =>
            @this
            .OnEnterComponentOption<T>()
            .SelectSome();

        public static (IObservable<CollisionObserved2D> onEnter, IObservable<CollisionObserved2D> onExit)
            ObserveCollisions(Collider2D colliderToObserve)
        {
            var comp = colliderToObserve.gameObject.GetOrAddComponent<M_ColliderObservable2D>();
            return (comp.OnEnter, comp.OnExit);
        }

        public static IObservable<EnterExitable<CollisionObserved2D>> ObserveCollisionsGrouped(IReadOnlyList<Collider2D> collidersToObserve) =>
            Observable.Create<EnterExitable<CollisionObserved2D>>(observer =>
            {
                List<M_ColliderObservable2D> attachedObservables = new List<M_ColliderObservable2D>();

                for (int i = 0; i < collidersToObserve.Count(); i++)
                {
                    var colObservable = collidersToObserve[i].gameObject.GetOrAddComponent<M_ColliderObservable2D>();

                    attachedObservables.Add(colObservable);
                }

                var entered = attachedObservables.OnEnterAny();
                var exited = attachedObservables.OnExitAny();

                return OBV_EnterExit.EnterExitByCount(entered, exited, col => col.CollidingOther).Subscribe(observer);
            });
    }


}
