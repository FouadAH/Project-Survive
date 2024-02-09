using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerSpider : Entity
{
    public FlamethrowerController flamethrowerController;
    public float attackTime = 5f;

    public override void Start()
    {
        base.Start();
        playerDetectedState = new PlayerDetectedState(this, stateMachine, PlayerDetectedStateData);
        attackState = new FlamethrowerAttackState(this, stateMachine, PlayerDetectedStateData);

        stateMachine.Initialize(idleState);
    }
}
