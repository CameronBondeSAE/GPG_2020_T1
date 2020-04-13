using System.Collections;
using System.Collections.Generic;
using ReGoap.Unity;
using UnityEngine;

public class LuReachTargetGoal : ReGoapGoal<string, object>
{
    
    protected override void Awake()
    {
        base.Awake();
        
        goal.Set("targetPositionReached", true);
        
        
    }
    
}
