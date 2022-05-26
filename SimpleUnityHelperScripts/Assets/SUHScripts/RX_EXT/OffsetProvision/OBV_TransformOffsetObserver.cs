using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace SUHScripts
{
    public class OBV_TransformOffsetObserver 
    {
        public static void PushLocalPositionOffsetProvider(IObservable<Vector3> offsetProvider, Transform target)
        {
            var obvs = target.gameObject.GetOrAddComponent<TransformOffsetObserver>();
            obvs.PushPositionOffsetProvider(offsetProvider);
        }

        public static void PushLocalEulerOffsetProvider(IObservable<Vector3> offsetProvider, Transform target)
        {
            var obvs = target.gameObject.GetOrAddComponent<TransformOffsetObserver>();
            obvs.PushEulerOffsetProvider(offsetProvider);
        }

        class TransformOffsetObserver : MonoBehaviour
        {
            Subject<IObservable<Vector3>> m_localPositionOffsetProvider = new Subject<IObservable<Vector3>>();
            Subject<IObservable<Vector3>> m_localEulerOffsetProvider = new Subject<IObservable<Vector3>>();
            public Vector3 SourceLocalPositionOffset { get; set; } = Vector3.zero;
            public Vector3 SourceLocalEulerOffset { get; set; }

            private void Awake()
            {

                SourceLocalPositionOffset = transform.localPosition;

                m_localPositionOffsetProvider.AddTo(this);

                m_localPositionOffsetProvider
                    .ReduceLatestBy(this.FixedUpdateAsObservable(), (prev, next) => Vector3.Max(prev, next), (index, tick, total) => total)
                    .Subscribe(offset => transform.localPosition = SourceLocalPositionOffset + offset)
                    .AddTo(this);

                SourceLocalEulerOffset = transform.localEulerAngles;

                m_localEulerOffsetProvider
                    .ReduceLatestBy(this.FixedUpdateAsObservable(), (prev, next) => Vector3.Max(prev, next), (index, tick, total) => total)
                    .Subscribe(offset => transform.localEulerAngles = SourceLocalEulerOffset + offset)
                    .AddTo(this);
            }

            public void PushPositionOffsetProvider(IObservable<Vector3> provider)
            {
                m_localPositionOffsetProvider.OnNext(provider);
            }

            public void PushEulerOffsetProvider(IObservable<Vector3> provider)
            {
                m_localEulerOffsetProvider.OnNext(provider);
            }
        }
    }

}
