using System.Collections.Generic;
using UnityEngine;

namespace SUHScripts
{
    public class NoiseProcessor3D
    {
        List<NoiseConstant3D> m_constantNoiseSources = new List<NoiseConstant3D>();
        List<NoisePulse3D> m_pulseNoiseSources = new List<NoisePulse3D>();
        public Vector3 Noise { get; private set; }
        public void PushConstant(NoiseConstant3D noise)
        {
            m_constantNoiseSources.Add(noise);
        }

        //TODO: FIX GARBAGE ALLOCATION
        public void PushPulse(NoisePulse3D pulse)
        {
            m_pulseNoiseSources.Add(pulse);
        }

        public void ClearAll()
        {
            m_constantNoiseSources.Clear();
            m_pulseNoiseSources.Clear();
        }

        public void ClearNoiseConstants()
        {
            m_constantNoiseSources.Clear();
        }

        public void ClearNoisePulses()
        {
            m_pulseNoiseSources.Clear();
        }
        //TODO: REmove annoying dependency with pulseCurve
        //TODO: remove annoying fact that option can return a NOne.default when it has a non-zero time remaining smaller than delta
        public Vector3 Update(float delta)
        {
            Noise = Vector3.zero;

            for (int i = 0; i < m_constantNoiseSources.Count; i++)
            {
                var c = m_constantNoiseSources[i];
                Noise = c.Update(delta);
                m_constantNoiseSources[i] = c;
            }

            for (int i = m_pulseNoiseSources.Count - 1; i != -1; i--)
            {
                var p = m_pulseNoiseSources[i];
                var n = p.Update(delta);
                m_pulseNoiseSources[i] = p;

                if (n.IsSome)
                {
                    Noise += n.Value;
                }
                else
                {
                    m_pulseNoiseSources.RemoveAt(i);
                }
            }
            return Noise;
        }
    }

    public class NoiseProcessor
    {
        List<NoiseConstant3D> m_constantNoiseSources = new List<NoiseConstant3D>();
        List<NoisePulse3D> m_pulseNoiseSources = new List<NoisePulse3D>();
        public Vector3 Noise { get; private set; }
        public void PushConstant(NoiseConstant3D noise)
        {
            m_constantNoiseSources.Add(noise);
        }

        //TODO: FIX GARBAGE ALLOCATION
        public void PushPulse(NoisePulse3D pulse)
        {
            m_pulseNoiseSources.Add(pulse);
        }

        public void Clear()
        {
            m_constantNoiseSources.Clear();
            m_pulseNoiseSources.Clear();
        }
        //TODO: REmove annoying dependency with pulseCurve
        //TODO: remove annoying fact that option can return a NOne.default when it has a non-zero time remaining smaller than delta
        public Vector3 Update(float delta)
        {
            Noise = Vector3.zero;

            for (int i = 0; i < m_constantNoiseSources.Count; i++)
            {
                var c = m_constantNoiseSources[i];
                Noise = c.Update(delta);
                m_constantNoiseSources[i] = c;
            }

            for (int i = m_pulseNoiseSources.Count - 1; i != -1; i--)
            {
                var p = m_pulseNoiseSources[i];
                var n = p.Update(delta);
                m_pulseNoiseSources[i] = p;

                if (n.IsSome)
                {
                    Noise += n.Value;
                }
                else
                {
                    m_pulseNoiseSources.RemoveAt(i);
                }
            }
            return Noise;
        }
    }
}