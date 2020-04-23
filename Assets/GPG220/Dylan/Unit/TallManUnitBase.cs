using System;
using GPG220.Luca.Scripts.Unit;
using UnityEngine;

namespace GPG220.Dylan.Unit
{
    public class TallManUnitBase : UnitBase
    {
        private void Start()
        {
            GetComponent<Health>().deathEvent += Death;
            Initialize();
        }

        private void Death(Health health)
        {
            Destroy(gameObject);
        }

        public override void OnExecuteAction(Vector3 worldPosition, GameObject g)
        {
            base.OnExecuteAction(worldPosition, g);
        }

        public override void OnSelected()
        {
            base.OnSelected();
        }

        public override void OnDeSelected()
        {
            base.OnDeSelected();
        }

        public override bool Selectable()
        {
            return base.Selectable();
        }

        public override bool GroupSelectable()
        {
            return base.GroupSelectable();
        }
    }
}
