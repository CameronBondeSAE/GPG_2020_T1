using System;
using System.Collections;
using ReGoap.Core;
using ReGoap.Unity;
using UnityEngine;

public class VampireAction : ReGoapAction<string, object>
{
	protected override void Awake()
	{
		base.Awake();
		
		effects.Set("hasEnoughEnergy", true);
		preconditions.Set("nearAllies", true);
	}

	public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next, ReGoapState<string, object> settings, ReGoapState<string, object> goalState, Action<IReGoapAction<string, object>> done,
							 Action<IReGoapAction<string, object>> fail)
	{
		base.Run(previous, next, settings, goalState, done, fail);
		
		StartCoroutine(Vampire());
	}

	private IEnumerator Vampire()
	{
		Debug.Log("Vampiring ally....");
		yield return new WaitForSeconds(2);
		Debug.Log("Vamped!");
		doneCallback(this);
	}
}
