using System.Collections.Generic;
using GPG220.Luca.Scripts.Abilities;
using GPG220.Luca.Scripts.Unit;
using ReGoap.Core;
using ReGoap.Unity;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GPG220.Luca.Scripts.GOAP.Actions
{
    public class LuSelectAllyToHealAction : ReGoapAction<string, object>
    {
        public int healAbilityIndex;
        [ShowInInspector]
        private UnitBase _injuredUnitToHeal;
        
        protected override void Awake()
        {
            base.Awake();
        }
        
        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next, ReGoapState<string, object> settings, ReGoapState<string, object> goalState, System.Action<IReGoapAction<string, object>> done, System.Action<IReGoapAction<string, object>> fail)
        {
            base.Run(previous, next, settings, goalState, done, fail);

            // TODO Select a unit to be healed
            if (_injuredUnitToHeal != null)
            {
                //OnDoneCallback();
            }
            else if (settings.HasKey("injuredAllyUnitsInProximity"))
            {
                List<UnitBase> injuredAllyUnits = (List<UnitBase>)settings.Get("injuredAllyUnitsInProximity");

                if (injuredAllyUnits != null && injuredAllyUnits.Count > 0)
                {
                    // Find ally unit in proximity with lowest health
                    UnitBase lowestHealthUnit = null;
                    float currentLowestHealth = 0;
                    foreach (var injuredUnit in injuredAllyUnits)
                    {
                        if (lowestHealthUnit == null ||
                            (injuredUnit != null && injuredUnit.health != null &&
                             injuredUnit.health.CurrentHealth < currentLowestHealth))
                        {
                            lowestHealthUnit = injuredUnit;
                            currentLowestHealth = injuredUnit.health.CurrentHealth;
                        }
                    }

                    if (lowestHealthUnit != null)
                    {
                        _injuredUnitToHeal = lowestHealthUnit;
                        effects.Set("abilityTarget",_injuredUnitToHeal.gameObject);
                        agent.GetMemory().GetWorldState().Set("abilityTarget",_injuredUnitToHeal.gameObject);
                        OnDoneCallback();
                        return;
                    }
                    
                }
                
            }
            
            OnFailureCallback();
        }

        public override ReGoapState<string, object> GetEffects(GoapActionStackData<string, object> stackData)
        {
            effects.Set("abilityIndex", healAbilityIndex);
            effects.Set("executeAbility", true);
            stackData.agent.GetMemory().GetWorldState().Set("abilityIndex",healAbilityIndex);
            if (_injuredUnitToHeal != null)
            {
                // Check if in range
                var distance = Vector3.Distance(transform.position, _injuredUnitToHeal.transform.position);
                if (distance > 10) // TODO Replace "10" with actual heal ability range.
                {
                    // Set effect for unit to move closer to target.
                    stackData.agent.GetMemory().GetWorldState().Set("targetPos",_injuredUnitToHeal.transform.position);
                }
                else
                {
                    // Set effect for unit to execute ability
                    // TODO effects.Set("abilityTarget",_injuredUnitToHeal.gameObject);
                    //effects.Set("abilityIndex", healAbilityIndex);
                    //effects.Set("executeAbility", true);
                }
            }
            else
            {
                //effects.Clear();
            }
            return base.GetEffects(stackData);
        }
        
        public override List<ReGoapState<string, object>> GetSettings(GoapActionStackData<string, object> stackData)
        {
            settings.Set("injuredAllyUnitsInProximity", stackData.agent.GetMemory().GetWorldState().Get("injuredAllyUnitsInProximity"));
            
            return base.GetSettings(stackData);
            /*if (!stackData.goalState.HasKey("targetUnit") ||
                stackData.goalState.Get("targetUnit") == null)
            {
                // TODO Get all injuredUnits from UnitSensor and select the closest one.... maybe even check if current one is out of range or already healred
                settings.Set("targetUnit", stackData.agent.GetMemory().GetWorldState().Get("injuredUnit"));
                return base.GetSettings(stackData);
            }
            return new List<ReGoapState<string, object>>();*/
        }

        public override bool CheckProceduralCondition(GoapActionStackData<string, object> stackData)
        {
            
            bool hasInjuredAllyNearby = (stackData.agent.GetMemory().GetWorldState().HasKey("injuredAllyUnitsInProximityCount")?(int)stackData.agent.GetMemory().GetWorldState().Get("injuredAllyUnitsInProximityCount"):-1) > 0;
            
            return base.CheckProceduralCondition(stackData) && hasInjuredAllyNearby;
        }
        
        public override void Exit(IReGoapAction<string, object> next)
        {
            base.Exit(next);

            _injuredUnitToHeal = null;
            /*var worldState = agent.GetMemory().GetWorldState();
            foreach (var pair in effects.GetValues()) {
                worldState.Set(pair.Key, pair.Value);
            }*/
        }

        protected virtual void OnFailureCallback()
        {
            failCallback(this);
        }

        protected virtual void OnDoneCallback()
        {
            
            doneCallback(this);
        }
    }
}
