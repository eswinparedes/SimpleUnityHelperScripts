using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using COps = SUHScripts.CollisionObservableEffectOperations;
using UniRx;
using SUHScripts;
using UnityEngine.UI;

namespace SUHScripts.Tests
{
    public class CollisionObservableTesting : MonoBehaviour
    {
        [Header("Normal Observing")]
        [SerializeField] Collider m_singleCollisionCollider = default;
        [Header("Grouped Observing")]
        [SerializeField] List<Collider> m_groupedCollisionColliders = default;
        [Header("Text")]
        [SerializeField] Text m_singleText = default;
        [SerializeField] Text m_groupedText = default;
        [SerializeField] Text m_toggleText = default;

        public void Awake()
        {
            var obvsSingle = COps.ObserveCollisions(m_singleCollisionCollider);
            var obvsGrouped = COps.ObserveCollisionsGrouped(m_groupedCollisionColliders);

            var obvsToggled = OBV_EnterExit.EnterExit(obvsSingle.onEnter, obvsGrouped.OnEnter(), t => t.CollidingOther);

            obvsSingle.onEnter
                .ToggleEach((state, cols) => state ? Color.green : Color.blue, true)
                .Subscribe(color =>
                {
                    m_singleText.text = "Single Entered";
                    m_singleText.color = color;
                }).AddTo(this);

            obvsSingle.onExit
                .ToggleEach((state, cols) => state ? Color.red : Color.magenta, true)
                .Subscribe(color =>
                 {
                     m_singleText.text = "Single Exited";
                     m_singleText.color = color;
                 }).AddTo(this);

            obvsGrouped.OnEnter()
                .ToggleEach((state, cols) => state ? Color.green : Color.blue, true)
                .Subscribe(color =>
                {
                    m_groupedText.color = color;
                    m_groupedText.text = "Grouped Entered";
                }).AddTo(this);

            obvsGrouped.OnExit()
                .ToggleEach((state, cols) => state ? Color.red : Color.magenta, true)
                .Subscribe(color =>
                {
                    m_groupedText.color = color;
                    m_groupedText.text = "Grouped Exited";
                }).AddTo(this);

            obvsToggled.OnEnter()
                .ToggleEach((state, cols) => state ? Color.green : Color.blue, true)
                .Subscribe(color =>
                {
                    m_toggleText.color = color;
                    m_toggleText.text = "Toggle Entered";
                }).AddTo(this);

            obvsToggled.OnExit()
                .ToggleEach((state, cols) => state ? Color.red : Color.magenta, true)
                .Subscribe(color =>
                {
                    m_toggleText.color = color;
                    m_toggleText.text = "Toggle Exited";
                }).AddTo(this);

        }
    }
}

