using System;
using System.Collections;
using System.Collections.Generic;
using GPG220.Blaide_Fedorowytsch.Scripts;
using Mirror;
using UnityEngine;

public class RTSNetworkManager : NetworkManager
{

    public event Action <NetworkConnection> OnClientConnected;

    public PlayerBase Player;
    
    // Start is called before the first frame update
    void Start()
    {
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        
        OnClientConnected.Invoke(conn);
        
    }


}
