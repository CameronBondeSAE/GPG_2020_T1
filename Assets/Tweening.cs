using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tweening : MonoBehaviour
{

    public Collider collider;
    
    private void Start()
    {
        transform.localScale = new Vector3(1f, 0f, 1f);
    }

    public void Appear()
    {
        collider.enabled = true;
        transform.localScale = new Vector3(1f, 0f, 1f);
        //  transform.DOScale(new Vector3(2f, 11.34f, 2f), 5f).SetEase(Ease.Linear);
        transform.DOScale(new Vector3(1f, 1f, 1f), 5f).SetEase(Ease.Linear);
    }

    public void Disappear()
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
        transform.DOScale(new Vector3(1f, 0f, 1f), 5f).SetEase(Ease.Linear);
        collider.enabled = false;
    }
}