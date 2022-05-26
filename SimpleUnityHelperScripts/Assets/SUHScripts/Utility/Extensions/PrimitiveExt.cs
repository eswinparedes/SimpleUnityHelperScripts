using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUHScripts
{
    public static class PrimitiveExt 
    {
        public static bool NotNull<T>(this T @this)
        {
            return !@this.IsNull();
        }
        public static bool IsNull<T>(this T @this)
        {
            return @this == null || ReferenceEquals(@this, null) || @this.Equals(null);
        }
    }

}
