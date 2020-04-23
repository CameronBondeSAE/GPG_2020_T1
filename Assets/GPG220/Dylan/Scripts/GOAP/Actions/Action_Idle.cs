using ReGoap.Unity;
using UnityEngine;

namespace GPG220.Dylan.Scripts.GOAP.Actions
{
    // ReSharper disable once InconsistentNaming
    public class Action_Idle : ReGoapAction<string,object>
    {
        protected override void Awake()
        {
            base.Awake();
            
            Debug.Log("Do Nothing");
        }
    }
}
