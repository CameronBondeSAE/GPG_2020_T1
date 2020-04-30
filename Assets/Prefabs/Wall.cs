using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GPG220.Blaide_Fedorowytsch.Scripts.ProcGen;
using Mirror;

public class Wall : NetworkBehaviour
{
	public float duration = 4f;

	public bool hidden = true;
	
	public Collider col;
	Renderer rend;
	public Transform target;
	public Vector3 targetScale;
	public Vector2Int gridPos;
	public ProceduralGrowthSystem procGrow;
	private ObstacleSpawnNotifier _spawnNotifier;
	
    private void Awake()
	{
		 col = GetComponent<Collider>();
		rend = GetComponentInChildren<Renderer>();
		rend.enabled = false;
		
		target.localScale = new Vector3(targetScale.x, 0, targetScale.z);
		_spawnNotifier = GetComponent<ObstacleSpawnNotifier>();
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

    public void DestroyWall()
    {
	    procGrow.SetBoolGridPosition(gridPos,false);
	    _spawnNotifier.OnDisappear();
	    
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