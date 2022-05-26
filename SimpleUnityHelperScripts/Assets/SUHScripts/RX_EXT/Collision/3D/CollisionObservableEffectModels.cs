using SUHScripts.Functional;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUHScripts
{

    public struct CollisionObserved 
    {
        public CollisionObserved(Option<Collision> collisionData, Collider collidingOther)
        {
            CollisionData = collisionData;
            CollidingOther = collidingOther;
        }

        public Option<Collision> CollisionData { get; }

        public Collider CollidingOther { get;}
    }
}

