using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUHScripts
{
    public class M_DetachOnAwake : MonoBehaviour
    {
        private void Awake()
        {
            this.transform.parent = null;
        }
    }
}

