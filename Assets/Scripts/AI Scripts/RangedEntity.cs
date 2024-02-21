using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEntity : Entity
{
    public override void Start()
    {
        base.Start();
        playerDetectedState = new PlayerDetectedRanged(this, stateMachine, PlayerDetectedStateData);
        attackState = new RangeAttackState(this, stateMachine, PlayerDetectedStateData);

        stateMachine.Initialize(playerDetectedState);
    }
}
