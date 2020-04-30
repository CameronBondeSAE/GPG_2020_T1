using ReGoap.Unity;
using UnityEngine;

public class GoapyAgent : ReGoapAgent<string, object>
{

	// To be able to call this myself, you can override the base version and make it public
	public override bool CalculateNewGoal(bool forceStart = false)
	{
		return base.CalculateNewGoal(forceStart);
	}
}
