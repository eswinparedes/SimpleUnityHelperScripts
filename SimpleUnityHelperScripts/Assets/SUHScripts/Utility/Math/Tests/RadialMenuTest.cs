using System.Collections.Generic;
using UnityEngine;

namespace SUHScripts.Tests
{
    using static Radial;
    using static MathHelper;
    public class RadialMenuTest : MonoBehaviour
    {
        [SerializeField] int m_sections = default;
        [SerializeField] GameObject m_prefabInstance = default;

        Material m_default;
        List<GameObject> m_objects = new List<GameObject>();
        // Start is called before the first frame update
        RadialSectionData data;
        void Start()
        {
            m_default = m_prefabInstance.GetComponent<MeshRenderer>().material;

            data = GetRadialSectionData(m_sections);
            List<Vector3> positions = RadialPositions(Vector3.forward, Vector3.up, data, this.transform.rotation);

            for (int i = 0; i < m_sections; i++)
            {
                GameObject inst = Instantiate(m_prefabInstance);
                inst.transform.position = positions[i] * 3;
                m_objects.Add(inst);
            }

            return;
        }
        Vector2 input => new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        // Update is called once per frame
        void Update()
        {
            float angle = AngleDegreesFrom360(input.x, input.y);
            int selection = AngleSectionIndex(angle, data);
            Debug.Log($"SelectedIndex {selection}");
            Debug.Log($"Angle: {angle} Data: {data}");
            m_objects.ForEach(obj => obj.GetComponent<MeshRenderer>().material = m_default);
            m_objects[selection].GetComponent<MeshRenderer>().material = null;
        }  
    }

}
