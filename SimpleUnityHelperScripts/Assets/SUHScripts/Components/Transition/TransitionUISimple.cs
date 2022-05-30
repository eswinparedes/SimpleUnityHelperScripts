using SUHScripts;
using UnityEngine;
using UnityEngine.Events;

namespace SUHScripts
{

    public class TransitionUISimple : ATransitionMonoBehaviourSubject
    {
        [Header("Transition Components")]
        [SerializeField] CanvasGroup m_canvasGroup = default;
        [SerializeField] RectTransform m_root;

        [Header("Settings")]
        [SerializeField] float m_transitionDuration = 0.3f;
        [SerializeField] AnimationCurve m_transitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] bool m_useScale = false;
        [SerializeField] Vector3 m_exitPosition = Vector3.zero;
        [SerializeField] Vector3 m_enterPosition = Vector3.zero;

        [Header("Events")]
        [SerializeField] protected UnityEvent m_onShow = default;
        [SerializeField] protected UnityEvent m_onHide = default;

        float m_elapsed = 0;
        bool m_isAppearing = true;
        bool m_sourceBlocksRaycasts;
        bool m_sourceInteractable;
        Vector3 m_targetScale;

        public float TransitionDuration => m_transitionDuration;
        public float Alpha { get; private set; }
        protected override void Awake()
        {
            m_targetScale = m_root.localScale;

            m_sourceBlocksRaycasts = m_canvasGroup.blocksRaycasts;
            m_sourceInteractable = m_canvasGroup.interactable;

            OnShow += OnShowCallback;
            OnHide += OnHideCallback;

            base.Awake();
        }

        private void OnDestroy()
        {
            OnShow -= OnShowCallback;
            OnHide -= OnHideCallback;
        }

        //TODO: Deactivate monobehaviour and stop running these calculations when fully shown/hidden
        protected virtual void Update()
        {
            m_elapsed = m_isAppearing ? m_elapsed + Time.deltaTime : m_elapsed - Time.deltaTime;
            m_elapsed = Mathf.Clamp(m_elapsed, 0, m_transitionDuration);

            var alpha = m_elapsed / m_transitionDuration;

            Alpha = m_transitionCurve.Evaluate(alpha);
            m_canvasGroup.alpha = Alpha;
            m_root.anchoredPosition = Vector3.Lerp(m_enterPosition, m_exitPosition, Alpha);

            if (m_useScale)
            {
                m_root.localScale = Vector3.Lerp(Vector3.zero, m_targetScale, Alpha);
            }
        }

        public void OnShowCallback()
        {
            m_canvasGroup.blocksRaycasts = m_sourceBlocksRaycasts;
            m_canvasGroup.interactable = m_sourceInteractable;
            m_isAppearing = true;

            if (m_useScale)
            {
                m_root.localScale = Vector3.zero;
            }

            m_onShow.Invoke();
        }

        public void OnHideCallback()
        {
            m_canvasGroup.blocksRaycasts = false;
            m_canvasGroup.interactable = false;
            m_isAppearing = false;
            m_onHide.Invoke();
        }
    }
}
