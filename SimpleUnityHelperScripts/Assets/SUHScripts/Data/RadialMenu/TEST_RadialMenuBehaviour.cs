using UnityEngine;

namespace SUHScripts
{
    public class TEST_RadialMenuBehaviour : MonoBehaviour
    {
        [SerializeField] RadialMenuBehaviour m_radialMenu = default;
        [SerializeField] KeyCode m_actionBehaviourKey = KeyCode.Space;

        Vector2 input => new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        void Start()
        {
            m_radialMenu.Initialize();
        }

        // Update is called once per frame
        void Update()
        {
            m_radialMenu.Update(new Vector2(input.x, input.y));
            if (Input.GetKeyDown(m_actionBehaviourKey))
            {
                m_radialMenu.RequestActionOnSelectedRadialObject();
            }
        }
    }
}