using UnityEngine;

namespace SUHScripts
{
    public abstract class AAudioOneShotRequestSO : ScriptableObject
    {
        public abstract void Play(AudioSource audioSource, Vector3 position = default, float volumeMultiplier = 1, float pitchMultiplier = 1);
        public abstract void Play(AAudioOneShotPool pool, Vector3 position = default, float volumeMultiplier = 1, float pitchMultiplier = 1);
    }

}
