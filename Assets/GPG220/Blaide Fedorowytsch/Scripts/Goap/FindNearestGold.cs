using System;
using System.Collections.Generic;
using System.Linq;
using GPG220.Luca.Scripts.Unit;
using ReGoap.Core;
using ReGoap.Unity;
using UnityEngine;

namespace GPG220.Blaide_Fedorowytsch.Scripts.Goap
{
    public class FindNearestGold : ReGoapAction<string,object>
    {
        public ResourceUnit closestGold;
        protected override void Awake()
        {
            base.Awake();
            effects.Set("GoldFound", true);
        }
        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next, ReGoapState<string, object> settings, ReGoapState<string, object> goalState, Action<IReGoapAction<string, object>> done, Action<IReGoapAction<string, object>> fail)
        {
            base.Run(previous, next,settings,goalState, done, fail);
            // do your own game logic here

            List<ResourceUnit> resourceUnits = FindObjectsOfType<ResourceUnit>().ToList();
            if (resourceUnits.Count > 1)
            {
                closestGold = resourceUnits[0];
                float closesetGoldDistance = Vector3.Distance(transform.position, closestGold.transform.position);
                foreach (ResourceUnit resourceUnit in resourceUnits)
                {
                    if (Vector3.Distance(transform.position, resourceUnit.transform.position) < closesetGoldDistance)
                    {
                        closestGold = resourceUnit;
                        closesetGoldDistance = Vector3.Distance(transform.position, closestGold.transform.position);
                    }
                }
                doneCallback(this);
            }
            else
            {
                failCallback(this);
            }

            // when done, in this function or outside this function, call the done or fail callback, automatically saved to doneCallback and failCallback by ReGoapAction
             // this will tell the ReGoapAgent that the action is succerfully done and go ahead in the action plan
            // if the action has failed then run failCallback(this), the ReGoapAgent will automatically invalidate the whole plan and ask the ReGoapPlannerManager to create a new plan
        }
    }
}
