using SUHScripts.Functional;

namespace SUHScripts
{
    
    public static class RaycastDataExt
    {
        public static Option<T> GetComponentOption<T>(this RaycastData @this)
        {
            if (@this.RaycastHitOption.IsSome)
                return @this.RaycastHitOption.Value.collider.gameObject.GetComponent<T>().AsOption_SAFE();
            else
                return None.Default;
        }
    }
}

