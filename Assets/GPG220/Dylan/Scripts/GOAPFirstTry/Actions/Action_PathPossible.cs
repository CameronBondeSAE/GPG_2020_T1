using System;
using System.Collections.Generic;
using GPG220.Blaide_Fedorowytsch.Scripts.PathFinding;
using GPG220.Dylan.Scripts.GOAP.States;
using ReGoap.Core;
using ReGoap.Unity;
using UnityEngine;
using GPG220.Blaide_Fedorowytsch.Scripts.PathFinding;
using UnityEngine.Experimental.VFX.Utility;


namespace GPG220.Dylan.Scripts.GOAPFirstTry.Actions
{
    // ReSharper disable once InconsistentNaming
    public class Action_PathPossible : ReGoapAction<string, object>
    {
        //TODO find some way to determine this bools state
        public bool isPathPossible;
        public bool allowedToTeleport;
        public List<Node> currentPath = new List<Node>();
        public Vector3 targetPosition;
        public SimplePathfinder simplePathfinder;
        protected override void Awake()
        {
            // simplePathfinder = FindObjectOfType<SimplePathfinder>();
            
            base.Awake();
        }

        
        public override ReGoapState<string, object> GetPreconditions(GoapActionStackData<string, object> stackData)
        {
            preconditions.Set("hasTarget", true);

            return base.GetPreconditions(stackData);
        }

        public override ReGoapState<string, object> GetEffects(GoapActionStackData<string, object> stackData)
        {
            if (CheckIfPathIsPossible())
            {
                effects.Set("pathPossible", true);
            }
            else
            {
                effects.Set("pathPossible", false);
                effects.Set("energyRequired", true);
            }

            return base.GetEffects(stackData);
        }

        private bool CheckIfPathIsPossible()
        {
            if(currentPath == null)
            {
                isPathPossible = false;
                allowedToTeleport = true;
            }
            else
            {
                isPathPossible = true;
                allowedToTeleport = false;
            }

            return isPathPossible;
        }
        

        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next,
            ReGoapState<string, object> settings, ReGoapState<string, object> goalState,
            Action<IReGoapAction<string, object>> done,
            Action<IReGoapAction<string, object>> fail)
        {
            base.Run(previous, next, settings, goalState, done, fail);

            doneCallback(this);
        }
        
        public void SetPath(List<Node> list)
        {
            currentPath = list;
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