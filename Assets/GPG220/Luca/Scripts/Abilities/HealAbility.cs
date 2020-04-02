using System;
using System.Collections;
using System.Linq;
using GPG220.Luca.Scripts.Unit;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GPG220.Luca.Scripts.Abilities
{
    /// <summary>
    /// Test Ability - Heals a player.
    /// </summary>
    public class HealAbility : AbilityBase
    {
        public LayerMask layerMask;
        public float range = 10;
        public float sphereCastRadius = 2;
        public int healthAmount = 0;
        public GameObject targetParticleEffect;
    
        [FoldoutGroup("Debug")]
        public GameObject testExecutor;

        [FoldoutGroup("Debug"),Button("Execute Ability")]
        public void TestExecuteAbility()
        {
            if (testExecutor == null) return;
            bool exec = Execute(testExecutor);
            Debug.Log("Executed ability? "+exec);
        }

        private void Awake()
        {
            abilityName = "Heal Shot";
            abilityDescription = "Heals 1 unit with the lowest health within a given range infront of the executor.";
        }

        public override bool Execute(GameObject executorGameObject, GameObject targets = null)
        {
            if (!CheckRequirements())
                return false;

            //Physics.SphereCast(executorGameObject.transform.position, 2f, executorGameObject.transform.forward, out var hit, range, layerMask);

            var hits = Physics.SphereCastAll(executorGameObject.transform.position, sphereCastRadius,
                executorGameObject.transform.forward, range, layerMask);

            if (hits != null && hits.Length > 0)
            {
                Health lowestHealthUnitHealth = null;
                float lowestHealth = 0;
                foreach (var hit in hits)
                {
                    var unit = hit.collider?.GetComponent<UnitBase>();
            
                    // TODO Check if its an allied unit!
                    if (unit == null || unit.health == null || unit.health.CurrentHealth <= 0) continue;
                    if (lowestHealthUnitHealth != null && unit.health.CurrentHealth > lowestHealth) continue;
                    lowestHealthUnitHealth = unit.health;
                    lowestHealth = unit.health.CurrentHealth;
                }

                if (lowestHealthUnitHealth == null) return false;
                NotifyAbilityStartExecution(executorGameObject);
                StartCoroutine(SpawnHealEffect(lowestHealthUnitHealth.transform));
                lowestHealthUnitHealth.ChangeHealth(healthAmount);
        
                NotifyAbilityExecuted(executorGameObject);
                return true;
            }

            return false;
        }

        IEnumerator SpawnHealEffect(Transform target)
        {
            if (targetParticleEffect != null)
            {
                GameObject healEffect = Instantiate(targetParticleEffect, target);
                ParticleSystem ps = healEffect.GetComponent<ParticleSystem>();
                yield return new WaitForSeconds(ps?.main.duration ?? 1);
                Destroy(healEffect);
            }

            yield return 0;
        }
    }
}