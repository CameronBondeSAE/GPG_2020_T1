using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ClientDisabler : NetworkBehaviour
{
	public MonoBehaviour[] disableOnClient;
	
    void Awake()
    {
		GameManager gameManager = FindObjectOfType<GameManager>();
		gameManager.startGameEvent += GameManagerOnstartGameEvent;
    }

	private void GameManagerOnstartGameEvent()
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
