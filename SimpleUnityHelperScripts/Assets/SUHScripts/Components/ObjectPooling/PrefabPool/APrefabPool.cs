using UnityEngine;

namespace SUHScripts
{
    /// <summary>
    /// A pool that automates spawning and despawning any prefab, executes callback on IPoolableComponent
    /// </summary>
    public abstract class APrefabPool
    {
        public abstract T GetFromPrefab<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component;
        //public abstract bool Release<T>(T instance) where T : Component;
        public abstract GameObject GetFromPrefab(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null);
        public abstract bool Release(GameObject instance);
    }
}
