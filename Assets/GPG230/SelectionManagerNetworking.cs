using System;
using System.Collections;
using System.Collections.Generic;
using GPG220.Blaide_Fedorowytsch.Scripts;
using Mirror;
using UnityEngine;

public class SelectionManagerNetworking : NetworkBehaviour
{

    public UnitSelectionManager unitSelectionManager;
    
        
    
    // Start is called before the first frame update
    void Start()
    {
        // TODO sub to the event 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Command]
    void CmdDoAction()
    {
        
        
        
    }

    private void OnDestroy()
    {
       // TODO un sub to the event  
    }
}
