using System;
using GPG220.Dylan.Scripts.GOAP.States;
using ReGoap.Core;
using ReGoap.Unity;
using UnityEngine;

namespace GPG220.Dylan.Scripts.GOAP.Actions
{
    // ReSharper disable once InconsistentNaming
    public class Action_Teleport : ReGoapAction<string, object>
    {
        public Vector3 targetPosition;
        
        public float energyAmount = 10;

        protected override void Awake()
        {
            base.Awake();

            
            preconditions.Set("hasEnergy", true );
            
            effects.Set("moveToTarget", true);
        }


        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next,
            ReGoapState<string, object> settings, ReGoapState<string, object> goalState,
            Action<IReGoapAction<string, object>> done,
            Action<IReGoapAction<string, object>> fail)
        {
            base.Run(previous, next, settings, goalState, done, fail);
            // 
            energyAmount -= 1;

            doneCallback(this);


        }

        public override void Exit(IReGoapAction<string, object> next)
        {
            base.Exit(next);

            var worldState = agent.GetMemory().GetWorldState();
            foreach (var pair in effects.GetValues())
            {
                worldState.Set(pair.Key, pair.Value);
            }
        }
    }
}