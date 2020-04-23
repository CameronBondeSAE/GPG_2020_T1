﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Mirror;

public class Wall : NetworkBehaviour
{
	public Collider col;
	Renderer rend;
	public Transform target;
	public Vector3 targetScale;
	
    private void Awake()
	{
		// col = GetComponent<Collider>();
		rend = GetComponentInChildren<Renderer>();
		rend.enabled = false;
		
		target.localScale = new Vector3(targetScale.x, 0, targetScale.z);
	}

    public void Appear()
    {
		col.enabled = true;
		rend.enabled = true;
        // target.localScale = new Vector3(2f, 0f, 2f);
        target.DOScale(targetScale, 5f).SetEase(Ease.Linear);
		
		// target.localScale = targetScale;
	}

    public void Disappear()
    {
		col.enabled = false;
		rend.enabled = false; // TODO do this when tween finishes
		// target.localScale = new Vector3(1f, 1f, 1f);
        // target.DOScale(new Vector3(1f, 0f, 1f), 5f).SetEase(Ease.Linear);
	}
}