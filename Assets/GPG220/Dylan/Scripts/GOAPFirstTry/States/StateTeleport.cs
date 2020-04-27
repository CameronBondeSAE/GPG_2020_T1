using System;
using System.Collections;
using ReGoap.Unity.FSM;
using UnityEngine;

namespace GPG220.Dylan.Scripts.GOAP.States
{
    public class StateTeleport : SmState
    {
        public Transform target;
        public float speed = 1.0f;

        public bool needsToMove;

        public enum States
        {
            teleporting,Done,Fail
        }

        public States currentState;
        public float teleportTimeDelay = 2f;
        
        protected override void Awake()
        {
            base.Awake();
            Debug.Log("Move");
        }
        
        
        
        public override void Init(StateMachine stateMachine)
        {
            base.Init(stateMachine);
            var state = new SmTransition(GetPriority(), GetTeleportState);
            var stateDone = new SmTransition(GetPriority(), GetIdleState);
            stateMachine.GetComponent<StateIdle>().Transitions.Add(state);
            Transitions.Add(stateDone);
        }

        public Type GetIdleState(ISmState state)
        {
            if (currentState != States.teleporting)
            {
                return typeof(StateIdle);
            }

            return null;
        }
        
        public Type GetTeleportState(ISmState state)
        {
            if (currentState == States.teleporting)
            {
                return typeof(StateTeleport);
            }

            return null;
        }

        
        public void TeleportTo()
        {
            currentState = States.teleporting;
            StartCoroutine("TeleportDelay");

        }

        public IEnumerator TeleportDelay()
        {
            yield return new WaitForSeconds(teleportTimeDelay);
            transform.position = target.position;
            
        }
    }
}
