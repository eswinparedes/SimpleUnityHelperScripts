using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUHScripts
{
    public class TestCameraLook : MonoBehaviour {

        public Transform LookTransform;
        public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
        public RotationAxes axes = RotationAxes.MouseXAndY;
        public float sensitivityX = 15F;
        public float sensitivityY = 15F;

        public float minimumX = -360F;
        public float maximumX = 360F;

        public float minimumY = -60F;
        public float maximumY = 60F;

        float rotationY = 0F;

        void Update()
        {
            UpdateCameraMovement();
        }

        void UpdateCameraMovement()
        {
            if (axes == RotationAxes.MouseXAndY)
            {
                float rotationX = LookTransform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

                LookTransform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
            }
            else if (axes == RotationAxes.MouseX)
            {
                LookTransform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
            }
            else
            {
                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

                LookTransform.localEulerAngles = new Vector3(-rotationY, LookTransform.localEulerAngles.y, 0);
            }
        }

        void DoNothing() { }
    }

}
