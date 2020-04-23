using ReGoap.Unity.FSM;
using UnityEngine;
using System;

namespace GPG220.Dylan.Scripts.GOAP.States
{
    public class StateMove : SmState
    {
        public Transform target;
        public float speed = 1.0f;

        public bool needsToMove;

        public enum States
        {
            moving,Done,Fail
        }

        public States currentState;
        
        protected override void Awake()
        {
            base.Awake();
            Debug.Log("Move");
        }

        protected override void Update()
        {
            base.Update();

            if (CheckDistance())
            {
                MoveTowardsTarget();
            }
        }

        public override void Init(StateMachine stateMachine)
        {
            base.Init(stateMachine);
            var state = new SmTransition(GetPriority(), GetMoveState);
            var stateDone = new SmTransition(GetPriority(), GetIdleState);
            stateMachine.GetComponent<StateIdle>().Transitions.Add(state);
            Transitions.Add(stateDone);
            
        }

        public Type GetIdleState(ISmState state)
        {
            if (currentState != States.moving)
            {
                return typeof(StateIdle);
            }

            return null;
        }
        
        public Type GetMoveState(ISmState state)
        {
            if (currentState == States.moving)
            {
                return typeof(StateMove);
            }

            return null;
        }
        
        

        public bool CheckDistance()
        {
            var distanceTilTargetReached = Vector3.Distance(transform.position, target.position);
            if (distanceTilTargetReached <= 0.5f)
            {
                needsToMove = false;
                currentState = States.Done;
            }
            else
            {
                needsToMove = true;
                
            }

            return needsToMove;
        }

        public void MoveTowardsTarget()
        {
            currentState = States.moving;
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
            
            
        }
    }
}