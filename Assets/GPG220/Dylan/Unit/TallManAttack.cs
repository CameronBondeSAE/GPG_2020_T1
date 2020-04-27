using System;
using GPG220.Dylan.Scripts.GOAP.Goals;
using GPG220.Dylan.Scripts.GOAPFirstTry;
using GPG220.Luca.Scripts.Abilities;
using UnityEngine;

namespace GPG220.Dylan.Unit
{
    public class TallManAttack : AbilityBase
    {
        public Vector3 targetPosition;

        public float explosionRadius;
        public float explosionDamage;

        public GoapAgentDylan goapAgentDylan;
        public GoalTargetReached goal;

        public void Awake()
        {
            goapAgentDylan = GetComponent<GoapAgentDylan>();
            goal = GetComponent<GoalTargetReached>();
        }

        public override bool SelectedExecute()
        {
            //used to call set goal thing
            // goapAgentDylan.CalculateNewGoal(true);


            //use world     pos as target position for ability

            ExplodeAttack(transform.position);
            
            return base.SelectedExecute();
        }

        public void ExplodeAttack(Vector3 position)
        {
            Collider[] colliders = Physics.OverlapSphere(position, explosionRadius);
            foreach (Collider nearbyUnit in colliders)
            {
                if (nearbyUnit.GetComponent<Health>())
                {
                    float unitHealth = nearbyUnit.GetComponent<Health>().health;
                    unitHealth -= explosionDamage;
                    
                }
            }

            GetComponent<Health>().health = 0;
        }
    }
}