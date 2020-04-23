using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ClientDisabler : NetworkBehaviour
{
	public MonoBehaviour[] disableOnClient;
	
    void Awake()
    {
		if (!isServer)
		{
			foreach (var monoBehaviour in disableOnClient)
			{
				monoBehaviour.enabled = false;
			}
		}
    }
}
