using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace SUHScripts
{
    public class TestInEditorHelper : MonoBehaviour {

        [SerializeField] UnityEvent OnIsEditorEvent = new UnityEvent();
        [SerializeField] UnityEvent OnIsNotEditorEvent = new UnityEvent();
        bool m_isInEditor = false;
        private void Start()
        {
    #if UNITY_EDITOR
            m_isInEditor = true;
    #endif
            if (m_isInEditor)
            {
                OnIsEditorEvent.Invoke();
            }
            else
            {
                OnIsNotEditorEvent.Invoke();
            }
        }

    }

}
