using UnityEngine;

namespace SUHScripts
{
    /// <summary>
    /// Represents world space orientation of a transform
    /// </summary>
    [System.Serializable]
    public struct TransformOrientation
    {
        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; }

        public TransformOrientation(Vector3 position, Quaternion rotation)
        {
            this.Position = position;
            this.Rotation = rotation;
        }
    }
}

