using SUHScripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

namespace SUHScripts
{
    public class HashSetPrefabPool : APrefabPool
    {
        Dictionary<GameObject, HashSetObjectPool<GameObject>> m_prefabSourcesMappedToTheirPools = new Dictionary<GameObject, HashSetObjectPool<GameObject>>();
        Dictionary<GameObject, HashSetObjectPool<GameObject>> m_prefabsWaitingToBeReleasedMappedToTheirOwningPool = new Dictionary<GameObject, HashSetObjectPool<GameObject>>();

        Dictionary<object, IPoolableComponent[]> m_instancesMappedToTheirCorrespondingPoolableComponents = new Dictionary<object, IPoolableComponent[]>();

        public override T GetFromPrefab<T>(T prefab, Vector3 position, Quaternion rotation)
        {
            return GetFromPrefab(prefab.gameObject, position, rotation).GetComponent<T>();
        }

        public override GameObject GetFromPrefab(GameObject prefab, Vector3 position, Quaternion rotation, Transform simulateParent = null)
        {
            if (!m_prefabSourcesMappedToTheirPools.ContainsKey(prefab))
            {
                var newPool = new HashSetObjectPool<GameObject>(
                    onInstantiateFunction: () => OnPoolInstantiateGameObject(prefab),
                    onDestroyActionOption: OnPoolDestroyGameObject,
                    null,
                    null);

                m_prefabSourcesMappedToTheirPools.Add(prefab, newPool);
            }

            var pool = m_prefabSourcesMappedToTheirPools[prefab];
            var go = pool.Get();

            m_prefabsWaitingToBeReleasedMappedToTheirOwningPool.Add(go, pool);

            var transform = go.transform;

            transform.position = position;
            transform.rotation = rotation;

            if(simulateParent != null)
            {
                transform.parent = simulateParent;
            }

            go.SetActive(true);

            if (m_instancesMappedToTheirCorrespondingPoolableComponents.ContainsKey(go))
            {
                var c = m_instancesMappedToTheirCorrespondingPoolableComponents[go];

                for (int i = 0; i < c.Length; i++)
                {
                    c[i].OnGet();
                }
            }

            return go;
        }

        public override bool Release(GameObject instance)
        {
            if (!m_prefabsWaitingToBeReleasedMappedToTheirOwningPool.ContainsKey(instance))
            {
                Debug.LogWarning($"GameObject: {instance.name} not managed by this pooling system!");
                return false;
            }

            //This is how the prefab instance is released back into the pool so we can get it
            var pool = m_prefabsWaitingToBeReleasedMappedToTheirOwningPool[instance];
            var didRelease = pool.Release(instance);
            //Need to remove this since its only for prefabs yet to be released
            m_prefabsWaitingToBeReleasedMappedToTheirOwningPool.Remove(instance);

            if (!didRelease)
            {
                Debug.LogError($"instance {instance.name} not managed by internal pool!");
                return false;
            }
        
            //This usually happens because prefab pool is cleared on scene end but the destroy order is unknown or uncontrollable, unity overrides == null
            if (instance == null)
            {
                if (m_instancesMappedToTheirCorrespondingPoolableComponents.ContainsKey(instance))
                {
                    m_instancesMappedToTheirCorrespondingPoolableComponents.Remove(instance);
                }

                return false;
            }

            //in case this object has been parented, remove it
            var transform = instance.transform;
            if(transform is RectTransform t)
            {
                t.SetParent(null, false);
            }
            else
            {
                transform.parent = null;
            }

            //Call On Released for any poolable components
            if (m_instancesMappedToTheirCorrespondingPoolableComponents.ContainsKey(instance))
            {
                var c = m_instancesMappedToTheirCorrespondingPoolableComponents[instance];

                for (int i = 0; i < c.Length; i++)
                {
                    c[i].OnRelease();
                }
            }

            instance.SetActive(false);

            return didRelease;
        }

        GameObject OnPoolInstantiateGameObject(GameObject prefab)
        {
            var go = GameObject.Instantiate(prefab);

            var poolableComponents = go.GetComponentsInChildren<IPoolableComponent>();

            if (poolableComponents != null && poolableComponents.Length > 0)
            {
                m_instancesMappedToTheirCorrespondingPoolableComponents.Add(go, poolableComponents);
            }

            return go;
        }

        void OnPoolDestroyGameObject(GameObject go)
        {
            //this usually happens because destory order is unpredictable and the pool was cleared on destroy
            if(go == null)
            {
                if (m_instancesMappedToTheirCorrespondingPoolableComponents.ContainsKey(go))
                {
                    m_instancesMappedToTheirCorrespondingPoolableComponents.Remove(go);
                }
            }
            else
            {
                if (m_instancesMappedToTheirCorrespondingPoolableComponents.ContainsKey(go))
                {
                    m_instancesMappedToTheirCorrespondingPoolableComponents.Remove(go);
                }

                GameObject.Destroy(go);
            }
        }

        public void Clear()
        {
            foreach (var kvp in m_prefabSourcesMappedToTheirPools)
            {
                kvp.Value.Clear();
            }

            m_prefabSourcesMappedToTheirPools.Clear();
            m_prefabsWaitingToBeReleasedMappedToTheirOwningPool.Clear();
            m_instancesMappedToTheirCorrespondingPoolableComponents.Clear();
        }
    }
}