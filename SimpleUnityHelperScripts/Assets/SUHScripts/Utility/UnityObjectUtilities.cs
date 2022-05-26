using UnityEngine;
using SUHScripts.Functional;

namespace SUHScripts
{
    public static class UnityObjectUtilities
    {
        public static T GetOrAddComponent<T>(this GameObject @this) where T : Component
        {
            var comp = @this.GetComponent<T>();
            if (comp != null)
            {
                return comp;
            }
            else
            {
                return @this.AddComponent<T>();
            }
        }

        public static Option<T> GetComponentOption<T>(this GameObject @this) 
        {
            var comp = @this.GetComponent<T>();

            return Option.SAFE(comp);
        }

        public static Option<T> GetComponentOption<T>(this Component @this) 
        {
            var comp = @this.GetComponent<T>();

            return Option.SAFE(comp);
        }

        public static Ray GetRay(this Transform @this)
        {
            return new Ray(@this.position, @this.forward);
        }

        public static void ConstrainedLookAt(this Transform trans, Vector3 position, Vector3 constraints)
        {
            Vector3 pos = trans.position;
            pos = Vector3.Scale(pos, Vector3.one - constraints);
            position = Vector3.Scale(position, constraints);
            Vector3 lookPos = pos + position;
            trans.LookAt(lookPos, trans.up);
        }

        public static Quaternion LookToMouseRotation_OnX(this Transform _transform)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 5f;

            Vector3 objectPos = Camera.main.WorldToScreenPoint(_transform.position);

            mousePos.x = mousePos.x - objectPos.x;
            mousePos.y = mousePos.y - objectPos.y;

            float angle = -Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg + Camera.main.transform.rotation.eulerAngles.z;

            return Quaternion.Euler(new Vector3(angle, _transform.rotation.x, _transform.rotation.y));
        }

        

    }
}

