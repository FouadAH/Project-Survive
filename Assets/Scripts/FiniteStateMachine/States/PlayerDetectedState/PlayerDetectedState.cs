using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerDetectedState : State
{
    protected PlayerDetectedStateData stateData;
    protected float lastFireTime;

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
        else if (RangeCheck())
        {
            entity.stateMachine.ChangeState(entity.attackState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        entity.SetHasTarget(true);

        entity.MoveToTarget(entity.runtimeData.playerPosition);
    }

    public virtual bool RangeCheck()
    {
        return entity.PlayerWithinRangedAttackRange_Min();
    }
}
