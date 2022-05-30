using UnityEngine;

namespace SUHScripts
{
    [CreateAssetMenu(menuName = "SUHS/Scriptable Values/String Read Only")]
    public class StringReadOnlyValueSO : ScriptableObject
    {
        [SerializeField] string m_value = default;
        public string Value => m_value;
    }


}