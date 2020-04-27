﻿using System;
using GPG220.Dylan.Scripts.GOAP.States;
using ReGoap.Core;
using ReGoap.Unity;
using UnityEngine;

namespace GPG220.Dylan.Scripts.GOAPFirstTry.Actions
{
    // ReSharper disable once InconsistentNaming
    public class Action_CheckEnergy : ReGoapAction<string, object>
    {
        public float energyAmount;
        protected override void Awake()
        {
            base.Awake();
            
            preconditions.Set("energyRequired", true );
            if (energyAmount > 0)
            {
                effects.Set("hasEnergy", true);
                
            }
            else
            {
                effects.Set("notPossible",true);
            }
        }


        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next,
            ReGoapState<string, object> settings, ReGoapState<string, object> goalState,
            Action<IReGoapAction<string, object>> done,
            Action<IReGoapAction<string, object>> fail)
        {
            base.Run(previous, next, settings, goalState, done, fail);
            // 
            if (energyAmount > 0)
            {
                
                doneCallback(this);
            }
            else
            {
                failCallback(this);
            }
            
            
            
            
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
