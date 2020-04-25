using System;
using System.Collections;
using ReGoap.Core;
using ReGoap.Unity;
using UnityEngine;

namespace GPG220.Dylan.Scripts.GOAPFirstTry.Actions
{
    // ReSharper disable once InconsistentNaming
    public class Action_Teleport : ReGoapAction<string, object>
    {
        public Transform targetPosition;
        
        public float teleportDelay;
        private bool isRunning;

        protected override void Awake()
        {
            base.Awake();

            // preconditions.Set("hasEnergy", true);
            //
            // effects.Set("moveToTarget", true);
        }
        
        public override ReGoapState<string, object> GetPreconditions(GoapActionStackData<string, object> stackData)
        {
            
            preconditions.Set("hasEnergy", true);

            return base.GetPreconditions(stackData);
        }

        public override ReGoapState<string, object> GetEffects(GoapActionStackData<string, object> stackData)
        {
            effects.Set("moveToTarget", true);

            return base.GetEffects(stackData);
        }

        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next,
            ReGoapState<string, object> settings, ReGoapState<string, object> goalState,
            Action<IReGoapAction<string, object>> done,
            Action<IReGoapAction<string, object>> fail)
        {
            base.Run(previous, next, settings, goalState, done, fail);
            
            
            
            isRunning = false;
            
            if (!isRunning)
            {
                isRunning = true;
                StartCoroutine("TeleportDelay");
            }

            doneCallback(this);
        }

        public IEnumerator TeleportDelay()
        {
            yield return new WaitForSeconds(teleportDelay);
            transform.position = targetPosition.position;
            isRunning = false;
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