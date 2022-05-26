using System;
using UnityEngine;

namespace SUHScripts
{
    public static class BoolStateOperations
    {
        public static BoolState Update(this BoolState @this, bool isTrue)
        {
            bool trueThisFrame = isTrue && @this != BoolState.TrueThisFrame && @this != BoolState.TrueStay;

            if (trueThisFrame) return BoolState.TrueThisFrame;

            bool trueStay = isTrue && !trueThisFrame;

            if (trueStay) return BoolState.TrueStay;

            //If this value turned false this frame, then:
            //- the new input must be false
            //- the current BoolState must not be FalseThisFrame
            //- the current Boolstate must be either TrueThisFrame or TrueStay
            //TODO: Remove possible rendundant check of @this != BoolState.FalseThisFrame and test
            bool falseThisFrame = !isTrue && @this != BoolState.FalseThisFrame &&
                (@this == BoolState.TrueThisFrame || @this == BoolState.TrueStay);

            if (falseThisFrame) return BoolState.FalseThisFrame;

            return BoolState.FalseStay;
        }

        public static bool IsTrue(this BoolState @this) =>
            @this == BoolState.TrueThisFrame || @this == BoolState.TrueStay;
        public static BoolState FromMouseButton(int button)
        {
            if (Input.GetMouseButtonDown(button)) return BoolState.TrueThisFrame;
            else if (Input.GetMouseButton(button)) return BoolState.TrueStay;
            else if (Input.GetMouseButtonUp(button)) return BoolState.FalseThisFrame;
            else return BoolState.FalseStay;
        }

        public static BoolState FromKey(KeyCode key)
        {
            if (Input.GetKeyDown(key)) return BoolState.TrueThisFrame;
            else if (Input.GetKey(key)) return BoolState.TrueStay;
            else if (Input.GetKeyUp(key)) return BoolState.FalseThisFrame;
            else return BoolState.FalseStay;
        }

        /// <summary>
        /// Returns a function that ensures FalseThisFrame is called whenever the function is paused "pause" before False this frame is called
        /// Will return falseStay the next frame update is called after a pause
        /// Does not pass on the input state while paused and waits for the input state to be FalseStay before passing it on
        /// </summary>
        /// <returns></returns>
        public static Func<BoolState, bool, BoolState> BuildFrameStaggerFlow(bool seedIsFlowStopped = false)
        {
            var isFlowStopped = seedIsFlowStopped;
            var isStaggered = false;
            var internalState = BoolState.FalseStay;

            return (inputState, stopFlow) =>
            {
                //We consider this a stagger if the inputFlow doesn't match the internal flow
                if (stopFlow != isFlowStopped)
                {
                    isStaggered = true;
                }

                isFlowStopped = stopFlow;

                //When the flow is stopped, don't really care about staggering since that only matters when flow is not paused
                if (isFlowStopped)
                {
                    //ignore the input flow and only worry about returning false this frame
                    switch (internalState)
                    {
                        //need to ensure the internal state is synched back to the expected order of operations
                        case BoolState.TrueThisFrame:
                        case BoolState.TrueStay:
                            internalState = BoolState.FalseThisFrame;
                            return internalState;

                        case BoolState.FalseThisFrame:
                            internalState = BoolState.FalseStay;
                            return internalState;

                        case BoolState.FalseStay:
                            return internalState;

                        default: throw new Exception($"Pattern match not exhaustive on: {internalState}");
                    }
                }
                else if (isStaggered)
                {
                    switch (internalState)
                    {
                        //The flow was unpaused before FalseThisFrame was returned, so make sure false this frame is returned
                        case BoolState.TrueThisFrame:
                        case BoolState.TrueStay:
                            internalState = BoolState.FalseThisFrame;
                            return internalState;

                        //False this frame was returned, however, we cannot freely return the input state until it is TrueThisFrame or FalseStay once again
                        case BoolState.FalseThisFrame when inputState == BoolState.FalseStay || inputState == BoolState.TrueThisFrame:
                        case BoolState.FalseStay when inputState == BoolState.FalseStay || inputState == BoolState.TrueThisFrame:
                            isStaggered = false;
                            internalState = inputState;
                            return internalState;

                        //Until the above case is matched, return false stay.  we are waiting for the input state to either be falseStay or TrueThisFrame
                        case BoolState.FalseThisFrame:
                        case BoolState.FalseStay:
                            internalState = BoolState.FalseStay;
                            return internalState;

                        default: throw new Exception($"Pattern match not exhaustive on: {internalState}");
                    }
                }
                else
                {
                    //We are no longer staggered, so can freely return the input state
                    internalState = inputState;
                    return internalState;
                }
            };
        }
    }
}
