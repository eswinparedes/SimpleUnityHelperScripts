using SUHScripts.Functional;
using System.Collections.Generic;

namespace SUHScripts
{
    public static class DictionaryExtensions
    {
        public static Option<TValue> LookUp<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key)
        {
            if (@this.ContainsKey(key)) return @this[key].AsOption_UNSAFE();
            else return None.Default;
        }

    }

}
