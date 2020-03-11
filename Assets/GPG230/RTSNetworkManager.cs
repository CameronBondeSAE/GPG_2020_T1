using System;
using System.Collections;
using System.Collections.Generic;
using GPG220.Blaide_Fedorowytsch.Scripts;
using Mirror;
using UnityEngine;

public class RTSNetworkManager : NetworkManager
{
    public event Action<NetworkConnection> OnClientPlayerSpawnEvent;
    public event Action<NetworkConnection> OnClientDisconnectedEvent;

    public void OnServerConnect(NetworkConnection conn)
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