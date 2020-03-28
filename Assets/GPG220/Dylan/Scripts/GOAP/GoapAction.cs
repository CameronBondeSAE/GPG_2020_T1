using System;
using Cinemachine;
using ReGoap.Core;
using ReGoap.Unity;
using UnityEngine;

namespace GOAP
{
    public class GoapAction : ReGoapAction<string, object>
    {
        public Transform target;
        public float speed = 1.0f;
        protected override void Awake()
        {
            base.Awake();
            
            preconditions.Set("myPrecondition", target.position != transform.position );
            effects.Set("myEffects", transform.position = target.position);
        }

        private object MoveTowardsTarget()
        {
            float step =  speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
            return transform.position;
        }


        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next, ReGoapState<string, object> settings, ReGoapState<string, object> goalState, Action<IReGoapAction<string, object>> done,
            Action<IReGoapAction<string, object>> fail)
        {
            base.Run(previous, next, settings, goalState, done, fail);
            // do your own game logic here
            
            Debug.Log("Hi");
            
            
            
            // when done, in this function or outside this function, call the done or fail callback, automatically saved to doneCallback and failCallback by ReGoapAction
            doneCallback(this); 
            // this will tell the ReGoapAgent that the action is successfully done and go ahead in the action plan
            // if the action has failed then run failCallback(this), the ReGoapAgent will automatically invalidate the whole plan and ask the ReGoapPlannerManager to create a new plan
        }
        
        public override void Exit(IReGoapAction<string, object> next)
        {
            base.Exit(next);

            var worldState = agent.GetMemory().GetWorldState();
            foreach (var pair in effects.GetValues()) {
                worldState.Set(pair.Key, pair.Value);
            }
        }

    }
}
