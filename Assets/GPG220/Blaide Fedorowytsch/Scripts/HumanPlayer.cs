using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GPG220.Blaide_Fedorowytsch.Scripts
{
    public class HumanPlayer : PlayerBase
    {
        // HACK
        public uint netIdMine;
        public bool isLocalPlayerMine;
    
        // HACK
        private void Update()
        {
            isLocalPlayerMine = isLocalPlayer;
            netIdMine = netId;
        }

        public void Start()
        {
            playerColour = new Color(Random.Range(0F,1F), Random.Range(0, 1F), Random.Range(0, 1F));
			
			// HACK? Need to set the localPlayer for CLIENTS so they can Units owned by the player can assign ownership when THEY request units spawned (the king)
			if (isLocalPlayer)
			{
				FindObjectOfType<GameManager>().localPlayer = this;
			}
		}
    }
}
