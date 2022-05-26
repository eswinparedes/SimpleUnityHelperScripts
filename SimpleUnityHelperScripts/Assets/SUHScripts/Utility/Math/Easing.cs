using UnityEngine;

namespace SUHScripts
{
    public static class Easing
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

        public static float UpDown(float alpha)
        {
            if(alpha < .5f)
            {
                return MathHelper.Remap(alpha, new Vector2(0, .5f), new Vector3(0, 1));
            }
            else
            {
                return MathHelper.Remap(alpha, new Vector2(.5f, 1), new Vector2(1, 0));
            }
        }

    }
}

