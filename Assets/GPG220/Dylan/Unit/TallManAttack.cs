using System;
using System.Collections;
using System.Collections.Generic;
using GPG220.Blaide_Fedorowytsch.Scripts.PathFinding;
using GPG220.Dylan.Scripts.GOAP;
using GPG220.Dylan.Scripts.GOAP.Actions;
using GPG220.Dylan.Scripts.GOAP.Goals;
using GPG220.Dylan.Scripts.GOAPFirstTry;
using GPG220.Dylan.Scripts.GOAPFirstTry.Actions;
using GPG220.Luca.Scripts.Abilities;
using UnityEngine;
using UnityEngine.Experimental.VFX.Utility;
using Random = UnityEngine.Random;


namespace GPG220.Dylan.Unit
{
    public class TallManAttack : AbilityBase
    {
        public Vector3 targetPosition;
        public float energy;

        public float explosionRadius;
        public float explosionDamage;
        public float teleportCost;
        public float teleportDelay;

        [HideInInspector] public GoapAgentDylan goapAgentDylan;
        [HideInInspector] public GoalTargetReached goal;
        [HideInInspector] public Action_Teleport teleportAction;
        [HideInInspector] public Action_PathPossible pathPossibleAction;
        [HideInInspector] public Action_CheckEnergy checkEnergyAction;
        [HideInInspector] public Action_Move moveAction;
        [HideInInspector] public Action_TargetReached targetReachedAction;
        public LayerMask unitLayerMask;

        public Rigidbody rb;
        public LayerMask ground;
        public List<Node> currentPath = new List<Node>();
        public int currentPathNodeIndex;
        public float nodeDistanceMin = 1.5f;
        public float moveForce = 1000;
        public SimplePathfinder simplePathfinder;

        public void Awake()
        {
            abilityName = "Explosive Surprise";
            abilityDescription = "Surprise the enemy, by running at them waving a grenade in your hand," +
                                 " or if that isn't possible teleport to them and give them an explosive hello";
            targetRequired = true;

            rb = GetComponent<Rigidbody>();
            simplePathfinder = FindObjectOfType<SimplePathfinder>();

            goapAgentDylan = GetComponent<GoapAgentDylan>();
            goal = GetComponent<GoalTargetReached>();
            teleportAction = GetComponent<Action_Teleport>();
            moveAction = GetComponent<Action_Move>();
            pathPossibleAction = GetComponent<Action_PathPossible>();
            checkEnergyAction = GetComponent<Action_CheckEnergy>();

            checkEnergyAction.energyAmount = energy;
            teleportAction.teleportDelay = teleportDelay;

            targetReachedAction = GetComponent<Action_TargetReached>();
            SubToEvent();
        }

        public void SubToEvent()
        {
            targetReachedAction.targetReached += ExplodeAttack;
        }


        public override bool TargetExecute(Vector3 worldPos)
        {
            Debug.Log("Target");

            targetPosition = worldPos;
            simplePathfinder.RequestPathFind(transform.position, targetPosition, SetPath);
            pathPossibleAction.currentPath = currentPath;
            currentPathNodeIndex = 0;
            // goapAgentDylan.CalculateNewGoal(true);

            // pathPossibleAction.targetPosition = targetPosition;
            // targetReachedAction.targetPosition = targetPosition;
            // moveAction.targetPosition = targetPosition;
            // moveAction.currentPathNodeIndex = 0;

            moveAction.canMove = false;
            // goapAgentDylan.CalculateNewGoal(true);

            if (CheckIfPathIsPossible())
            {
                // moveAction.canMove = true;
                goapAgentDylan.CalculateNewGoal(true);
            }
            else
            {
                if (energy > 0)
                {
                    if (!teleportAction.isRunning)
                    {
                        Debug.Log("Target");
                        teleportDelay = Vector3.Distance(transform.position, targetPosition) / 10f;
                        teleportAction.isRunning = true;
                        StartCoroutine("TeleportDelay");
                    }

                    checkEnergyAction.energyAmount -= teleportCost;
                    checkEnergyAction.energyAmount = energy;
                }

                goapAgentDylan.CalculateNewGoal(true);
            }

            // goapAgentDylan.CalculateNewGoal(true);
            return true;
            return base.TargetExecute(worldPos);
        }

        private bool CheckIfPathIsPossible()
        {
            if(currentPath == null)
            {
                pathPossibleAction.isPathPossible = false;
            }
            else
            {
                pathPossibleAction.isPathPossible = true;
            }


            return pathPossibleAction.isPathPossible;
        }

        public void FixedUpdate()
        {
            if (moveAction.canMove)
            {
                if (Vector3.Distance(this.gameObject.transform.position, targetPosition) > nodeDistanceMin)
                {
                    Vector3 nextPos = currentPath[currentPathNodeIndex].worldPosition;
                    nextPos = new Vector3(nextPos.x, transform.position.y, nextPos.z);
                    if (Vector3.Distance(this.gameObject.transform.position,
                            nextPos) > nodeDistanceMin)
                    {
                        Move(nextPos);
                    }
                    else
                    {
                        if (currentPathNodeIndex < currentPath.Count - 1)
                            currentPathNodeIndex += 1;
                    }
                }
                else
                {
                    moveAction.canMove = false;
                }
            }
            else
            {
                CheckDistance();
            }
        }

        void Move(Vector3 v)
        {
            Ray ray = new Ray(transform.position, -transform.up);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 3f, ground, QueryTriggerInteraction.Ignore))
            {
                rb.AddForce(Vector3.ProjectOnPlane((v - transform.position), hit.normal) * moveForce);
            }
        }

        public void SetPath(List<Node> list)
        {
            currentPath = list;
        }

        public void ExplodeAttack()
        {
            Debug.Log("Explode!");
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, unitLayerMask);

            if (colliders.Length > 0)
            {
                foreach (Collider unit in colliders)
                {
                    if (unit.GetComponent<Health>())
                    {
                        Debug.Log("Explode!!");
                        Health unitHealth = unit.GetComponent<Health>();
                        unitHealth.health -= explosionDamage;
                    }
                }
            }

            // StartCoroutine("Death");
            Death();

            targetReachedAction.targetReached -= ExplodeAttack;
        }

        public void CheckDistance()
        {
            if (Vector3.Distance(transform.position, targetPosition) < 4f)
            {
                Debug.Log("check distance");
                targetReachedAction.TriggerEvent();
                // ExplodeAttack();
            }
        }

        public IEnumerator TeleportDelay()
        {
            yield return new WaitForSeconds(teleportDelay);
            transform.position = new Vector3(targetPosition.x, targetPosition.y + 2f, targetPosition.z);
            teleportAction.isRunning = false;
            CheckDistance();
        }

        public void Death()
        {
            Health health = this.gameObject.GetComponent<Health>();
            health.ChangeHealth(-999);

            // Destroy(gameObject);
        }

        // public IEnumerator Death()
        // {
        //     yield return new WaitForSeconds(2f);
        //     Health health = this.gameObject.GetComponent<Health>();
        //     health.ChangeHealth(-999);
        //
        //     // Destroy(gameObject);
        // }
    }
}