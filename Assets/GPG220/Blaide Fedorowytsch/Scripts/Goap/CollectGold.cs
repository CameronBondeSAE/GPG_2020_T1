using ReGoap.Unity;

namespace GPG220.Blaide_Fedorowytsch.Scripts.Goap
{
    public class CollectGold : ReGoapGoal<string,object>
    {
        protected override void Awake()
        {
            base.Awake();
            goal.Set("HasGold", true);
            
        }
    }
}
