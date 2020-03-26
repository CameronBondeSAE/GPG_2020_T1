using GPG220.Blaide_Fedorowytsch.Scripts;
using GPG220.Luca.Scripts.Unit;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class NPCSpearmen : UnitBase
{
    public float cooldown;
    
    public enum States
    {
        Attacking,
        Hiding,
        Charging
    }

    public States currentStates;
    // Start is called before the first frame update
    
    // Update is called once per frame
    private void Update()
    {
        cooldown += Time.deltaTime;
        if (cooldown > 5)
        {
            Debug.Log("Working");
            
        }
    }

    public override void OnSelected()
    {
        base.OnSelected();
        Debug.Log("Spearmen selected");
    }

    public override void OnExecuteAction(Vector3 worldPosition, GameObject g)
    {
        base.OnExecuteAction(worldPosition, g);
        
        Debug.Log("Spearmen:" +worldPosition);
    }
}
