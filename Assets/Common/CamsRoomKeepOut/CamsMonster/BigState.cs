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

        camMonster.abilityController.ExecuteAbility<GetBigAbility>();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
