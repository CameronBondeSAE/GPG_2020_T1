﻿using System;
using System.Collections;
using System.Collections.Generic;
using GPG220.Blaide_Fedorowytsch.Scripts;
using Mirror;
using UnityEngine;

public class RTSNetworkManager : NetworkManager
{
    public event Action <NetworkConnection> OnClientConnectedEvent;
    public event Action<NetworkConnection> OnClientDisconnectedEvent; 
    public void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        
        
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
        
        OnClientConnectedEvent?.Invoke(conn);
        
    }

    public override void OnServerRemovePlayer(NetworkConnection conn, NetworkIdentity player)
    {
        base.OnServerRemovePlayer(conn, player);
        
        OnClientDisconnectedEvent?.Invoke(conn);
    }

}
