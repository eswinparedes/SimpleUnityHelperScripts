using SUHScripts.Functional;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SUHScripts
{
    public abstract class ADependencySourceMono : MonoBehaviour
    {
        protected IDependencySource m_dependencySource;

        public IDependencySource GetDependencySource()
        {
            if (m_dependencySource == null)
            {
                m_dependencySource = GetSource();
            }

            return m_dependencySource;
        }

        protected abstract IDependencySource GetSource();
    }

}
