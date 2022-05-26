using UnityEngine;

namespace SUHScripts.Tests
{
    using static MathHelper;

    public class OrthoNormalTest : MonoBehaviour
    {
        public Transform m_rayOrigin;
        public Transform relativeUp;
        public Transform relativeForward;

        public Vector3 localMovement;
        public Transform rotationTest;

        private void Update()
        {
            Vector3 lookDir = OrthoNormalVector(relativeForward.forward, relativeUp.up);
            Debug.DrawRay(m_rayOrigin.position, lookDir * 5, Color.magenta);

            Quaternion rot = OrthoNormalRotation(relativeForward.forward, relativeUp.up);
            rotationTest.rotation = rot;

            Vector3 moveDir = OrthoNormalMoveDirection(localMovement, relativeForward.forward, relativeUp.up);
            Debug.DrawRay(m_rayOrigin.position, moveDir * 5, Color.green);
        }
    }

}

