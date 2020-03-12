using GPG220.Luca.Scripts.Abilities;
using UnityEngine;

public class GetBigAbility : AbilityBase
{
    public CamMonster camMonster;
    public float scale;

    public override bool SelectedExecute()
    {
        camMonster.transform.localScale = Vector3.one * scale;

        return base.SelectedExecute();
    }
}
