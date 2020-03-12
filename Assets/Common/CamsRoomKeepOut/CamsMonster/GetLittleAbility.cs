using DG.Tweening;
using GPG220.Luca.Scripts.Abilities;
using UnityEngine;

public class GetLittleAbility : AbilityBase
{
    public CamMonster camMonster;
    public float scale;
    
    public override bool SelectedExecute()
    {
        camMonster.transform.DOScale(Vector3.one * scale, 0.5f).SetEase(Ease.OutBounce);

        return base.SelectedExecute();
    }
}
