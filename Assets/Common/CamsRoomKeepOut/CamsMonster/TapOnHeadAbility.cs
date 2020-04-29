using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GPG220.Luca.Scripts.Abilities;
using UnityEngine;

public class TapOnHeadAbility : AbilityBase
{
	public Vector3 getReadyScale = Vector3.one * 2f;
	public int damage;
	public float duration = 1f;
	private Renderer mainRenderer;


	private void Awake()
	{
		mainRenderer = GetComponentInChildren<Renderer>();
	}

	// User clicked on Ability UI. I just play an effect, but you could actually do your whole ability code here if it doesn't need a target
	public override bool SelectedExecute()
	{
		transform.DOScale(getReadyScale, duration).SetEase(Ease.OutFlash);
		mainRenderer.material.color = Color.green;
		mainRenderer.material.DOColor(Color.white, duration);

		return base.SelectedExecute();
	}

	// Right clicked on ANY UnitBase (I can kill myself even, you should check for ownership with localPlayer)
	public override bool TargetExecute(GameObject target = null)
	{
		transform.DOScale(getReadyScale*2, duration).SetEase(Ease.OutElastic);
		mainRenderer.material.color = Color.red;
		mainRenderer.material.DOColor(Color.white, duration);

		// KILL
		target.GetComponent<Health>().ChangeHealth(-damage);
		
		return base.TargetExecute(target);
	}
}