using UnityEngine;

namespace SUHScripts
{
    [CreateAssetMenu(menuName = "Dependency Source Tag")]
    public class DependencySourceTagSO : ScriptableObject
    {
        public string Name => this.name;
    }

}
