using ReGoap.Core;
using ReGoap.Unity;
using UnityEngine;

namespace GPG220.Dylan.Scripts.GOAP.Sensors
{
    public class TargetSensor : ReGoapSensor<string, object>
    {
        public Transform target;

        public override void Init(IReGoapMemory<string, object> memory)
        {
            base.Init(memory);
            var state = memory.GetWorldState();
            state.Set("targetPosition", target.position);
        }

        public override void UpdateSensor()
        {
            var state = memory.GetWorldState();
            state.Set("targetPosition", target.position);
            Debug.Log("Target is" + target.name);
        }
    }
}
