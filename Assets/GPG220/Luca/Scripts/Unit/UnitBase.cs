using System;
using System.Collections.Generic;
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
        [SyncVar]
        public uint ownerNetID;
        public UnitStats unitStats;
        public Inventory inventory;
        public Rigidbody rb;
        public Health health;
        public AbilityController abilityController;
        public List<ISelectable> currentSelectionGroup;
        // Debug
		public uint myNetID;
		public string debug;

		private void LateUpdate()
		{
			// DEBUG for inspector
			myNetID = netId;

			debug = "Auth = " + hasAuthority;
			// debug = netIdentity.connectionToServer.
		}


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
        public virtual void OnSelected( List<ISelectable> selectionGroup )
        {
	        currentSelectionGroup = selectionGroup;
        }

        public virtual void OnDeSelected()
        {
	        currentSelectionGroup = null;
        }

        public virtual void OnExecuteAction(Vector3 worldPosition, GameObject g)
        {
	        if (g != null)
	        {
		        GameObject[] gs = new GameObject[1];
		        gs[0] = g;
		        abilityController.TargetExecuteDefaultAbility(gs);
	        }
	        else 
	        {
		        abilityController.TargetExecuteDefaultAbility(worldPosition);
	        }
        }

        private void OnDestroy()
        {
            // TODO this only will work if the actual GO is destroyed. It won't work if the unit's death code, doesn't actually destroy itself
            Unload();
        }

		[ClientRpc]
		public void RpcSyncID(uint id)
		{
			ownerNetID = id;
		}
	}
}
