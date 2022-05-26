using UnityEngine;
using System.Threading;

namespace SUHScripts
{
    [System.Serializable]
    public static class DependencySourceSubjectInjector
    {
        public static DependencySourceSubject InjectDependenciesIntoSourceSubjectFromGameObjects(DependencySourceSubject sourceSubject, params GameObject[] gameObjectsToExtractFrom)
        {
            for (int i = 0; i < gameObjectsToExtractFrom.Length; i++)
            {
                //Pull dependencies from dependency sources
                var dependencySources = gameObjectsToExtractFrom[i].GetComponents<IDependencySource>();

                if (dependencySources != null && dependencySources.Length > 0)
                {
                    for (int depIndex = 0; depIndex < dependencySources.Length; depIndex++)
                    {
                        sourceSubject.InjectDependencies(dependencySources[depIndex]);
                    }
                }


                //Run dependency source injections
                var subs = gameObjectsToExtractFrom[i].GetComponents<IDependencySourceSubjectInjector>();

                if (subs != null && subs.Length > 0)
                {
                    for (int subIndex = 0; subIndex < subs.Length; subIndex++)
                    {
                        subs[subIndex].InjectIntoSubject(sourceSubject);
                    }
                }
            }

            return sourceSubject;
        }

        public static DependencySourceSubject InjectDependenciesIntoSourceSubjectFromGameObjects(params GameObject[] gameObjectsToExtractFrom)
        {
            return InjectDependenciesIntoSourceSubjectFromGameObjects(new DependencySourceSubject(), gameObjectsToExtractFrom);
        }
    }
}
