using UnityEngine;

namespace SUHScripts
{
    public static class Interpolation
    {
        public static float FlerpTime(float percentPerSecond, float deltaTime) =>
            1 - Mathf.Pow(1 - percentPerSecond, deltaTime);

        public static float Flerp(float current, float target, float percentPerSecond, float deltaTime)
        {
            var flerpTime = FlerpTime(percentPerSecond, deltaTime);
            return Mathf.Lerp(current, target, flerpTime);
        }

        public static float Flerp(float current, float target, float flerpTime) =>
            Mathf.Lerp(current, target, flerpTime);

        public static Vector3 Flerp(Vector3 current, Vector3 target, float percentPerSecond, float deltaTime)
        {
            float flerpTime = FlerpTime(percentPerSecond, deltaTime);
            float _x = Flerp(current.x, target.x, flerpTime);
            float _y = Flerp(current.y, target.y, flerpTime);
            float _z = Flerp(current.z, target.z, flerpTime);
            return new Vector3(_x, _y, _z);
        }

        public static Vector3 Flerp(Vector3 current, Vector3 target, float flerpTime)
        {
            float _x = Flerp(current.x, target.x, flerpTime);
            float _y = Flerp(current.y, target.y, flerpTime);
            float _z = Flerp(current.z, target.z, flerpTime);
            return new Vector3(_x, _y, _z);
        }

        public static float Fractional(float from, float to, float coefficient)
        {
            return (to - from) * coefficient;
        }

        public static float Fractional(float from, float to, float coefficient, float min, float max)
        {
            float val = to - from;
            float smooth = Mathf.Clamp(Mathf.Abs(val) * coefficient, min, max) * Mathf.Sign(val);

            if (Mathf.Abs(smooth) > Mathf.Abs(val))
            {
                smooth = val;
            }
            return smooth;
        }

        public static Vector3 Fractional(Vector3 from, Vector3 to, float coefficient)
        {
            Vector3 val = to - from;
            Vector3 smooth = val * coefficient;

            return smooth;
        }

        public static Vector3 Fractional(Vector3 from, Vector3 to, float coefficient, float minMagnitude, float maxMagnitude)
        {
            Vector3 val = to - from;
            Vector3 smooth = val * coefficient;

            float mag = Mathf.Clamp(smooth.magnitude, minMagnitude, maxMagnitude);

            if (mag > val.magnitude)
            {
                smooth = val;
            }
            else
            {
                smooth = smooth.normalized * mag;
            }
            return smooth;
        }
    }
}

