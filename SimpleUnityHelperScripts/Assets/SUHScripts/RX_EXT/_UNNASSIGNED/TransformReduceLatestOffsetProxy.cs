using System.Collections.Generic;
using UnityEngine;
using System;

namespace SUHScripts
{
    [DisallowMultipleComponent]
    public class TransformReduceLatestOffsetProxy : MonoBehaviour
    {
        ReduceLatestProxy<Vector3> m_rotationLatestProxy;
        List<Func<float, Vector3>> m_rotationLatestSources = new List<Func<float, Vector3>>();

        ReduceLatestProxy<Vector3> m_scaleLatestProxy;
        List<Func<float, Vector3>> m_scaleLatestSources = new List<Func<float, Vector3>>();

        ReduceLatestProxy<Vector3> m_positionLatestProxy;
        List<Func<float, Vector3>> m_positionLatestSources = new List<Func<float, Vector3>>();

        Vector3 m_sourceLocalPosition;
        Vector3 m_sourceLocalEuler;
        Vector3 m_sourceLocalScale;

        public bool UseUnscaledTime { get; set; }

        private void Awake()
        {
            Func<Vector3, Vector3, Vector3> aggregator = (v0, v1) => v0 + v1;
            Func<int, Vector3, Vector3> reducer = (i, v) => v;
            Func<Vector3, Vector3, Vector3> interpolator = (vCurrent, vNext) => Vector3.Lerp(vCurrent, vNext, 7f * (UseUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime));
            Vector3 defaultValue = Vector3.zero;

            m_rotationLatestProxy = new ReduceLatestProxy<Vector3>(aggregator, reducer, interpolator, defaultValue);
            m_scaleLatestProxy = new ReduceLatestProxy<Vector3>(aggregator, reducer, interpolator, defaultValue);
            m_positionLatestProxy = new ReduceLatestProxy<Vector3>(aggregator, reducer, interpolator, defaultValue);

            m_sourceLocalPosition = this.transform.localPosition;
            m_sourceLocalEuler = this.transform.localEulerAngles;
            m_sourceLocalScale = this.transform.localScale;
        }

        private void Update()
        {
            var dt = UseUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

            var localPositionSource = Vector3.zero;
            for(int i =0; i < m_positionLatestSources.Count; i++)
            {
                localPositionSource += m_positionLatestSources[i](dt);
            }

            var localEulerSource = Vector3.zero;
            for (int i = 0; i < m_rotationLatestSources.Count; i++)
            {
                localEulerSource += m_rotationLatestSources[i](dt);
            }

            var localScaleSource = Vector3.zero;
            for (int i = 0; i < m_scaleLatestSources.Count; i++)
            {
                localScaleSource += m_scaleLatestSources[i](dt);
            }

            var localEulerOffset = m_rotationLatestProxy.Update() + localEulerSource + m_sourceLocalEuler;
            var localScaleOFfset = m_scaleLatestProxy.Update() + localScaleSource + m_sourceLocalScale;
            var localPositionOffset = m_positionLatestProxy.Update() + localPositionSource + m_sourceLocalPosition;

            this.transform.localEulerAngles = localEulerOffset;
            this.transform.localScale = localScaleOFfset;
            this.transform.localPosition = localPositionOffset;
        }

        public void SendLatestLocalPostionValue(object key, Vector3 localPostion)
        {
            m_positionLatestProxy.SendLatest(key, localPostion);
        }

        public void PushLocalPositionSource(Func<float, Vector3> localPositionSource)
        {
            m_positionLatestSources.Add(localPositionSource);
        }

        public bool TryRemoveLocalPositionSource(Func<float, Vector3> localPositionSource)
        {
            return m_positionLatestSources.Remove(localPositionSource);
        }



        public void SendLatestLocalEulerValue(object key, Vector3 localEuler)
        {
            m_rotationLatestProxy.SendLatest(key, localEuler);
        }

        public void PushLocalEulerSource(Func<float, Vector3> localEulerSource)
        {
            m_rotationLatestSources.Add(localEulerSource);
        }

        public bool TryRemoveLocalEulerSource(Func<float, Vector3> localEulerSource)
        {
            return m_rotationLatestSources.Remove(localEulerSource);
        }



        public void PushLocalScaleValue(object key, Vector3 localScale)
        {
            m_scaleLatestProxy.SendLatest(key, localScale);
        }

        public void PushLocalScaleSource(Func<float, Vector3> localScaleSource)
        {
            m_scaleLatestSources.Add(localScaleSource);
        }

        public bool TryRemoveLocalScaleSource(Func<float, Vector3> localScaleSource)
        {
            return m_scaleLatestSources.Remove(localScaleSource);
        }
    }
}

