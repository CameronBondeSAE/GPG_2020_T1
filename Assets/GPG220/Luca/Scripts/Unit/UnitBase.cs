using GPG220.Blaide_Fedorowytsch.Scripts.Interfaces;
using GPG220.Luca.Scripts.Resources;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GPG220.Luca.Scripts.Unit
{
    /// <summary>
    /// Base class for units. (A unit can be a building, movable unit, ...)
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public abstract class UnitBase : SerializedMonoBehaviour, ISelectable
    {
        // TODO Unit Abilities
        public UnitStats unitStats;
        public Inventory inventory;
        

        protected virtual void Initialize()
        {
            unitStats = GetComponent<UnitStats>();
            inventory = GetComponent<Inventory>();
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
