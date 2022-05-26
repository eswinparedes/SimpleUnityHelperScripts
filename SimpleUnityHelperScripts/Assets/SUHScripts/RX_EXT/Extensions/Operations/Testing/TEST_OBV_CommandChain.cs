using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using SUHScripts;
using SUHScripts.Functional;

namespace SUHScripts
{
    public class TEST_OBV_CommandChain : MonoBehaviour
    {
        void Start()
        {
            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButtonDown(0))
                .Select(_ => PhysicsCasting.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), Mathf.Infinity))
                .Choose(data => data.RaycastHitOption)
                .CommandChain(
                    hit => "HIT but found NONE",
                    hit => hit.collider.GetComponentOption<AudioSource>().Map(comp => $"{comp.name} has audiosource"),
                    hit => hit.collider.GetComponentOption<Rigidbody>().Map(comp => $"{ comp.name} has rigidbody"),
                    hit => hit.collider.GetComponentOption<Animator>().Map(comp => $"{comp.name} has animator"))
                .Subscribe(msg =>
                {
                    Debug.Log(msg);
                }).AddTo(this);

        }

        void Update()
        {

        }
    }
}

