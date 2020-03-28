using ReGoap.Unity;
using UnityEngine;

namespace GOAP
{
    public class GoapGoal : ReGoapGoal<string, object>
    {
        protected override void Awake()
        {
            base.Awake();
            goal.Set("AtTargetPositon", true);
        }
    }
}
