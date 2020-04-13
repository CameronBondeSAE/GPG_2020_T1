using System;
using GPG220.Luca.Scripts.Unit;
using ReGoap.Unity.FSM;
using UnityEngine;

namespace GPG220.Luca.Scripts.GOAP.States
{
    public class LuGoToState : SmState
    {
        private Transform targetTransform;
        public Vector3 targetPosition;
        private Action onMovementDone;
        private Action onMovementFailure;

        private CharacterController _characterController;

        public float goalProximityThreshold = 0.5f;

        public float movementSpeed = 2;
        
        public enum GoToStates
        {
            Disabled, Moving, Active, Success, Failure
        }
        public GoToStates _currentState;
        
        //private MovableUnit _movableUnit;

        protected override void Awake()
        {
            base.Awake();

            _characterController = GetComponent<CharacterController>();
            //_movableUnit = GetComponent<MovableUnit>();
        }

        protected override void Update()
        {
            base.Update();

            DoMove();
        }

        protected virtual void DoMove()
        {
            if (_characterController == null)
                onMovementFailure?.Invoke();
            
            targetPosition = targetTransform?.position ?? targetPosition;

            var distanceToTarget = Vector3.Distance(transform.position, targetPosition);

            var movement = targetPosition - transform.position;
            _characterController.SimpleMove(movement.normalized * movementSpeed);

            if (distanceToTarget <= goalProximityThreshold)
                _currentState = GoToStates.Success; //onMovementDone?.Invoke();
        }
        
        public void GoTo(Vector3 position, Action onDoneMovement, Action onFailureMovement)
        {
            targetPosition = position;
            GoTo(onDoneMovement, onFailureMovement);
        }
        
        public void GoTo(Vector3 position, float targetDistanceThreshold, Action onDoneMovement, Action onFailureMovement)
        {
            targetPosition = position;
            goalProximityThreshold = targetDistanceThreshold;
            GoTo(onDoneMovement, onFailureMovement);
        }
        
        public void GoTo(Transform transform, Action onDoneMovement, Action onFailureMovement)
        {
            targetTransform = transform;
            GoTo(onDoneMovement, onFailureMovement);
        }

        void GoTo(Action onDoneMovement, Action onFailureMovement)
        {
            _currentState = GoToStates.Moving;
            onMovementDone = onDoneMovement;
            onMovementFailure = onFailureMovement;
        }
        
        public override void Enter()
        {
            base.Enter();
            _currentState = GoToStates.Active;
        }

        public override void Exit()
        {
            base.Exit();
            if (_currentState == GoToStates.Success)
                onMovementDone();
            else
                onMovementFailure();
        }
        
        public override void Init(StateMachine stateMachine)
        {
            base.Init(stateMachine);
            var transition = new SmTransition(GetPriority(), Transition);
            var doneTransition = new SmTransition(GetPriority(), DoneTransition);
            stateMachine.GetComponent<LuIdleState>().Transitions.Add(transition);
            Transitions.Add(doneTransition);
        }

        private Type DoneTransition(ISmState state)
        {
            if (_currentState != GoToStates.Active)
                return typeof(LuIdleState);
            return null;
        }

        private Type Transition(ISmState state)
        {
            if (_currentState == GoToStates.Moving)
                return typeof(LuGoToState);
            return null;
        }
        
        
    }
}
