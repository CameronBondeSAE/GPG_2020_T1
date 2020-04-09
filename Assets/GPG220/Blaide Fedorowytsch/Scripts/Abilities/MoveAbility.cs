using System;
using System.Collections.Generic;
using GPG220.Blaide_Fedorowytsch.Scripts.Interfaces;
using GPG220.Luca.Scripts.Abilities;
using GPG220.Luca.Scripts.Unit;
using UnityEngine;

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
        public override bool SelectedExecute()
        {
            return true;
        }

        private void Awake()
        {
            abilityName = "Move";
            abilityDescription = "Should move the character.";
        }

        public override bool TargetExecute(Vector3 worldPos)
        {
            rb = GetComponent<Rigidbody>();
            ub = GetComponent<UnitBase>();
            heightOffset = GetComponent<Collider>().bounds.extents.y;
            target = worldPos;
            moving = true;
            return true;
        }

        private void FixedUpdate()
        {
            if (moving)
            {
                if (Vector3.Distance(this.gameObject.transform.position,
                        target + (Vector3.up * heightOffset) + OffsetPosition(ub.currentSelectionGroup)) > 0.1f)
                {
                    Move(target + (Vector3.up * heightOffset) + OffsetPosition(ub.currentSelectionGroup));
                }
                else
                {
                    moving = false;
                }
            }
        }
        Vector3 OffsetPosition(List<ISelectable> selectionGroup)
        {
            Vector3 total = new Vector3();
            foreach (ISelectable s in selectionGroup)
            {
                total += ((MonoBehaviour) s).gameObject.transform.position;
            }
            return transform.position - total / selectionGroup.Count;
        }

        void Move(Vector3 v)
        {
            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, v, 0.5f);
        }
    }
}
