using System.Collections.Generic;
using GPG220.Luca.Scripts.Unit;
using Mirror;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GPG220.Blaide_Fedorowytsch.Scripts
{
	public class UnitSpawner : NetworkBehaviour
	{
		[ReorderableList]
		public List<UnitBase> unitBases;

		public int spawnNumber = 1;

		private MapUtilities mapUtilities;

		private void Start()
		{
			mapUtilities = FindObjectOfType<MapUtilities>();
		}


		public UnitBase SpawnUnit(NetworkIdentity owner, UnitBase unit, Vector3 position, Quaternion rotation)
		{
			if (isServer)
			{
				GameObject g = Instantiate(unit.gameObject, position, rotation);

				// Networking
				UnitBase uB = g.GetComponent<UnitBase>();
				uB.owner      = owner;
				uB.ownerNetID = owner.netId;

				// Assign ownership of spawned units to client
				NetworkServer.Spawn(g, owner.gameObject);
				// g.GetComponent<NetworkIdentity>().AssignClientAuthority(playerBaseOwner.GetComponent<NetworkIdentity>().connectionToClient);

				return uB;
			}
			else
			{
				Debug.LogWarning("Trying to spawn from client");
				return null;
			}
		}

		//[Button (Name = "RandomSpawn" )]
		public void RandomSpawns(NetworkIdentity owner)
		{
			Vector3 position;
			for (int i = 0; i < spawnNumber; i++)
			{
				int randIndex = Random.Range(0, unitBases.Count);

				position =
					mapUtilities.RandomGroundPointInBounds(new Bounds(transform.position, mapUtilities.boundrySize),
														   unitExtents(unitBases[randIndex]));
				SpawnUnit(owner, unitBases[randIndex], position, Quaternion.Euler(Vector3.forward));
			}
		}

		public List<UnitBase> SpawnOneOfEach(NetworkIdentity owner)
		{
			List<UnitBase> unitBases = new List<UnitBase>();

			Vector3 position;
			for (int clones = 0; clones < spawnNumber; clones++)
			{
				for (int i = 0; i < unitBases.Count; i++)
				{
					position =
						mapUtilities.RandomGroundPointInBounds(new Bounds(transform.position, mapUtilities.boundrySize),
															   unitExtents(unitBases[i]));
					unitBases.Add(SpawnUnit(owner, unitBases[i], position, Quaternion.identity));
				}
			}

			return unitBases;
		}

		public Vector3 unitExtents(UnitBase unit)
		{
			//TODO Figure out a nicer collision check.
			// return unit.gameObject.GetComponent<Collider>().bounds.extents; // this always returns vector3.zero
			return
				unit.gameObject.GetComponentInChildren<Renderer>().bounds
					.extents; // Not ideal as the physical collider could easily be different to the renderer Bounds... 
		}
	}
}