namespace GPG220.Blaide_Fedorowytsch.Scripts
{
    public class LocalPlayer : PlayerBase
    {
        public UnitSpawner unitSpawner;
        
        // Start is called before the first frame update
        public override void BuildUnits()
        {
            base.BuildUnits();

            unitSpawner.owner = this;
            unitSpawner.RandomSpawns();
        }
    }
}
