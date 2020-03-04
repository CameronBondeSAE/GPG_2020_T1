using System.Collections.Generic;
using GPG220.Blaide_Fedorowytsch.Scripts.Interfaces;
using GPG220.Luca.Scripts.Abilities;
using UnityEngine;

namespace GPG220.Blaide_Fedorowytsch.Scripts.Abilities
{
    public class MoveAbility : AbilityBase
    {
        private Rigidbody rb;
        private Vector3 target;
        public bool moving = false;
        public UnitSelectionManager usm;
        public Collider collider;
        private float heightOffset;

        public Vector3 targetPos;

        public override bool Execute(GameObject executorGameObject, GameObject[] targets = null)
        {
            rb = executorGameObject.GetComponent<Rigidbody>();
            heightOffset = gameObject.GetComponent<Collider>().bounds.extents.y;
            usm = FindObjectOfType<UnitSelectionManager>();
            target = usm.targetPoint;
            moving = true;
        
            return true;
        }

        private void FixedUpdate()
        {
            if (moving)
            {
                if (Vector3.Distance(this.gameObject.transform.position,
                        target + (Vector3.up * heightOffset) + OffsetPosition(usm.selectedIselectables)) > 0.1f)
                {
                    Move(target + (Vector3.up * heightOffset) + OffsetPosition(usm.selectedIselectables));
                }
                else
                {
                    moving = false;
                }
            }
        }
        Vector3 OffsetPosition(List<ISelectable> selctionGroup)
        {
            Vector3 total = new Vector3();

            foreach (ISelectable s in selctionGroup)
            {
                total += ((MonoBehaviour) s).gameObject.transform.position;
            }

            return transform.position - total / selctionGroup.Count;
        }

        void Move(Vector3 v)
        {
            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, v, 0.5f);
        }
    }
}
