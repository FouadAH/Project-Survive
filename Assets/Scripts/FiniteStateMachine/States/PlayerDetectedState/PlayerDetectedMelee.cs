using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectedMelee : PlayerDetectedState
{
    public PlayerDetectedMelee(Entity entity, FiniteStateMachine stateMachine, PlayerDetectedStateData stateData) : base(entity, stateMachine, stateData)
    {
        this.stateData = stateData;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        entity.lookAtPlayer = true;
        //if (!entity.PlayerWithinRange_Max())
        //{
        //    entity.stateMachine.ChangeState(entity.idleState);
        //}
        //else if (entity.PlayerWithinRangedAttackRange())
        //{
        //    entity.stateMachine.ChangeState(entity.rangeAttackState);
        //}
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        entity.SetHasTarget(true);

        entity.MoveToTarget(entity.runtimeData.playerPosition);
    }

    public override bool RangeCheck()
    {
        return entity.PlayerWithinMeleeAttackRange();
    }
}
