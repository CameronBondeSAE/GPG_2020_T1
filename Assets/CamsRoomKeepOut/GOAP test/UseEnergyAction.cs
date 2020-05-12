using System;
using System.Collections;
using System.Collections.Generic;
using ReGoap.Core;
using ReGoap.Unity;
using UnityEngine;

public class UseEnergyAction : ReGoapAction<string, object>
{
	public bool hasEnergy = true;

	protected override void Awake()
	{
		base.Awake();

		effects.Set("hasEnoughEnergy", true);
	}

	public override void PlanEnter(IReGoapAction<string, object> previousAction,
								   IReGoapAction<string, object> nextAction, ReGoapState<string, object> settings,
								   ReGoapState<string, object>   goalState)
	{
		base.PlanEnter(previousAction, nextAction, settings, goalState);

		Debug.Log("UseEnergy Planenter");
	}

	public override void PostPlanCalculations(IReGoapAgent<string, object> goapAgent)
	{
		base.PostPlanCalculations(goapAgent);

		Debug.Log("UseEnergy postplan calcs");
	}

	public override bool CheckProceduralCondition(GoapActionStackData<string, object> stackData)
	{
		Debug.Log("UseEnergy check proc condition");
		
		return base.CheckProceduralCondition(stackData) && hasEnergy;
	}

	public override void Exit(IReGoapAction<string, object> next)
	{
		base.Exit(next);
		Debug.Log("UseEnergy exit");
	}

	public override void PlanExit(IReGoapAction<string, object> previousAction,
								  IReGoapAction<string, object> nextAction, ReGoapState<string, object> settings,
								  ReGoapState<string, object>   goalState)
	{
		base.PlanExit(previousAction, nextAction, settings, goalState);

		Debug.Log("UseEnergy plan exit");
	}

	public override void Precalculations(GoapActionStackData<string, object> stackData)
	{
		base.Precalculations(stackData);

		Debug.Log("UseEnergy Precalc");

		if (hasEnergy)
		{
			// doneCallback(this);
		}
		else
		{
			// failCallback(this);
		}
	}

	public override ReGoapState<string, object> GetEffects(GoapActionStackData<string, object> stackData)
	{
		Debug.Log("UseEnergy GetEffects");

		// if (hasEnergy)
		// {
		// 	effects.Set("hasEnoughEnergy", true);
		// }
		// else
		// {
		// 	Debug.Log("UseEnergy NOT ENOUGH ENERGY");
		// 	effects.Clear();
		// 	// effects.Set("hasEnoughEnergy", false);
		// 	// Not enough
		// }
		//

		return base.GetEffects(stackData);
	}

	public override ReGoapState<string, object> GetPreconditions(GoapActionStackData<string, object> stackData)
	{
		Debug.Log("UseEnergy getpreconditions");

		return base.GetPreconditions(stackData);
	}

	public override float GetCost(GoapActionStackData<string, object> stackData)
	{
		Debug.Log("UseEnergy getcost");

		return base.GetCost(stackData);
	}


	public override void Run(IReGoapAction<string, object>         previous, IReGoapAction<string, object> next,
							 ReGoapState<string, object>           settings, ReGoapState<string, object>   goalState,
							 Action<IReGoapAction<string, object>> done,
							 Action<IReGoapAction<string, object>> fail)
	{
		base.Run(previous, next, settings, goalState, done, fail);

		Debug.Log("UseEnergy : Run START");

		StartCoroutine(UseEnergy());
	}

	private IEnumerator UseEnergy()
	{
		Debug.Log("UsingEnergy...");
		yield return new WaitForSeconds(2);
		Debug.Log("Used energy!");
		doneCallback(this);
	}
}