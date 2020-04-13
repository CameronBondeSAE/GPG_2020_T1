using System.Collections.Generic;
using System.Numerics;
using GPG220.Luca.Scripts.Abilities;
using GPG220.Luca.Scripts.Unit;
using ReGoap.Core;
using ReGoap.Unity;
using UnityEngine;

namespace GPG220.Luca.Scripts.GOAP.Actions
{
    public class LuExecuteAbilityAction : ReGoapAction<string, object>
    {
        public AbilityController _abilityController;
        
        protected override void Awake()
        {
            base.Awake();

            _abilityController = GetComponent<AbilityController>();
            preconditions.Set("executeAbility",true);
            // Precon abilityIndex set, ability exists, no cooldown
            //preconditions.Set("hasTargetPosition", true);
            effects.Set("abilityExecuted", true); // TODO
            
            //TODO _abilitySensor = GetComponent<UnitAbilitySensor>();
        }
        
        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next, ReGoapState<string, object> settings, ReGoapState<string, object> goalState, System.Action<IReGoapAction<string, object>> done, System.Action<IReGoapAction<string, object>> fail)
        {
            base.Run(previous, next, settings, goalState, done, fail);

            
            if (!settings.HasKey("abilityIndex") || _abilityController == null)
            {
                failCallback(this);
                return;
            }
            Debug.Log("1>>>>>>>>>Run Execute ABility");
            
            int abilityIndex = (int)settings.Get("abilityIndex");
            
            if (!_abilityController.abilities.ContainsKey(abilityIndex))
            {
                failCallback(this);
                return;
            }
            Debug.Log("2>>>>>>>>>Run Execute ABility");
            
            AbilityBase ability = _abilityController.abilities[abilityIndex];
            GameObject abilityTarget =
                (GameObject) (settings.HasKey("abilityTarget") ? settings.Get("abilityTarget") : null);

            if (ability != null)
            {
                Debug.Log("3>>>>>>>>>Run Execute ABility");
                if(abilityTarget == null)
                    _abilityController.SelectedExecuteAbility(ability);
                else
                    _abilityController.TargetExecuteAbility(ability, abilityTarget);

                doneCallback(this);
            }
            else
                failCallback(this);
        }

        public override List<ReGoapState<string, object>> GetSettings(GoapActionStackData<string, object> stackData)
        {
            Debug.Log("0>>>>>>>>>Run Execute ABility "+stackData.goalState.HasKey("abilityIndex")+" --- "+settings.HasKey("abilityIndex")+" --- "+stackData.currentState.HasKey("abilityIndex")+" --- "+effects.HasKey("abilityIndex"));
            if (stackData.currentState.HasKey("abilityTarget"))
            {
                settings.Set("abilityTarget",stackData.currentState.Get("abilityTarget"));
                Debug.Log(">>>>>>>>>y<<<<< HAS AbilityTarget: "+(GameObject)stackData.currentState.Get("abilityTarget"));
            }

            if (stackData.currentState.HasKey("abilityIndex"))
            {
                settings.Set("abilityIndex",stackData.currentState.Get("abilityIndex"));
                Debug.Log(">>>>>>>>>y<<<<< HAS AbilityIndex: "+(int)stackData.currentState.Get("abilityIndex"));
            }
            
            return base.GetSettings(stackData);
            
            //return new List<ReGoapState<string, object>>();
        }
        
        public override ReGoapState<string, object> GetPreconditions(GoapActionStackData<string, object> stackData)
        {
            if (stackData.settings.HasKey("abilityIndex") && _abilityController != null)
            {
                int abilityIndex = (int)stackData.settings.Get("abilityIndex");
            
                if (_abilityController.abilities.ContainsKey(abilityIndex))
                {
                    preconditions.Set("abilityIndex",abilityIndex);
                }
            }
            
            return base.GetPreconditions(stackData);
        }
/*

        public override bool CheckProceduralCondition(GoapActionStackData<string, object> stackData)
        {
            bool hasValidAbility = false;
            bool canExecuteAbility = false;
            
            int abilityIndex = -1;
            AbilityBase ability = null;
            //Debug.Log("=>=>=>=>=> Has Ability Index? "+stackData.goalState.HasKey("abilityIndex"));
            if (stackData.settings.HasKey("abilityIndex") && _abilityController != null)
            {
                abilityIndex = (int)stackData.settings.Get("abilityIndex");
                Debug.Log("=>=>=>=>=> Ability INdex: "+abilityIndex);
            
                if (_abilityController.abilities.ContainsKey(abilityIndex))
                {
                    ability = _abilityController.abilities[abilityIndex];
                }
            }

            if (ability != null)
            {
                hasValidAbility = true;
                canExecuteAbility = ability.CheckRequirements();
                Debug.Log("============= CAN EXECUTE ABILITY? "+canExecuteAbility);
            }
            
            /#1#/ has target, target in range
            var target = (UnitBase)(stackData.settings.HasKey("targetUnit") ? stackData.settings.Get("targetUnit") : null);
            bool hasTarget = target != null;

            var distToTarget = target == null ? float.PositiveInfinity : Vector3.Distance(transform.position, target.transform.position);
            bool targetInRange = distToTarget <= 10; // TODO Change value "10" to dynamic val from ability#1#
            
            return base.CheckProceduralCondition(stackData) && hasValidAbility && canExecuteAbility/*hasTarget && targetInRange && #1#;
        }*/
        /*

        public override void Exit(IReGoapAction<string, object> next)
        {
            base.Exit(next);

            var worldState = agent.GetMemory().GetWorldState();
            foreach (var pair in effects.GetValues()) {
                worldState.Set(pair.Key, pair.Value);
            }
        }*/

        protected virtual void OnFailureMovement()
        {
            failCallback(this);
        }

        protected virtual void OnDoneMovement()
        {
            
            doneCallback(this);
        }
    }
}
