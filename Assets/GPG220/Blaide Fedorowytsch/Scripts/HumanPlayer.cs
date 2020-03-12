using System;

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
    }
}
