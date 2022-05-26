using UnityEngine;
using static SUHScripts.Functional.Functional;

namespace SUHScripts
{
    using Functional;
    using System.Collections.Generic;

    public static class PhysicsCasting
    {
        //SUHS todo: Create overloads that just get an Option<RaycastHit> for lightweight version
        #region Raycasting
        //TODO make speparate struct?
        public static RaycastData PhysicsLinecast(Vector3 prev, Vector3 next, LayerMask mask)
        {
            Ray sourceRay = new Ray(prev, next - prev);
            bool didHit = Physics.Linecast(prev, next, out RaycastHit hit, mask);
            return new RaycastData(sourceRay, didHit ? hit.AsOption_UNSAFE() : (Option<RaycastHit>)None.Default);
        }

        public static RaycastData Raycast(Ray ray, float range = float.PositiveInfinity, LayerMask? mask = null)
        {
            LayerMask _mask = mask == null ? (LayerMask)~0 : mask.Value;
            return WorldCastGetOutcome(ray.origin, ray.direction, range, _mask);
        }

        public static RaycastData Raycast(Vector3 origin, Vector3 direction, float range = float.PositiveInfinity, LayerMask? mask = null) =>
            Raycast(new Ray(origin, direction), range, mask);

        public static RaycastData Raycast(Transform rayIdentity, float range = float.PositiveInfinity, LayerMask? mask = null) =>
            Raycast(new Ray(rayIdentity.position, rayIdentity.forward), range, mask);

        public static RaycastData Raycast(Camera screen, Vector3 screenPoint, float range = float.PositiveInfinity, LayerMask? mask = null) =>
            Raycast(screen.ScreenPointToRay(screenPoint), range, mask);

        public static RaycastData WorldCastGetOutcome(Vector3 origin, Vector3 direction, float range, LayerMask mask) =>
            WorldCastGetOutcome(new Ray(origin, direction), range, mask);

        public static RaycastData WorldCastGetOutcome(Ray ray, float range, LayerMask mask)
        {
            if (Physics.Raycast(ray, out RaycastHit outcomeRaycastHit, range, mask))
            {
                return new RaycastData(ray, outcomeRaycastHit.AsOption_UNSAFE());
            }
            else
            {
                return new RaycastData(ray, None.Default);
            }
        }

        #endregion

        #region SphereCasting
        public static SphereCastData SphereCast(Vector3 origin, float radius, Vector3 direction,
            float maxdistance, int layerMask = ~0, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore)
        {
            RaycastHit hit;
            bool didHit = Physics.SphereCast(origin, radius, direction, out hit, maxdistance, layerMask, queryTriggerInteraction);

            Debug.Log($"Sphere cast hit :{didHit}");

            var hitOption = didHit ? hit.AsOption_UNSAFE() : None.Default;

            return new SphereCastData(new Ray(origin, direction), radius, hitOption);
        }

        public static RaycastData SphereCastRaycastData(Vector3 origin, float radius, Vector3 direction,
            float maxdistance, int layerMask = ~0, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore)
        {
            RaycastHit hit;
            bool didHit =
                Physics.SphereCast(origin, radius, direction, out hit, maxdistance, layerMask, queryTriggerInteraction);

            var hitOption = didHit ? hit.AsOption_UNSAFE() : None.Default;

            return new RaycastData(new Ray(origin, direction), hitOption);
        }
        #endregion

        public static void GetPointsAroundFacingForward(List<Vector3> source, Vector3 origin, Vector3 up, Vector3 forward, float radius, int count)
        {
            source.Clear();

            Vector3 zeroOffsetDir = up;
            var angleDelta = (float) 360 / count;

            for(int i = 0; i < count; i++)
            {
                var angle = angleDelta * i;

                var rot = Quaternion.AngleAxis(angle, forward);
                var offset = origin + (radius * ( rot * zeroOffsetDir));
                source.Add(offset);
            }
        }

        public static Option<RaycastData> CylinderRaycastNearest(List<Vector3> source, List<RaycastData> results, Vector3 origin, Vector3 up, Vector3 forward, float distance, float radius, int count, LayerMask mask)
        {
            GetPointsAroundFacingForward(source, origin, up, forward, radius, count);

            results.Clear();
            RaycastData nearest = default;
            var distCurrent = 0f;
            var hasValue = false;

            for(int i = 0; i < source.Count; i++)
            {
                var result = PhysicsCasting.Raycast(source[i], forward, distance, mask);

                if (result.RaycastHitOption.IsSome)
                {
                    if (hasValue)
                    {
                        var dist = Vector3.Distance(result.RaycastHitOption.Value.point, origin);
                        if(dist < distCurrent)
                        {
                            nearest = result;
                        }
                    }
                    else
                    {
                        nearest = result;
                        distCurrent = Vector3.Distance(nearest.RaycastHitOption.Value.point, origin);
                        hasValue = true;
                    }
                }
                results.Add(result);
            }

            return hasValue ? nearest.AsOption_UNSAFE() : None.Default;
        }
    }
}

