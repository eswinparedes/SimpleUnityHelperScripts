using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using System.Linq;
using System;
using SUHScripts.Functional;

namespace SUHScripts.Tests
{
    public class TestLocalOffsetProvider : MonoBehaviour
    {
        [SerializeField] KeyCode m_hitKey = KeyCode.Space;
        [SerializeField] Vector3 m_hitVector = new Vector3(0, 10, 0);
        [SerializeField] Transform m_targetTransform = default;
        [SerializeField] float length = 3f;
        private void Awake()
        {
            var smashHitAlpha =
                this.FixedUpdateAsObservable()
                .Scan(0f, (p, _) => p + Time.fixedDeltaTime)
                .TakeWhile(t => t < length)
                .Select(t => Easing.UpDown(t / length));

            Func<IEnumerable<Vector3>, Vector3> aggregation =
                vs =>
                {
                    var vMax = Vector3.zero;
                    var mag = 0f;
                    foreach(var v in vs)
                    {
                        var m = v.magnitude;
                        if(m > mag)
                        {
                            mag = m;
                            vMax = v;
                        }
                    }

                    return vMax;
                };

            var offsetStream =
                this.WhenKey(m_hitKey)
                    .Select(_ => smashHitAlpha.AsNew())
                    .ReduceLatestBy(this.FixedUpdateAsObservable(), (prev, next) => prev + next, (index, tick, total) => total / 2)
                    .Subscribe(offset => m_targetTransform.localPosition = Vector3.Lerp(Vector3.zero, m_hitVector, offset))
                    .AddTo(this);

            
        }
    }
}

