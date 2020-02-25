using GPG220.Luca.Scripts.Pathfinding;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GPG220.Luca.Scripts.Unit
{
    public class MovableUnit : UnitBase
    {
        #region Component Vars

        public PathFinderController pathFinderController = null;
        public CharacterController characterController;

        #endregion
        

        #region Movement Vars
        [ShowInInspector]
        public PathFinderPath currentPath = null;
        public float repathCooldown = 3; // It can calculate a new path only every x seconds.
        [ShowInInspector]
        private float repathCountdown = 0;
        
        public bool move = false;
        public float moveDirPollCooldown = .5f; // It can calculate a new path only every x seconds.
        [ShowInInspector]
        private float moveDirPollCountdown = 0;
        Vector3 movementDirection = Vector3.zero;

        public float gravity = 9.81f;
        public bool grounded = false;
        private float distanceToGround;

        #endregion
        
        #region Debug
        [FoldoutGroup("X"),Button("Move To Location"), DisableInEditorMode]
        public void DebugMoveTo(Transform locationTransform = default)
        {
            MoveToLocation(locationTransform.position);
        }

        #endregion
        
        // TODO Unit Movement System
        private void Start()
        {
            Initialize();
        }

        protected override void Initialize()
        {
            base.Initialize();
            if (pathFinderController == null)
                pathFinderController = FindObjectOfType<PathFinderController>();
            if (characterController == null)
                characterController =
                    GetComponent<CharacterController>() ?? gameObject.AddComponent<CharacterController>();
            rb.isKinematic = true;
            
            distanceToGround = GetComponent<Collider>().bounds.size.y / 2 + .1f;
        }

        private void Update()
        {
            grounded = Physics.Raycast(transform.position, Vector3.down, distanceToGround);
            
            if (repathCountdown > 0)
                repathCountdown -= Time.deltaTime;
            if (moveDirPollCountdown > 0)
                moveDirPollCountdown -= Time.deltaTime;
            
            Move();
        }

        public override void OnExecuteAction(Vector3 worldPosition, GameObject g)
        {
            MoveToLocation(worldPosition); // TODO
        }
        
        public void MoveToLocation(Vector3 location)
        {
            if (repathCountdown > 0 || pathFinderController == null)
                return;

            pathFinderController.FindPathTo(transform.position, location, true, (path) => { 
                currentPath = path;
                move = true;
            });
            
            repathCountdown = repathCooldown;
        }
        
        private void Move()
        {

            if (currentPath != null && move)
            {
                if (moveDirPollCountdown <= 0)
                {
                    movementDirection = currentPath.GetDirectionAtPos(transform.position);
                    moveDirPollCountdown = moveDirPollCooldown;
                }
                
            }
            else
            {
                movementDirection = Vector3.zero;
            }

            movementDirection.y -= gravity;

            characterController.SimpleMove(movementDirection);
        }
    }
}