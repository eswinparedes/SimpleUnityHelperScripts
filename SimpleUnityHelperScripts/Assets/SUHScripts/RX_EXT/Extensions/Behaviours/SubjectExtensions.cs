using UniRx;
using UnityEngine;

namespace SUHScripts
{
    public static class SubjectExtensions 
    {
        public static void AddedReturn<T>(this Subject<T> subject, Component addTo, T value)
        {
            subject.AddTo(addTo);
            subject.OnNext(value);
            subject.OnCompleted();
        }

        public static void AddedReturn<T>(this ReplaySubject<T> subject, Component addTo, T value)
        {
            subject.AddTo(addTo);
            subject.OnNext(value);
            subject.OnCompleted();
        }
        public static void Return<T>(this Subject<T> subject, T value)
        {
            subject.OnNext(value);
            subject.OnCompleted();
        }

        public static void Return<T>(this ReplaySubject<T> subject,  T value)
        {
            subject.OnNext(value);
            subject.OnCompleted();
        }
    }

}
