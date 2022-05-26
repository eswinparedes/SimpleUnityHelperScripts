using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUHScripts.Tests
{
    public class QuaternionExtensionsTest : MonoBehaviour
    {
        [SerializeField] Transform m_sourceTransform = default;
        [SerializeField] Transform m_targetTransform = default;
        [SerializeField] Vector3 m_mirrorNormal = default;
        [SerializeField] bool useXMirror = true;

        private void Update()
        {
            if (useXMirror)
                m_targetTransform.rotation = m_sourceTransform.rotation.MirrorX();
            else
                m_targetTransform.rotation = m_sourceTransform.rotation.MirrorNormal(m_mirrorNormal);
        }
    }

}
