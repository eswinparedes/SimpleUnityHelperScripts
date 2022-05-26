using UnityEngine;

namespace SUHScripts
{
    [System.Serializable]
    public struct Ellipse
    {
        [SerializeField] float m_width;
        [SerializeField] float m_height;
        [SerializeField] int m_segments;

        public float EllipseHeight { get => m_height; private set => m_height = value; }
        public float EllipseWidth { get => m_width; private set => m_width = value; }
        public int Segments { get => m_segments; private set => m_segments = value; }

        public Vector2[] EllipsePoints { get; private set; }
        public Vector2 Center { get => new Vector2(EllipseWidth / 2, EllipseHeight / 2); }

        public Ellipse(float width, float height, int segments)
        {
            m_width = width;
            m_height = height;
            m_segments = segments;
            EllipsePoints = new Vector2[0];
        }
        public Vector2 EvaluateElipse(float t)
        {
            float angle = Mathf.Deg2Rad * 360f * t;
            float x = Mathf.Sin(angle) * EllipseWidth;
            float y = Mathf.Cos(angle) * EllipseHeight;
            return new Vector2(x, y);
        }
        public void CalculateElipse()
        {
            EllipsePoints = new Vector2[Segments];

            for (int i = 0; i < Segments; i++)
            {
                float t = (float)i / (float)Segments;
                EllipsePoints[i] = EvaluateElipse(t);
            }
        }
    }
}
