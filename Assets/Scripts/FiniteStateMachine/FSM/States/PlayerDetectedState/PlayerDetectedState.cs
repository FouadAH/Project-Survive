using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerDetectedState : State
{
    private PlayerDetectedStateData stateData;
    private float lastFireTime;

    public PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, PlayerDetectedStateData stateData) : base(entity, stateMachine)
    {
        this.stateData = stateData;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        lastFireTime += Time.deltaTime;

        if (!entity.PlayerWithinRange_Max())
        {
            entity.stateMachine.ChangeState(entity.idleState);
        }
        //else
        //{
        //    if (lastFireTime >= stateData.fireCooldown)
        //    {
        //        entity.FireProjectile();
        //        lastFireTime = 0;
        //    }
        //}
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        entity.SetHasTarget(true);

        entity.MoveToTarget(entity.runtimeData.playerPosition);
    }
}
