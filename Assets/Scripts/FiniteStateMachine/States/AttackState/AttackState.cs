using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    protected PlayerDetectedStateData stateData;

    public AttackState(Entity entity, FiniteStateMachine stateMachine, PlayerDetectedStateData stateData) : base(entity, stateMachine)
    {
        this.stateData = stateData;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!RangeCheck())
        {
            entity.stateMachine.ChangeState(entity.playerDetectedState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        entity.SetHasTarget(false);
    }

    public virtual bool RangeCheck()
    {
        return entity.PlayerWithinMeleeAttackRange();
    }
}
