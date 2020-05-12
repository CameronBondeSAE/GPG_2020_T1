using System;
using System.Collections;
using ReGoap.Core;
using ReGoap.Unity;
using UnityEngine;

public class KillAction : ReGoapAction<string, object>
{
	protected override void Awake()
	{
		base.Awake();
		
		effects.Set("doDamage", true);
		preconditions.Set("hasEnoughEnergy", true);
	}

	public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next, ReGoapState<string, object> settings, ReGoapState<string, object> goalState, Action<IReGoapAction<string, object>> done,
							 Action<IReGoapAction<string, object>> fail)
	{
		base.Run(previous, next, settings, goalState, done, fail);

		Debug.Log("KillAction: Run");

		StartCoroutine(Kill());
	}

	private IEnumerator Kill()
	{
		Debug.Log("KILLING....");
		yield return new WaitForSeconds(2);
		Debug.Log("KILLED!");
		doneCallback(this);
	}
}
