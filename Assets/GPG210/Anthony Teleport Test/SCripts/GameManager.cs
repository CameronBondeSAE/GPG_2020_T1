using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using GPG220.Blaide_Fedorowytsch.Scripts;
using GPG220.Luca.Scripts.Unit;
using Mirror;
using Mirror.Examples.Basic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour
{
	public float unitBuildDistanceThreshold = 10f;

	public List<UnitBase>   globalUnitBases   = new List<UnitBase>();
	public List<SpawnPoint> listOfSpawns      = new List<SpawnPoint>();
	public List<PlayerBase> listofPlayerBases = new List<PlayerBase>();
	public List<UnitBase>   defaultUnitBases  = new List<UnitBase>();

	public RTSNetworkManager networkManager;

	public PlayerBase localPlayer;

	public event Action gameOverEvent;
	public event Action startGameEvent;

	// Hack: No references to View UI stuff generally from 'Model' managers etc
	public MultiplayerMenu   multiPlayerMenu;
	public UnitSpawner unitSpawner;

//subscribing to all the events
	private void Start()
	{
		UnitBase.SpawnStaticEvent   += UnitBaseOnSpawnStaticEvent;
		UnitBase.DespawnStaticEvent += UnitBaseOnDespawnStaticEvent;

		if (networkManager != null)
		{
			networkManager.OnClientPlayerSpawnEvent  += NetworkManagerOnOnClientPlayerSpawnEvent;
			networkManager.OnClientDisconnectedEvent += NetworkManagerOnOnClientDisconnectedEvent;
		}
	}

//player has left the game, remove from the list
	private void NetworkManagerOnOnClientDisconnectedEvent(NetworkConnection conn)
	{
		if (networkManager != null)
		{
			PlayerBase playerBase = conn.identity.GetComponent<PlayerBase>();
			listofPlayerBases.Remove(playerBase);

			if (playerBase.isLocalPlayer)
			{
				localPlayer = null;
			}

			//conn.identity.GetComponent<PlayerBase>().BuildUnits();
			//conn.identity.GetComponent<UnitBase>().owner.units
		}
	}

//player has connected to the game, add to the list
	private void NetworkManagerOnOnClientPlayerSpawnEvent(NetworkConnection conn)
	{
		if (networkManager != null)
		{
			PlayerBase playerBase = conn.identity.GetComponent<PlayerBase>();

			listofPlayerBases.Add(playerBase);

			if (playerBase.isLocalPlayer)
			{
				localPlayer = playerBase;
			}

			Debug.Log("Build units for new player");
			BuildUnits(conn.identity);
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
	public void BuildUnits(NetworkIdentity owner)
	{
		if (unitSpawner != null)
		{
			// unitSpawner.RandomSpawns(owner);
			unitSpawner.SpawnOneOfEach(owner);
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
	}

//Player spawns on a spawn point within the map
	private void UnitBaseOnSpawnStaticEvent(UnitBase obj)
	{
		// TODO Define Enemies and Players
		globalUnitBases.Add(obj);
		obj.GetComponent<Health>().deathEvent += HealthOndeathStaticEvent;
		listOfSpawns                          =  FindObjectsOfType<SpawnPoint>().ToList();

		//listOfSpawns = globalUnitBases[(Random.Range(0,listOfSpawns.Count))];
	}

//game over check if there are still units on the map
	public void CheckIfGameOver()
	{
		if (globalUnitBases.Count == 0 && gameOverEvent != null)
		{
			gameOverEvent?.Invoke();
		}
	}
}