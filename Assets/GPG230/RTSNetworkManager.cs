using System;
using System.Collections;
using System.Collections.Generic;
using GPG220.Blaide_Fedorowytsch.Scripts;
using Mirror;
using UnityEngine;

public class RTSNetworkManager : NetworkManager
{
	public GameManager gameManagerPrefab;
	public PlayMenu playMenuHack;

	public event Action<NetworkConnection> OnClientPlayerSpawnEvent;
    public event Action<NetworkConnection> OnClientDisconnectedEvent;
	public event Action OnStartedHost;


	public override void Awake()
	{
		base.Awake();
	}

	public override void OnStartHost()
	{
		base.OnStartHost();
		
		// // Spawn managers etc here to give them authority to do network stuff
		// GameManager gameManager = Instantiate(gameManagerPrefab);
		// gameManager.networkManager = this;
		// gameManager.playMenu       = playMenuHack;
		// NetworkServer.Spawn(gameManager.gameObject);
		
		OnStartedHost?.Invoke();
	}

	public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
	}

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        OnClientPlayerSpawnEvent?.Invoke(conn);
    }
    

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        OnClientDisconnectedEvent?.Invoke(conn);
        
        base.OnServerDisconnect(conn);
        
    }

    public void SetHostname(string hostname)
    {
        this.networkAddress = hostname;
    }
}