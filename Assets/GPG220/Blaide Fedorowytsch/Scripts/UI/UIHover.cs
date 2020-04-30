using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIHover : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    public string text;
    public GameObject toolTip;
    private Vector3 defaultScale;
    private bool visible = true;

    private Tween t;
    // Start is called before the first frame update
    private void Start()
    {
        defaultScale = toolTip.transform.localScale;
        toolTip.transform.localScale = Vector3.zero;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        show(eventData.position);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        hide();
    }

    private void OnDisable()
    {
        hide();
    }


    public void show( Vector2 f)
    {
        if (toolTip != null)
        {
            toolTip.GetComponentInChildren<TextMeshProUGUI>().text = text;
            toolTip.transform.position = f;
            t = toolTip.transform.DOScale(defaultScale, 0.1f);
            visible = true;
        }
    }

    public void hide()
    {
        if (t.IsActive() && !t.IsComplete())
        {
            t.onComplete += ()=> toolTip.transform.DOScale(Vector3.zero, 0.1f); 
        }
        else
        {
            toolTip.transform.DOScale(Vector3.zero, 0.1f);  
        }
        visible = false;
    }


}
