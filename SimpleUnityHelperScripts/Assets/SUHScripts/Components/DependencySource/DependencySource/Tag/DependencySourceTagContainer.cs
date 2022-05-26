using System.Collections.Generic;
using System.Linq;

namespace SUHScripts
{
    public class DependencySourceTagContainer
    {
        public DependencySourceTagContainer(IEnumerable<DependencySourceTagSO> tags)
        {
            m_tags = new HashSet<DependencySourceTagSO>(tags);
        }

        public DependencySourceTagContainer(params DependencySourceTagSO[] tags)
        {
            m_tags = new HashSet<DependencySourceTagSO>(tags);
        }

        public DependencySourceTagContainer(HashSet<DependencySourceTagSO> tags)
        {
            m_tags = tags;
        }

        HashSet<DependencySourceTagSO> m_tags;
        public IEnumerable<DependencySourceTagSO> Tags => m_tags;

        public bool Contains(DependencySourceTagSO tag)
        {
            return m_tags.Contains(tag);
        }

        public bool ContainsAny(IReadOnlyList<DependencySourceTagSO> tags)
        {
            for (int i = 0; i < tags.Count; i++)
            {
                if (m_tags.Contains(tags[i])) return true;
            }

            return false;
        }

        public bool ContainsAll(IReadOnlyList<DependencySourceTagSO> tags)
        {
            for (int i = 0; i < tags.Count; i++)
            {
                if (!m_tags.Contains(tags[i])) return false;
            }

            return true;
        }

        public bool AddTag(DependencySourceTagSO tag)
        {
            return m_tags.Add(tag);
        }
    }
}
