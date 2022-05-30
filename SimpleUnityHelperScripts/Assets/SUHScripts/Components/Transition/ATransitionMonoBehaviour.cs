using System;
using UnityEngine;
using SUHScripts;

namespace SUHScripts
{

    //TODO: REFACTOR USING EVENT WRAPPER, REMOVE UNIRX DEPENDENCY
    [DisallowMultipleComponent]
    public abstract class ATransitionMonoBehaviour : MonoBehaviour
    {
        [SerializeField] bool m_hideOnAwake = true;

        public event Action OnHide;
        public event Action OnShow;

        public bool IsShown { get; private set; } = false;

        protected virtual void Awake()
        {
            if (m_hideOnAwake)
            {
                IsShown = true;
                RaiseOnHide();
            }
            else
            {
                IsShown = false;
                RaiseOnShow();
            }
        }
        protected void RaiseOnHide()
        {
            if (!IsShown)
            {
                Debug.LogWarning($"GameObject : {gameObject.name} - Attempting to hide transition that is already hidden!");
                return;
            }

            IsShown = false;
            OnHide?.Invoke();
        }
        protected void RaiseOnShow()
        {
            if (IsShown)
            {
                Debug.LogWarning($"GameObject : {gameObject.name} - Attempting to show transition that is already shown!");
                return;
            }

            IsShown = true;
            OnShow?.Invoke();
        }
    }


}