using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SUHScripts
{
    public class M_MonoEvent : MonoBehaviour
    {
        [SerializeField] UnityEvent m_onAwake = default;
        [SerializeField] UnityEvent m_onStart = default;
        [SerializeField] UnityEvent m_onEnable = default;
        [SerializeField] UnityEvent m_onDisable = default;

        void Awake() => m_onAwake.Invoke();
        void Start() => m_onStart.Invoke();
        void OnEnable() => m_onEnable.Invoke();
        void OnDisable() => m_onDisable.Invoke();
    }
}

