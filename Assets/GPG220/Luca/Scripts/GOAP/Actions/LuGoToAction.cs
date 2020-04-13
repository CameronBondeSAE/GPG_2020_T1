using System.Collections.Generic;
using GPG220.Luca.Scripts.GOAP.States;
using ReGoap.Core;
using ReGoap.Unity;
using UnityEngine;

namespace GPG220.Luca.Scripts.GOAP.Actions
{
    public class LuGoToAction : ReGoapAction<string, object>
    {
        private LuGoToState _gts;
        protected override void Awake()
        {
            base.Awake();
            //preconditions.Set("hasTargetPosition", true);
            //effects.Set("targetPosition", Vector3.zero);
            
            _gts = GetComponent<LuGoToState>();
        }

        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next, ReGoapState<string, object> settings, ReGoapState<string, object> goalState, System.Action<IReGoapAction<string, object>> done, System.Action<IReGoapAction<string, object>> fail)
        {
            base.Run(previous, next, settings, goalState, done, fail);


            if (settings.HasKey("objectivePos"))
            {
                Debug.Log("_gts is null?... "+(_gts == null)+" or... settings.objectivePos..."+(settings.Get("objectivePos") == null));
                
                if (settings.HasKey("targetWithinRange"))
                {
                    
                    _gts.GoTo((Vector3) settings.Get("objectivePos"), (float)settings.Get("targetWithinRange"), OnDoneMovement, OnFailureMovement);
                }
                else
                {
                    _gts.GoTo((Vector3) settings.Get("objectivePos"), OnDoneMovement, OnFailureMovement);
                }
                
            }
            else
                failCallback(this);
        }

        public override ReGoapState<string, object> GetEffects(GoapActionStackData<string, object> stackData)
        {
            effects.Set("targetPositionReached", true);
            if (stackData.settings.HasKey("objectivePos"))
            {
                effects.Set("targetPos", (Vector3)stackData.settings.Get("objectivePos"));
            }
            if (stackData.settings.HasKey("targetWithinRange"))
            {
                effects.Set("targetWithinRange", (float)stackData.settings.Get("targetWithinRange"));
            }
            /*if (stackData.settings.HasKey("objectivePos"))
            {
                //_eyeSensor.UpdateSensor(); // TODO correct?
                effects.Set("targetPosition", stackData.settings.Get("objectivePos"));
                if (stackData.settings.HasKey("targetPositionReached"))
                    effects.Set("targetPositionReached", true);
            }
            else
            {
                effects.Clear();
            }*/
            
            return base.GetEffects(stackData);
        }
        
        public override List<ReGoapState<string, object>> GetSettings(GoapActionStackData<string, object> stackData)
        {
            Debug.Log("GoToAction HasKey... GoalState: "+stackData.goalState.HasKey("targetPos")+" ...CurrentState: "+stackData.currentState.HasKey("targetPos"));
            if (stackData.goalState.HasKey("targetPos"))
            {
                settings.Set("objectivePos", (Vector3)stackData.goalState.Get("targetPos"));
            }
            if (stackData.goalState.HasKey("targetWithinRange"))
            {
                settings.Set("targetWithinRange", (float)stackData.goalState.Get("targetWithinRange"));
            }
            return base.GetSettings(stackData);
            /*else if (stackData.goalState.HasKey("targetPositionReached") && stackData.goalState.Count == 1)
            {
                /*settings.Set("objectivePos", stackData.agent.GetMemory().GetWorldState().Get("targetPos"));
                settings.Set("targetPositionReached", true);#1# // TODO Might be wrong..
                return base.GetSettings(stackData);
            }*/
            //return new List<ReGoapState<string, object>>();
        }
/*

        public override bool CheckProceduralCondition(GoapActionStackData<string, object> stackData)
        {
            return base.CheckProceduralCondition(stackData) && stackData.settings.HasKey("objectivePos");
        }*/
        
        /*public override void Exit(IReGoapAction<string, object> next)
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
