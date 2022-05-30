using UnityEngine;
using SUHScripts.Functional;
using SUHScripts;

namespace SUHScripts
{

    [System.Serializable]
    public struct NoisePulse3D
    {
        [SerializeField] float m_frequency;
        [SerializeField] float m_amplitude;
        [SerializeField] float m_duration;
        [SerializeField] Vector2 m_offsetRange;
        [SerializeField] AnimationCurve m_curve;
        float m_timeRemaining;

        public NoisePulse3D(float frequency, float amplitude, float duration, Vector2 offsetRange, AnimationCurve evalCurve)
        {
            m_frequency = frequency;
            m_amplitude = amplitude;
            m_runningOffset = SUHScripts.Vector3Extensions.RandomComponents(offsetRange);
            m_duration = duration;
            m_timeRemaining = duration;
            CurrentValue = Vector3.zero.AsOption_UNSAFE();
            m_curve = evalCurve;
            m_hasStarted = true;
            m_offsetRange = offsetRange;
        }

        public NoisePulse3D Reseeded()
        {
            return new NoisePulse3D(m_frequency, m_amplitude, m_duration, m_offsetRange, m_curve);
        }

        public float Frequency => m_frequency;
        public float Amplitude => m_amplitude;
        public float Duration => m_duration;
        public AnimationCurve EvalCurve => m_curve;
        public Vector3 RunningOffset => m_runningOffset;
        public Vector2 OffsetRange => m_offsetRange;
        public Option<Vector3> CurrentValue { get; private set; }

        bool m_hasStarted;
        Vector3 m_runningOffset;
        public Option<Vector3> Update(float delta)
        {
            if (!m_hasStarted)
            {
                m_timeRemaining = m_duration;
                m_hasStarted = true;
                m_runningOffset = SUHScripts.Vector3Extensions.RandomComponents(m_offsetRange);
            }

            m_timeRemaining -= delta;

            float noiseOffsetDelta = delta * m_frequency;

            m_runningOffset.x += noiseOffsetDelta;
            m_runningOffset.y += noiseOffsetDelta;
            m_runningOffset.z += noiseOffsetDelta;

            Vector3 noise = Vector3.zero;
            noise.x = Mathf.PerlinNoise(m_runningOffset.x, 0.0f);
            noise.y = Mathf.PerlinNoise(m_runningOffset.y, 1.0f);
            noise.z = Mathf.PerlinNoise(m_runningOffset.z, 2.0f);

            noise -= Vector3.one * 0.5f;

            noise *= m_amplitude;

            float agePercent = 1.0f - (m_timeRemaining / m_duration);
            noise *= m_curve.Evaluate(agePercent);

            CurrentValue = noise.AsOption_UNSAFE();

            if (m_timeRemaining > 0.0f)
            {
                return CurrentValue;
            }
            else
            {
                return None.Default;
            }
        }

        public NoisePulse3D WithAlpha(float amplitudeAlpha = 1, float frequencyAlpha = 1)
        {
            return new NoisePulse3D(
                Frequency * frequencyAlpha,
                Amplitude * amplitudeAlpha,
                m_duration,
                OffsetRange,
                EvalCurve);
        }
    }

    [System.Serializable]
    public struct NoisePulse
    {
        [SerializeField] float m_frequency;
        [SerializeField] float m_amplitude;
        [SerializeField] float m_duration;
        [SerializeField] Vector2 m_offsetRange;
        [SerializeField] AnimationCurve m_curve;
        float m_timeRemaining;
        public NoisePulse(float frequency, float amplitude, float duration, Vector2 offsetRange, AnimationCurve evalCurve)
        {
            m_frequency = frequency;
            m_amplitude = amplitude;
            m_runningOffset = offsetRange.RandomRange();
            m_duration = duration;
            m_timeRemaining = duration;
            CurrentValue = 0f.AsOption_UNSAFE();
            m_curve = evalCurve;
            m_hasStarted = true;
            m_offsetRange = offsetRange;
        }

        public NoisePulse Reseeded()
        {
            return new NoisePulse(m_frequency, m_amplitude, m_duration, m_offsetRange, m_curve);
        }

        public float Frequency => m_frequency;
        public float Amplitude => m_amplitude;
        public float Duration => m_duration;
        public AnimationCurve EvalCurve => m_curve;
        public float RunningOffset => m_runningOffset;
        public Option<float> CurrentValue { get; private set; }

        bool m_hasStarted;
        float m_runningOffset;
        public Option<float> Update(float delta)
        {
            if (!m_hasStarted)
            {
                m_timeRemaining = m_duration;
                m_hasStarted = true;
                m_runningOffset = m_offsetRange.RandomRange();
            }

            m_timeRemaining -= delta;

            float noiseOffsetDelta = delta * m_frequency;

            m_runningOffset += noiseOffsetDelta;

            var noise = 0f;

            noise = Mathf.PerlinNoise(m_runningOffset, 0.0f);


            noise -= 0.5f;

            noise *= m_amplitude;

            float agePercent = 1.0f - (m_timeRemaining / m_duration);
            noise *= m_curve.Evaluate(agePercent);

            CurrentValue = noise.AsOption_UNSAFE();

            if (m_timeRemaining > 0.0f)
            {
                return CurrentValue;
            }
            else
            {
                return None.Default;
            }
        }
    }

}