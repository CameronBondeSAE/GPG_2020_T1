using ReGoap.Unity;

public class DoDamageGoal : ReGoapGoal<string,object>
{
	protected override void Awake()
	{
		base.Awake();
		
		goal.Set("doDamage", true);
	}
}
