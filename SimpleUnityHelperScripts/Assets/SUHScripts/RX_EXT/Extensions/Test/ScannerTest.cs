using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace SUHScripts.Tests
{
    public class ScannerTest : MonoBehaviour
    {
        [SerializeField] KeyCode m_restartIncrementingKey = KeyCode.A;
        [SerializeField] KeyCode m_restartDecrementingKey = KeyCode.B;
        [SerializeField] KeyCode m_singleTimerKey = KeyCode.S;

        // Start is called before the first frame update
        void Start()
        {
            var tick = this.UpdateAsObservable().Select(_ => Time.deltaTime);

            var incrementTrigger = tick.Where(_ => Input.GetKeyDown(m_restartIncrementingKey));
            var decrementTrigger = tick.Where(_ => Input.GetKeyDown(m_restartDecrementingKey));
            var singleTrigger = tick.Where(_ => Input.GetKeyDown(m_singleTimerKey));

            var upwardsScan = tick.TimerScan(val => val, () => new FTimer(1, 0));
            var downwardsScan = tick.TimerScan(val => val, () => new FTimer(1, 1, false));

            var indexStream = OBV_Ext.MergeTriggersIntoIndex(incrementTrigger, decrementTrigger);

            var scanStream =
                indexStream
                .SwitchTo(index => index == 0 ? upwardsScan : downwardsScan)
                .Subscribe(
                    onNext: inputs =>
                    {
                        Debug.Log($"timer: {inputs.timer} value: {inputs.value}");
                    },
                    onCompleted: () => Debug.Log("Completed"))
                .AddTo(this);

            singleTrigger
                .Subscribe(_ => upwardsScan.Subscribe(onNext: t => {  }, onCompleted: () => Debug.Log("Single Completed")).AddTo(this))
                .AddTo(this);

            /*SUHS TODO: Create test for a timer that bounces up and down indefinitely (increments/decrements) at intervals
            var bounceStream =
                tick
                .TimerScan(tick => tick, () => new FTimer(1, 0))
                .Select(inputs => inputs.timer)
                .TakeWhile_IncludeLast(timer => timer.HasCompleted())
                .Scan()
                */
        }
    }

}
