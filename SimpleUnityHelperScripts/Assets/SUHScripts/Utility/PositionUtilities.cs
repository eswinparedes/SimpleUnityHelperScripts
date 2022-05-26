using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using SUHScripts.Functional;

namespace SUHScripts
{
    public static class PositionUtilities
{
    public static Option<T> TryClosest<T>(this IReadOnlyList<T> @this, Vector2 point) where T : Component
    {
        var count = @this.Count;

        if (count == 0) return None.Default;

        Option<T> nearest = None.Default;
        var nearestDist = float.MaxValue;

        foreach (var i in @this)
        {
            var dist = Vector2.Distance(i.transform.position, point);
            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearest = i.AsOption_SAFE();
            }
        }

        return nearest;

    }

    public static Option<T> TryClosest<T>(this IReadOnlyList<T> @this, Vector3 point) where T : Component
    {
        var count = @this.Count;

        if (count == 0) return None.Default;

        Option<T> nearest = None.Default;
        var nearestDist = float.MaxValue;

        foreach (var i in @this)
        {
            var dist = Vector3.Distance(i.transform.position, point);
            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearest = i.AsOption_SAFE();
            }
        }

        return nearest;

    }

    public static Option<TGetComponent> TryClosestComponentWhere<TComponent, TGetComponent>(this IReadOnlyList<TComponent> @this, Func<TGetComponent, bool> where, Vector3 point) where TComponent : Component
    {
        var count = @this.Count;

        if (count == 0) return None.Default;

        Option<TGetComponent> nearest = None.Default;
        var nearestDist = float.MaxValue;

        for (int i = 0; i < @this.Count; i++)
        {
            var component = @this[i];
            var getComponentOption = component.GetComponentOption<TGetComponent>();
            var dist = Vector3.Distance(component.transform.position, point);

            if (!getComponentOption.IsSome) continue;

            var isValid = where(getComponentOption.Value);

            if (!isValid) continue;

            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearest = getComponentOption;
            }
        }

        return nearest;
    }

   
}

}
