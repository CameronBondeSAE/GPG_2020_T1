using System;
using System.Collections.Generic;
using System.Linq;
using GPG220.Blaide_Fedorowytsch.Scripts;
using GPG220.Blaide_Fedorowytsch.Scripts.ProcGen;
using GPG220.Luca.Scripts.Unit;
using Mirror;
using Sirenix.OdinInspector;
using UnityEngine;


public class GameManager : MonoBehaviour
{
	[ReadOnly]
	public RTSNetworkManager networkManager;
	[ReadOnly]
	public ProceduralMeshGenerator proceduralMeshGenerator;
	[ReadOnly]
	public ProceduralGrowthSystem  proceduralGrowthSystem;
	
	public List<UnitBase>   globalUnitBases   = new List<UnitBase>();
	public List<PlayerBase> playerBases = new List<PlayerBase>();


	
	// Game Mode - King
	public float unitBuildDistanceThreshold = 10f;
	
	public UnitSpawner unitSpawner;
	public UnitSpawner unitSpawnerKing;
	public HealthbarViewModel healthbarPrefab;

	[ReadOnly]
	public PlayerBase localPlayer;
	[ReadOnly]
	public List<King> kings;

	public event Action GameOverEvent;
	public event Action startGameEvent;

	[ReadOnly]
	public MapUtilities mapUtilities;
	
	
	// Subscribing to all the events
	private void Start()
	{
		mapUtilities = FindObjectOfType<MapUtilities>();
		
		UnitBase.SpawnStaticEvent   += UnitBaseOnSpawnStaticEvent;
		UnitBase.DespawnStaticEvent += UnitBaseOnDespawnStaticEvent;

		if (networkManager != null)
		{
			networkManager.OnClientPlayerSpawnEvent  += NetworkManagerOnClientPlayerSpawnEvent;
			networkManager.OnClientDisconnectedEvent += NetworkManagerOnClientDisconnectedEvent;
		}
	}

	// Player has left the game, remove from the list
	private void NetworkManagerOnClientDisconnectedEvent(NetworkConnection conn)
	{
		if (networkManager != null)
		{
			PlayerBase playerBase = conn.identity.GetComponent<PlayerBase>();
			playerBases.Remove(playerBase);

			if (playerBase.isLocalPlayer)
			{
				localPlayer = null;
			}

			//conn.identity.GetComponent<PlayerBase>().BuildUnits();
			//conn.identity.GetComponent<UnitBase>().owner.units
		}
	}

//player has connected to the game, add to the list
	private void NetworkManagerOnClientPlayerSpawnEvent(NetworkConnection conn)
	{
		if (networkManager != null)
		{
			PlayerBase playerBase = conn.identity.GetComponent<PlayerBase>();

			playerBases.Add(playerBase);

			if (playerBase.isLocalPlayer)
			{
				localPlayer = playerBase;
			}

			Debug.Log("Build units for new player");

			BuildKing(conn.identity, playerBase);

			BuildUnits(conn.identity, playerBase);

			// TODO: Networking
			HealthbarViewModel healthBar = Instantiate(healthbarPrefab);
			
		}
	}

//game starts
	public void OnStartGameEvent()
	{
		if (startGameEvent != null)
		{
			startGameEvent.Invoke();
		}
	}

	//spawn the units randomly around the map
	public void BuildUnits(NetworkIdentity owner, PlayerBase playerBase)
	{
		if (unitSpawner != null)
		{
			// unitSpawner.RandomSpawns(owner);
			playerBase.units = unitSpawner.SpawnOneOfEach(owner);
		}
	}

	public void BuildKing(NetworkIdentity owner, PlayerBase playerBase)
	{
		King king = null;

		if (unitSpawnerKing != null)
		{
			UnitBase unitBaseOfKing = unitSpawnerKing.unitBases[0];

			
			// HACK: Have to spawn a real instance to get the actual bounds, because you can't from a prefab??
			GameObject temp = Instantiate(unitBaseOfKing.gameObject);

			Vector3 unitExtents = temp.GetComponent<Collider>().bounds.extents;
			
			DestroyImmediate(temp); // HACK

			Vector3 rndPoint = mapUtilities.RandomGroundPointInBounds(proceduralMeshGenerator.mesh.bounds, unitExtents*2f);
			king = unitSpawnerKing.SpawnUnit(owner, unitBaseOfKing, rndPoint, Quaternion.identity) as King;

			playerBase.king = king;
		}
	}


	private void HealthOndeathStaticEvent(Health health)
	{
	}

//player is removed from the match and unsubscribes from the health event
	private void UnitBaseOnDespawnStaticEvent(UnitBase obj)
	{
		globalUnitBases.Remove(obj);
		obj.GetComponent<Health>().deathEvent -= HealthOndeathStaticEvent;

		CheckForGameOver();
	}

	private void CheckForGameOver()
	{
		List<King> kings = new List<King>();
		
		foreach (PlayerBase playerBase in playerBases)
		{
			// Count Kings
			kings.Add(playerBase.king);
		}

		if (kings.Count == 1)
		{
			// There's only one king left AFTER a king just got destroyed. So someone won
			GameOverEvent?.Invoke();
		}

		if (kings.Count <= 0)
		{
			// TODO: Handle the last king dying quickly after the win state
			Debug.Log("LAST KING DIED AS WELL! Probably shouldn't happen!");
		}
	}

	//Player spawns on a spawn point within the map
	private void UnitBaseOnSpawnStaticEvent(UnitBase obj)
	{
		// TODO Define Enemies and Players
		globalUnitBases.Add(obj);
		obj.GetComponent<Health>().deathEvent += HealthOndeathStaticEvent;
		// listOfSpawns                          =  FindObjectsOfType<SpawnPoint>().ToList();

		//listOfSpawns = globalUnitBases[(Random.Range(0,listOfSpawns.Count))];
	}

//game over check if there are still units on the map
	public void CheckIfGameOver()
	{
		if (globalUnitBases.Count == 0 && GameOverEvent != null)
		{
			GameOverEvent?.Invoke();
		}
	}
}