using ReGoap.Unity;

namespace GPG220.Dylan.Scripts.GOAPFirstTry
{
    public class GoapAgentDylan : ReGoapAgent<string , object>
    {
        public override bool CalculateNewGoal(bool forceStart = false)
        {
            return base.CalculateNewGoal(forceStart);
        }
    }
}
