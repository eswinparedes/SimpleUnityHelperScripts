using System.Collections.Generic;
using UnityEngine;

namespace SUHScripts
{
    public class AudioOneShotPool : AAudioOneShotPool
    {
        public AudioOneShotPool(AudioSource audioSourcePrefab)
        {
            m_audioSourcePool = new HashSetObjectPool<AudioSource>(
                onInstantiateFunction: () => GameObject.Instantiate(audioSourcePrefab),
                onDestroyActionOption: a =>
                {
                    if (a == null) return;
                    GameObject.Destroy(a.gameObject);
                },
                onGetActionOption: a =>
                {
                    if (a == null) return;

                    a.gameObject.SetActive(true);
                },
                onReleaseActionOption: a =>
                {
                    if (a == null) return;

                    a.Stop();
                    a.gameObject.SetActive(false);
                });
        }

        HashSetObjectPool<AudioSource> m_audioSourcePool;
        List<AudioSource> m_cacheList = new List<AudioSource>();

        public override AudioSource Get()
        {
            return m_audioSourcePool.Get();
        }

        /// <summary>
        /// Release spawned audio back into the pool
        /// </summary>
        public void Update()
        {
            foreach(var source in m_audioSourcePool.WaitingForRelease)
            {
                if (!source.isPlaying)
                {
                    m_cacheList.Add(source);
                }
            }

            for(int i = 0; i < m_cacheList.Count; i++)
            {
                m_audioSourcePool.Release(m_cacheList[i]);
            }

            m_cacheList.Clear();
        } 

        public void Clear()
        {
            m_audioSourcePool.Clear();
        }
    }

}
