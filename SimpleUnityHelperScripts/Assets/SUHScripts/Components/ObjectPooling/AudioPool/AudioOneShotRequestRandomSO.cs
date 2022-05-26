using UnityEngine;

namespace SUHScripts
{
    [CreateAssetMenu(menuName = "SUHS/Audio/Random Audio One Shot Request")]
    public class AudioOneShotRequestRandomSO : AAudioOneShotRequestSO
    {
        [SerializeField] Vector2 m_volumeRange = new Vector2(1, 1);
        [SerializeField] Vector2 m_pitchrange = new Vector2(1, 1);
        [SerializeField] Vector2 m_spatialBlendRange = new Vector2(0, 0);
        [SerializeField] AudioClip[] m_clips = default;

        public override void Play(AudioSource audioSource, Vector3 position = default, float volumeMultiplier = 1, float pitchMultiplier = 1)
        {
            audioSource.loop = false;
            audioSource.transform.position = position;
            audioSource.volume = m_volumeRange.RandomRange() * volumeMultiplier;
            audioSource.pitch = m_pitchrange.RandomRange() * pitchMultiplier;
            audioSource.spatialBlend = m_spatialBlendRange.RandomRange();

            audioSource.PlayOneShot(m_clips.RandomElement());
        }
        public override void Play(AAudioOneShotPool pool, Vector3 position = default, float volumeMultiplier = 1, float pitchMultiplier = 1)
        {
            var source = pool.Get();

            Play(source);
        }
    }

}
