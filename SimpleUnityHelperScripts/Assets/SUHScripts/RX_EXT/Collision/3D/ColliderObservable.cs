using System;
using UnityEngine;
using UniRx;

namespace SUHScripts
{
    using Functional;
    using static Functional.Functional;
    [DisallowMultipleComponent]
    public class ColliderObservable : AColliderObservable
    {
        Subject<CollisionObserved> m_onEnter = new Subject<CollisionObserved>();
        Subject<CollisionObserved> m_onExit = new Subject<CollisionObserved>();
   
        public override IObservable<CollisionObserved> OnEnter => m_onEnter;
        public override IObservable<CollisionObserved> OnExit => m_onExit;

        private void Awake()
        {
            m_onEnter.AddTo(this);
            m_onExit.AddTo(this);
        }

        private void OnCollisionEnter(Collision collision)
        {
            var col = new CollisionObserved(collision.AsOption_SAFE(), collision.collider);
            m_onEnter.OnNext(col);
        }

        private void OnCollisionExit(Collision collision)
        {
            var col = new CollisionObserved(collision.AsOption_SAFE(), collision.collider);
            m_onExit.OnNext(col);
        }

        private void OnTriggerEnter(Collider other)
        {
            var col = new CollisionObserved(None.Default, other);
            m_onEnter.OnNext(col);
        }

        private void OnTriggerExit(Collider other)
        {
            var col = new CollisionObserved(None.Default, other);
            m_onExit.OnNext(col); 
        }
    }
}
