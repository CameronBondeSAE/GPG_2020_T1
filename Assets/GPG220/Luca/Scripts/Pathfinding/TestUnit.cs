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
                Debug.Log("Done calculating path. " + path.tilePath?.Count);
                if (calculateFlowFieldPath)
                {
                    Action<PathFinderPath> onFinallyDoneFunc = PathCalculationDone;
                    StartCoroutine(pfController.FindFlowFieldPath(path, onFinallyDoneFunc));
                }

                currentPath = path;
            };
            pfController.FindPathTo(transform.position, testTarget.transform.position, onDoneFunc);
            //StartCoroutine(pfController.FindPath(transform.position, testTarget.transform.position, onDoneFunc));
        }

        
        private void PathCalculationDone(PathFinderPath path)
        {
            move = true;
        }
        
        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();

            if (pfController == null)
                pfController = FindObjectOfType<PathFinderController>();
        }

        // Update is called once per frame
        void Update()
        {
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
            Move(moveDir);
        }

        public float acceleration = 10;
        private void Move(Vector3 dir)
        {
            rb.AddForce(rb.mass * acceleration* dir);
        }
    }
}
