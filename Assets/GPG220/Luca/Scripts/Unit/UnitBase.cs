using GPG220.Luca.Scripts.Resources;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GPG220.Luca.Scripts.Unit
{
    /// <summary>
    /// Base class for units. (A unit can be a building, movable unit, ...)
    /// </summary>
    public abstract class UnitBase : SerializedMonoBehaviour
    {
        // TODO Unit Abilities
        public UnitStats unitStats;
        public Inventory inventory;

        protected virtual void Initialize()
        {
            unitStats = GetComponent<UnitStats>();
            inventory = GetComponent<Inventory>();
        }
    }
}
