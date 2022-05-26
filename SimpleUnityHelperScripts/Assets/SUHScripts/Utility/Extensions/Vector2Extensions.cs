using UnityEngine;
using System;

namespace SUHScripts
{
    public static class Vector2Extensions 
    {
        public static (float x, float y) Deconstruct(this Vector2 @this) =>
            (@this.x, @this.y);

        public static Vector2 Vector2Construct(this (float, float) @this) =>
            new Vector2(@this.Item1, @this.Item2);

        public static Vector2 WithX(this Vector2 @this, float x) =>
            new Vector2(x, @this.y);

        public static Vector2 WithY(this Vector2 @this, float y) =>
            new Vector2(@this.x, y);

        public static Vector2 WithX(this Vector2 @this, Func<float, float> xMap) =>
            new Vector2(xMap(@this.x), @this.y);

        public static Vector2 WithY(this Vector2 @this, Func<float, float> yMap) =>
            new Vector2(@this.x, yMap(@this.y));

        public static Vector2 With(this Vector2 @this, float? x = null, float? y = null) =>
            (x == null ? @this.x : x.Value, y == null ? @this.y : y.Value)
            .Vector2Construct();

        public static Vector3 ToVector3Z(this Vector2 @this, float z) =>
            new Vector3(@this.x, @this.y, z);

        public static Vector3 AsXZ(this Vector2 @this) =>
            new Vector3(@this.x, 0, @this.y);

        public static float RandomRange(this Vector2 minMax) =>
                UnityEngine.Random.Range(minMax.x, minMax.y);

        public static float JoystickDot(this Vector2 @this) =>
            Vector2.Dot(Vector2.up, @this);

        public static bool ValueInForwardDot(this Vector2 @this, float range = .7f) =>
            @this.JoystickDot() > range;

        public static float JoystickAngle(this Vector2 @this) =>
            Vector2.Angle(Vector2.up, @this);

        public static int RandomRange(this Vector2Int @this) =>
            UnityEngine.Random.Range(@this.x, @this.y);

    }
}
