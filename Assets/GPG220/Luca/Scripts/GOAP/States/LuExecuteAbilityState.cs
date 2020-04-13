using System;
using System.Linq;
using GPG220.Luca.Scripts.Abilities;
using ReGoap.Unity.FSM;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GPG220.Luca.Scripts.GOAP.States
{
    [RequireComponent(typeof(AbilityController))]
    public class LuExecuteAbilityState : SmState
    {
        public GameObject target;
        public AbilityBase ability;
        private AbilityController _abilityController;
        
        private Action _onExecutionDone;
        private Action _onExecutionFailure;
        
        public enum ExecutionStates
        {
            Disabled, Executing, WaitForRequirements, Active, Success, Failure
        }
        
        [ShowInInspector]
        private ExecutionStates _currentState;
        
        //private MovableUnit _movableUnit;

        protected override void Awake()
        {
            base.Awake();

            _abilityController = GetComponent<AbilityController>();
        }

        protected override void Update()
        {
            base.Update();

            DoExecution();
        }

        protected virtual void DoExecution()
        {
            if (_abilityController == null || ability == null)
            {
                _onExecutionFailure?.Invoke();
                return;
            }
                
            
            if (ability.currentCooldown > 0)
            {
                _currentState = ExecutionStates.WaitForRequirements;
                return;
            }

            _currentState = ExecutionStates.Executing;

            if (target == null)
            {
                _abilityController.SelectedExecuteAbility(ability);
                _currentState = ExecutionStates.Success;
            }
            else
            {
                _abilityController.TargetExecuteAbility(ability);
                _currentState = ExecutionStates.Success;
            }
        }
        
        public void ExecuteAbility(AbilityBase ability, Action onDoneExecuting, Action onFailureExecuting)
        {
            this.ability = ability == null ? _abilityController.defaultTargetAbility : ability;

            ExecuteAbility(onDoneExecuting, onFailureExecuting);
        }
        
        public void ExecuteAbility(AbilityBase ability, GameObject target, Action onDoneExecuting, Action onFailureExecuting)
        {
            this.ability = ability == null ? _abilityController.defaultTargetAbility : ability;
            this.target = target;
            ExecuteAbility(onDoneExecuting, onFailureExecuting);
        }

        void ExecuteAbility(Action onDoneExecuting, Action onFailureExecuting)
        {
            _currentState = ExecutionStates.Executing;
            _onExecutionDone = onDoneExecuting;
            _onExecutionFailure = onFailureExecuting;
        }
        
        public override void Enter()
        {
            base.Enter();
            _currentState = ExecutionStates.Active;
        }

        public override void Exit()
        {
            base.Exit();

            ability = null;
            target = null;
            
            if (_currentState == ExecutionStates.Success)
                _onExecutionDone();
            else
                _onExecutionFailure();
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
            if (_currentState != ExecutionStates.Active)
                return typeof(LuIdleState);
            return null;
        }

        private Type Transition(ISmState state)
        {
            if (_currentState == ExecutionStates.Executing)
                return typeof(LuExecuteAbilityState);
            return null;
        }
    }
}
