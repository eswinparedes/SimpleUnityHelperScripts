using SUHScripts.Functional;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CollisionObserved2D
{
    public CollisionObserved2D(Option<Collision2D> collisionData, Collider2D collidingOther)
    {
        CollisionData = collisionData;
        CollidingOther = collidingOther;
    }

    public Option<Collision2D> CollisionData { get; }

    public Collider2D CollidingOther { get; }
}