using System;
using ReGoap.Core;
using ReGoap.Unity;
using UnityEngine;
using System.Collections.Generic;
using GPG220.Blaide_Fedorowytsch.Scripts.PathFinding;


namespace GPG220.Dylan.Scripts.GOAPFirstTry.Actions
{
    // ReSharper disable once InconsistentNaming
    public class Action_Move : ReGoapAction<string, object>
    {
        [HideInInspector] public bool canMove;
        public Vector3 targetPosition;

        public Rigidbody rb;
        public LayerMask ground;
        public List<Node> currentPath = new List<Node>();
        public int currentPathNodeIndex;
        public float nodeDistanceMin = 1.5f;
        public float moveForce = 1000;
        
        [HideInInspector] public Action_TargetReached targetReachedAction;

        protected override void Awake()
        {
            // rb = GetComponent<Rigidbody>();
            canMove = false;
            // targetReachedAction.GetComponent<Action_TargetReached>();
            // targetReachedAction.targetReached += AreWeThereYet;
            base.Awake();
        }

        private void AreWeThereYet()
        {
            canMove = false;
        }

        public override ReGoapState<string, object> GetPreconditions(GoapActionStackData<string, object> stackData)
        {
            preconditions.Set("pathPossible", true);

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

            canMove = true;
            
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
        
        
        // public void FixedUpdate()
        // {
        //     if (canMove)
        //     {
        //         
        //         if (Vector3.Distance(this.gameObject.transform.position, targetPosition) > nodeDistanceMin)
        //         {
        //             Vector3 nextPos = currentPath[currentPathNodeIndex].worldPosition;
        //             //  nextPos = new Vector3(nextPos.x, procMesh.GetHeightAtPosition(new Vector2(nextPos.x, nextPos.z)) + 1, nextPos.z);
        //             nextPos = new Vector3(nextPos.x, transform.position.y, nextPos.z);
        //             if (Vector3.Distance(this.gameObject.transform.position,
        //                     nextPos) > nodeDistanceMin)
        //             {
        //                 Move(nextPos);
        //             }
        //             else
        //             {
        //                 if (currentPathNodeIndex < currentPath.Count - 1)
        //                     currentPathNodeIndex += 1;
        //             }
        //         }
        //         else
        //         {
        //             canMove = false;
        //         }
        //     }
        //     
        // }
        //
        // void Move(Vector3 v)
        // {
        //     //this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, v, 0.5f);
        //     Ray ray = new Ray(transform.position, -transform.up);
        //     RaycastHit hit;
        //     if (Physics.Raycast(ray, out hit, 3f, ground, QueryTriggerInteraction.Ignore))
        //     {
        //         rb.AddForce(Vector3.ProjectOnPlane((v - transform.position), hit.normal) * moveForce);
        //     }
        // }

        
    }
}