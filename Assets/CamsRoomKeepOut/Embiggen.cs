using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GPG220.Luca.Scripts.Abilities;
using UnityEngine;

public class Embiggen : AbilityBase
{
	public float bigScale;

	public override bool TargetExecute(GameObject targets = null)
	{
		targets.transform.DOScale(bigScale, 1f).SetEase(Ease.OutElastic);

		return base.TargetExecute(targets);
	}
}
