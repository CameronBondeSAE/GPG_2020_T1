using GPG220.Blaide_Fedorowytsch.Scripts.Interfaces;
using GPG220.Luca.Scripts.Resources;
using Mirror;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GPG220.Luca.Scripts.Unit
{
    /// <summary>
    /// Base class for units. (A unit can be a building, movable unit, ...)
    /// </summary>
    [RequireComponent(typeof(Rigidbody), typeof(Health), typeof(Inventory))]
    public abstract class UnitBase : NetworkBehaviour, ISelectable
    {
        // TODO Unit Abilities
        public UnitStats unitStats;
        public Inventory inventory;
        public Rigidbody rb;
        public Health health;
        

        protected virtual void Initialize()
        {
            unitStats = GetComponent<UnitStats>();
            inventory = GetComponent<Inventory>();
            rb = GetComponent<Rigidbody>();
            health = GetComponent<Health>();
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
    }
}
