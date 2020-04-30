using System.Collections;
using System.Collections.Generic;
using AnthonyY;
using UnityEngine;

public class BigState : StateBase
{
    public CamMonster camMonster;

    public override void Enter()
    {
        base.Enter();

        camMonster.abilityController.SelectedExecuteAbility(camMonster.GetComponent<GetBigAbility>());
    }

    public override void Execute()
    {
        base.Execute();
    }

   
    public override void Exit()
    {
        base.Exit();
    }
}
