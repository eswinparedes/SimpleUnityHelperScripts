using UnityEngine;

namespace SUHScripts
{
    [System.Serializable]
    public class DependencySourceRequirements
    {
        [SerializeField] DependencySourceTagSO[] m_allRequired = default;
        [SerializeField] DependencySourceTagSO[] m_anyRequired = default;
        [SerializeField] DependencySourceTagSO[] m_noneRequired = default;
        public bool DoesSourceMeetRequirements(IDependencySource source)
        {
            var tagsContainerOpt = source.Get<DependencySourceTagContainer>();

            if (!tagsContainerOpt.IsSome)
            {
                if (m_allRequired.Length > 0) return false;
                if (m_anyRequired.Length > 0) return false;
                return true;
            }

            var tagsContainer = tagsContainerOpt.Value;

            if (m_allRequired.Length > 0 && !tagsContainer.ContainsAll(m_allRequired))
                return false;

            if (m_anyRequired.Length > 0 && !tagsContainer.ContainsAny(m_anyRequired))
                return false;

            if (m_noneRequired.Length > 0 && tagsContainer.ContainsAny(m_noneRequired))
                return false;

            return true;
        }
    }

}
