using ReGoap.Core;
using ReGoap.Unity;
using UnityEngine;

namespace GPG220.Luca.Scripts.GOAP.Sensors
{
    public class LuTargetSensor : ReGoapSensor<string, object>
    {
        public Transform targetPos;
        
        public override void Init(IReGoapMemory<string, object> memory)
        {
            base.Init(memory);
            var state = memory.GetWorldState();
            state.Set("targetPos", targetPos.position);
        }

        public override void UpdateSensor()
        {
            if (targetPos == null || !targetPos.gameObject.activeInHierarchy)
                return;
            var state = memory.GetWorldState();
            state.Set("targetPos", targetPos.position);
        }
    }
}