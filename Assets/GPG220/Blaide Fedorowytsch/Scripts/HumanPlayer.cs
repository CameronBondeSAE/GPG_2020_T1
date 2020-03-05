using System;

namespace GPG220.Blaide_Fedorowytsch.Scripts
{
    public class HumanPlayer : PlayerBase
    {
        public UnitSpawner unitSpawner;
        
        // HACK
        public uint netIdMine;
        public bool isLocalPlayerMine;
        
        // Start is called before the first frame update
        public override void BuildUnits()
        {
            base.BuildUnits();

            if (unitSpawner != null)
            {
                unitSpawner.owner = this;
                unitSpawner.RandomSpawns();
            }
        }

        // HACK
        private void Update()
        {
            isLocalPlayerMine = isLocalPlayer;
            netIdMine = netId;
        }
    }
}
