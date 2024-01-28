using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    protected IdleStateData stateData;

    protected float idleTime;
    protected bool isIdleTimeOver;

    public IdleState(Entity entity, FiniteStateMachine stateMachine, IdleStateData stateData) : base(entity, stateMachine)
    {
        this.stateData = stateData;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (entity.PlayerWithinRange_Min())
        {
            entity.stateMachine.ChangeState(entity.playerDetectedState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        entity.SetHasTarget(false);

    }
}
