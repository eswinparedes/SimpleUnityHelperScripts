using UnityEngine;

namespace SUHScripts
{
    public static class FrameStateOperations
    {
        

        public static BoolState FromKey(KeyCode keyCode)
        {
            if (Input.GetKeyDown(keyCode)) return BoolState.TrueThisFrame;
            if (Input.GetKey(keyCode)) return BoolState.TrueStay;
            if (Input.GetKeyUp(keyCode)) return BoolState.FalseThisFrame;
            return BoolState.FalseStay;
        }

        public static BoolState FromMouse(int index)
        {
            if (Input.GetMouseButtonDown(index)) return BoolState.TrueThisFrame;
            if (Input.GetMouseButtonUp(index)) return BoolState.FalseThisFrame;
            if (Input.GetMouseButton(index)) return BoolState.TrueStay;
            return BoolState.FalseStay;
        }
    }
}
