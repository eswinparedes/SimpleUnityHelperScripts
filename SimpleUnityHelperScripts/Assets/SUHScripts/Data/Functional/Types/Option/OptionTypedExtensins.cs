using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUHScripts.Functional
{
    public static class OptionTypedExtensions
    {
        public static int ReducedAdd(this Option<int> @this, int baseValue, int addValue) =>
        @this.Match(baseValue, baseValue + addValue);

        public static int ReducedGet(this Option<int> @this, int defaultValue = 1) =>
            @this.Reduce(defaultValue);

        public static float ReducedAdd(this Option<float> @this, float baseValue, float addValue) =>
        @this.Match(baseValue, baseValue + addValue);

        public static float ReducedGet(this Option<float> @this, float defaultValue = 1) =>
            @this.Reduce(defaultValue);
    }
}
