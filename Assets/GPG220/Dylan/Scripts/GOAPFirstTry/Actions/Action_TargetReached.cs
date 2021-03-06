﻿using System;
using GPG220.Dylan.Scripts.GOAP.States;
using ReGoap.Core;
using ReGoap.Unity;
using UnityEngine;

namespace GPG220.Dylan.Scripts.GOAPFirstTry.Actions
{
    // ReSharper disable once InconsistentNaming
    public class Action_TargetReached : ReGoapAction<string, object>
    {
        public event Action targetReached;
        public Vector3 targetPosition;
        
        public override ReGoapState<string, object> GetPreconditions(GoapActionStackData<string, object> stackData)
        {
            preconditions.Set("moveToTarget", true);

            return base.GetPreconditions(stackData);
        }

        public override ReGoapState<string, object> GetEffects(GoapActionStackData<string, object> stackData)
        {
            effects.Set("targetReached", true);
            
            return base.GetEffects(stackData);
        }


        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next,
            ReGoapState<string, object> settings, ReGoapState<string, object> goalState,
            Action<IReGoapAction<string, object>> done,
            Action<IReGoapAction<string, object>> fail)
        {
            base.Run(previous, next, settings, goalState, done, fail);

            doneCallback(this);
        }

        // public void FixedUpdate()
        // {
        //     if (Vector3.Distance(transform.position, targetPosition) < 4f)
        //     {
        //         targetReached?.Invoke();
        //     }
        // }

        public void TriggerEvent()
        {
            targetReached?.Invoke();
        }

        public override void PlanExit(IReGoapAction<string, object> previousAction, IReGoapAction<string, object> nextAction, ReGoapState<string, object> settings, ReGoapState<string, object> goalState)
        {
            preconditions.Clear();
            effects.Clear();
        
            base.PlanExit(previousAction, nextAction, settings, goalState);
        }

        public override void Exit(IReGoapAction<string, object> next)
        {
            base.Exit(next);

            
            preconditions.Clear();
            effects.Clear();

            var worldState = agent.GetMemory().GetWorldState();
            foreach (var pair in effects.GetValues())
            {
                worldState.Set(pair.Key, pair.Value);
            }
        }
    }
}