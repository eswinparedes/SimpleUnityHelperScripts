using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUHScripts
{
    public class DrawBoxByScale : MonoBehaviour
    {
        void OnDrawGizmos()
        {
            // Draw a semitransparent blue cube at the transforms position
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawCube(transform.position, transform.lossyScale);
       
        }
    }
}

