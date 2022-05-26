using SUHScripts.Functional;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace SUHScripts.Pending
{
    public static class PendingObservableExtensions 
    {
        static GameObject m_sceneInstance = null;
        static GameObject SceneInstance
        {
            get
            {
                if(m_sceneInstance == null)
                {
                    m_sceneInstance = new GameObject();
                }

                return m_sceneInstance ;
            }
        }

        public static IObservable<float> m_staticUpdateAsObservable = null;
        public static IObservable<float> StaticUpdateAsObservable
        {
            get
            {
                if (m_staticUpdateAsObservable == null)
                {
                    m_staticUpdateAsObservable = SceneInstance.UpdateAsObservable().Select(_ => Time.deltaTime);
                }

                return m_staticUpdateAsObservable;
            }
        }


        public static IObservable<Unit> UpdateAsObservable(this Component @this) =>
            ObservableTriggerExtensions.UpdateAsObservable(@this).Where(_ => Time.timeScale > 0);

        public static IObservable<Unit> FixedUpdateAsObservable(this Component @this) =>
            ObservableTriggerExtensions.FixedUpdateAsObservable(@this).Where(_ => Time.timeScale > 0);
    }   
}