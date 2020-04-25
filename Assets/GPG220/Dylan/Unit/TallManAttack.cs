using System;
using GPG220.Dylan.Scripts.GOAP;
using GPG220.Dylan.Scripts.GOAP.Actions;
using GPG220.Dylan.Scripts.GOAP.Goals;
using GPG220.Dylan.Scripts.GOAPFirstTry;
using GPG220.Dylan.Scripts.GOAPFirstTry.Actions;
using GPG220.Luca.Scripts.Abilities;
using UnityEngine;

namespace GPG220.Dylan.Unit
{
    public class TallManAttack : AbilityBase
    {
        public Vector3 targetPosition;
        public float energy;

        public float explosionRadius;
        public float explosionDamage;
        public float teleportCost;

        public GoapAgentDylan goapAgentDylan;
        public GoalTargetReached goal;
        public Action_Teleport teleportAction;
        public Action_PathPossible pathPossibleAction;
        public Action_CheckEnergy checkEnergyAction;
        public Action_Move moveAction;
        public Action_TargetReached targetReachedAction;

        public void Awake()
        {
            goapAgentDylan = GetComponent<GoapAgentDylan>();
            goal = GetComponent<GoalTargetReached>();
            teleportAction = GetComponent<Action_Teleport>();
            moveAction = GetComponent<Action_Move>();
            pathPossibleAction = GetComponent<Action_PathPossible>();
            checkEnergyAction = GetComponent<Action_CheckEnergy>();
            checkEnergyAction.energyAmount = energy;
            
            
            targetReachedAction = GetComponent<Action_TargetReached>();
            targetReachedAction.targetReached += ExplodeAttack;
        }

        public override bool SelectedExecute()
        {
            return base.SelectedExecute();
        }

        public override bool TargetExecute(Vector3 worldPos)
        {
            //these need to come before the calculate goal otherwise the actions wont have a target position
            pathPossibleAction.targetPosition.position = worldPos;
            moveAction.targetPosition.position = worldPos;
            teleportAction.targetPosition.position = worldPos;
            
            //used to call set goal thing
            
            

            goapAgentDylan.CalculateNewGoal(true);

            return base.TargetExecute(worldPos);
        }

        public void ExplodeAttack()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (Collider nearbyUnit in colliders)
            {
                if (nearbyUnit.GetComponent<Health>())
                {
                    Health unitHealth = nearbyUnit.GetComponent<Health>();
                    unitHealth.health -= explosionDamage;
                }
            }

            
            Destroy(gameObject);
            targetReachedAction.targetReached -= ExplodeAttack;
        }
    }
}