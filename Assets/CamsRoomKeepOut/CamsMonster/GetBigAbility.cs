using DG.Tweening;
using GPG220.Luca.Scripts.Abilities;
using UnityEngine;

public class GetBigAbility : AbilityBase
{
    public CamMonster camMonster;
    public float scale;

	public AnimationCurve curve;
	
    public override bool SelectedExecute()
    {
        // camMonster.transform.DOScale(Vector3.one * scale, 0.5f).SetEase(Ease.OutBounce);

		Sequence sequence = DOTween.Sequence();
		sequence.Append(DOTween.To(Get, Set, scale, 4f));
		sequence.AppendInterval(2);
		sequence.InsertCallback(2.5f, SayCam);
		sequence.Append(DOTween.To(Get, Set, 1, 2f));
		sequence.Play();

		
		
		// DOTween.To(value => camMonster.transform.localScale.x, value => camMonster.transform.localScale = new Vector3(value, value, value), scale, 2.5f).OnComplete(Finished);
		// DOTween.To(Get, Set, scale, 2.5f);
		
		
		
		
        return base.SelectedExecute();
    }

	private void SayCam()
	{
		Debug.Log("Cam");
		
	}

	private void Finished()
	{
		Debug.Log("Done");
	}

	private float Get()
	{
		return camMonster.transform.localScale.x;
	}

	private void Set(float pnewvalue)
	{
		camMonster.transform.localScale = new Vector3(pnewvalue, pnewvalue, pnewvalue);
	}
}
