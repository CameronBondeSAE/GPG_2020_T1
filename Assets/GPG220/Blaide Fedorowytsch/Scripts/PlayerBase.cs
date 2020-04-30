using System;
using System.Collections.Generic;
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

		private void Awake()
		{
			UnitBase.SpawnStaticEvent += UnitBaseOnSpawnStaticEvent;
			UnitBase.DespawnStaticEvent += UnitBaseOnDespawnStaticEvent;
		}

		// HACK: gamemode stuff
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

		private void UnitBaseOnSpawnStaticEvent(UnitBase obj)
		{
			if (obj.GetComponent<King>() && obj.netIdentity == netIdentity)
			{
				king = obj.GetComponent<King>();
			}
			else
			{
				units.Add(obj);
			}
		}
	}
}
