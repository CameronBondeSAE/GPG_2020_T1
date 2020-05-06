using System;
using System.Collections;
using GPG220.Blaide_Fedorowytsch.Scripts.ProcGen;
using GPG220.Luca.Scripts.Abilities;
using GPG220.Luca.Scripts.Unit;
using UnityEngine;

namespace GPG210.Scripts.RankUpUnit
{
    public class Shotgun : AbilityBase
    {
        public GameObject pelletPrefab;
        public GameObject Prong;
        public float bulletGrowthMultiplier;
        public float bulletSpawnDelay = 1f;
        private bool canSpawnBullet;

        public void Awake()
        {
            // targetRequired = true;
            canSpawnBullet = true;
        }

        public override bool SelectedExecute()
        {
            if (canSpawnBullet)
            {
                StartCoroutine(SpawnDelay());
                GameObject pellet = Instantiate(pelletPrefab);
                Physics.IgnoreCollision(pellet.GetComponent<Collider>(),
                    FindObjectOfType<ProceduralMeshGenerator>().collider);
                pellet.transform.position = Prong.transform.position + Prong.transform.forward;
                pellet.transform.forward = Prong.transform.forward;
                pellet.GetComponent<Pellet>().unitBase = GetComponent<UnitBase>();
                pellet.GetComponent<Pellet>().growMultiplier = bulletGrowthMultiplier;
            }
            
            return true;
        }

        private IEnumerator SpawnDelay()
        {
            canSpawnBullet = false;
            yield return new WaitForSeconds(bulletSpawnDelay);
            canSpawnBullet = true;
        }
        
        // public override bool TargetExecute(Vector3 worldPos)
        // {
        //     //split world pos so that only x and z coordinates are changed and just use the object transform
        //     gameObject.transform.LookAt(worldPos);
        //     GameObject pellet = Instantiate(pelletPrefab);
        //     pellet.transform.position = Prong.transform.position + Prong.transform.forward;
        //     pellet.transform.forward = Prong.transform.forward;
        //     pellet.GetComponent<Pellet>().unitBase = GetComponent<UnitBase>();
        //     return base.TargetExecute(worldPos);
        // }
    }
}