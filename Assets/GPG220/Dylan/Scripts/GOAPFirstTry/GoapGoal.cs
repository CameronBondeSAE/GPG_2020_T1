using ReGoap.Unity;

namespace GPG220.Dylan.Scripts.GOAP
{
    public class GoapGoal : ReGoapGoal<string, object>
    {
        protected override void Awake()
        {
            base.Awake();
            goal.Set("Goal Name", true);
        }
    }
}
