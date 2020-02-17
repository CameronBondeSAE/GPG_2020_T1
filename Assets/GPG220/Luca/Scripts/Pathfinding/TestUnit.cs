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
            if (move)
            {
                if (currentPath.flowFieldAvailable)
                {
                    MoveOnFlowField();
                }
                else
                {
                    MoveAlongPath();
                }
            }  
        }

        private void MoveAlongPath()
        {
            // TODO
        }

        private void MoveOnFlowField()
        {
            var moveDir = currentPath.GetFlowDirectionAtPos(transform.position);
            Move(moveDir);
        }

        public float acceleration = 10;
        private void Move(Vector3 dir)
        {
            rb.AddForce(dir * acceleration);
        }
    }
}
