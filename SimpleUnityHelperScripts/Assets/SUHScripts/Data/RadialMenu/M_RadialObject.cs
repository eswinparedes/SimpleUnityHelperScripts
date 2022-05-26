using UnityEngine;
using UnityEngine.Events;

namespace SUHScripts
{
    public class M_RadialObject : MonoBehaviour
    {
        [SerializeField] UnityEvent m_onRadialObjectSelected = new UnityEvent();
        [SerializeField] UnityEvent m_onRadialObjectDeselected = new UnityEvent();
        [SerializeField] UnityEvent m_onRadialObjectRequestAction = new UnityEvent();

        public UnityEvent OnRadialObjectSelected => m_onRadialObjectSelected;
        public UnityEvent OnRadialObjectDeslected => m_onRadialObjectDeselected;
        public UnityEvent OnRadialObjectRequestAction => m_onRadialObjectRequestAction;
    }
}