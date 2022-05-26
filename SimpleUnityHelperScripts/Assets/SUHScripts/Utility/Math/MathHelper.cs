using UnityEngine;

namespace SUHScripts
{
    public static class MathHelper 
    {
        public static Vector2 Range01 => new Vector2(0, 1);

        public static float Remap(float inValue, Vector2 inMinMax, Vector2 outMinMax) =>
            outMinMax.x + (inValue - inMinMax.x) * (outMinMax.y - outMinMax.x) /
            (inMinMax.y - inMinMax.x);

        public static float Remap(float inValue, float inMin, float inMax, float outMin, float outMax) =>
            outMin + (inValue - inMin) * (outMax - outMin) /
            (inMax - inMin);

        public static int CoordinateToFlattenedIndex(Vector2Int coordinate, Vector2Int widthHeight) =>
            coordinate.x + widthHeight.x * coordinate.y;

        public static Vector2Int FlattenedToCoordinateIndex(int index, Vector2Int widthHeight) =>
            new Vector2Int(index % widthHeight.x, index / widthHeight.x);

        public static float GetCenterAngleDeg(float angle1, float angle2) =>
            angle1 + Mathf.DeltaAngle(angle1, angle2) / 2f;

        public static float NormalizeAngleDeg360(float angle)
        {
            while (angle < 0)
                angle += 360;

            if (angle >= 360)
                angle %= 360;

            return angle;
        }

        public static float NormalizeAngleDeg180(float angle)
        {
            while (angle < -180)
                angle += 360;

            while (angle >= 180)
                angle -= 360;

            return angle;
        }

        public static float AngleDegreesFrom360(float x, float y) =>
            NormalizeAngleDeg360(AngleDegreesFrom(x, y));

        public static float AngleDegreesFrom(float x, float y) =>
            AngleRadiansFrom(x, y) * Mathf.Rad2Deg;

        public static float AngleRadiansFrom(float x, float y) =>
            Mathf.Atan2(x, y);

        public static float AngleOffAroundAxisRadians(Vector3 v, Vector3 forward, Vector3 axis)
        {
            Vector3 right = Vector3.Cross(axis, forward);
            forward = Vector3.Cross(right, axis);
            Vector2 v2 = new Vector2(Vector3.Dot(v, forward), Vector3.Dot(v, right));
            v2.Normalize();
            return Mathf.Atan2(v2.y, v2.x);
        }

        public static float AngleOffAroundAxisDegrees(Vector3 v, Vector3 forward, Vector3 axis) =>
            AngleOffAroundAxisRadians(v, forward, axis) * Mathf.Rad2Deg;

        public static Vector3 OrthoNormalVector(Vector3 relativeForward, Vector3? relativeUp = null)
        {
            Vector3 up = relativeUp ?? Vector3.up;

            Vector3 refForward = relativeForward;
            Vector3 refUp = up;

            Vector3.OrthoNormalize(ref refUp, ref refForward);

            return refForward;
        }

        public static Quaternion OrthoNormalRotation(Vector3 relativeForward, Vector3? relativeUp = null) =>
            Quaternion.LookRotation(OrthoNormalVector(relativeForward, relativeUp));

        public static Vector3 OrthoNormalMoveDirection(Vector3 localMoveDirection, Vector3 relativeForward, Vector3? relativeUp = null) =>
            OrthoNormalRotation(relativeForward, relativeUp) * localMoveDirection;

        public static Quaternion ClampRotationAroundXAxis(Quaternion q, float minAngle, float maxAngle)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

            angleX = Mathf.Clamp(angleX, minAngle, maxAngle);

            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }

        public static Quaternion XLookRotation(Vector3 right, Vector3 up = default)
        {
            if (up == default)
                up = Vector3.up;

            Quaternion rightToForward = Quaternion.Euler(0f, -90f, 0f);
            Quaternion forwardToTarget = Quaternion.LookRotation(right, up);

            return forwardToTarget * rightToForward;
        }

        public static Quaternion XLookRotation2D(Vector3 right)
        {
            Quaternion rightToUp = Quaternion.Euler(0f, 0f, 90f);
            Quaternion upToTarget = Quaternion.LookRotation(Vector3.forward, right);

            return upToTarget * rightToUp;
        }

    }
}
