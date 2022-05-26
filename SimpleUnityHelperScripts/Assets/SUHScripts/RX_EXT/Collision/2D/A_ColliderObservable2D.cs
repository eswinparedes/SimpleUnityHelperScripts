using SUHScripts.Functional;
using System;
using UnityEngine;


    public abstract class A_ColliderObservable2D : MonoBehaviour
    {
        public abstract IObservable<CollisionObserved2D> OnEnter { get; }
        public abstract IObservable<CollisionObserved2D> OnExit { get; }
    }

