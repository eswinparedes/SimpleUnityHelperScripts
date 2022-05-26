using UnityEngine;

namespace SUHScripts
{
    [CreateAssetMenu(menuName = "SUHS/Audio/Composite Audio One Shot Request")]
    public class AudioOneShotRequestCompositeSO: AAudioOneShotRequestSO
    {
        [SerializeField] AAudioOneShotRequestSO[] m_oneShotRequestSOs = default;

        public override void Play(AudioSource audioSource, Vector3 position = default, float volumeMultiplier = 1, float pitchMultiplier = 1)
        {
            for (int i = 0; i < m_oneShotRequestSOs.Length; i++)
            {
                m_oneShotRequestSOs[i].Play(audioSource, position, volumeMultiplier, pitchMultiplier);
            }
        }
    
        public override void Play(AAudioOneShotPool pool, Vector3 position = default, float volumeMultiplier = 1, float pitchMultiplier = 1)
        {
            for(int i = 0; i < m_oneShotRequestSOs.Length; i++)
            {
                m_oneShotRequestSOs[i].Play(pool, position, volumeMultiplier, pitchMultiplier);
            }
        }
    }

}
