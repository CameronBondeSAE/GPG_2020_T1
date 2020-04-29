using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Unit;
using UnityEngine;
using DG.Tweening;
public class RocketManUnitBase : UnitBase
{
    public int damage;

    private Rigidbody rb;

    public float tweenduration;

    public AudioSource deathSound;
    
    public override void OnSelected()
    {
        base.OnSelected();
        Debug.Log("Selected!");
        transform.DOShakeScale(tweenduration, new Vector3(1f, 0, 2f), 1, 0.2f, false).SetEase(Ease.InBounce);
    }

    public override void OnDeSelected()
    {
        base.OnDeSelected();
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Health>().deathEvent += Death;
        Initialize();
    }

    public void Death(Health health)
    {
        Destroy(gameObject);
        deathSound.Play();
    }

   
}
