using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using SUHScripts.Functional;
using static SUHScripts.Functional.Functional;
using UniRx.Triggers;
using SUHScripts;

namespace SUHScripts.Tests
{
    public class TEST_SingleSElect : MonoBehaviour
    {
        [SerializeField] KeyCode m_resetKey = KeyCode.Space;

        private void Awake()
        {
            var key =
                this.UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(m_resetKey))
                .Select(_ => (Option<Transform>) None.Default);

            var singleSelector =
                this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButtonDown(0))
                .Select(_ => Input.mousePosition)
                .Select(pos => Camera.main.ScreenPointToRay(pos))
                .Choose(ray => SUHScripts.PhysicsCasting.Raycast(ray).RaycastHitOption)
                .Select(hit => hit.collider.transform.AsOption_SAFE())
                .Merge(key)
                .ToggleSelect((t0, t1) => t0 == t1);


            singleSelector
                .Choose(s => s.Select)
                .Subscribe(t => t.position = t.position.WithY(2))
                .AddTo(this);

            singleSelector
                .Choose(s => s.Deselect)
                .Subscribe(t => t.position = t.position.WithY(0))
                .AddTo(this);

        }
    }
}