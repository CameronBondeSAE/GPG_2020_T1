using System.Collections.Generic;
using GPG220.Luca.Scripts.Abilities;
using GPG220.Luca.Scripts.GOAP.States;
using GPG220.Luca.Scripts.Unit;
using ReGoap.Core;
using ReGoap.Unity;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GPG220.Luca.Scripts.GOAP.Actions
{
    public class LuHealUnitAction : ReGoapAction<string, object>
    {
        //public int healAbilityIndex;
        /*[ShowInInspector]
        private UnitBase _injuredUnitToHeal;*/
        public AbilityBase healAbility;
        
        private LuExecuteAbilityState _exeAbState;
        
        protected override void Awake()
        {
            base.Awake();

            _exeAbState = GetComponent<LuExecuteAbilityState>();
        }
        
        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next, ReGoapState<string, object> settings, ReGoapState<string, object> goalState, System.Action<IReGoapAction<string, object>> done, System.Action<IReGoapAction<string, object>> fail)
        {
            base.Run(previous, next, settings, goalState, done, fail);

            if (settings.HasKey("abilityTarget"))
            {
                //agent.GetMemory().GetWorldState().Set("abilityTarget",_injuredUnitToHeal.gameObject);
                _exeAbState.ExecuteAbility(healAbility, OnDoneCallback, OnFailureCallback);
                return;
            }
            
            OnFailureCallback();
        }

        public override ReGoapState<string, object> GetEffects(GoapActionStackData<string, object> stackData)
        {
            effects.Set("unitHealed", true);
            
            if(stackData.agent.GetMemory().GetWorldState().HasKey("injuredAllyUnitsInProximityCount"))
                effects.Set("injuredAllyUnitsInProximityCount", (int)stackData.agent.GetMemory().GetWorldState().Get("injuredAllyUnitsInProximityCount"));
            
            return base.GetEffects(stackData);
        }
        
        public override List<ReGoapState<string, object>> GetSettings(GoapActionStackData<string, object> stackData)
        {
            var injuredAllyUnits = (List<UnitBase>)stackData.agent.GetMemory().GetWorldState().Get("injuredAllyUnitsInProximity");
            //settings.Set("injuredAllyUnitsInProximity", injuredAllyUnits); // TODO Eventually not needed anymore
            
            // Select lowest health ally unit in proximity
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
                    settings.Set("abilityTarget",lowestHealthUnit.gameObject);
                }
                    
            }
            
            
            
            return base.GetSettings(stackData);
        }

        public override ReGoapState<string, object> GetPreconditions(GoapActionStackData<string, object> stackData)
        {
            if (stackData.settings.HasKey("abilityTarget"))
            {
                GameObject abilityTarget = (GameObject) stackData.settings.Get("abilityTarget");
                var distanceToTarget = Vector3.Distance(transform.position, abilityTarget.transform.position);

                if (distanceToTarget > 5f) // TODO Replace 5 with ability range or so
                {
                    /*// Calculate a position to move to which is near the target
                    var dirTargetToSelf = transform.position - abilityTarget.transform.position;
                    var targetPos = abilityTarget.transform.position + dirTargetToSelf.normalized * 5f; // TODO Replace 5 with ability range or so*/
                    
                    //stackData.agent.GetMemory().GetWorldState().Set("targetPos", abilityTarget.transform.position);
                    preconditions.Set("targetPos", abilityTarget.transform.position);
                    preconditions.Set("targetWithinRange", 5f); // TODO Replace 5 with ability range or so
                }
                else
                {
                    preconditions.Clear();
                    
                }
                
            }
            
            return base.GetPreconditions(stackData);
        }

        public override bool CheckProceduralCondition(GoapActionStackData<string, object> stackData)
        {
            
            bool hasInjuredAllyNearby = (stackData.agent.GetMemory().GetWorldState().HasKey("injuredAllyUnitsInProximityCount")?(int)stackData.agent.GetMemory().GetWorldState().Get("injuredAllyUnitsInProximityCount"):-1) > 0;
            
            return base.CheckProceduralCondition(stackData) && hasInjuredAllyNearby;
        }
        
        public override void Exit(IReGoapAction<string, object> next)
        {
            base.Exit(next);

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
