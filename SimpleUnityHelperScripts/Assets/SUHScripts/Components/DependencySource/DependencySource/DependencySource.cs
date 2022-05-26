using SUHScripts.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SUHScripts
{
    public static class DependencySource
    {
        /// <summary>
        /// Will simply throw an exception if the value is not found, however will return T if it is
        /// </summary>
        /// <returns></returns>
        public static T UGet<T>(this IDependencySource @this)
        {
            var opt = @this.Get<T>();

            if(opt.IsSome)
            {
                return opt.Value;
            }
            else
            {
                throw new System.Exception($"Dependency of type: {typeof(T)} not present in this IDependencySource");
            }
        }

        /// <summary>
        /// returns the Read value of an IReadValue<T> found in the DependencySource, throws exception if no IReadValue<T> is found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T URead<T>(this IDependencySource @this)
        {
            return @this.UGet<IReadValue<T>>().Read();
        }

        /// <summary>
        /// Writes 'writeValue' into an IWriteValue<T> found in the DependencySource, throws exception if not IWriteValue<T> is found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="writeValue"></param>
        public static void UWrite<T>(this IDependencySource @this, T writeValue)
        {
            @this.UGet<IWriteValue<T>>().Write(writeValue);
        }

        /// <summary>
        /// Injects the type from target into subject. Throws exception if dependency is not present
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subject"></param>
        /// <param name="target"></param>
        public static void UInject<T>(this DependencySourceSubject subject, IDependencySource target)
        {
            subject.Inject(target.UGet<T>());
        }

        public static IDependencySource GetDepedencySources(params GameObject[] gos)
        {
            var dependencies =
                gos
                .SelectMany(target => target.GetComponents<ADependencySourceMono>())
                .Select(t => t.GetDependencySource())
                .ToArray();


            var depSubject = new DependencySourceSubject();
            depSubject.InjectDependencies(dependencies);
            return depSubject;
        }
        public static IEnumerable<IDependencySource> EnumerateDependencySourcesOnGameObjects(params GameObject[] gos)
        {
            var dependencies = gos.SelectMany(target => target.GetComponents<ADependencySourceMono>());

            foreach (var dep in dependencies)
            {
                yield return dep.GetDependencySource();
            }
        }

        public static Option<T> StartFailEarly<T>(this IDependencySource @this, out Option<T> tOpt)
        {
            tOpt = @this.Get<T>();
            return tOpt;
        }

        public static Option<R> FailEarly<T, R>(this Option<T> @this, IDependencySource source, out Option<R> rOpt)
        {
            if (!@this.IsSome)
            {
                rOpt = Option<R>._NONE;

                return rOpt;
            }

            rOpt = source.Get<R>();

            return rOpt;
        }

        public static Option<(T0, T1)> FailEarly<T0, T1>(this IDependencySource @this)
        {
            var finalOption =
                @this.StartFailEarly<T0>(out Option<T0> toOpt)
                .FailEarly(@this, out Option<T1> t1Opt);

            if (!finalOption.IsSome) return Option<(T0, T1)>._NONE;

            return Option.UNSAFE((toOpt.Value, t1Opt.Value));
        }
        public static Option<(T0, T1, T2)> FailEarly<T0, T1, T2>(this IDependencySource @this)
        {
            var finalOption =
                @this.StartFailEarly<T0>(out Option<T0> toOpt)
                .FailEarly(@this, out Option<T1> t1Opt)
                .FailEarly(@this, out Option<T2> t2Opt);

            if (!finalOption.IsSome) return Option<(T0, T1, T2)>._NONE;

            return Option.UNSAFE((toOpt.Value, t1Opt.Value, t2Opt.Value));
        }

        public static Option<(T0, T1, T2, T3)> FailEarly<T0, T1, T2, T3>(this IDependencySource @this)
        {
            var finalOption =
                @this.StartFailEarly<T0>(out Option<T0> toOpt)
                .FailEarly(@this, out Option<T1> t1Opt)
                .FailEarly(@this, out Option<T2> t2Opt)
                .FailEarly(@this, out Option<T3> t3Opt);

            if (!finalOption.IsSome) return Option<(T0, T1, T2, T3)>._NONE;

            return Option.UNSAFE((toOpt.Value, t1Opt.Value, t2Opt.Value, t3Opt.Value));
        }

        public static Option<(T0, T1, T2, T3, T4)> FailEarly<T0, T1, T2, T3, T4>(this IDependencySource @this)
        {
            var finalOption =
                @this.StartFailEarly<T0>(out Option<T0> toOpt)
                .FailEarly(@this, out Option<T1> t1Opt)
                .FailEarly(@this, out Option<T2> t2Opt)
                .FailEarly(@this, out Option<T3> t3Opt)
                .FailEarly(@this, out Option<T4> t4Opt);

            if (!finalOption.IsSome) return Option<(T0, T1, T2, T3, T4)>._NONE;

            return Option.UNSAFE((toOpt.Value, t1Opt.Value, t2Opt.Value, t3Opt.Value, t4Opt.Value));
        }

        public static Option<(T0, T1, T2, T3, T4, T5)> FailEarly<T0, T1, T2, T3, T4, T5>(this IDependencySource @this)
        {
            var finalOption =
                @this.StartFailEarly<T0>(out Option<T0> toOpt)
                .FailEarly(@this, out Option<T1> t1Opt)
                .FailEarly(@this, out Option<T2> t2Opt)
                .FailEarly(@this, out Option<T3> t3Opt)
                .FailEarly(@this, out Option<T4> t4Opt)
                .FailEarly(@this, out Option<T5> t5Opt);

            if (!finalOption.IsSome) return Option<(T0, T1, T2, T3, T4, T5)>._NONE;

            return Option.UNSAFE((toOpt.Value, t1Opt.Value, t2Opt.Value, t3Opt.Value, t4Opt.Value, t5Opt.Value));
        }

        public static Option<(T0, T1, T2, T3, T4, T5, T6)> FailEarly<T0, T1, T2, T3, T4, T5, T6>(this IDependencySource @this)
        {
            var finalOption =
                @this.StartFailEarly<T0>(out Option<T0> toOpt)
                .FailEarly(@this, out Option<T1> t1Opt)
                .FailEarly(@this, out Option<T2> t2Opt)
                .FailEarly(@this, out Option<T3> t3Opt)
                .FailEarly(@this, out Option<T4> t4Opt)
                .FailEarly(@this, out Option<T5> t5Opt)
                .FailEarly(@this, out Option<T6> t6Opt);

            if (!finalOption.IsSome) return Option<(T0, T1, T2, T3, T4, T5, T6)>._NONE;

            return Option.UNSAFE((toOpt.Value, t1Opt.Value, t2Opt.Value, t3Opt.Value, t4Opt.Value, t5Opt.Value, t6Opt.Value));
        }

        public static Option<(T0, T1, T2, T3, T4, T5, T6, T7)> FailEarly<T0, T1, T2, T3, T4, T5, T6, T7>(this IDependencySource @this)
        {
            var finalOption =
                @this.StartFailEarly<T0>(out Option<T0> toOpt)
                .FailEarly(@this, out Option<T1> t1Opt)
                .FailEarly(@this, out Option<T2> t2Opt)
                .FailEarly(@this, out Option<T3> t3Opt)
                .FailEarly(@this, out Option<T4> t4Opt)
                .FailEarly(@this, out Option<T5> t5Opt)
                .FailEarly(@this, out Option<T6> t6Opt)
                .FailEarly(@this, out Option<T7> t7Opt);

            if (!finalOption.IsSome) return Option<(T0, T1, T2, T3, T4, T5, T6, T7)>._NONE;

            return Option.UNSAFE((toOpt.Value, t1Opt.Value, t2Opt.Value, t3Opt.Value, t4Opt.Value, t5Opt.Value, t6Opt.Value, t7Opt.Value));
        }
    }

}