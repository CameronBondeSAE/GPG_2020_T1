using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Mirror;

public class Wall : NetworkBehaviour
{
	public float duration = 4f;

	public bool hidden = true;
	
	public Collider col;
	Renderer rend;
	public Transform target;
	public Vector3 targetScale;
	
    private void Awake()
	{
		 col = GetComponent<Collider>();
		rend = GetComponentInChildren<Renderer>();
		rend.enabled = false;
		
		target.localScale = new Vector3(targetScale.x, 0, targetScale.z);
	}

    public void Appear()
    {
		col.enabled = true;
		rend.enabled = true;

		if (hidden)
		{
			var tweenerCore = target.DOScale(targetScale, duration).SetEase(Ease.Linear);
			
		}
	}

    public void Disappear()
    {
		Tween t = null;
		
		if (!hidden)
		{
			t = target.DOScale(new Vector3(1f, 0f, 1f), 5f).SetEase(Ease.Linear);
			t.onComplete += delegate
							{
								col.enabled  = false;
								rend.enabled = false;
							}; 
		}

		hidden = true;

		
	}
}