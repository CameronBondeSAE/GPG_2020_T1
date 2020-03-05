using System;

namespace GPG220.Blaide_Fedorowytsch.Scripts
{
    public class LocalPlayer : PlayerBase
    {
        public UnitSpawner unitSpawner;
        
        // HACK
        public uint netIdMine;
        
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
            netIdMine = netId;
        }
    }
}
