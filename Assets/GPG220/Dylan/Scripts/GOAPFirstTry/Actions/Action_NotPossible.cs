using System;
using GPG220.Dylan.Scripts.GOAP.States;
using ReGoap.Core;
using ReGoap.Unity;
using UnityEngine;

namespace GPG220.Dylan.Scripts.GOAPFirstTry.Actions
{
    public class Action_NotPossible : ReGoapAction<string, object>
    {
        protected override void Awake()
        {
            base.Awake();
            
            preconditions.Set("notPossible", true);
            effects.Set("targetReached", true);
            
        }


        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next, ReGoapState<string, object> settings, ReGoapState<string, object> goalState, Action<IReGoapAction<string, object>> done,
            Action<IReGoapAction<string, object>> fail)
        {
            base.Run(previous, next, settings, goalState, done, fail);

            doneCallback(this);


        }

        public override void PlanExit(IReGoapAction<string, object> previousAction, IReGoapAction<string, object> nextAction, ReGoapState<string, object> settings, ReGoapState<string, object> goalState)
        {
            base.PlanExit(previousAction, nextAction, settings, goalState);
            
            preconditions.Clear();
            effects.Clear();
            
            var worldState = agent.GetMemory().GetWorldState();
            foreach (var pair in effects.GetValues()) {
                worldState.Set(pair.Key, pair.Value);
            }
        }

        public override void Exit(IReGoapAction<string, object> next)
        {
            base.Exit(next);
            
            preconditions.Clear();
            effects.Clear();
            
            var worldState = agent.GetMemory().GetWorldState();
            foreach (var pair in effects.GetValues()) {
                worldState.Set(pair.Key, pair.Value);
            }
        }
    }
}
