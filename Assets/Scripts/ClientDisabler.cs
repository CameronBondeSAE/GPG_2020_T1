using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ClientDisabler : NetworkBehaviour
{
	public MonoBehaviour[] disableOnClient;
	// public MonoBehaviour[] disableOnClient;
	
    void Awake()
    {
		// if (!isServer)
		// {
			// foreach (var monoBehaviour in disableOnClient)
			// {
				// if (monoBehaviour != null) monoBehaviour.enabled = false;
			// }
		// }

		// GameManager gameManager = FindObjectOfType<GameManager>();
		// gameManager.startGameEvent += GameManagerOnstartGameEvent;

		RTSNetworkManager rtsNetworkManager = FindObjectOfType<RTSNetworkManager>();
		rtsNetworkManager.ClientConnectedCalledEvent += GameManagerOnstartGameEvent;
	}

	// private void Update()
	// {
		// Debug.Log("isServerOnly??! = "+is + " : isclientonly :"+isClientOnly);
	// }

	private void GameManagerOnstartGameEvent(NetworkConnection networkConnection)
	{
		if (!isServer)
		{
			foreach (var monoBehaviour in disableOnClient)
			{
				if (monoBehaviour != null) monoBehaviour.enabled = false;
			}
		}
	}
}
