using System;
using System.Collections.Generic;
using GPG220.Blaide_Fedorowytsch.Scripts.Interfaces;
using GPG220.Luca.Scripts.Abilities;
using GPG220.Luca.Scripts.Unit;
using NSubstitute.Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GPG220.Blaide_Fedorowytsch.Scripts.Abilities
{
    public class MoveAbility : AbilityBase
    {
        private Rigidbody rb;
        private Vector3 target;
        public bool moving = false;
        private float heightOffset = 0.5f;
        private UnitBase ub;
        public Vector3 targetPos;
        public Vector3 offsetPosition;
        
        public override bool SelectedExecute()
        {
            return true;
        }

        private void Awake()
        {
            abilityName = "Move";
            abilityDescription = "Should move the character.";
            targetRequired = true;
        }

        public override bool TargetExecute(Vector3 worldPos)
        {
            rb = GetComponent<Rigidbody>();
            ub = GetComponent<UnitBase>();
            heightOffset = GetComponent<Collider>().bounds.extents.y;
            target = worldPos;
            moving = true;
            float x = Random.Range(0f, 1f);
            float z = Random.Range(0f, 1f);
            offsetPosition = new Vector3(x, 0, z);
            return true;
       
        }

        private void FixedUpdate()
        {
            if (moving)
            {
                if (Vector3.Distance(this.gameObject.transform.position,
                        target + (Vector3.up * heightOffset) + offsetPosition) > 0.3f)
                {
                    Move(target + (Vector3.up * heightOffset) + offsetPosition);
                }
                else
                {
                    moving = false;
                }
            }
        }
       

        void Move(Vector3 v)
        {
            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, v, 0.5f);
        }
    }
}
