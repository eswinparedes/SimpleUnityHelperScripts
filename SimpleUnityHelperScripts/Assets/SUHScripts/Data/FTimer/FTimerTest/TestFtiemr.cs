using UnityEngine;
using UnityEngine.Events;

namespace SUHScripts.Tests
{
    public class TestFtiemr : MonoBehaviour
    {
        [SerializeField] Transform m_mover = default;
        [SerializeField] float m_timerLength = 1;
        [SerializeField] Vector3 moveVector3 = Vector3.one;
        [SerializeField] bool loop = false;
        [SerializeField] bool pingPong = false;
        [SerializeField] bool canUpdate = false;

        [SerializeField] UnityEvent onTimerCmoplete = new UnityEvent();
        [SerializeField] [Range(0, 1)] float alpha = 0;

        public FTimer Timer { get; private set; }
        Vector3 a;

        void Start()
        {
            Timer = new FTimer(m_timerLength, 0);
            a = m_mover.position;
        }

        public void Bounce()
        {
            Timer = Timer.Restarted(pingPong, true);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Bounce();
            }

            if (canUpdate)
            {
                Timer = Timer.Tick(Time.deltaTime, loop: loop, pingPong: pingPong);//, onComplete: onTimerCmoplete.Invoke);
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                Timer = Timer.AtAlpha(alpha);
            }

            m_mover.position =
                Vector3.Lerp(a, a + moveVector3, Timer.TimeAlpha());
        }
    }
}

