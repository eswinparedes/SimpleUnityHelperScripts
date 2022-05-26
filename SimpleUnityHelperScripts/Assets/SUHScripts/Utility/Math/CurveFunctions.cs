using UnityEngine;

namespace SUHScripts
{
    public static class CurveFunctions
    {
        public static float SmoothStepAlpha(float Alpha) =>
            Alpha * Alpha * (3f - 2f * Alpha);

        public static float SmootherStepAlpha(float Alpha) =>
            Alpha * Alpha * Alpha * (Alpha * (6f * Alpha - 15f) + 10f);

        public static float EaseOutAlpha(float Alpha) =>
            Mathf.Sin(Alpha * Mathf.PI * 0.5f);

        public static float EaseInAlpha(float Alpha) =>
            1f - Mathf.Cos(Alpha * Mathf.PI * 0.5f);

        public static float ExponentialAlpha(float Alpha) =>
            Alpha * Alpha;

    }
}

