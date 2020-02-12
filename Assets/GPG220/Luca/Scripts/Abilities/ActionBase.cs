using GPG220.Luca.Scripts.Unit;

namespace GPG220.Luca.Scripts.Abilities
{
    public abstract class ActionBase
    {
        public string actionName = "";

        public virtual void ExecuteAction(UnitBase masterUnit)
        {
            
        }
    }
}