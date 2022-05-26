using UnityEngine;

namespace SUHScripts
{
    using Functional;
    public struct SphereCastData 
    {
        public readonly Ray SourceRay;
        public readonly float Radius;
        public Option<RaycastHit> RaycastHitOption;

        public SphereCastData(Ray sourceRay,
            float radius, Option<RaycastHit> hitData)
        {
            this.SourceRay = sourceRay;
            this.Radius = radius;
            this.RaycastHitOption = hitData;
        }
    }
}