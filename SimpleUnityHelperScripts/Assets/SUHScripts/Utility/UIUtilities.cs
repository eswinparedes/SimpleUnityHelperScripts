using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace SUHScripts
{
    public static class UIUtilities
    {
        public static bool IsPointerOverUIObject(Vector2 position)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }

            return IsPointerOverUIObject(position, new List<RaycastResult>());
        }

        public static bool IsPointerOverUIObject(Vector2 position, List<RaycastResult> raycastResultsCache)
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = position;
            raycastResultsCache.Clear();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, raycastResultsCache);
            return raycastResultsCache.Count > 0;
        }

        public static bool IsPointerOverUIObject(Vector2 position, out List<RaycastResult> results)
        {
            results = new List<RaycastResult>();
            return IsPointerOverUIObject(position, results);
        }

        public static bool IsPointerOverUIObject(Vector2 position, out RaycastResult result)
        {
            var results = new List<RaycastResult>();
            var didHit = IsPointerOverUIObject(position, results);
            result = didHit ? results[0] : default;
            return didHit;
        }
    }
}

