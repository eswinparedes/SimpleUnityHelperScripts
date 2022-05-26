using System;
using UnityEngine;
using UnityEngine.Events;

namespace SUHScripts.Tests
{
    public class TestTransitionScale : MonoBehaviour
    {
        [Header("Transition Settings")]
        [SerializeField] Transform m_transitionTransform = default;
        [SerializeField] Vector3 m_scaleEnter = Vector3.one;
        [SerializeField] Vector3 m_scaleExit = Vector3.zero;
        [SerializeField] AnimationCurve m_movementCurve = new AnimationCurve();
        [SerializeField] float m_transitionTime = 0.2f;
        [SerializeField] bool m_startExited = true;
        [SerializeField] bool m_eventsFireOnFullTransitionOnly = false;
        [Header("Events")]
        [SerializeField] UnityEvent m_onEnterStart = default;
        [SerializeField] UnityEvent m_onEnterEnd = default;
        [SerializeField] UnityEvent m_onExitStart = default;
        [SerializeField] UnityEvent m_onExitEnd = default;

        public UnityEvent OnEnterStart => m_onEnterStart;
        public UnityEvent OnEnterEnd => m_onEnterEnd;
        public UnityEvent OnExitStart => m_onExitStart;
        public UnityEvent OnExitEnd => m_onExitEnd;

        Vector3 m_lerpEnter;
        Vector3 m_lerpExit;

        FTimer m_timer;
        Action OnTimerComplete = () => { };

        bool m_isEntered;

        public void Start()
        {
            m_lerpEnter = Vector3.Scale(m_transitionTransform.localScale, m_scaleEnter);
            m_lerpExit = Vector3.Scale(m_transitionTransform.localScale, m_scaleExit);

            m_timer = new FTimer(m_transitionTime, m_transitionTime, !m_startExited);
            m_isEntered = !m_startExited;
        }

        void Update()
        {
            m_timer = m_timer.Tick(Time.deltaTime);//, onComplete: OnTimerComplete.Invoke);
            m_transitionTransform.localScale = Vector3.Lerp(m_lerpExit, m_lerpEnter, m_movementCurve.Evaluate(m_timer.TimeAlpha()));
        }

        public void Enter()
        {
            OnEnterStarted();

            m_transitionTransform.localScale = m_lerpEnter;
            m_timer = m_timer.RestartedIncrementing(true);
            OnTimerComplete = OnEnterEnded;
        }

        public void Exit()
        {
            OnExitStarted();

            m_transitionTransform.localScale = m_lerpEnter;
            m_timer = m_timer.RestartedDecrementing(true);
            OnTimerComplete = OnExitEnded;
        }

        void OnEnterStarted()
        {
            if (m_eventsFireOnFullTransitionOnly && !m_isEntered || !m_eventsFireOnFullTransitionOnly)
            {
                Debug.Log("Enter Started");
                OnEnterStart.Invoke();
            }
        }

        void OnEnterEnded()
        {
            if (m_eventsFireOnFullTransitionOnly && !m_isEntered || !m_eventsFireOnFullTransitionOnly)
            {
                Debug.Log("Enter ended");
                m_isEntered = true;
                OnEnterEnd.Invoke();
            }    
        }

        void OnExitStarted()
        {
            if (m_eventsFireOnFullTransitionOnly && m_isEntered || !m_eventsFireOnFullTransitionOnly)
            {
                Debug.Log("Exit Started");
                OnExitStart.Invoke();
            }
        }

        void OnExitEnded()
        {
            if (m_eventsFireOnFullTransitionOnly && m_isEntered || !m_eventsFireOnFullTransitionOnly)
            {
                m_isEntered = false;
                Debug.Log("Exit Ended");
                OnExitEnd.Invoke();
            }
        }
    }
}

