using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LRStateBase : MonoBehaviour
{
    public LRStateBase idleState;
    public LRStateBase movingState;
    public LRStateBase deadState;

    public LRStateBase currentState;
    
    public void ChangeState()

    {
    
    }

}
