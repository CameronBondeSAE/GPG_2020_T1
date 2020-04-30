using System.Collections;
using System.Collections.Generic;
using ReGoap.Unity;
using UnityEngine;

public class UseEnergyAction : ReGoapAction<string, object>
{
	protected override void Awake()
	{
		base.Awake();
		
		effects.Set("hasEnoughEnergy", true);
	}
}
