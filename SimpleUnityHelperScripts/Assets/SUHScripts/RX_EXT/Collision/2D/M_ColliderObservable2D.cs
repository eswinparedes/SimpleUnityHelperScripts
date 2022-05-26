using System;
using UnityEngine;
using UniRx;
using SUHScripts.Functional;


    [DisallowMultipleComponent]
    public class M_ColliderObservable2D : A_ColliderObservable2D
    {
        Subject<CollisionObserved2D> m_onEnter = new Subject<CollisionObserved2D>();
        Subject<CollisionObserved2D> m_onExit = new Subject<CollisionObserved2D>();
   
        public override IObservable<CollisionObserved2D> OnEnter => m_onEnter;
        public override IObservable<CollisionObserved2D> OnExit => m_onExit;

        private void Awake()
        {
            m_onEnter.AddTo(this);
            m_onExit.AddTo(this);
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            var col = new CollisionObserved2D(collision.AsOption_SAFE(), collision.collider);
            m_onEnter.OnNext(col);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            var col = new CollisionObserved2D(collision.AsOption_SAFE(), collision.collider);
            m_onExit.OnNext(col);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var col = new CollisionObserved2D(None.Default, other);
            m_onEnter.OnNext(col);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var col = new CollisionObserved2D(None.Default, other);
            m_onExit.OnNext(col); 
        }
    }
