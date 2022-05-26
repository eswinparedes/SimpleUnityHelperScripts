using System.Collections.Generic;
using UnityEngine;

namespace SUHScripts
{
    using static Radial;
    using static MathHelper;
    
    [System.Serializable]
    public class RadialMenuBehaviour
    {
        [SerializeField] List<M_RadialObject> m_interactablePrefabs = new List<M_RadialObject>();
        [SerializeField] Transform m_identity = default;
        [SerializeField] Vector3 m_radialAxis = default;
        [SerializeField] Vector3 m_offset = default;
   
        public RadialSectionData RadialSectionData { get; private set; } = default;
        public List<M_RadialObject> RadialObjects { get; private set; } = new List<M_RadialObject>();
        public List<Vector3> RadialPositions { get; private set; } = new List<Vector3>();
        public int CurrentSelectedIndex { get; private set; } = -1;

        public void Initialize()
        {
            RadialSectionData = GetRadialSectionData(m_interactablePrefabs.Count);
            RadialPositions = RadialPositions(m_radialAxis, m_offset, RadialSectionData, m_identity.rotation);

            for(int i = 0; i < m_interactablePrefabs.Count; i++)
            {
                M_RadialObject inst = GameObject.Instantiate(m_interactablePrefabs[i], m_identity);
                inst.transform.position = m_identity.position + RadialPositions[i];
                RadialObjects.Add(inst);
            }
        }

        public void Update(Vector2 input)
        {
            bool inputValid = input != Vector2.zero;
            float angle = AngleDegreesFrom360(input.x, input.y);
            int selectedIndex = AngleSectionIndex(angle, RadialSectionData);

            bool isSelectionTheSame = selectedIndex == CurrentSelectedIndex;

            if (inputValid)
            {
                if(selectedIndex != CurrentSelectedIndex)
                {
                    if(CurrentSelectedIndex >= 0)
                    {
                        Debug.Log($"Releasing {CurrentSelectedIndex}");
                        RadialObjects[CurrentSelectedIndex].OnRadialObjectDeslected.Invoke();
                        CurrentSelectedIndex = selectedIndex;
                        RadialObjects[CurrentSelectedIndex].OnRadialObjectSelected.Invoke();
                        Debug.Log($"Selecting {CurrentSelectedIndex}");
                    }
                    else
                    {
                        CurrentSelectedIndex = selectedIndex;
                        RadialObjects[CurrentSelectedIndex].OnRadialObjectSelected.Invoke();
                        Debug.Log($"Selecting {CurrentSelectedIndex}");
                    }
                }
            }
            else
            {
                if(CurrentSelectedIndex >= 0)
                {
                    Debug.Log($"Releasing {CurrentSelectedIndex}");
                    RadialObjects[CurrentSelectedIndex].OnRadialObjectDeslected.Invoke();
                    CurrentSelectedIndex = -1;
                }
            }
        }

        public void RequestActionOnSelectedRadialObject()
        {
            if(CurrentSelectedIndex >= 0)
            {
                RadialObjects[CurrentSelectedIndex].OnRadialObjectRequestAction.Invoke();
            }
        }
    }
}

