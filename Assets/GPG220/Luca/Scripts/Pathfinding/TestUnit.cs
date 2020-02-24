using System;
using GPG220.Luca.Scripts.Unit;
using Sirenix.OdinInspector;
using Stephen;
using UnityEngine;

namespace GPG220.Luca.Scripts.Pathfinding
{
    public class TestUnit : UnitBase
    {
        public Rigidbody rb;
        public Collider col;
        public PathFinderController pfController;
        
        public bool calculateFlowFieldPath = true;
        public PathFinderPath currentPath;

        public bool move = false;
        public float stopMovingBelowDistToTarget = 2f;
        
        public GameObject testTarget;
        [Button("Recalculate Path"), DisableInEditorMode]
        public void RecalculatePathToTarget()
        {
            
            Debug.Log("Test Unit Recalculate Path...");
            move = false;
            Action<PathFinderPath> onDoneFunc = path =>
            {
                /*Debug.Log("Done calculating path. " + path.tilePath?.Count);
                if (calculateFlowFieldPath)
                {
                    Action<PathFinderPath> onFinallyDoneFunc = PathCalculationDone;
                    StartCoroutine(pfController.FindFlowFieldPath(path, onFinallyDoneFunc));
                }*/

                currentPath = path;
            };
            pfController.FindPathTo(transform.position, testTarget.transform.position, true, onDoneFunc);
            
            //StartCoroutine(pfController.FindPath(transform.position, testTarget.transform.position, onDoneFunc));
        }

        
        private void PathCalculationDone(PathFinderPath path)
        {
            move = true;
        }

        public float unitHeight = 1;
        
        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            col = GetComponent<Collider>();
            if (pfController == null)
                pfController = FindObjectOfType<PathFinderController>();

            unitHeight = GetComponent<Collider>().bounds.size.y;
        }

        public bool grounded = false;

        // Update is called once per frame
        void Update()
        {
            var r = new Ray(transform.position, Vector3.down);
            grounded = Physics.Raycast(transform.position, Vector3.down, ((unitHeight / 2) + 0.5f));//col.Raycast(r, out var info, ((unitHeight / 2) + 0.1f));
            
            if (move && currentPath != null)
            {
                if (stopMovingBelowDistToTarget >= 0 && currentPath.GetAproxDistanceToTargetAtPos(transform.position) <=
                    stopMovingBelowDistToTarget)
                    move = false;
                
                if (currentPath.flowFieldAvailable)
                {
                    MoveOnFlowField();
                }
                else
                {
                    MoveAlongPath();
                }
            }else if (stopMovingBelowDistToTarget >= 0 && currentPath != null &&
                      currentPath.GetAproxDistanceToTargetAtPos(transform.position) >
                      stopMovingBelowDistToTarget)
            {
                move = true;
            }
            
        }

        private void MoveAlongPath()
        {
            // TODO
        }

        private void MoveOnFlowField()
        {
            var moveDir = currentPath.GetDirectionAtPos(transform.position);
            moveDir.y *= Physics.gravity.y; // TODO HACK to increase up force...
            Move(moveDir);
        }

        public float acceleration = 10;
        private void Move(Vector3 dir)
        {
            if(grounded)
                rb.AddForce(rb.mass * acceleration* dir);
        }
    }
}
