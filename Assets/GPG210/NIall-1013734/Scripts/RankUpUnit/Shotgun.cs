using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Abilities;
using UnityEngine;

public class Shotgun : AbilityBase
{
    public GameObject pelletPrefab;
    public GameObject Prong;


    public override bool SelectedExecute()
    {
     GameObject pellet = Instantiate(pelletPrefab);
     pellet.transform.position = Prong.transform.position + Prong.transform.forward;
     pellet.transform.forward = Prong.transform.forward;
     
     return false;
    }


}
