using GPG220.Dylan.Scripts.GOAP.States;
using ReGoap.Unity;
using UnityEngine;

namespace GPG220.Dylan.Scripts.GOAP.Actions
{
    // ReSharper disable once InconsistentNaming
    public class Action_Idle : ReGoapAction<string,object>
    {
        public StateMove stateMove;
        protected override void Awake()
        {
            base.Awake();
            
            stateMove = GetComponent<StateMove>();

            preconditions.Set("noLongerAtTarget", transform.position != stateMove.target.position );
            
        }
    }
}
