using System;
using GPG220.Luca.Scripts.Abilities;
using GPG220.Luca.Scripts.Unit;
using Mirror;
using UnityEngine;

namespace GPG210.Scripts.RankUpUnit
{
    public class Pellet : AbilityBase
    {
        private float speed;
        private float lifeDuration;

        private float lifeTimer;
        public int amount;
        [HideInInspector] public UnitBase unitBase;
        public float growMultiplier;

        public void Awake()
        {
            // var pelletsSpawned = FindObjectsOfType<Pellet>();
            //
            // foreach (var pellet in pelletsSpawned)
            // {
            //     Physics.IgnoreCollision(this.GetComponent<Collider>(),pellet.GetComponent<Collider>());
            // }
            
            Physics.IgnoreCollision(this.GetComponent<Collider>(), FindObjectOfType<Pellet>().GetComponent<Collider>());
        }

        void Start()
        {
            lifeDuration = 1.5f;
            speed = 15f;
            lifeTimer = lifeDuration;
        }

        void FixedUpdate()
        {
            transform.position += transform.forward * (speed * Time.deltaTime);
            transform.localScale += transform.localScale * (growMultiplier * Time.deltaTime);
            lifeTimer -= Time.deltaTime;
            if (lifeTimer <= 0f)
            {
                Destroy(gameObject);
            }
        }


        public void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<UnitBase>())
            {
                if (unitBase.owner != other.GetComponent<UnitBase>().owner)
                {
                    other.transform.GetComponent<Health>().ChangeHealth(-amount);
                }
            }

            Destroy(gameObject);
        }
    }
}