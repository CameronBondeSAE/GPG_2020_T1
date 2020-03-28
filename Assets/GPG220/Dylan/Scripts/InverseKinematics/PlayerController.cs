using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;
 

namespace InverseKinematics
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables

        [Header("Movement")] 
        public float InputX;
        public float InputZ;
        public Vector3 desiredMoveDirection;
        public bool blockRotationPlayer;
        public float desiredRotationSpeed;
        public bool isGrounded;
        private float verticalVel;
        private Vector3 moveVector;
        public float moveSpeed;
        public float allowPlayerRotation;

        [Header("Declarations")]
        public Animator animator;
        public Camera cam;
        public CharacterController controller;

        private Vector3 rightFootPosition, leftFootPosition, leftFootIKPosition, rightFootIKPosition;
        private Quaternion leftFootIkRotation, rightFootIkRotation;
        private float lastPelvisPositionY, lastRightFootPositionY, lastLeftFootPositionY;

        [Header("Feet Grounder")] 
        public bool enabledFeetIk = true;
        [Range(0,2),SerializeField] private float heightFromGroundRaycast = 1.14f;
        [Range(0, 2), SerializeField] private float raycastDownDistance = 1.5f;
        [SerializeField] private LayerMask environmentLayer;
        [SerializeField] private float pelvisOffset = 0f;
        [Range(0, 1), SerializeField] private float pelvisUpAndDownSpeed = 0.28f;
        [Range(0, 1), SerializeField] private float feetToIkPostionSpeed = 0.5f;

        public string leftFootAnimVariableName = "LeftFootCurve";
        public string rightFootAnimVariable = "RightFootCurve";

        public bool useProIkFeature = false;
        public bool showSolverDebug = true;
        #endregion Variables

        #region Intialization

        

        
        private void Awake()
        {
            animator = this.GetComponent<Animator>();
            cam = Camera.main;
            controller = this.GetComponent<CharacterController>();
            
        }

        private void LateUpdate()
        {
            InputMagnitude();

            isGrounded = controller.isGrounded;
            if (isGrounded)
            {
                verticalVel -= 0;
            }
            else
            {
                verticalVel -= 0;
            }
            //moveVector = new Vector3(0,verticalVel,0);
            
            moveVector = new Vector3(0, verticalVel, 0).normalized;
            
            controller.Move(moveVector);
        }
        #endregion

        #region PlayerMovement

        

        
        void PlayerMoveAndRotation()
        {
            InputX = Input.GetAxisRaw("Horizontal");
            InputZ = Input.GetAxisRaw("Vertical");

            var camera = Camera.main;
            var forward = cam.transform.forward;
            var right = cam.transform.right;

            forward.y = 0f;
            right.y = 0f;
            
            forward.Normalize();
            right.Normalize();

            desiredMoveDirection = forward * InputZ + right * InputX;

            if (blockRotationPlayer == false)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);
                //controller.Move(desiredMoveDirection * Time.deltaTime * 2f);
            }
        }

        void InputMagnitude()
        {
            //Calculate input vectors
            InputX = Input.GetAxisRaw("Horizontal");
            InputZ = Input.GetAxisRaw("Vertical");
            
            //change 0.0 values if you want to dampen animation time,
            animator.SetFloat("InputZ", InputZ,0.0f,Time.deltaTime * 2f);
            animator.SetFloat("InputX", InputX,0.0f,Time.deltaTime * 2f);
            
            //Calcute the Input Magnitude
            moveSpeed = new Vector2(InputX, InputZ).normalized.sqrMagnitude;

            if (moveSpeed > allowPlayerRotation)
            {
                animator.SetFloat("InputMagnitude", moveSpeed, 0.0f, Time.deltaTime);
                PlayerMoveAndRotation();
            }
            else if (moveSpeed <= allowPlayerRotation)
            {
                animator.SetFloat("InputMagnitude", moveSpeed, 0.0f, Time.deltaTime);
            }
        }
        #endregion

        #region FeetGrounding

        /// <summary>
        /// We are updating the adjust feet target method and also finding the position of each foot inside our solver position.
        /// </summary>
        private void FixedUpdate()
        {
            if (enabledFeetIk == false)
            {
                return;
            }
            if (animator == null)
            {
                return;
            }
            
            AdjustFeetTarget(ref rightFootPosition, HumanBodyBones.RightFoot);
            AdjustFeetTarget(ref leftFootPosition, HumanBodyBones.LeftFoot);
            
            //find and reycast to the ground to find positions
            FeetPositionSolver(rightFootPosition, ref rightFootIKPosition, ref rightFootIkRotation);
            FeetPositionSolver(leftFootPosition,ref leftFootIKPosition,ref leftFootIkRotation);
        }
        

        private void OnAnimatorIK(int layerIndex)
        {
            if (enabledFeetIk == false)
            {
                return;
            }
            if (animator == null)
            {
                return;
            }
            
            MovePelvisHeight();
            
            //right foot ik position rotation
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot,1);

            if (useProIkFeature)
            {
                animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, animator.GetFloat(rightFootAnimVariable));
            }
            
            MoveFeetToIkPoint(AvatarIKGoal.RightFoot, rightFootIKPosition,rightFootIkRotation,ref lastRightFootPositionY);
            
            //left foot ik position rotation
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot,1);

            if (useProIkFeature)
            {
                animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, animator.GetFloat(leftFootAnimVariableName));
            }
            
            MoveFeetToIkPoint(AvatarIKGoal.LeftFoot, leftFootIKPosition,leftFootIkRotation,ref lastLeftFootPositionY);


        }

        #endregion

        #region FeetGroundingMethods

        
        /// <summary>
        /// Moves feet to Ik points
        /// </summary>
        /// <param name="foot"></param>
        /// <param name="positionIkHolder"></param>
        /// <param name="rotationIkHolder"></param>
        /// <param name="lastFootPositionY"></param>
        void MoveFeetToIkPoint(AvatarIKGoal foot, Vector3 positionIkHolder, Quaternion rotationIkHolder,
            ref float lastFootPositionY)
        {
            Vector3 targetIkPosition = animator.GetIKPosition(foot);

            if (positionIkHolder != Vector3.zero)
            {
                targetIkPosition = transform.InverseTransformPoint(targetIkPosition);
                positionIkHolder = transform.InverseTransformPoint(positionIkHolder);

                float yVariable = Mathf.Lerp(lastFootPositionY, positionIkHolder.y, feetToIkPostionSpeed);
                targetIkPosition.y += yVariable;

                lastFootPositionY = yVariable;

                targetIkPosition = transform.TransformPoint(targetIkPosition);
                animator.SetIKRotation(foot, rotationIkHolder);
                
            }
            
            animator.SetIKPosition(foot,targetIkPosition);
        }

        /// <summary>
        /// adjusts pelvis height
        /// </summary>
        private void MovePelvisHeight()
        {
            if (rightFootIKPosition == Vector3.zero || leftFootIKPosition == Vector3.zero || lastPelvisPositionY == 0)
            {
                lastPelvisPositionY = animator.bodyPosition.y;
                return;
            }

            float lOffsetPosition = leftFootIKPosition.y - transform.position.y;    
            float rOffsetPosition = rightFootIKPosition.y - transform.position.y;

            float totalOffset = (lOffsetPosition < rOffsetPosition) ? lOffsetPosition : rOffsetPosition;

            Vector3 newPelvisPosition = animator.bodyPosition + Vector3.up * totalOffset;

            newPelvisPosition.y = Mathf.Lerp(lastPelvisPositionY, newPelvisPosition.y, pelvisUpAndDownSpeed);

            animator.bodyPosition = newPelvisPosition;

            lastPelvisPositionY = animator.bodyPosition.y;
        }

        /// <summary>
        /// We are locationing the feet position via a raycast and then solving 
        /// </summary>
        /// <param name="fromSkyPosition"></param>
        /// <param name="feetIkPostions"></param>
        /// <param name="feetIkRotations"></param>
        private void FeetPositionSolver(Vector3 fromSkyPosition, ref Vector3 feetIkPostions,
            ref Quaternion feetIkRotations)
        {
            //raycast handling section
            RaycastHit feetOutHit;

            if (showSolverDebug)
            {
                Debug.DrawLine(fromSkyPosition, fromSkyPosition +Vector3.down * (raycastDownDistance + heightFromGroundRaycast), Color.yellow);
            }


            if (Physics.Raycast(fromSkyPosition, Vector3.down, out feetOutHit,
                raycastDownDistance + heightFromGroundRaycast, environmentLayer))
            {
                //finding feet ik positions from sky position
                feetIkPostions = fromSkyPosition;
                feetIkPostions.y = feetOutHit.point.y + pelvisOffset;
                feetIkRotations = Quaternion.FromToRotation(Vector3.up, feetOutHit.normal) * transform.rotation;

                return;
            }
            
            feetIkPostions = Vector3.zero;
        }

        private void AdjustFeetTarget(ref Vector3 feetPositions, HumanBodyBones foot)
        {
            feetPositions = animator.GetBoneTransform(foot).position;
            feetPositions.y = transform.position.y + heightFromGroundRaycast;

        }

        #endregion
        
    }
}
