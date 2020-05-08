using System;
using System.Collections.Generic;
using Anthony;
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
    [Serializable]
	public abstract class UnitBase : NetworkBehaviour, ISelectable, IDescribable
    {
        public static event Action<UnitBase> SpawnStaticEvent;
        public static event Action<UnitBase> DespawnStaticEvent;
          
		[SyncVar]
		public uint ownerNetID;
		[SyncVar]
		public NetworkIdentity owner;
		[SyncVar] 
		public Color myColour;
		
		private MeshRenderer meshRenderer;
		public UnitStats unitStats;
        public Inventory inventory;
        public Rigidbody rb;
        public Health health;
        public AbilityController abilityController;
        public List<ISelectable> currentSelectionGroup;
        // Debug
		public uint myNetID;
		public NetworkIdentity myOwnerIdentity;
		public string debug;

		public string unitName;
		[TextArea]
		public string unitDescription;
		public Sprite unitImage;
		
		public string Name 
		{
			get { return unitName;}
		}

		public string Description
		{
			get { return unitDescription; }
		}

		public Sprite Image
		{
			get { return unitImage; }
			set { unitImage = value; }
		}

		private void LateUpdate()
		{
			// DEBUG for inspector
			myNetID = netId;
			myOwnerIdentity = owner;

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

			// Set default drags etc You'd have to override this
			rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY |
							 RigidbodyConstraints.FreezeRotationZ;

			rb.drag = 10f;
			
			
			if (owner != null)
			{
				// Is it a player? (Resources are units also)
				var playerBase = owner.gameObject.GetComponent<PlayerBase>();
				if (playerBase != null)
				{
					myColour = playerBase.playerColour;

					if (GetComponent<MeshRenderer>() != null)
					{
						meshRenderer = GetComponent<MeshRenderer>();
					}
					else
					{
						meshRenderer = GetComponentInChildren<MeshRenderer>();
					}
					meshRenderer.material.SetColor("_TeamColour", myColour);
				}
			}

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

		        abilityController.TargetExecuteDefaultAbility(g);
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

        

        // [ClientRpc]
		// public void RpcSyncID(uint id)
		// {
			// ownerNetID = id;
			// print("Sync ID from server to client");
		// }
	}
}
