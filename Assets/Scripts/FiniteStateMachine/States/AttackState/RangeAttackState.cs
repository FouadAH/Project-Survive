using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackState : AttackState
{
    public RangeAttackState(Entity entity, FiniteStateMachine stateMachine, PlayerDetectedStateData stateData) : base(entity, stateMachine, stateData)
    {
        this.stateData = stateData;
    }
    public override void Enter()
    {
        base.Enter();
        entity.lookAtPlayer = true;
    }

    public override void Exit()
    {
        base.Exit();
        entity.lookAtPlayer = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (entity.PlayerWithinRangedAttackRange_Max())
        {
            entity.SetHasTarget(true);
            Vector3 direction = entity.transform.position - entity.runtimeData.playerPosition;
            entity.MoveToTarget(direction);
        }

        if (RangeCheck())
        {
            entity.projectileController.LaunchProjectile(entity.runtimeData.playerPosition, entity.runtimeData.playerMotion);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (entity.PlayerWithinRangedAttackRange_Max())
        {
            entity.SetHasTarget(true);
            Vector3 direction = entity.transform.position - entity.runtimeData.playerPosition;
            entity.MoveToTarget(direction);
        }
        else
        {
            entity.SetHasTarget(false);
        }
    }

    public override bool RangeCheck()
    {
        return entity.PlayerWithinRangedAttackRange_Min();
    }
}
