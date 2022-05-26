using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SUHScripts
{
    public static class TransformOrientationBehaviours 
    {
        public static void ApplyTransformOrientation(Transform target, TransformOrientation data)
        {
            target.position = data.Position;
            target.rotation = data.Rotation;
        }

        public static void ApplyOrientation(this Transform @this, TransformOrientation data) =>
            ApplyTransformOrientation(@this, data);

        public static void ApplyToTransform(this TransformOrientation @this, Transform target) =>
            ApplyTransformOrientation(target, @this);

        public static void ApplyOrientation(this Transform @this, Transform other) =>
            other.Orientation()
            .ApplyToTransform(other);

        //SUHS TODO: Apply Orientation with offset for rotation and position

        //SUHS TODO: Simulate parent / childe
    }
}

