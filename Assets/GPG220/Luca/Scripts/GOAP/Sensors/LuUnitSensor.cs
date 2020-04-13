using System;
using System.Collections.Generic;
using System.Linq;
using GPG220.Luca.Scripts.Unit;
using ReGoap.Core;
using ReGoap.Unity;
using UnityEngine;

namespace GPG220.Luca.Scripts.GOAP.Sensors
{
    [RequireComponent(typeof(UnitEyeSensor))]
    public class LuUnitSensor : ReGoapSensor<string, object>
    {
        private UnitEyeSensor _unitEyeSensor;
        
        List<UnitBase> injuredAllyUnits = new List<UnitBase>();

        private void Awake()
        {
            _unitEyeSensor = GetComponent<UnitEyeSensor>();
            _unitEyeSensor.UnitEnteredSightEvent += UnitEnteredSight;
            _unitEyeSensor.UnitExitedSightEvent += UnitExitedSight;
            UpdateUnitData();
        }

        public override void Init(IReGoapMemory<string, object> memory)
        {
            base.Init(memory);

            
            
            
            //var state = memory.GetWorldState();
        }

        private void UnitEnteredSight(UnitEyeSensor arg1, UnitBase arg2)
        {
            UpdateUnitData();
            Debug.Log("Unit Entered Sight...");
        }

        private void UnitExitedSight(UnitEyeSensor arg1, UnitBase arg2)
        {
            UpdateUnitData();
            Debug.Log("Unit Exited Sight...");
        }

        private void UpdateUnitData()
        {
            var state = memory?.GetWorldState();
            
            state?.Set("allyUnitsInProximity", _unitEyeSensor?.alliedUnits);
            //state.Set("enemyUnitsInProximity", _unitEyeSensor.enemyUnits);
            
            UpdateUnitHealthData();
        }

        // Determines the health status of units in sight
        private void UpdateUnitHealthData()
        {
            var state = memory?.GetWorldState();
            injuredAllyUnits = _unitEyeSensor?.alliedUnits?.Where(unit =>
                unit.health.CurrentHealth < unit.health.startingHealth && unit.health.CurrentHealth > 0).ToList();
            state?.Set("injuredAllyUnitsInProximity", injuredAllyUnits);
            state?.Set("injuredAllyUnitsInProximityCount", injuredAllyUnits?.Count ?? 0);
        }

        public override void UpdateSensor()
        {
            //var state = memory.GetWorldState();
            //state.Set("targetPos", targetPos.position);
            UpdateUnitHealthData();
        }
    }
}
