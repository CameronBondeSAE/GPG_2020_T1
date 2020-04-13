using ReGoap.Core;
using ReGoap.Planner;
using ReGoap.Unity;

namespace GPG220.Luca.Scripts.GOAP.Goals
{
    public class LuNoInjuredAllyUnitsGoal : ReGoapGoal<string, object>
    {
        protected override void Awake()
        {
            base.Awake();
        
            //goal.Set("injuredAllyUnitsInProximityCount", 0);
            //goal.Set("abilityExecuted", true);
            goal.Set("unitHealed", true);
            
        }
        
        public override ReGoapState<string, object> GetGoalState()
        {
            
            
            return base.GetGoalState();
        }
    }
}
