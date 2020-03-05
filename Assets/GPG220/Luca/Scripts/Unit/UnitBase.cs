using System;
using GPG220.Blaide_Fedorowytsch.Scripts;
using GPG220.Blaide_Fedorowytsch.Scripts.Interfaces;
using GPG220.Luca.Scripts.Abilities;
using GPG220.Luca.Scripts.Resources;
using Mirror;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GPG220.Luca.Scripts.Unit
{
    /// <summary>
    /// Base class for units. (A unit can be a building, movable unit, ...)
    /// </summary>
    [RequireComponent(typeof(Rigidbody), typeof(Health), typeof(Inventory)), RequireComponent(typeof(AbilityController))]
    public abstract class UnitBase : NetworkBehaviour, ISelectable
    {
        public static event Action<UnitBase> SpawnStaticEvent;
        public static event Action<UnitBase> DespawnStaticEvent;
                
        // TODO set owner somewhere
        public PlayerBase owner;

        public UnitStats unitStats;
        public Inventory inventory;
        public Rigidbody rb;
        public Health health;
        public AbilityController abilityController;

        private uint netID;
        

        protected virtual void Initialize()
        {
            unitStats = GetComponent<UnitStats>();
            inventory = GetComponent<Inventory>();
            rb = GetComponent<Rigidbody>();
            health = GetComponent<Health>();
            abilityController = GetComponent<AbilityController>();
            
            SpawnStaticEvent?.Invoke(this);
        }

        protected virtual void Unload()
        {
            DespawnStaticEvent?.Invoke(this);
        }

        public virtual bool Selectable()
        {
            return true;
        }

        public virtual bool GroupSelectable()
        {
            return true;
        }

        public virtual void OnSelected()
        {
            
        }

        public virtual void OnDeSelected()
        {
            
        }

        public virtual void OnExecuteAction(Vector3 worldPosition, GameObject g)
        {
            
        }

        private void OnDestroy()
        {
            // TODO this only will work if the actual GO is destroyed. It won't work if the unit's death code, doesn't actually destroy itself
            Unload();
        }
    }
}
