using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace SUHScripts.Tests
{
    public class TEST_FirstCompleted : MonoBehaviour
    {
        [SerializeField] KeyCode m_tryLastKey = KeyCode.Space;
        [SerializeField] KeyCode m_firstCompletedKey0 = KeyCode.A;
        [SerializeField] KeyCode m_firstCompletedKey1 = KeyCode.D;
        [SerializeField] KeyCode m_completeKey_1 = KeyCode.Alpha1;
        [SerializeField] KeyCode m_completeKey_2 = KeyCode.Alpha2;
        [SerializeField] KeyCode m_completeKey_3 = KeyCode.Alpha3;
        [SerializeField] KeyCode m_completeKey_4 = KeyCode.Alpha4;
        [SerializeField] KeyCode m_completeKey_5 = KeyCode.Alpha5;
        [SerializeField] KeyCode m_completeKey_6 = KeyCode.Alpha6;
        [SerializeField] KeyCode m_terminateCompleteKeyMultiStream = KeyCode.T;

        private void Awake()
        {
            this.WhenKey(m_tryLastKey).Take(1)
                .TryLast()
                .Subscribe(onNext: k => Debug.Log(k.IsSome ? $"Try Last Value {k.Value}" : "Try Last No last value"), onCompleted: () => Debug.Log("Try last completed"))
                .AddTo(this);

            //Returns the last value to be emitted by either stream or NONE if neither emits a value
            this.WhenKey(m_firstCompletedKey0).Take(1)
                .TryFirstLast(this.WhenKey(m_firstCompletedKey1).Take(1), t => t.ToString(), t => t.ToString())
                .Subscribe(onNext: k => Debug.Log(k.IsSome ? $"Try First Last Value {k.Value}" : "Try First Last No last value"), onCompleted: () => Debug.Log("Try First last completed"))
                .AddTo(this);

            //Completes a chain of try lasts, returns when the first one completes >>WITH A VALUE<< and returns its last value or NONE if all complete without a value
            this.WhenKey(m_completeKey_1).Take(1)
                .TryFirstLast(this.WhenKey(m_completeKey_2).Take(1), t => t.ToString(), t => t.ToString())
                .BindTryFirstLast(this.WhenKey(m_completeKey_3).Take(1), t => t, t => t.ToString())
                .BindTryFirstLast(this.WhenKey(m_completeKey_4).Take(1), t => t, t => t.ToString())
                .BindTryFirstLast(this.WhenKey(m_completeKey_5).Take(1), t => t, t => t.ToString())
                .BindTryFirstLast(this.WhenKey(m_completeKey_6).Take(1), t => t, t => t.ToString())
                .Subscribe(onNext: k => Debug.Log(k.IsSome ? $"Bind Try Last Value {k.Value}" : "Bind Try First Last No last value"), onCompleted: () => Debug.Log("Bind Try First last completed"))
                .AddTo(this);

            //Merges the key streams into one stream and emits the last value before the "TakeUntil" terminates or "NONE" if no value was emitted
            this.WhenKey(m_completeKey_1)
                .Merge(this.WhenKey(m_completeKey_2))
                .Merge(this.WhenKey(m_completeKey_3))
                .Merge(this.WhenKey(m_completeKey_4))
                .Merge(this.WhenKey(m_completeKey_5))
                .Merge(this.WhenKey(m_completeKey_6))
                .TakeUntil(this.WhenKey(m_terminateCompleteKeyMultiStream).Take(1))
                .TryLast()
                .Subscribe(onNext: k => Debug.Log(k.IsSome ? $"Merged Bind Try First last Value {k.Value}" : "Merged Try First Last No last value"), onCompleted: () => Debug.Log("Merged Bind Try First last completed"))
                .AddTo(this);
        }
    }

    public static class TEST_FirstCompletedExt
    {
        public static IObservable<KeyCode> WhenKey(this Component @this, KeyCode key) =>
            @this.UpdateAsObservable().Where(_ => Input.GetKeyDown(key)).Select(_ => key);
    }
}
