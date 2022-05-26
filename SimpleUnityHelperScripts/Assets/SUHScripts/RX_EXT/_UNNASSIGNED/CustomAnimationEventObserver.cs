using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using SUHScripts;

public class CustomAnimationEventObserver : MonoBehaviour
{
    Animator m_animator = default;
    int m_count = int.MinValue;
    Dictionary<int, Subject<Unit>> m_eventEmissions = new Dictionary<int, Subject<Unit>>();
    HashSet<AnimationClip> m_clips = default;

    bool m_hasInitted = false;
    private void TryInit()
    {
        if (m_hasInitted) return;

        m_animator = this.GetComponent<Animator>();

        if (m_animator == null) return;

        var clips = m_animator.runtimeAnimatorController.animationClips;
        m_clips = new HashSet<AnimationClip>(clips);

        m_hasInitted = true;
    }

    public IObservable<Unit> ObserveEvent(AnimationClip clip, float pointInSeconds)
    {
        TryInit();

        if (m_animator == null)
        {
            Debug.LogError($"No animator on {gameObject.name}");
            return Observable.Never<Unit>();
        }

        if (!m_clips.Contains(clip))
        {
            Debug.LogError($"Suggested Clip is not part of Animator Controller on {gameObject.name}");
            return Observable.Never<Unit>();
        }

        AnimationEvent animEvent = new AnimationEvent();
        animEvent.time = pointInSeconds >= 0 ? pointInSeconds : clip.length;
        animEvent.intParameter = m_count;
        animEvent.functionName = "___AnimationEventFired";

        var sub = new Subject<Unit>();

        clip.AddEvent(animEvent);

        m_eventEmissions.Add(m_count, sub);

        m_count++;

        return sub;
    }

    public void ___AnimationEventFired(int i)
    {
        m_eventEmissions[i].OnNext(Unit.Default);
    }

    ///Used only to bypass the annoying unity error message for empty clips that only play with an event
    public void __ANIM_EVENT_EMPTY()
    {

    }

    public static IObservable<Unit> ObserveAnimationEventFromClip(Animator animator, AnimationClip clip, float pointInSeconds)
    {
        var animEventObserver = animator.gameObject.GetOrAddComponent<CustomAnimationEventObserver>();
        return animEventObserver.ObserveEvent(clip, pointInSeconds);
    }
}
