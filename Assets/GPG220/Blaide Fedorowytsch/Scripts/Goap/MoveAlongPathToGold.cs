using System;
using System.Collections.Generic;
using GPG220.Blaide_Fedorowytsch.Scripts.PathFinding;
using ReGoap.Core;
using ReGoap.Unity;
using UnityEngine;

namespace GPG220.Blaide_Fedorowytsch.Scripts.Goap
{
    public class MoveAlongPathToGold : ReGoapAction<string,object>
    {
        private PathFindToGold _pathFindToGold;
        private List<Node> currentPath;
        private Vector3 target;
        public float nodeDistanceMin;
        private int currentPathNodeIndex;
        private Rigidbody rB;
        public float moveForce;
        private bool moving;
        public float timeout = 8;
        public float timer;
        
        protected override void Awake()
        {
            base.Awake();
            preconditions.Set("HasPathToGold", true);
            effects.Set("HasGold", true);
            _pathFindToGold = GetComponent<PathFindToGold>();

            rB = GetComponent<Rigidbody>();
        }

        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next,
            ReGoapState<string, object> settings, ReGoapState<string, object> goalState,
            Action<IReGoapAction<string, object>> done, Action<IReGoapAction<string, object>> fail)
        {
            base.Run(previous, next, settings, goalState, done, fail);
            moving = true;
            currentPath = _pathFindToGold.Path;
            currentPathNodeIndex = 0;
            target = _pathFindToGold.target;
            resetTimer();
        }



        public void resetTimer()
        {
            timer = timeout;
        }

        private void Update()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                if (Vector3.Distance(this.gameObject.transform.position,
                    new Vector3(target.x, transform.position.y, target.z)) > nodeDistanceMin)
                {
                    if (currentPath != null)
                    {


                        Vector3 nextPos = currentPath[currentPathNodeIndex].worldPosition;
                        nextPos = new Vector3(nextPos.x, transform.position.y, nextPos.z);



                        if (Vector3.Distance(this.gameObject.transform.position,
                            nextPos) > nodeDistanceMin)
                        {
                            Move(nextPos);
                            rB.AddForce(Vector3.down * (rB.mass * 9));
                        }
                        else if (currentPathNodeIndex < currentPath.Count - 1)
                        {
                            currentPathNodeIndex += 1;
                            resetTimer();
                        }
                        else
                        {
                            Move(new Vector3(target.x, transform.position.y, target.z));
                        }

                        transform.LookAt(new Vector3(nextPos.x, transform.position.y, nextPos.z));
                    }
                    else
                    {

                    }
                }
                else
                {
                    failCallback(this);
                    resetTimer();
                }
            }
            else
            {
                failCallback(this);
                resetTimer();
            }



        }

        void Move(Vector3 v)
        {
            //this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, v, 0.5f);
            rB.AddForce((v -transform.position).normalized * moveForce);
        }
        
        private void OnDrawGizmosSelected()
        {
            if (currentPath != null)
            {
                if (currentPath.Count != 0)
                {
                    Gizmos.DrawSphere(currentPath[currentPathNodeIndex].worldPosition,1);   
                }
            }
        }
        
    }
}
