using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tweening : MonoBehaviour
{
    private void Start()
    {
        transform.localScale = new Vector3(2f, 1f, 2f);
    }

    public void OnEnable()
    {
        transform.localScale = new Vector3(2f, 1f, 2f);
        transform.DOScale(new Vector3(2f, 11.34f, 2f), 5f).SetEase(Ease.Linear);
    }

    public void OnDisable()
    {
        transform.localScale = new Vector3(2f, 1f, 2f);
    }
}