using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GPG220.Blaide_Fedorowytsch.Scripts.ProcGen;
using Mirror;

public class Wall : NetworkBehaviour
{
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
        // target.localScale = new Vector3(2f, 0f, 2f);
        target.DOScale(targetScale, 5f).SetEase(Ease.Linear);
		
		// target.localScale = targetScale;
	}

    public void DestroyWall()
    {
	    procGrow.SetBoolGridPosition(gridPos,false);
	    _spawnNotifier.OnDisappear();
	    
    }

    public void Disappear()
    {

	    target.localScale = new Vector3(1f, 1f, 1f);
	    Tween t = target.DOScale(new Vector3(1f, 0f, 1f), 5f).SetEase(Ease.Linear);
	    t.onComplete += ()=> col.enabled = false;
	    t.onComplete += ()=> rend.enabled = false;
	}
}