using System;
using GPG220.Dylan.Scripts.GOAP.States;
using ReGoap.Core;
using ReGoap.Unity;
using UnityEngine;


namespace GPG220.Dylan.Scripts.GOAPFirstTry.Actions
{
    // ReSharper disable once InconsistentNaming
    public class Action_PathPossible : ReGoapAction<string, object>
    {
        public bool pathPossible;
        public bool teleportInstead;

        protected override void Awake()
        {
            base.Awake();

            preconditions.Set("hasTarget", true);


            if (pathPossible)
            {
                effects.Set("pathPossible", true);
            }
            else
            {
                teleportInstead = true;
            }

            if (teleportInstead)
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
            // check if path is possible if so call done and continue else try to teleport

            if (pathPossible)
            {
                doneCallback(this);
            }

            if (teleportInstead)
            {
                doneCallback(this);
            }
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