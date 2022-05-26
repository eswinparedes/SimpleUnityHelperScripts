using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUHScripts
{
    public static class TransformOrientationOperations 
    {
        //SUHS TODO: Include Local orientation?
        public static TransformOrientation Orientation(this Transform @this) =>
            new TransformOrientation(@this.position, @this.rotation);

        public static TransformOrientation ExtractOrientationOffset(this Transform @this, Vector3? positionOffset = null, Quaternion? rotationOffset = null)
        {
            var orientation = @this.Orientation();
            var pos2 = positionOffset ?? Vector3.zero;
            var rot2 = rotationOffset ?? Quaternion.identity;

            return new TransformOrientation(pos2 + orientation.Position, rot2 * orientation.Rotation);
        }

        public static TransformOrientation WithPosition(this TransformOrientation @this, Vector3 position) =>
            new TransformOrientation(position, @this.Rotation);

        public static TransformOrientation WithRotation(this TransformOrientation @this, Quaternion rotation) =>
            new TransformOrientation(@this.Position, rotation);

        public static TransformOrientation With
            (this TransformOrientation @this, Vector3? position = null, Quaternion? rotation = null) =>
            new TransformOrientation(position ?? @this.Position, rotation ?? @this.Rotation);

        public static float DotProductLookToNormalized(this TransformOrientation lookFrom, TransformOrientation lookTo, Vector3? relativeLHS = null)
        {
            var vector = relativeLHS ?? Vector3.forward;
            Vector3 lhs = lookFrom.Rotation * vector;
            Vector3 rhs = (lookTo.Position - lookFrom.Position).normalized;
            return Vector3.Dot(lhs, rhs);
        }
        public static float DotProductLookTo(this TransformOrientation lookFrom, TransformOrientation lookTo, Vector3? relativeLHS = null)
        {
            var vector = relativeLHS ?? Vector3.forward;

            Vector3 lhs = lookFrom.Rotation * vector;
            Vector3 rhs = (lookTo.Position - lookFrom.Position);
            return Vector3.Dot(lhs, rhs);
        }

        /// <summary>
        /// direction transformed from world to local
        /// </summary>
        /// <param name="this"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Vector3 TransformDirection(this TransformOrientation @this, Vector3 direction) =>
            @this.Rotation * direction;

        public static Vector3 Forward(this TransformOrientation @this) =>
            @this.TransformDirection(Vector3.forward);

        public static Vector3 Up(this TransformOrientation @this) =>
            @this.TransformDirection(Vector3.up);

        public static Vector3 Right(this TransformOrientation @this) =>
            @this.TransformDirection(Vector3.right);
    }

}
