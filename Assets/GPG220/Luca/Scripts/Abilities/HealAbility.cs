using System.Collections;
using GPG220.Luca.Scripts.Unit;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GPG220.Luca.Scripts.Abilities
{
    public class HealAbility : Ability
    {
        public LayerMask layerMask;
        public float range = 10;
        public int healthAmount = 0;
        public GameObject targetParticleEffect;

        public GameObject testExecutor;

        [Button("Execute Ability")]
        public void TestExecuteAbility()
        {
            if (testExecutor == null) return;
            bool exec = Execute(testExecutor);
            Debug.Log("Executed ability? "+exec);
        }

        protected override bool Execute(GameObject executorGameObject)
        {
            if (!CheckRequirements())
                return false;

            Physics.SphereCast(executorGameObject.transform.position, 2f, executorGameObject.transform.forward, out var hit, range, layerMask);

            var unit = hit.collider?.GetComponent<UnitBase>();
            Health targetHealth = null;
            
            // TODO Check if its an allied unit!
            if (unit != null)
            {
                targetHealth = unit.GetComponent<Health>();

                StartCoroutine(SpawnHealEffect(unit.transform));
            }
            
            
            targetHealth?.ChangeHealth(healthAmount);
            
            NotifyAbilityExecuted(executorGameObject);
            return true;
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