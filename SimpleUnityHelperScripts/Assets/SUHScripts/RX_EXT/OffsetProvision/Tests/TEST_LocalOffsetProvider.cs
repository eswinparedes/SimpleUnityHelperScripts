using SUHScripts.Functional;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using static SUHScripts.Functional.Functional;

namespace SUHScripts.Tests
{
    public class TEST_LocalOffsetProvider : MonoBehaviour
    {
        [SerializeField] Transform m_target = default;
        [SerializeField] Vector2 m_freqMag_A = new Vector2(1, 1);
        [SerializeField] Vector2 m_freqMag_B = new Vector2(1, -1);
        [SerializeField] Vector2 m_freqMag_C = new Vector2(-1, 1);
        [SerializeField] Vector2 m_freqMag_D = new Vector2(-1, -1);
        [SerializeField] bool m_useA = true;
        [SerializeField] bool m_useB = true;
        [SerializeField] bool m_useC = true;
        [SerializeField] bool m_useD = true;

        void Start()
        {
            Option<Subject<Vector3>> offsetA = None.Default;
            Option<Subject<Vector3>> offsetB = None.Default;
            Option<Subject<Vector3>> offsetC = None.Default;
            Option<Subject<Vector3>> offsetD = None.Default;
            Subject<IObservable<Vector3>> offsetStream = new Subject<IObservable<Vector3>>();

            offsetStream
                .StartWith(Observable.Return(Vector3.one))
                .Subscribe(offset => OBV_TransformOffsetObserver.PushLocalPositionOffsetProvider(offset, m_target))
                .AddTo(this);

            ///A
            this.FixedUpdateAsObservable()
                .Where(_ => m_useA && !offsetA.IsSome)
                .Subscribe(_ =>
                {
                    offsetA = new Subject<Vector3>().AddTo(this).AsOption_SAFE();
                    offsetStream.OnNext(offsetA.Value);
                }).AddTo(this);

            this.FixedUpdateAsObservable()
                .Where(_ => !m_useA && offsetA.IsSome)
                .Subscribe(_ =>
                {
                    offsetA.Value.OnCompleted();
                    offsetA.Value.Dispose();
                    offsetA = None.Default;
                }).AddTo(this);

            this.FixedUpdateAsObservable()
                .Choose(_ => offsetA)
                .Subscribe(_ => offsetA.Value.OnNext(m_freqMag_A))
                .AddTo(this);

            ///B
            this.FixedUpdateAsObservable()
                .Where(_ => m_useB && !offsetB.IsSome)
                .Subscribe(_ =>
                {
                    offsetB = new Subject<Vector3>().AddTo(this).AsOption_SAFE();
                    offsetStream.OnNext(offsetB.Value);
                }).AddTo(this);

            this.FixedUpdateAsObservable()
                .Where(_ => !m_useB && offsetB.IsSome)
                .Subscribe(_ =>
                {
                    offsetB.Value.OnCompleted();
                    offsetB.Value.Dispose();
                    offsetB = None.Default;
                }).AddTo(this);

            this.FixedUpdateAsObservable()
                .Choose(_ => offsetB)
                .Subscribe(_ => offsetB.Value.OnNext(m_freqMag_B))
                .AddTo(this);

            ///C
            this.FixedUpdateAsObservable()
                .Where(_ => m_useC && !offsetC.IsSome)
                .Subscribe(_ =>
                {
                    offsetC = new Subject<Vector3>().AddTo(this).AsOption_SAFE();
                    offsetStream.OnNext(offsetC.Value);
                }).AddTo(this);

            this.FixedUpdateAsObservable()
                .Where(_ => !m_useC && offsetC.IsSome)
                .Subscribe(_ =>
                {
                    offsetC.Value.OnCompleted();
                    offsetC.Value.Dispose();
                    offsetC = None.Default;
                }).AddTo(this);

            this.FixedUpdateAsObservable()
                .Choose(_ => offsetC)
                .Subscribe(_ => offsetC.Value.OnNext(m_freqMag_C))
                .AddTo(this);

            ///D
            this.FixedUpdateAsObservable()
                .Where(_ => m_useD && !offsetD.IsSome)
                .Subscribe(_ =>
                {
                    offsetD = new Subject<Vector3>().AddTo(this).AsOption_SAFE();
                    offsetStream.OnNext(offsetD.Value);
                }).AddTo(this);

            this.FixedUpdateAsObservable()
                .Where(_ => !m_useD && offsetD.IsSome)
                .Subscribe(_ =>
                {
                    offsetD.Value.OnCompleted();
                    offsetD.Value.Dispose();
                    offsetD = None.Default;
                }).AddTo(this);

            this.FixedUpdateAsObservable()
                .Choose(_ => offsetD)
                .Subscribe(_ => offsetD.Value.OnNext(m_freqMag_D))
                .AddTo(this);
        }
    }
}
