using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace SUHScripts
{
    public static class Vector3Extensions
    {
        public static (float x, float y, float z) Deconstruct(this Vector3 @this) =>
            (@this.x, @this.y, @this.z);

        public static Vector3 Vector3Construct(this (float, float, float) @this) =>
            new Vector3(@this.Item1, @this.Item2, @this.Item3);

        public static Vector3 WithX(this Vector3 @this, float x) =>
            new Vector3(x, @this.y, @this.z);

        public static Vector3 WithY(this Vector3 @this, float y) =>
            new Vector3(@this.x, y, @this.z);

        public static Vector3 WithZ(this Vector3 @this, float z) =>
            new Vector3(@this.x, @this.y, z);

        public static Vector3 WithX(this Vector3 @this, Func<float, float> xMap) =>
            new Vector3(xMap(@this.x), @this.y, @this.z);

        public static Vector3 WithY(this Vector3 @this, Func<float, float> yMap) =>
            new Vector3(@this.x, yMap(@this.y), @this.z);

        public static Vector3 WithZ(this Vector3 @this, Func<float, float> zMap) =>
            new Vector3(@this.x, @this.y, zMap(@this.z));

        public static Vector3 With(this Vector3 @this, float? x = null, float? y = null, float? z = null) =>
            (x == null ? @this.x : x.Value, y == null ? @this.y : y.Value, z == null ? @this.z : z.Value)
            .Vector3Construct();

        public static Vector2 AsXY(this Vector3 @this) =>
            new Vector2(@this.x, @this.y);

        public static Vector3 Average(this IReadOnlyList<Vector3> @this)
        {
            var v = Vector3.zero;

            for (int i = 0; i < @this.Count; i++)
            {
                v += @this[i];
            }

            return @this.Count > 0 ? v / @this.Count : v;
        }

        public static Vector3 Average(this IEnumerable<Vector3> @this)
        {
            var v = Vector3.zero;

            foreach (var ve in @this)
            {
                v += ve;
            }
            var c = @this.Count();

            return c > 0 ? v / c : v;
        }

        public static Vector3 MaxBySqMagnitude(this IReadOnlyList<Vector3> @this)
        {
            var v = @this[0];

            for (int i = 0; i < @this.Count; i++)
            {
                if (@this[i].sqrMagnitude > v.sqrMagnitude) v = @this[i];
            }

            return v;
        }

        public static Vector3 MaxBySqMagnitude(this IEnumerable<Vector3> @this)
        {
            var v = @this.First();

            foreach (var ve in @this)
            {
                if (ve.sqrMagnitude > v.sqrMagnitude) v = ve;
            }

            return v;
        }

        public static Vector3 MinBySqMagnitude(this IReadOnlyList<Vector3> @this)
        {
            var v = @this[0];

            for (int i = 0; i < @this.Count; i++)
            {
                if (@this[i].sqrMagnitude < v.sqrMagnitude) v = @this[i];
            }

            return v;
        }

        public static Vector3 MinBySqMagnitude(this IEnumerable<Vector3> @this)
        {
            var v = @this.First();

            foreach (var ve in @this)
            {
                if (ve.sqrMagnitude < v.sqrMagnitude) v = ve;
            }

            return v;
        }

        public static Vector3 MaxComponents(this IReadOnlyList<Vector3> @this)
        {
            var v = @this[0];

            for (int i = 0; i < @this.Count; i++)
            {
                var n = @this[i];

                if (n.x > v.x) v.x = n.x;
                if (n.y > v.y) v.y = n.y;
                if (n.z > v.z) v.z = n.z;
            }

            return v;
        }

        public static Vector3 MaxComponents(this IEnumerable<Vector3> @this)
        {
            var v = @this.First();

            foreach (var ve in @this)
            {
                var n = ve;

                if (n.x > v.x) v.x = n.x;
                if (n.y > v.y) v.y = n.y;
                if (n.z > v.z) v.z = n.z;
            }

            return v;
        }

        public static Vector3 MinComponents(this IReadOnlyList<Vector3> @this)
        {
            var v = @this[0];

            for (int i = 0; i < @this.Count; i++)
            {
                var n = @this[i];

                if (n.x < v.x) v.x = n.x;
                if (n.y < v.y) v.y = n.y;
                if (n.z < v.z) v.z = n.z;
            }

            return v;
        }

        public static Vector3 MinComponents(this IEnumerable<Vector3> @this)
        {
            var v = @this.First();

            foreach (var ve in @this)
            {
                if (ve.x < v.x) v.x = ve.x;
                if (ve.y < v.y) v.y = ve.y;
                if (ve.z < v.z) v.z = ve.z;
            }

            return v;
        }



        public static Vector3 Sum(this IReadOnlyList<Vector3> @this)
        {
            var v = Vector3.zero;

            for (int i = 0; i < @this.Count; i++)
            {
                v += @this[i];
            }

            return v / @this.Count;
        }

        public static Vector3 Sum(this IEnumerable<Vector3> @this)
        {
            var v = Vector3.zero;

            foreach (var ve in @this)
            {
                v += ve;
            }

            var c = @this.Count();

            return c > 0 ? v / c : v;
        }

        public static Vector3 AccelerateTowardsDesiredVectorClamped(this Vector3 current, Vector3 desired, float acceleration)
        {
            var dir = desired - current;
            var mag = dir.magnitude;
            var accel = mag > acceleration ? acceleration : mag;
            return current + dir.normalized * accel;
        }

        public static Vector3 AccelerateMagnitudeTowardsDesiredVector
            (this Vector3 from, Vector3 to, float acceleration)
        {
            var magTarget = to.magnitude;
            var magCurrent = from.magnitude;
            var targetNormalized = to.normalized;

            if (to != Vector3.zero && magCurrent == 0)
            {
                return targetNormalized * acceleration;
            }

            var mag = Mathf.MoveTowards(magCurrent, magTarget, acceleration);
            return targetNormalized * mag;
        }

        public static Vector3 AlongNormal(this Vector3 @this, Vector3 normal) =>
            Vector3.ProjectOnPlane(@this, normal).normalized;

        public static Vector3 RandomComponents(Vector2 range)
        {
            return RandomComponents(range.x, range.y);
        }

        public static Vector3 RandomComponents(float x, float y)
        {
            Vector3 vector = Vector3.zero;
            vector.x = UnityEngine.Random.Range(x, y);
            vector.y = UnityEngine.Random.Range(x, y);
            vector.z = UnityEngine.Random.Range(x, y);
            return vector;
        }
        public static Vector3 MaxByComponent(this Vector3 a, Vector3 b)
        {
            var x = a.x >= b.x ? a.x : b.x;
            var y = a.y >= b.y ? a.y : b.y;
            var z = a.z >= b.z ? a.z : b.z;

            return new Vector3(x, y, z);
        }
    }
}
