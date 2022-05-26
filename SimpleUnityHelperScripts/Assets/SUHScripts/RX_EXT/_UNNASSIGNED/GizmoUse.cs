using UniRx;
using System;
using UnityEngine;

namespace SUHScripts
{
    public static class GizmoUse
    {
        static Color m_cacheColor = Gizmos.color;

        static IDisposable m_gizmosColorDisposable =
            Disposable.Create(() =>
            {
                Gizmos.color = m_cacheColor;
            });

        public static IDisposable Color(Color color)
        {
            m_cacheColor = Gizmos.color;
            Gizmos.color = color;
            return m_gizmosColorDisposable;
        }

        public static void DrawSphereLine(Vector3 startPos, Vector3 direction, float distance, float radius, int length = 100, Color? color = null)
        {
            var c = color ?? UnityEngine.Color.white;
            using (GizmoUse.Color(c))
            {
                var div = distance / length;
                var dir = direction.normalized;

                for (int i = 0; i < length; i++)
                {
                    Gizmos.DrawSphere(startPos + (dir * (div * i)), radius);
                }
            }
                
        }
    }

}

