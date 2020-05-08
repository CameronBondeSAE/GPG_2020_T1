using System;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Resources;
using GPG220.Luca.Scripts.Unit;
using Mirror;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GPG220.Blaide_Fedorowytsch.Scripts
{
    public class PlayerBase : NetworkBehaviour
    {
        // HACK: Shouldn't have gamemode specific stuff in the base class
		public List<UnitBase> units;
		public King king;
		
        [SyncVar]
        public Color playerColour;
		[SyncVar]
        public string playerName;

		Inventory inventory;

		// Really we should network Luca's inventory system IF the client even needs to know this (usually just for UI elements)
		[SyncVar]
		public int hackGoldAmount;

		private void Awake()
		{
			UnitBase.SpawnStaticEvent += UnitBaseOnSpawnStaticEvent;
			UnitBase.DespawnStaticEvent += UnitBaseOnDespawnStaticEvent;

			// HACK. Inventory should be syncing
			inventory = GetComponent<Inventory>();
			inventory.ResQuantityChangedEvent += OnResQuantityChangedEvent;
		}

		private void OnResQuantityChangedEvent(Inventory inventory, ResourceType resourcetype, int amtchange)
		{
			// TODO. Fire event
		}

		// HACK: gamemode stuff
		private void UnitBaseOnSpawnStaticEvent(UnitBase obj)
		{
			if (obj.GetComponent<King>() && obj.netIdentity == netIdentity)
			{
				king = obj.GetComponent<King>();
			}
			else if (obj.netIdentity == netIdentity)
			{
				units.Add(obj);
			}
		}
		private void UnitBaseOnDespawnStaticEvent(UnitBase obj)
		{
			if (obj.GetComponent<King>() && obj.netIdentity == netIdentity)
			{
				king = null;
			}
			else
			{
				units.Remove(obj);
			}
		}

	}
}
