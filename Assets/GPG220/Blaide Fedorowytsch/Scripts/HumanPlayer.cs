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

        public void Awake()
        {
            playerColour = new Color(Random.Range(0F,1F), Random.Range(0, 1F), Random.Range(0, 1F));
        }
    }
}
