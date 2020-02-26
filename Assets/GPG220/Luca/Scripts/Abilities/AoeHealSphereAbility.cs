using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Unit;
using UnityEngine;
using UnityEngine.Rendering;

namespace GPG220.Luca.Scripts.Abilities
{
    public class AoeHealSphereAbility : AbilityBase
    {

        public float radius = 10;
        public float expansionSpeed = 1; // Units per second
        public float healAmount = 10;

        public Material sphereMaterial;
        public GameObject targetParticleEffect;
        
        private bool isExecuting;
        private readonly List<UnitBase> healedUnits = new List<UnitBase>();
        
        // Start is called before the first frame update
        private void Start()
        {
            abilityName = "Heal Sphere";
        }

        public override bool CheckRequirements()
        {
            
            return !isExecuting && base.CheckRequirements();
        }

        public override bool Execute(GameObject executorGameObject, GameObject[] targets = null)
        {
            if (!CheckRequirements())
                return false;

            NotifyAbilityStartExecution(executorGameObject);
            
            isExecuting = true;
            StartCoroutine(SpawnAndExpandHealSphere(executorGameObject));
            
            NotifyAbilityExecuted(executorGameObject);
            return true;
        }

        private IEnumerator SpawnAndExpandHealSphere(GameObject executorGameObject)
        {
            var healSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            healSphere.transform.SetParent(executorGameObject.transform, false);
            healSphere.GetComponent<SphereCollider>().isTrigger = true;
            MeshRenderer mr = healSphere.GetComponent<MeshRenderer>();
            mr.material = sphereMaterial;
            mr.shadowCastingMode = ShadowCastingMode.Off;
            healSphere.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
            var colNot = healSphere.AddComponent<CollisionNotifier>();
            colNot.TriggerEnterEvent += OnHealSphereTriggerEntered;
            yield return new WaitForEndOfFrame();
            while (healSphere.transform.localScale.x < radius)
            {
                var increment = expansionSpeed * Time.deltaTime;
                healSphere.transform.localScale += new Vector3(increment,increment,increment);
                
                yield return new WaitForEndOfFrame();
            }
            

            Destroy( colNot.gameObject);
            healedUnits.Clear();
            isExecuting = false;
            yield return 0;
        }

        private void OnHealSphereTriggerEntered(Collider obj)
        {
            var unit = obj.GetComponent<UnitBase>();
            if (unit == null || healedUnits.Contains(unit)) return;
            unit.health.ChangeHealth((int)healAmount);
            healedUnits.Add(unit);
            StartCoroutine(SpawnHealEffect(unit.transform));
        }

        private IEnumerator SpawnHealEffect(Transform target)
        {
            if (targetParticleEffect != null)
            {
                var healEffect = Instantiate(targetParticleEffect, target);
                var ps = healEffect.GetComponent<ParticleSystem>();
                yield return new WaitForSeconds(ps?.main.duration ?? 1);
                Destroy(healEffect);
            }

            yield return 0;
        }
    }
}
