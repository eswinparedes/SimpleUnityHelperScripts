using System;
using UnityEngine;

namespace SUHScripts
{
    //TODO: REFACTOR USING EVENT WRAPPER, REMOVE UNIRX DEPENDENCY
    [DisallowMultipleComponent]
    public abstract class ATransitionMonoBehaviourSubject : ATransitionMonoBehaviour
    {
        public void Show() => RaiseOnShow();
        public void Hide() => RaiseOnHide();

    }


}