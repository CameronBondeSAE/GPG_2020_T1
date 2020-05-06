using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GPG220.Blaide_Fedorowytsch.Scripts.ProcGen;
using Mirror;

public class Wall : NetworkBehaviour
{
	
	
	public float appearDuration = 4f;
	public float disappearDuration = 1f;
	
	
	[SyncVar(hook = nameof(NetworkHidden))]
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

		col.enabled = false;
		rend.enabled = false;
		
		target.localScale = new Vector3(targetScale.x, 0, targetScale.z);
		_spawnNotifier = GetComponent<ObstacleSpawnNotifier>();
	}

   
    public void NetworkHidden(bool oldValue , bool newValue)
    {

	    if (isClientOnly)
	    {
		    
		    if (newValue)
		    {
			    Appear();
		    }
		    else
		    {
			    Disappear();
		    }
	    }
    }
    
    
    public void Appear()
    {
		col.enabled = true;
		rend.enabled = true;

		if (hidden)
		{
			var tweenerCore = target.DOScale(targetScale, appearDuration).SetEase(Ease.Linear);
		}

	
			hidden = false;
		
		
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
			t = target.DOScale(new Vector3(1f, 0f, 1f), disappearDuration).SetEase(Ease.InOutElastic);
			t.onComplete += delegate
							{
								col.enabled  = false;
								rend.enabled = false;
							}; 
		}
		
			hidden = true;
			
		
	}
}