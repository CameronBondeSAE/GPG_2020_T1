using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPG220.Luca.Scripts.Unit
{
    [RequireComponent(typeof(UnitBase))]
    public class UnitEyeSensor : MonoBehaviour
    {
        public UnitBase unitSelf;
        public CollisionNotifier viewBoxCollisionNotifier;
        
        public event Action<UnitEyeSensor,UnitBase> UnitEnteredSightEvent;
        public event Action<UnitEyeSensor,UnitBase> UnitExitedSightEvent;
        
        public List<UnitBase> allUnits;
        public List<UnitBase> alliedUnits;
        public List<UnitBase> enemyUnits;


        private void Awake()
        {
            unitSelf = GetComponent<UnitBase>();
            
            if (viewBoxCollisionNotifier == null)
            {
                viewBoxCollisionNotifier = GetComponentInChildren<CollisionNotifier>();
            }

            if (viewBoxCollisionNotifier != null)
            {
                viewBoxCollisionNotifier.TriggerEnterEvent += ObjectEnteredSight;
                viewBoxCollisionNotifier.TriggerExitEvent += ObjectExitedSight;
            }
        }

        private void ObjectEnteredSight(Collider obj)
        {
            var unit = obj.GetComponent<UnitBase>();
            if (unit != null && unit != unitSelf)
            {
                if(!allUnits.Contains(unit))
                    allUnits.Add(unit);
                
                if(unitSelf?.owner == unit.owner && !alliedUnits.Contains(unit))
                    alliedUnits.Add(unit);
                
                if(unitSelf?.owner != unit.owner && !enemyUnits.Contains(unit))
                    enemyUnits.Add(unit);
                
                UnitEnteredSightEvent?.Invoke(this,unit);
            }
        }

        private void ObjectExitedSight(Collider obj)
        {
            var unit = obj.GetComponent<UnitBase>();
            if (unit != null)
            {
                if(allUnits.Contains(unit)) allUnits.Remove(unit);
                if(alliedUnits.Contains(unit)) alliedUnits.Remove(unit);
                if(enemyUnits.Contains(unit)) enemyUnits.Remove(unit);
                
                UnitExitedSightEvent?.Invoke(this,unit);
            }
        }
    }
}
