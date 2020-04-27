using System;
using ReGoap.Unity;
using UnityEngine;

namespace GPG220.Dylan.Scripts.GOAP.Goals
{
    public class GoalTargetReached : ReGoapGoal<string, object>
    {
        protected override void Awake()
        {
            base.Awake();
            goal.Set("targetReached", true);
            
        }

        
    }
}