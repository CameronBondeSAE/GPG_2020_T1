using GPG220.Luca.Scripts.Abilities;
using UnityEngine;

public class GetLittleAbility : AbilityBase
{
    public override bool Execute(GameObject executorGameObject, GameObject[] targets = null)
    {
        Debug.Log("LITTLE!");

        return true;
    }
}
