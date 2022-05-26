using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UniRx.Triggers;

namespace SUHScripts.Tests
{
    public class TEST_TestTakeDuring : MonoBehaviour
    {
        [SerializeField] KeyCode m_sourceEmissionKey = KeyCode.Space;
        [SerializeField] KeyCode m_terminationKey = KeyCode.T;

        void Start()
        {
            var terminateStream = this.WhenKey(m_terminationKey).First();

            terminateStream
                .Subscribe(
                    onNext: x => Debug.Log($"Termination Key Logged {x.ToString()}"),
                    onCompleted: () => Debug.Log($"Termination Key {m_terminationKey.ToString()} Completed"))
                .AddTo(this);

            this.WhenKey(m_sourceEmissionKey)
                .TakeDuring(terminateStream)
                .Subscribe(
                    onNext: x => Debug.Log($"Source Emission Logged {x.ToString()}"),
                    onCompleted: () => Debug.Log($"Source Emission For {m_sourceEmissionKey.ToString()} Completed"))
                .AddTo(this);
        }
    }

    public static class TEST_TestTakeDuringExt
    {
       
    }


}
