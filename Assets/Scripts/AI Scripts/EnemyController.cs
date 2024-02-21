using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputProvider))]
public class EnemyController : Entity
{
    public override void Start()
    {
        base.Start();
        playerDetectedState = new PlayerDetectedMelee(this, stateMachine, PlayerDetectedStateData);
        attackState = new MeleeAttackState(this, stateMachine, PlayerDetectedStateData);

        stateMachine.Initialize(playerDetectedState);
    }

}
