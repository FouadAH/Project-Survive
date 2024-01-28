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

        stateMachine.Initialize(idleState);
    }

    public override void Update()
    {
        base.Update();
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerChecker.position, entityData.attackRangeMin);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerChecker.position, entityData.attackRangeMax);
    }
}
