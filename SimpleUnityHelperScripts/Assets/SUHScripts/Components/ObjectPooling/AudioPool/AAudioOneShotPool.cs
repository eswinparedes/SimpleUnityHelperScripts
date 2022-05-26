using UnityEngine;

namespace SUHScripts
{
    /// <summary>
    /// Plays audio through a pooled AudioSource and releases it back into the pool once its done.  Fire and Forget.
    /// </summary>
    public abstract class AAudioOneShotPool
    {
        public abstract AudioSource Get();
    }

}
