using System;
using System.Collections.Generic;
using GPG220.Blaide_Fedorowytsch.Scripts.PathFinding;
using ReGoap.Core;
using ReGoap.Unity;
using UnityEngine;

namespace GPG220.Blaide_Fedorowytsch.Scripts.Goap
{
    public class PathFindToGold : ReGoapAction<string,object>
    {
        private FindNearestGold _findNearestGold;
        public List<Node> Path;
        private bool findingPath = false;
        public Vector3 target;
        private SimplePathfinder pf;
        private MoveAlongPathToGold _moveAlongPathToGold;

        protected override void Awake()
        {
            pf = FindObjectOfType<SimplePathfinder>();
            base.Awake();
            preconditions.Set("GoldFound", true);
            //effects.Set("HasPathToGold", true);
            Cost = 1;
            _findNearestGold = GetComponent<FindNearestGold>();
            _moveAlongPathToGold = GetComponent<MoveAlongPathToGold>();

        }
        public void ElevateCost()
        {
            Cost += 0.2f;
        }

        public void ResetCost()
        {
            Cost = 1f;
        }

        private void SetPath(List<Node> list)
        {
            if (list != null)
            {
                Path = list;
                doneCallback(this);
            }
            else
            { 
                ElevateCost();
                failCallback(this);
            }
        }

        public override ReGoapState<string, object> GetEffects(GoapActionStackData<string, object> stackData)
        {
            effects.Set("HasPathToGold", true);

            return base.GetEffects(stackData);
        }

        /*
        public override bool CheckProceduralCondition(GoapActionStackData<string, object> stackData)
        {

            if (Path == null && _findNearestGold.closestGold != null)
            {
                
                pf.RequestPathFind(transform.position,_findNearestGold.closestGold.transform.position, SetPath);
            }

            return base.CheckProceduralCondition(stackData) && Path != null;
        }
        */

        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next,
            ReGoapState<string, object> settings, ReGoapState<string, object> goalState,
            Action<IReGoapAction<string, object>> done, Action<IReGoapAction<string, object>> fail)
        {
            base.Run(previous, next, settings, goalState, done, fail);
            // do your own game logic here
            target = _findNearestGold.closestGold.transform.position;
            
            pf.RequestPathFind(transform.position,target, SetPath);
        }

    }
}
