using UnityEngine;

namespace SUHScripts
{
    public static class IndexUtility 
    {

        public static bool IndexIsOutOfRange(int index, int length) => index < 0 || index >= length;

        public static int AddClamped(int currentIndex, int movementDelta, int length)
        {
            if (movementDelta == 0) return currentIndex;

            return Mathf.Clamp(currentIndex + movementDelta, 0, length - 1);
        }

        public static int AddModulated(this int currentIndex, int movementDelta, int length)
        {
            if (movementDelta == 0) return currentIndex;

            if(movementDelta > 0)
            {
                var next = currentIndex + movementDelta;
                if(next >= length)
                {
                    var mod = next % (length);
                    return mod;
                }
                else
                {
                    return next;
                }
            }
            else
            {
                var prev = currentIndex + movementDelta;
                if(prev < 0)
                {
                    return prev % (length - 1);
                }
                else
                {
                    return prev;
                }
            }
        }
        public static int AddCircular(int index, int movementDelta, int length)
        {
            int next = index + movementDelta;
            if(IndexIsOutOfRange(next, length))
            {
                return movementDelta < 0 ? length - 1 : 0;
            }
            else
            {
                return next;
            }
            
        }
    }

}
