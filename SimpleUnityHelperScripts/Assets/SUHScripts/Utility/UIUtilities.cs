using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace SUHScripts
{
    public static class UIUtilities
    {
        public static bool IsPointerOverUIObject(Vector2 position)
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = position;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
    }
}

