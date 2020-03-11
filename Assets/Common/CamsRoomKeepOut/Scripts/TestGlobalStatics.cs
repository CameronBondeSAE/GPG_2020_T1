using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Unit;
using Sirenix.OdinInspector;
using UnityEngine;

public class TestGlobalStatics : SerializedMonoBehaviour
{
    public static float mana;
    public List<UnitBase> enemyUnitBases;
    public List<UnitBase> playerUnitBases;
    
    // Start is called before the first frame update
    void Awake()
    {
        Health.deathStaticEvent += OnDeathStaticEvent;
    }

    private void OnDeathStaticEvent(Health health)
    {
        // Debug.Log(i);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
