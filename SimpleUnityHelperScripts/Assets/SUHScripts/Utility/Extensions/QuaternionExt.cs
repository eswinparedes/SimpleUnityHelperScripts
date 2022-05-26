using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUHScripts
{
    public static class QuaternionExt 
    {
        public static Quaternion MaskedRotation(this Quaternion from, Vector3 mask)
        {
            var euler = from.eulerAngles;
            var eulerTarget = Vector3.Scale(euler, mask);
            return Quaternion.Euler(eulerTarget);
        }

        /// <summary>
        /// Transforms vector from world space to local space
        /// </summary>
        /// <param name="this"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Vector3 InverseTransformDirection(this Quaternion @this, Vector3 direction) =>
            @this * direction;

        public static Quaternion MirrorNormal(this Quaternion source, Vector3 mirrorNormal)
        {
            var forward = source * Vector3.forward;
            var mirrored = Vector3.Reflect(forward, mirrorNormal);
            return Quaternion.LookRotation(mirrored, source * Vector3.up);
        }

        public static Quaternion MirrorX(this Quaternion source)
        {
            return new Quaternion(
                source.x * -1.0f,
                source.y,
                source.z,
                source.w * -1.0f);
        }

        public static Quaternion MirrorY(this Quaternion source)
        {
            return new Quaternion(
                source.x ,
                source.y * -1.0f,
                source.z,
                source.w * -1.0f);
        }

        public static Quaternion MirrorZ(this Quaternion source)
        {
            return new Quaternion(
                source.x,
                source.y,
                source.z * -1.0f,
                source.w * -1.0f);
        }
    }
}

